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

using GooAsset.Editor.Build;
using GooAsset.Editor.Simulation;

namespace GooAsset.Editor
{
    /// <summary>
    /// 编辑器资源加载设置初始化
    /// </summary>
    public static class EditorAssetLoadInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            AssetManagement.customGetOfflineModeFunc = GetOfflineMode;
            AssetPath.PlatformName = BuildUtils.GetActiveBuildPlatformName();

            AssetSettings settings = BuildUtils.GetOrCreateAssetSettings();
            Logger.DebuggingInformationPrintable = settings.isEnableLog;
            switch (settings.editorAssetLoadMode)
            {
                case EditorAssetLoadMode.使用资源目录原文件加载:
                    AssetHandler.CreateAssetFunc = EditorAsset.Create;
                    SceneHandler.CreateSceneFunc = EditorScene.Create;
                    RawFileHandler.CreateRawFileFunc = EditorRawFile.Create;
                    InitManifestOperation.CreateInitManifestOperationFunc = EditorInitManifestOperation.Create;
                    break;
                case EditorAssetLoadMode.使用打包目录的bundle加载:
                    // 将运行时读取的本地目录设置为打包目录
                    AssetPath.LocalDataPath = AssetPath.CombinePath(System.Environment.CurrentDirectory, BuildUtils.PlatformBuildPath);
                    RawFileHandler.CreateRawFileFunc = EditorPackedRawFile.Create;
                    break;
                case EditorAssetLoadMode.正式加载:
                    // 若StreamingAssets文件夹中不存在资源包, 则清空首包资源记录列表
                    if (!Directory.Exists(AssetPath.CombinePath(UnityEngine.Application.streamingAssetsPath, AssetPath.BuildPath)))
                        settings.buildInBundleFileNameList = null;
                    break;
            }
        }

        /// <summary>
        /// 获取离线模式
        /// </summary>
        static bool GetOfflineMode()
        {
            AssetSettings settings = BuildUtils.GetOrCreateAssetSettings();
            return settings.editorAssetLoadMode switch
            {
                EditorAssetLoadMode.使用资源目录原文件加载 => true,
                EditorAssetLoadMode.使用打包目录的bundle加载 => true,
                EditorAssetLoadMode.正式加载 => settings.offlineMode,
                _ => settings.offlineMode,
            };
        }
    }
}
