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

using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace GooAsset
{
    /// <summary>
    /// 获取最新的资源清单信息
    /// </summary>
    public sealed class UpdateManifestOperation : Operation
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="version">指定更新的版本</param>
        /// <param name="isLatest">是否为最新的</param>
        public UpdateManifestOperation(int version = 0, bool isLatest = false)
        {
            Version = version;
            IsLatest = isLatest;
        }

        /// <summary>
        /// 指定更新的版本
        /// </summary>
        public int Version { get; private set; }

        /// <summary>
        /// 是否白名单
        /// </summary>
        public bool IsLatest { get; }

        /// <summary>
        /// 版本文件下载保存目录
        /// </summary>
        string _downloadSavePath;

        /// <summary>
        /// 下载对象
        /// </summary>
        Download _download;

        /// <summary>
        /// 所有新清单文件的下载大小
        /// </summary>
        public long newManifestsDownloadSize;

        /// <summary>
        /// 需要更新的清单列表
        /// </summary>
        readonly List<ManifestVersion> _newManifestVersionList = new();

        /// <summary>
        /// 加载清单出错文本
        /// </summary>
        public string LoadManifestError { get; private set; }

        protected override void OnStart()
        {
            // 离线模式和WebGL模式下不需要更新
            if (AssetManagement.offlineMode || Application.platform == RuntimePlatform.WebGLPlayer)
            {
                Finish();
                return;
            }

            string downloadUrl;

            if (IsLatest)
            {
                downloadUrl = AssetPath.TranslateToDownloadUrl(Configure.File.GetLatestVersionFileName());
            }
            else
            {
                if (Version > 0)
                {
                    downloadUrl = AssetPath.TranslateToDownloadUrl(Configure.File.GetVersionFileName(Version));
                }
                else
                {
                    downloadUrl = AssetPath.TranslateToDownloadUrl(Configure.File.GetVersionFileName());
                }
            }

            _downloadSavePath = AssetPath.TranslateToDownloadDataPath(Configure.File.GetVersionFileName());
            if (File.Exists(_downloadSavePath))
            {
                File.Delete(_downloadSavePath);
            }
            _download = DownloadHandler.DownloadAsync(downloadUrl, _downloadSavePath);
        }

        protected override void OnUpdate()
        {
            if (Status != OperationStatus.Processing)
            {
                return;
            }

            if (!_download.IsDone)
            {
                Progress = _download.Progress;
                return;
            }

            if (_download.Status != DownloadStatus.Successful)
            {
                Finish(_download.Error);
                return;
            }

            ManifestVersionContainer downloadVersionContainer = ManifestHandler.LoadManifestVersionContainer(_downloadSavePath);
            if (downloadVersionContainer is null)
            {
                Finish("加载新下载的清单版本容器失败, versionContainer = null");
                return;
            }

            string buildInVersionContainerPath = AssetPath.TranslateToTempPath(Configure.File.GetVersionFileName());
            ManifestVersionContainer buildInVersionContainer = ManifestHandler.LoadManifestVersionContainer(buildInVersionContainerPath);
            if (buildInVersionContainer is null)
            {
                Finish("加载内置的清单版本容器失败, buildInVersionContainer = null, 请确保已初始化再检查清单更新");
                return;
            }

            // 若内置的构建时间戳比较新, 则使用内置的
            if (downloadVersionContainer.Timestamp <= buildInVersionContainer.Timestamp)
            {
                // 使用内置清单
                foreach (ManifestVersion manifestVersion in buildInVersionContainer.AllManifestVersions)
                {
                    if (ManifestHandler.IsManifestEffective(manifestVersion))
                    {
                        continue;
                    }

                    Manifest manifest = ManifestHandler.LoadManifest(AssetPath.TranslateToDownloadDataPath(manifestVersion.FileName));
                    if (manifest is null)
                    {
                        Finish($"清单文件({manifestVersion.Name})加载失败");
                        return;
                    }

                    manifest.name = manifestVersion.Name;
                    manifest.fileName = manifestVersion.FileName;
                    ManifestHandler.RefreshGlobalManifest(manifest);
                }

                Version = buildInVersionContainer.Version;
                Finish();
                return;
            }

            foreach (ManifestVersion manifestVersion in downloadVersionContainer.AllManifestVersions)
            {
                if (ManifestHandler.IsManifestFileExist(manifestVersion))
                {
                    continue;
                }

                newManifestsDownloadSize += manifestVersion.Size;
                _newManifestVersionList.Add(manifestVersion);
            }

            Version = downloadVersionContainer.Version;
            Finish();
        }

        /// <summary>
        /// 开始更新所有新的清单文件
        /// </summary>
        public DownloadOperation StartUpdateManifests()
        {
            List<DownloadInfo> downloadInfos = _newManifestVersionList.ConvertAll(v => new DownloadInfo
            {
                hash = v.Hash,
                size = v.Size,
                url = AssetPath.TranslateToDownloadUrl(v.FileName),
                savePath = AssetPath.TranslateToDownloadDataPath(v.FileName)
            });

            return AssetManagement.DownloadAsync(downloadInfos);
        }

        /// <summary>
        /// 新清单文件下载完成后需由外部调用, 用新的清单覆盖当前清单信息
        /// </summary>
        public void RefreshNewGlobalManifest()
        {
            foreach (ManifestVersion manifestVersion in _newManifestVersionList)
            {
                Manifest manifest = ManifestHandler.LoadManifest(AssetPath.TranslateToDownloadDataPath(manifestVersion.FileName));
                if (manifest != null)
                {
                    manifest.name = manifestVersion.Name;
                    manifest.fileName = manifestVersion.FileName;
                    ManifestHandler.RefreshGlobalManifest(manifest);
                }
                else
                {
                    if (!string.IsNullOrEmpty(LoadManifestError))
                    {
                        LoadManifestError += "\n";
                    }
                    LoadManifestError += $"加载清单失败:{manifestVersion.Name}, 文件名:{manifestVersion.FileName}, 请检查是否上传了错误的文件";
                }
            }
        }
    }
}
