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

using UnityEngine;
using System.Collections.Generic;

namespace GooAsset
{
    /// <summary>
    /// 资源依赖加载
    /// </summary>
    public sealed class Dependency : Loadable
    {
        /// <summary>
        /// 资源所在的ab包
        /// </summary>
        public AssetBundle AssetBundle => _bundleList.Count > 0 ? _bundleList[0].assetBundle : null;

        /// <summary>
        /// 资源包对象列表(包含其所在的资源包和所有依赖的资源包);
        /// 规定第一个(index = 0)为主资源包(即资源本身所在的资源包)
        /// </summary>
        readonly List<Bundle> _bundleList = new();

        /// <summary>
        /// 需加载的资源包数量
        /// </summary>
        internal int BundleCount => _bundleList.Count;

        protected override void OnLoad()
        {
            if (!ManifestHandler.GetMainBundleInfoAndDependencies(address, out var bundleInfo, out var dependentBundleInfoList))
            {
                Finish("清单中没有此资源");
                return;
            }

            if (bundleInfo == null)
            {
                Finish("获取资源包信息失败");
                return;
            }

            _bundleList.Add(BundleHandler.LoadAsync(bundleInfo));

            if (dependentBundleInfoList is { Length: > 0 })
                foreach (ManifestBundleInfo dependentBundleInfo in dependentBundleInfoList)
                    _bundleList.Add(BundleHandler.LoadAsync(dependentBundleInfo));
        }

        protected override void OnLoadImmediately()
        {
            foreach (Bundle bundle in _bundleList)
                bundle.LoadImmediately();
        }

        protected override void OnUpdate()
        {
            if (Status != LoadableStatus.Loading)
                return;

            bool allDone = true;
            float totalProgress = 0;

            foreach (Bundle bundle in _bundleList)
            {
                totalProgress += bundle.Progress;

                if (!bundle.IsDone)
                    allDone = false;
                else if (bundle.HasError)
                {
                    Finish(bundle.Error);
                    return;
                }
            }

            if (allDone)
            {
                Finish(AssetBundle ? null : "依赖加载失败");
                return;
            }

            Progress = totalProgress / _bundleList.Count;
        }

        protected override void OnUnload()
        {
            DependencyHandler.RemoveCache(address);

            if (_bundleList.Count == 0)
                return;

            foreach (Bundle bundle in _bundleList)
                bundle.Release();

            _bundleList.Clear();
        }
    }
}
