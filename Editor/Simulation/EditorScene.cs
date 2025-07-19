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

using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace HooAsset.Editor.Simulation
{
    /// <summary>
    /// 编辑器下场景加载
    /// </summary>
    public class EditorScene : Scene
    {
        /// <summary>
        /// 依赖
        /// </summary>
        EditorDependency _dependency;

        /// <summary>
        /// 创建EditorScene
        /// </summary>
        internal static EditorScene Create(string assetPath, bool isAdditive)
        {
            if (!File.Exists(assetPath))
            {
                Debug.LogError($"场景文件不存在:{assetPath}");
                return null;
            }

            if (!ManifestHandler.IsContainAsset(assetPath))
            {
                Debug.LogError($"场景加载失败, 因所有资源清单中都不存在此资源:{assetPath}");
                return null;
            }

            LoadSceneMode loadSceneMode = isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            return new EditorScene { address = assetPath, loadSceneMode = loadSceneMode };
        }

        protected override void OnLoad()
        {
            _dependency = new EditorDependency { address = address };
            _dependency.Load();

            LoadSceneParameters loadSceneParameters = new LoadSceneParameters { loadSceneMode = loadSceneMode };
            if (isSyncLoad)
            {
                _dependency.LoadImmediately();
                EditorSceneManager.LoadSceneInPlayMode(address, loadSceneParameters);
                Finish();
            }
            else
                asyncOperation = EditorSceneManager.LoadSceneAsyncInPlayMode(address, loadSceneParameters);
        }

        protected override void OnUpdate()
        {
            if (Status != LoadableStatus.Loading)
                return;

            Progress = asyncOperation.allowSceneActivation ? asyncOperation.progress : asyncOperation.progress / 0.9f;

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

            if (!_dependency.IsDone)
                return;

            Finish();
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            _dependency.Release();
            _dependency = null;
        }
    }
}
