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
using UnityEditor;
using HooAsset.Editor.GUI;
using HooAsset.Editor.Build;

namespace HooAsset.Editor
{
    /// <summary>
    /// Unity菜单选项
    /// </summary>
    public static class AssetManagementMenuItems
    {
        /*
        [MenuItem("资源管理/构建资源包", false, 0)]
        static void BuildAllManifests()
        {
            BuildExecuter.StartBuild();
        }

        [MenuItem("资源管理/首包资源环境配置", false, 1)]
        static void ArrangeBuildInFilesEnvironment()
        {
            BuildExecuter.ArrangeBuildInFilesEnvironment();
        }

        [MenuItem("资源管理/手动修改打包版本", false, 2)]
        static void ChangeBuildVersion()
        {
            BuildExecuter.ChangeManifestVersionContainerVersion();
        }

        [MenuItem("资源管理/清理过期文件", false, 3)]
        static void ClearHistoryFiles()
        {
            BuildExecuter.ClearHistoryFiles();
        }

        [MenuItem("资源管理/资源打包分析", false, 101)]
        static void OpenBuildAnalyzerWindow()
        {
            BuildAnalyzerWindow.Open();
        }

        [MenuItem("资源管理/资源运行分析", false, 102)]
        static void OpenRuntimeAnalyzerWindow()
        {
            RuntimeAnalyzerWindow.Open();
        }

        [MenuItem("资源管理/查看版本变更", false, 103)]
        static void OpenVersionContrastWindow()
        {
            VersionContrastWindow.Open();
        }

        [MenuItem("资源管理/多清单资源引用检查", false, 104)]
        static void StartAssetsInMultiManifestCheck()
        {
            BuildExecuter.StartAssetsInMultiManifestCheck();
        }

        [MenuItem("资源管理/多清单资源引用检查", true, 104)]
        static bool IsNeedAssetsInMultiManifestCheck()
        {
            return BuildUtils.GetAllManifestConfigs().Count > 1;
        }

        [MenuItem("资源管理/选中资源清单配置", false, 201)]
        static void SelectManifestConfig()
        {
            BuildUtils.SelectManifestConfig();
        }

        [MenuItem("资源管理/打开打包目录", false, 301)]
        static void OpenBuildFolder()
        {
            EditorUtility.OpenWithDefaultApp(BuildUtils.PlatformBuildPath);
        }

        [MenuItem("资源管理/版本上传目录", false, 302)]
        static void OpenVersionFileUploadFolder()
        {
            EditorUtility.OpenWithDefaultApp(BuildUtils.PlatformUploadVersionFilePath);
        }

        [MenuItem("资源管理/资源上传目录", false, 303)]
        static void OpenBundleUploadFolder()
        {
            EditorUtility.OpenWithDefaultApp(BuildUtils.PlatformUploadBundlePath);
        }

        [MenuItem("资源管理/打开下载目录", false, 304)]
        static void OpenDownloadFolder()
        {
            EditorUtility.OpenWithDefaultApp(Application.persistentDataPath);
        }

        [MenuItem("资源管理/打开临时目录", false, 305)]
        static void OpenTempFolder()
        {
            EditorUtility.OpenWithDefaultApp(Application.temporaryCachePath);
        }
        */
    }
}
