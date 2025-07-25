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

using System.Collections.Generic;

using UnityEngine;

using SystemFile = System.IO.File;
using SystemFileInfo = System.IO.FileInfo;

namespace HooAsset
{
    /// <summary>
    /// 资源清单管理类
    /// </summary>
    public static class ManifestHandler
    {
        /// <summary>
        /// 当前所有清单的列表
        /// </summary>
        private static readonly List<Manifest> _manifestList = new List<Manifest>();

        /// <summary>
        /// 清单名字和清单的对照字典
        /// </summary>
        static readonly Dictionary<string, Manifest> _nameToManifest = new();

        public static IList<Manifest> ManifestList => _manifestList;

        /// <summary>
        /// 清单文件对象和版本文件对象转换成字符串
        /// </summary>
        public static string ManifestObjectToJson(ScriptableObject scriptableObject)
        {
            string text = JsonUtility.ToJson(scriptableObject);
            if (Configure.Secret.ManifestFileEncryptEnabled)
            {
                text = Utility.Cryptography.Encrypt(text, Configure.Secret.Gd4H, Configure.Secret.ZNfR);
            }

            return text;
        }

        /// <summary>
        /// 获取指定清单文件或版本文件文件的内容
        /// </summary>
        static string ReadFileText(string filePath)
        {
            string text = SystemFile.ReadAllText(filePath);
            if (Configure.Secret.ManifestFileEncryptEnabled)
            {
                return Utility.Cryptography.Decrypt(text, Configure.Secret.Gd4H, Configure.Secret.ZNfR);
            }

            return text;
        }

        /// <summary>
        /// 读取清单版本列表文件容器信息
        /// </summary>
        public static ManifestVersionContainer LoadManifestVersionContainer(string versionFilePath)
        {
            if (!SystemFile.Exists(versionFilePath))
            {
                Logger.Error($"无法找到指定路径“{versionFilePath}”下的版本文件，加载目标文件失败！");
                return null;
            }

            ManifestVersionContainer manifestVersionContainer;

            try
            {
                manifestVersionContainer = ScriptableObject.CreateInstance<ManifestVersionContainer>();
                JsonUtility.FromJsonOverwrite(ReadFileText(versionFilePath), manifestVersionContainer);
            }
            catch (System.Exception e)
            {
                manifestVersionContainer = null;
                Logger.Exception(e);
                SystemFile.Delete(versionFilePath);
            }

            return manifestVersionContainer;
        }

        /// <summary>
        /// 获取清单
        /// </summary>
        public static Manifest GetManifest(string name)
        {
            return _nameToManifest.GetValueOrDefault(name);
        }

        /// <summary>
        /// 读取清单信息
        /// </summary>
        public static Manifest LoadManifest(string manifestFilePath)
        {
            if (!SystemFile.Exists(manifestFilePath))
            {
                Logger.Error($"无法找到指定路径“{manifestFilePath}”下的清单文件，加载目标文件失败！");
                return null;
            }

            Manifest manifest;

            try
            {
                manifest = ScriptableObject.CreateInstance<Manifest>();
                JsonUtility.FromJsonOverwrite(ReadFileText(manifestFilePath), manifest);
                manifest.Reload();
            }
            catch (System.Exception e)
            {
                manifest = null;
                Logger.Exception(e);
                SystemFile.Delete(manifestFilePath);
            }

            return manifest;
        }

        /// <summary>
        /// 刷新全局的指定清单信息
        /// </summary>
        public static void RefreshGlobalManifest(Manifest manifest)
        {
            string manifestName = manifest.name;
            if (_nameToManifest.TryGetValue(manifestName, out Manifest m))
            {
                m.OverrideManifest(manifest);
                return;
            }

            _manifestList.Add(manifest);
            _nameToManifest.Add(manifestName, manifest);
        }

        /// <summary>
        /// 根据指定的清单版本信息判断清单是否已生效
        /// </summary>
        public static bool IsManifestEffective(ManifestVersion manifestVersion)
        {
            return _nameToManifest.TryGetValue(manifestVersion.Name, out Manifest m) && m.fileName == manifestVersion.FileName;
        }

        /// <summary>
        /// 根据指定的清单版本信息判断清单文件是否存在
        /// </summary>
        public static bool IsManifestFileExist(ManifestVersion manifestVersion)
        {
            SystemFileInfo info = new SystemFileInfo(AssetPath.TranslateToDownloadDataPath(manifestVersion.FileName));
            return info.Exists && info.Length == manifestVersion.Size && Utility.Format.ComputeHashFromFile(info.FullName) == manifestVersion.Hash;
        }

        /// <summary>
        /// 判断全部清单中是否包含某个资源
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        public static bool IsAssetContains(string assetPath)
        {
            foreach (var manifest in _manifestList)
            {
                if (manifest.IsAssetContains(assetPath))
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
            foreach (Manifest manifest in _manifestList)
            {
                if (manifest.IsAssetContains(assetPath))
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
            foreach (Manifest manifest in _manifestList)
            {
                if (manifest.IsAssetContains(assetPath))
                {
                    return manifest.GetBundleInfo(assetPath);
                }
            }

            return null;
        }
    }
}
