/// -------------------------------------------------------------------------------
/// HooAsset Framework
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

using System.IO;
using System.Collections.Generic;

namespace HooAsset
{
    /// <summary>
    /// 清除历史文件操作
    /// </summary>
    public sealed class ClearHistoryOperation : Operation
    {
        /// <summary>
        /// 需要删除的所有文件路径
        /// </summary>
        readonly List<string> _needDeleteFilePaths = new();

        /// <summary>
        /// 需要删除文件总数, 用于计算进度
        /// </summary>
        int _totalCount;

        protected override void OnStart()
        {
            _needDeleteFilePaths.AddRange(Directory.GetFiles(AssetPath.DownloadDataPath));
            List<string> usedFileNameList = new(); // 当前使用的文件

            // 版本文件
            usedFileNameList.Add(ManifestHandler.VersionFileName);

            foreach (Manifest manifest in ManifestHandler.ManifestList)
            {
                // 清单文件
                usedFileNameList.Add(manifest.fileName);

                // ab包和原始文件
                foreach (ManifestBundleInfo bundleInfo in manifest.manifestBundleInfoList)
                    if (!string.IsNullOrEmpty(bundleInfo.SaveFileName))
                        usedFileNameList.Add(bundleInfo.SaveFileName);
            }

            _needDeleteFilePaths.RemoveAll(filePath => usedFileNameList.Contains(Path.GetFileName(filePath)));

            _totalCount = _needDeleteFilePaths.Count;
        }

        protected override void OnUpdate()
        {
            if (Status != OperationStatus.Processing)
                return;

            while (_needDeleteFilePaths.Count > 0)
            {
                Progress = (float)(_totalCount - _needDeleteFilePaths.Count + 1) / _totalCount;
                string filePath = _needDeleteFilePaths[0];
                if (File.Exists(filePath))
                    File.Delete(filePath);
                _needDeleteFilePaths.RemoveAt(0);

                if (AssetDispatcher.Instance.IsBusy)
                    return;
            }

            Finish();
        }
    }
}
