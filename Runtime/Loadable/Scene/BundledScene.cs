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

using UnityEngine.SceneManagement;

namespace HooAsset
{
    /// <summary>
    /// ab包里的场景加载
    /// </summary>
    public sealed class BundledScene : Scene
    {
        /// <summary>
        /// ab包相关依赖
        /// </summary>
        Dependency _dependency;

        /// <summary>
        /// 场景依赖包加载占比, ab包加载速度较快, 所以占个30%
        /// </summary>
        const float DependencyLoadProportion = 0.3f;

        protected override void OnLoad()
        {
            _dependency = DependencyHandler.LoadAsync(address);
            if (isSyncLoad)
            {
                _dependency.LoadImmediately();
                SceneManager.LoadScene(sceneName, loadSceneMode);
                Finish();
            }
            else
                Status = LoadableStatus.DependentLoading;
        }

        protected override void OnUpdate()
        {
            switch (Status)
            {
                case LoadableStatus.DependentLoading:
                    OnDependentLoadingUpdate();
                    break;
                case LoadableStatus.Loading:
                    OnLoadingUpdate();
                    break;
            }
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _dependency?.Release();
            _dependency = null;
        }

        /// <summary>
        /// 依赖资源包加载中Update处理
        /// </summary>
        void OnDependentLoadingUpdate()
        {
            Progress = DependencyLoadProportion * _dependency.Progress;

            if (!_dependency.IsDone)
                return;

            if (!_dependency.AssetBundle)
            {
                Finish($"加载场景时, 其ab包加载失败, address:{address}");
                return;
            }

            asyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            Status = LoadableStatus.Loading;
        }

        /// <summary>
        /// 加载Update处理
        /// </summary>
        void OnLoadingUpdate()
        {
            float loadProgress = asyncOperation.allowSceneActivation ? asyncOperation.progress : asyncOperation.progress / 0.9f;
            Progress = DependencyLoadProportion + (1 - DependencyLoadProportion) * loadProgress;

            if (asyncOperation.allowSceneActivation)
            {
                if (!asyncOperation.isDone)
                    return;
            }
            else
            {
                // 不允许场景自动激活时会停在0.9(但实际已完成加载), 直至设置allowSceneActivation = true
                // 文档:https://docs.unity3d.com/ScriptReference/AsyncOperation-allowSceneActivation.html
                if (asyncOperation.progress < 0.9f)
                    return;
            }

            Finish();
        }
    }
}
