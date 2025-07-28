/// -------------------------------------------------------------------------------
/// GooAsset Framework
///
/// Copyright (C) 2020 - 2022, Guangzhou Xinyuan Technology Co., Ltd.
/// Copyright (C) 2022 - 2023, Shanghai Bilibili Technology Co., Ltd.
/// Copyright (C) 2023 - 2024, Guangzhou Shiyue Network Technology Co., Ltd.
///
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.
/// -------------------------------------------------------------------------------

using System.Collections.Generic;

namespace GooAsset
{
    /// <summary>
    /// 下载操作
    /// </summary>
    public sealed class DownloadOperation : Operation
    {
        /// <summary>
        /// 需要下载的信息列表
        /// </summary>
        readonly List<DownloadInfo> _needDownloadInfoList = new();

        /// <summary>
        /// 正在下载的Download对象列表
        /// </summary>
        readonly List<Download> _downloadingList = new();

        /// <summary>
        /// 下载成功的Download对象列表
        /// </summary>
        readonly List<Download> _successfulDownloadList = new();

        /// <summary>
        /// 下载失败的Download对象列表
        /// </summary>
        readonly List<Download> _failedDownloadList = new();

        /// <summary>
        /// 是否正在重试
        /// </summary>
        bool _isRetrying;

        /// <summary>
        /// 每帧刷新监听
        /// </summary>
        public System.Action<DownloadOperation> updated;

        /// <summary>
        /// 下载总大小(单位:字节(B))
        /// </summary>
        public long TotalSize { get; private set; }

        /// <summary>
        /// 已下载的大小(单位:字节(B))
        /// </summary>
        public long DownloadedBytes { get; private set; }

        /// <summary>
        /// 下载操作
        /// </summary>
        /// <param name="downloadInfoList">需要下载的下载信息列表</param>
        public DownloadOperation(IList<DownloadInfo> downloadInfoList)
        {
            _needDownloadInfoList.AddRange(downloadInfoList);
        }

        protected override void OnStart()
        {
            if (_isRetrying)
                return;

            DownloadedBytes = 0;
            foreach (DownloadInfo downloadInfo in _needDownloadInfoList)
                TotalSize += downloadInfo.size;

            if (_needDownloadInfoList.Count > 0)
                foreach (DownloadInfo downloadInfo in _needDownloadInfoList)
                    _downloadingList.Add(DownloadHandler.DownloadAsync(downloadInfo));
            else
                Finish();
        }

        /// <summary>
        /// 下载失败后重试
        /// </summary>
        public void Retry()
        {
            Error = "";
            _isRetrying = true;

            Start();

            foreach (Download download in _failedDownloadList)
            {
                DownloadHandler.Retry(download);
                _downloadingList.Add(download);
            }
            _failedDownloadList.Clear();
        }

        protected override void OnUpdate()
        {
            if (Status != OperationStatus.Processing)
                return;

            int downloadingCount = _downloadingList.Count;
            if (downloadingCount > 0)
            {
                long downloadedBytes = 0;

                for (int i = 0; i < downloadingCount; i++)
                {
                    Download download = _downloadingList[i];
                    if (download.IsDone)
                    {
                        _downloadingList.RemoveAt(i);
                        i--;
                        downloadingCount--;
                        if (download.Status == DownloadStatus.Successful)
                            _successfulDownloadList.Add(download);
                        else
                            _failedDownloadList.Add(download);
                    }
                    else
                        downloadedBytes += download.DownloadedBytes;
                }

                foreach (Download download in _successfulDownloadList)
                    downloadedBytes += download.DownloadedBytes;

                foreach (Download download in _failedDownloadList)
                    downloadedBytes += download.DownloadedBytes;

                DownloadedBytes = downloadedBytes;
                Progress = (float)downloadedBytes / TotalSize;
                updated?.Invoke(this);
            }
            else
            {
                updated = null;

                if (_failedDownloadList.Count > 0)
                {
                    Finish($"共有{_failedDownloadList.Count}个文件下载失败, 首个文件失败原因：{_failedDownloadList[0].Error}");
                }
                else
                {
                    Finish();
                }
            }
        }
    }
}
