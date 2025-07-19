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
    /// 本地资源包对象
    /// </summary>
    public sealed class LocalBundle : Bundle
    {
        /// <summary>
        /// ab包加载请求
        /// </summary>
        AssetBundleCreateRequest _request;

        protected override void OnLoad()
        {
            _request = AssetBundle.LoadFromFileAsync(address, 0, (ulong)BundleHandler.BundleOffset);
        }

        protected override void OnLoadImmediately()
        {
            // 异步加载过程中(即request.isDone = false时)直接访问request.assetBundle, 会立即变成同步加载并返回加载的ab包
            // 文档:https://docs.unity.cn/cn/current/ScriptReference/AssetBundleCreateRequest-assetBundle.html
            OnBundleLoaded(_request.assetBundle);
            _request = null;
        }

        protected override void OnUpdate()
        {
            if (Status != LoadableStatus.Loading)
                return;

            Progress = _request.progress;

            if (_request.isDone)
            {
                OnBundleLoaded(_request.assetBundle);
                _request = null;
            }
        }
    }
}
