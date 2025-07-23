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

using UnityEngine;

namespace HooAsset
{
    /// <summary>
    /// 获取下载大小的操作
    /// </summary>
    public sealed class GetDownloadSizeOperation : Operation
    {
        /// <summary>
        /// 需要获取下载大小的清单bundle信息列表, 创建此类时传入
        /// </summary>
        readonly List<ManifestBundleInfo> _bundleInfos = new();

        /// <summary>
        /// 通过bundle信息计算后, 需要下载的下载信息列表
        /// </summary>
        readonly List<DownloadInfo> _downloadInfoResults = new();

        /// <summary>
        /// 总共大小(单位:字节(B))
        /// </summary>
        public long TotalSize { get; private set; }

        /// <summary>
        /// 获取下载大小的操作
        /// </summary>
        /// <param name="bundleInfoList">需要获取下载大小的清单bundle信息列表</param>
        public GetDownloadSizeOperation(IList<ManifestBundleInfo> bundleInfoList = null)
        {
            if (bundleInfoList != null)
            {
                _bundleInfos.AddRange(bundleInfoList);
            }
        }

        protected override void OnStart()
        {
            TotalSize = 0;

            // 离线模式和WebGL平台下不需获取下载大小
            if (AssetManagement.offlineMode || Application.platform == RuntimePlatform.WebGLPlayer)
            {
                Finish();
                return;
            }

            if (_bundleInfos.Count == 0)
            {
                Finish();
            }
        }

        protected override void OnUpdate()
        {
            if (Status != OperationStatus.Processing)
            {
                return;
            }

            while (_bundleInfos.Count > 0)
            {
                ManifestBundleInfo bundleInfo = _bundleInfos[0];
                string savePath = AssetPath.TranslateToDownloadDataPath(bundleInfo.SaveFileName);
                if (!AssetManagement.IsBuildInFile(bundleInfo.NameWithHash) && !AssetManagement.IsBundleFileAlreadyInDownloadPath(bundleInfo) && !_downloadInfoResults.Exists(info => info.savePath == savePath))
                {
                    if (File.Exists(savePath))
                    {
                        if (bundleInfo.IsRawFile)
                        {
                            // 原始文件下载后因没有使用Hash后缀的名字(不能保证唯一), 故不支持断点续传, 所以直接删除再重新下载
                            File.Delete(savePath);
                            TotalSize += bundleInfo.Size;
                        }
                        else
                        {
                            // 因Bundle文件名能保证唯一, 故下载支持断点续传, 所以文件已存在时这样计算大小
                            TotalSize += bundleInfo.Size - new FileInfo(savePath).Length;
                        }
                    }
                    else
                    {
                        TotalSize += bundleInfo.Size;
                    }

                    _downloadInfoResults.Add(new DownloadInfo
                    {
                        savePath = savePath,
                        hash = bundleInfo.Hash,
                        size = bundleInfo.Size,
                        url = AssetPath.TranslateToDownloadUrl(bundleInfo.NameWithHash)
                    });
                }

                _bundleInfos.RemoveAt(0);

                if (AssetDispatcher.Instance.IsBusy)
                {
                    return;
                }
            }

            Finish();
        }
    }
}
