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

using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace HooAsset
{
    /// <summary>
    /// 资源清单管理
    /// </summary>
    public static class ManifestHandler
    {
        /// <summary>
        /// 是否加密清单文件和版本文件并隐藏真实的文件名
        /// </summary>
        public static readonly bool IsEncryptManifestFile = true;

        private const string Gd4H = "diKj530tJ6gzqRhAsMIu5YbxPee8H4dg";

        private const string ZNfR = "xyN8sJI7IMRfNzD5";

        /// <summary>
        /// 当前所有清单的列表
        /// </summary>
        public static readonly List<Manifest> ManifestList = new();

        /// <summary>
        /// 清单名字和清单的对照字典
        /// </summary>
        static readonly Dictionary<string, Manifest> s_nameToManifest = new();

        /// <summary>
        /// 默认版本文件名字
        /// </summary>
        public const string DefaultVersionFileName = "version.json";

        /// <summary>
        /// 版本信息文件名
        /// </summary>
        public static string VersionFileName
        {
            get
            {
                if (IsEncryptManifestFile)
                {
                    // 一段自己写的致未来的寄语转成的md5值, 后缀unity3d是为了和ab包一样, 以达到混淆的效果
                    return "d4a27b33a023fdabc304433a38a64e11.unity3d";
                }

                return DefaultVersionFileName;
            }
        }

        /// <summary>
        /// 默认白名单版本文件名字
        /// </summary>
        const string DefaultWhiteListVersionFileName = "whiteListVersion.json";

        /// <summary>
        /// 白名单版本信息文件名
        /// </summary>
        public static string WhiteListVersionFileName => IsEncryptManifestFile ? "33a38a6422e11d4a27b33a023fdabc28.unity3d" : DefaultWhiteListVersionFileName;

        /// <summary>
        /// 获取带版本号的版本信息文件名
        /// </summary>
        public static string GetVersionFileNameWithVersion(int version)
        {
            if (IsEncryptManifestFile)
            {
                return $"{Utility.Format.ComputeHashFromString($"d4a27b33a023fdabc304433a38a64e11_v{version}")}.unity3d";
            }

            return $"version_v{version}.json";
        }

        /// <summary>
        /// 清单文件对象和版本文件对象转换成字符串
        /// </summary>
        public static string ManifestObjectToJson(ScriptableObject scriptableObject)
        {
            if (IsEncryptManifestFile)
            {
                return AESEncryptProvider.Encrypt(JsonUtility.ToJson(scriptableObject), Gd4H, ZNfR);
            }

            return JsonUtility.ToJson(scriptableObject);
        }

        /// <summary>
        /// 获取指定清单文件或版本文件文件的Json字符串
        /// </summary>
        static string GetFileJson(string filePath)
        {
            if (IsEncryptManifestFile)
            {
                return AESEncryptProvider.Decrypt(File.ReadAllText(filePath), Gd4H, ZNfR);
            }

            return File.ReadAllText(filePath);
        }

        /// <summary>
        /// 读取清单版本列表文件容器信息
        /// </summary>
        public static ManifestVersionContainer LoadManifestVersionContainer(string versionFilePath)
        {
            if (!File.Exists(versionFilePath))
            {
                Logger.Error($"版本文件不存在！！目录:{versionFilePath}");
                return null;
            }

            ManifestVersionContainer manifestVersionContainer;

            try
            {
                manifestVersionContainer = ScriptableObject.CreateInstance<ManifestVersionContainer>();
                JsonUtility.FromJsonOverwrite(GetFileJson(versionFilePath), manifestVersionContainer);
            }
            catch (Exception e)
            {
                manifestVersionContainer = null;
                Debug.LogException(e);
                File.Delete(versionFilePath);
            }

            return manifestVersionContainer;
        }

        /// <summary>
        /// 获取清单
        /// </summary>
        public static Manifest GetManifest(string name)
        {
            return s_nameToManifest.GetValueOrDefault(name);
        }

        /// <summary>
        /// 读取清单信息
        /// </summary>
        public static Manifest LoadManifest(string manifestFilePath)
        {
            if (!File.Exists(manifestFilePath))
            {
                Logger.Error($"清单文件不存在！！目录:{manifestFilePath}");
                return null;
            }

            Manifest manifest;

            try
            {
                manifest = ScriptableObject.CreateInstance<Manifest>();
                JsonUtility.FromJsonOverwrite(GetFileJson(manifestFilePath), manifest);
                manifest.Reload();
            }
            catch (Exception e)
            {
                manifest = null;
                Debug.LogException(e);
                File.Delete(manifestFilePath);
            }

            return manifest;
        }

        /// <summary>
        /// 刷新全局的指定清单信息
        /// </summary>
        public static void RefreshGlobalManifest(Manifest manifest)
        {
            string manifestName = manifest.name;
            if (s_nameToManifest.TryGetValue(manifestName, out Manifest m))
            {
                m.OverrideManifest(manifest);
                return;
            }

            ManifestList.Add(manifest);
            s_nameToManifest.Add(manifestName, manifest);
        }

        /// <summary>
        /// 根据指定的清单版本信息判断清单是否已生效
        /// </summary>
        public static bool IsManifestEffective(ManifestVersion manifestVersion)
        {
            return s_nameToManifest.TryGetValue(manifestVersion.Name, out Manifest m) && m.fileName == manifestVersion.FileName;
        }

        /// <summary>
        /// 根据指定的清单版本信息判断清单文件是否存在
        /// </summary>
        public static bool IsManifestFileExist(ManifestVersion manifestVersion)
        {
            FileInfo info = new FileInfo(AssetPath.TranslateToDownloadDataPath(manifestVersion.FileName));
            return info.Exists && info.Length == manifestVersion.Size && Utility.Format.ComputeHashFromFile(info.FullName) == manifestVersion.Hash;
        }

        /// <summary>
        /// 判断全部清单中是否包含某个资源
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        public static bool IsContainAsset(string assetPath)
        {
            foreach (var manifest in ManifestList)
            {
                if (manifest.IsContainAsset(assetPath))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 根据资源地址获取主资源包信息和其依赖资源包信息列表
        /// </summary>
        /// <param name="assetPath">资源真实路径</param>
        /// <param name="mainBundleInfo">主资源包信息</param>
        /// <param name="dependentBundleInfoList">依赖的资源包信息列表</param>
        /// <returns>清单中是否存在此资源</returns>
        public static bool GetMainBundleInfoAndDependencies(string assetPath, out ManifestBundleInfo mainBundleInfo, out ManifestBundleInfo[] dependentBundleInfoList)
        {
            foreach (Manifest manifest in ManifestList)
            {
                if (manifest.IsContainAsset(assetPath))
                {
                    mainBundleInfo = manifest.GetBundleInfo(assetPath);
                    dependentBundleInfoList = manifest.GetDependentBundleInfoList(mainBundleInfo);
                    return true;
                }
            }

            mainBundleInfo = null;
            dependentBundleInfoList = null;
            return false;
        }

        /// <summary>
        /// 根据资源地址获取其所在的资源包信息
        /// </summary>
        /// <param name="assetPath">资源真实路径</param>
        public static ManifestBundleInfo GetManifestBundleInfo(string assetPath)
        {
            foreach (Manifest manifest in ManifestList)
            {
                if (manifest.IsContainAsset(assetPath))
                {
                    return manifest.GetBundleInfo(assetPath);
                }
            }

            return null;
        }
    }
}
