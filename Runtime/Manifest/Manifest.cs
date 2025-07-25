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
using UnityEngine;
using System.Collections.Generic;

namespace HooAsset
{
    /// <summary>
    /// 资源清单
    /// </summary>
    public class Manifest : ScriptableObject
    {
        /// <summary>
        /// 清单所在的文件名(带Hash,即保持唯一)
        /// </summary>
        internal string fileName;

        /// <summary>
        /// 资源包信息列表, 需public, 由Json覆盖写入
        /// </summary>
        public List<ManifestBundleInfo> manifestBundleInfoList = new();

        /// <summary>
        /// 资源包名字和资源包的对照字典
        /// </summary>
        Dictionary<string, ManifestBundleInfo> _nameToBundleInfo = new();

        /// <summary>
        /// 资源的真实路径和资源包的对照字典
        /// </summary>
        Dictionary<string, ManifestBundleInfo> _assetPathToBundleInfo = new();

        /// <summary>
        /// 根据新清单对象覆盖清单原有配置
        /// </summary>
        public void OverrideManifest(Manifest manifest)
        {
            manifestBundleInfoList = manifest.manifestBundleInfoList;
            _nameToBundleInfo = manifest._nameToBundleInfo;
            _assetPathToBundleInfo = manifest._assetPathToBundleInfo;
        }

        /// <summary>
        /// 重载字典记录
        /// </summary>
        public void Reload()
        {
            _nameToBundleInfo.Clear();
            _assetPathToBundleInfo.Clear();

            foreach (ManifestBundleInfo bundleInfo in manifestBundleInfoList)
            {
                _nameToBundleInfo[bundleInfo.Name] = bundleInfo;
                foreach (string path in bundleInfo.AssetPathList)
                {
                    _assetPathToBundleInfo[path] = bundleInfo;
                    AssetPath.RecordCustomLoadPath(path);
                }
            }
        }

        /// <summary>
        /// 判断此清单是否包含某个资源
        /// </summary>
        /// <param name="assetPath">资源路径</param>
        public bool IsAssetContains(string assetPath)
        {
            return _assetPathToBundleInfo.ContainsKey(assetPath);
        }

        /// <summary>
        /// 根据Bundle名称获取该包的信息
        /// </summary>
        /// <param name="bundleName">Bundle名字</param>
        public ManifestBundleInfo GetBundleInfoByBundleName(string bundleName)
        {
            return _nameToBundleInfo.GetValueOrDefault(bundleName);
        }

        /// <summary>
        /// 根据资源的真实路径获取资源所在的包的信息
        /// </summary>
        /// <param name="assetPath">资源的真实路径</param>
        public ManifestBundleInfo GetBundleInfo(string assetPath)
        {
            return _assetPathToBundleInfo.GetValueOrDefault(assetPath);
        }

        /// <summary>
        /// 空清单包信息列表
        /// </summary>
        static readonly ManifestBundleInfo[] _emptyBundleInfoList = Array.Empty<ManifestBundleInfo>();

        /// <summary>
        /// 根据清单包信息获取该包的依赖包信息列表
        /// </summary>
        public ManifestBundleInfo[] GetDependentBundleInfoList(ManifestBundleInfo bundle)
        {
            return bundle?.DependentBundleIdList == null ? _emptyBundleInfoList : Array.ConvertAll(bundle.DependentBundleIdList, index => manifestBundleInfoList[index]);
        }

#if UNITY_EDITOR
        /// <summary>
        /// 空清单包信息对象
        /// </summary>
        static readonly ManifestBundleInfo _emptyBundleInfo = new();

        /// <summary>
        /// 记录资源路径, 但使用空的清单包信息
        /// (主要在编辑器模拟运行时使用, 因加载资源时需判断IsAssetContains(assetPath), 所以此处记录后就可以通过检测)
        /// (为什么可以使用空的打包信息？ 因模拟运行时不会使用打包文件加载, 会直接使用AssetDatabase.LoadAssetAtPath()加载(可查看EditorAsset.cs), 所以不需要真正的打包信息)
        /// </summary>
        public void RecordAssetButUseEmptyBundleInfo(string assetPath, ManifestBundleInfo manifestBundleInfo = null)
        {
            _assetPathToBundleInfo[assetPath] = manifestBundleInfo ?? _emptyBundleInfo;
            AssetPath.RecordCustomLoadPath(assetPath);
        }
#endif
    }
}
