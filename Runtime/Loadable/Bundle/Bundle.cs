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

using UnityEngine;

namespace HooAsset
{
    /// <summary>
    /// 资源包对象基类, 包含资源包的信息和持有一个其对应的ab包
    /// </summary>
    public class Bundle : Loadable
    {
        /// <summary>
        /// 包信息
        /// </summary>
        public ManifestBundleInfo bundleInfo;

        /// <summary>
        /// 资源包对应的ab包
        /// </summary>
        public AssetBundle assetBundle;

        /// <summary>
        /// ab包加载完成时由子类调用
        /// </summary>
        /// <param name="bundle"></param>
        protected void OnBundleLoaded(AssetBundle bundle)
        {
            assetBundle = bundle;
            Finish(assetBundle == null ? "ab包为空？？" : null);
            if (assetBundle)
                BundleHandler.AddAssetBundleRecord(bundleInfo.Name, assetBundle);
        }

        protected override void OnUnload()
        {
            BundleHandler.RemoveCache(bundleInfo.NameWithHash);

            if (!assetBundle)
                return;

            assetBundle.Unload(true);
            assetBundle = null;
            BundleHandler.RemoveAssetBundleRecord(bundleInfo.Name);
        }
    }
}
