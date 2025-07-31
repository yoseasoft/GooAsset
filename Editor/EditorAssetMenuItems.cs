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
using UnityEditor;
using GooAsset.Editor.GUI;
using GooAsset.Editor.Build;

namespace GooAsset.Editor
{
    /// <summary>
    /// Unity菜单选项
    /// </summary>
    public static class EditorAssetMenuItems
    {
        [MenuItem("GooAsset/构建资源包", priority = 10)]
        static void BuildAllManifests()
        {
            BuildExecuter.StartBuild();
        }

        [MenuItem("GooAsset/首包资源环境配置", priority = 11)]
        static void ArrangeBuildInFilesEnvironment()
        {
            BuildExecuter.ArrangeBuildInFilesEnvironment();
        }

        [MenuItem("GooAsset/手动修改打包版本", priority = 12)]
        static void ChangeBuildVersion()
        {
            BuildExecuter.ChangeManifestVersionContainerVersion();
        }

        [MenuItem("GooAsset/清理过期文件", priority = 13)]
        static void ClearHistoryFiles()
        {
            BuildExecuter.ClearHistoryFiles();
        }

        [MenuItem("GooAsset/资源打包分析", priority = 101)]
        static void OpenBuildAnalyzerWindow()
        {
            BuildAnalyzerWindow.Open();
        }

        [MenuItem("GooAsset/资源运行分析", priority = 102)]
        static void OpenRuntimeAnalyzerWindow()
        {
            RuntimeAnalyzerWindow.Open();
        }

        [MenuItem("GooAsset/查看版本变更", priority = 103)]
        static void OpenVersionContrastWindow()
        {
            VersionContrastWindow.Open();
        }

        [MenuItem("GooAsset/多清单资源引用检查", priority = 104)]
        static void StartAssetsInMultiManifestCheck()
        {
            BuildExecuter.StartAssetsInMultiManifestCheck();
        }

        [MenuItem("GooAsset/多清单资源引用检查", true, 104)]
        static bool IsNeedAssetsInMultiManifestCheck()
        {
            return BuildUtils.GetAllManifestConfigs().Count > 1;
        }

        [MenuItem("GooAsset/选中资源清单配置", priority = 201)]
        static void SelectManifestConfig()
        {
            BuildUtils.SelectManifestConfig();
        }

        [MenuItem("GooAsset/打开打包目录", priority = 301)]
        static void OpenBuildFolder()
        {
            EditorUtility.OpenWithDefaultApp(BuildUtils.PlatformBuildPath);
        }

        [MenuItem("GooAsset/版本上传目录", priority = 302)]
        static void OpenVersionFileUploadFolder()
        {
            EditorUtility.OpenWithDefaultApp(BuildUtils.PlatformUploadVersionFilePath);
        }

        [MenuItem("GooAsset/资源上传目录", priority = 303)]
        static void OpenBundleUploadFolder()
        {
            EditorUtility.OpenWithDefaultApp(BuildUtils.PlatformUploadBundlePath);
        }

        [MenuItem("GooAsset/打开下载目录", priority = 304)]
        static void OpenDownloadFolder()
        {
            EditorUtility.OpenWithDefaultApp(Application.persistentDataPath);
        }

        [MenuItem("GooAsset/打开临时目录", priority = 305)]
        static void OpenTempFolder()
        {
            EditorUtility.OpenWithDefaultApp(Application.temporaryCachePath);
        }
    }
}
