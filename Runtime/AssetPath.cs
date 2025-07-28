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

using UnityEngine;
using UnityEngine.Networking;

using SystemPath = System.IO.Path;
using SystemDirectory = System.IO.Directory;

namespace GooAsset
{
    /// <summary>
    /// 资源目录及路径相关的辅助工具类，用于统一提供对资源访问路径相关的接口函数
    /// </summary>
    public static class AssetPath
    {
        /// <summary>
        /// 打包目录
        /// </summary>
        public const string BuildPath = "Bundles";

        /// <summary>
        /// 本地路径的网址前缀(例:file://)
        /// </summary>
        private static string _localProtocol;

        /// <summary>
        /// 资源下载地址
        /// </summary>
        private static string _downloadUrl;

        /// <summary>
        /// 资源下载地址, 需外部设置
        /// </summary>
        public static string DownloadUrl
        {
            get => _downloadUrl;
            set
            {
                if (value.EndsWith("/") || value.EndsWith("\\"))
                {
                    _downloadUrl = value[..^1];
                }
                else
                {
                    _downloadUrl = value;
                }
            }
        }

        /// <summary>
        /// 平台名字
        /// </summary>
        public static string PlatformName { get; set; }

        /// <summary>
        /// 本地资源目录
        /// </summary>
        public static string LocalDataPath { get; set; }

        /// <summary>
        /// 下载资源目录
        /// </summary>
        public static string DownloadDataPath { get; set; }

        /// <summary>
        /// 初始化资源基本目录
        /// </summary>
        public static void InitAssetPath()
        {
            var pf = Application.platform;
            if (pf != RuntimePlatform.OSXEditor && pf != RuntimePlatform.OSXPlayer && pf != RuntimePlatform.IPhonePlayer)
            {
                _localProtocol = pf is RuntimePlatform.WindowsEditor or RuntimePlatform.WindowsPlayer ? "file:///" : string.Empty;
            }
            else
            {
                _localProtocol = "file://";
            }

            // 以下目录允许编辑器下直接修改, 所以需要先判空再初始化
            if (string.IsNullOrEmpty(PlatformName))
            {
                PlatformName = Utility.Platform.CurrentPlatformName;
            }

            if (string.IsNullOrEmpty(LocalDataPath))
            {
                LocalDataPath = CombinePath(Application.streamingAssetsPath, BuildPath);
            }

            if (string.IsNullOrEmpty(DownloadDataPath))
            {
                DownloadDataPath = CombinePath(Application.persistentDataPath, BuildPath);
            }

            if (!AssetManagement.isOfflineWindows && !SystemDirectory.Exists(DownloadDataPath))
            {
                SystemDirectory.CreateDirectory(DownloadDataPath);
            }
        }

        /// <summary>
        /// 将文件名转换成本地资源目录的具体路径
        /// </summary>
        public static string TranslateToLocalDataPath(string fileName)
        {
            return CombinePath(LocalDataPath, fileName);
        }

        /// <summary>
        /// 将文件名转换成下载保存目录的具体路径
        /// </summary>
        public static string TranslateToDownloadDataPath(string fileName)
        {
            return CombinePath(DownloadDataPath, fileName);
        }

        /// <summary>
        /// 将文件名转换成本地资源路径下的具体url
        /// </summary>
        public static string TranslateToLocalDataPathUrl(string fileName)
        {
            if (Application.platform != RuntimePlatform.WindowsPlayer)
            {
                return $"{_localProtocol}{CombinePath(LocalDataPath, fileName)}";
            }

            // #号在网页链接里代表分段, UnityWebRequest不支持分段, 但Windows下用户安装文件夹各种各样, 故本地文件路径需要使用EscapeURL保证文件路径正确(另外一些符号也可能导致问题, 例如:+)
            return $"{_localProtocol}{UnityWebRequest.EscapeURL(CombinePath(LocalDataPath, fileName))}";
        }

        /// <summary>
        /// 将文件名转换成下载地址内的具体url
        /// </summary>
        public static string TranslateToDownloadUrl(string fileName)
        {
            string customUrl = Configure.Callback.CustomizedDownloadUrlCallbackFunction?.Invoke(fileName);
            return !string.IsNullOrEmpty(customUrl) ? customUrl : $"{DownloadUrl}/{CombinePath(PlatformName, fileName)}";
        }

        /// <summary>
        /// 将文件名转换成临时目录下的具体路径
        /// </summary>
        public static string TranslateToTempPath(string fileName)
        {
            return CombinePath(Application.temporaryCachePath, fileName);
        }

        /// <summary>
        /// 连接目录
        /// </summary>
        public static string CombinePath(string path1, string path2)
        {
            return SystemPath.Combine(path1, path2).Replace('\\', '/');
        }

        #region 自定义资源加载地址

        /// <summary>
        /// 自定义资源加载地址(简化的名字或路径)和真实记录路径对照表
        /// </summary>
        static readonly Dictionary<string, string> _customAssetAddressToPath = new();

        /// <summary>
        /// 根据真实资源路径和自定义加载地址方法获得自定义加载地址并进行记录
        /// </summary>
        /// <param name="assetPath">真实资源路径</param>
        internal static void RecordCustomLoadPath(string assetPath)
        {
            if (null == Configure.Callback.CustomizedAssetLoadPathCallbackFunction)
                return;

            string customAddress = Configure.Callback.CustomizedAssetLoadPathCallbackFunction(assetPath);
            if (string.IsNullOrEmpty(customAddress))
                return;

            _customAssetAddressToPath[customAddress] = assetPath;
        }

        /// <summary>
        /// 根据加载地址获取真实加载路径
        /// </summary>
        internal static string GetActualPath(string address)
        {
            return _customAssetAddressToPath.GetValueOrDefault(address, address);
        }

        #endregion
    }
}
