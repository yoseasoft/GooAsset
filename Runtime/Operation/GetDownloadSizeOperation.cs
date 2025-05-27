using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace AssetModule
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
        public readonly List<DownloadInfo> downloadInfoResults = new();

        /// <summary>
        /// 总共大小(单位:字节(B))
        /// </summary>
        public long TotalSize { get; private set; }

        /// <summary>
        /// 获取下载大小的操作
        /// </summary>
        /// <param name="bundleInfoList">需要获取下载大小的清单bundle信息列表</param>
        public GetDownloadSizeOperation(List<ManifestBundleInfo> bundleInfoList = null)
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
                if (!AssetManagement.IsBuildInFile(bundleInfo.NameWithHash) && !AssetManagement.IsBundleFileAlreadyInDownloadPath(bundleInfo) && !downloadInfoResults.Exists(info => info.savePath == savePath))
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

                    downloadInfoResults.Add(new DownloadInfo
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