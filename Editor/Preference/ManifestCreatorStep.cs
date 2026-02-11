/// -------------------------------------------------------------------------------
/// Copyright (C) 2023, Guangzhou Shiyue Network Technology Co., Ltd.
/// Copyright (C) 2025, Hainan Yuanyou Information Technology Co., Ltd. Guangzhou Branch
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
using System.Collections.Generic;
using System.IO;
using NovaFramework.Editor.Manifest;
using GooAsset.Editor.Build;
using UnityEditor;
using UnityEngine;

namespace NovaFramework.Editor.Preference
{
    /// <summary>
    /// GooAsset资源清单配置创建器，实现模块安装接口
    /// </summary>
    public class ManifestCreatorStep : InstallationStep
    {
        // 保存ManifestConfig为Asset
        string assetPath = "Assets/Editor/AssetBuildConfig/SampleManifestConfig.asset";
        public override void Install(System.Action onComplete = null)
        {
            Debug.Log("GooAssetManifestCreator: 开始创建资源清单配置");
            
            try
            {
                // 确保Resources目录存在
                string resourcesPath = Path.Combine(Application.dataPath, "Editor/AssetBuildConfig");
                if (!Directory.Exists(resourcesPath))
                {
                    Directory.CreateDirectory(resourcesPath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                // 直接创建ManifestConfig实例
                var manifestConfig = ScriptableObject.CreateInstance<ManifestConfig>();

                // 设置构建选项
                manifestConfig.buildAssetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression;

                // 从repo_manifest.xml获取配置路径
                var localPaths = RepoManifest.Instance.localPaths;
                var aotPathObject = localPaths.Find(path => path.name == "AOT_LIBRARY_PATH");
                var linkPathObject = localPaths.Find(path => path.name == "LINK_LIBRARY_PATH");
                
                
                string aotPath = aotPathObject?.defaultValue ?? "Assets/_Resources/Aot";
                string codePath = linkPathObject?.defaultValue ?? "Assets/_Resources/Code";
                string guiPath = "Assets/_Resources/Gui";
                string texturePath = "Assets/_Resources/Texture";
                
                // 创建默认资源组
                // 创建默认资源组
                UnityEngine.Object aotTarget = GetOrCreateAssetAtPath(aotPath);
                UnityEngine.Object codeTarget = GetOrCreateAssetAtPath(codePath);
                UnityEngine.Object guiTarget = GetOrCreateAssetAtPath(guiPath);
                UnityEngine.Object textureTarget = GetOrCreateAssetAtPath(texturePath);
                
                Group GUI = CreateGroup("GUI", "t:Prefab", "Gui", BundleMode.单独打包, guiTarget);
                Group AotGroup = CreateGroup("运行库", "t:TextAsset", "Aot",BundleMode.整组打包, aotTarget);
                Group CodeGroup = CreateGroup("代码", "t:TextAsset", "Code", BundleMode.单独打包, codeTarget);
                Group TextureGroup = CreateGroup("图片", "t:Texture", "Texture", BundleMode.按文件夹打包, textureTarget);
          

                // 将默认组添加到组列表
                manifestConfig.groups = new List<Group>() { AotGroup, CodeGroup,GUI, TextureGroup};

               
                AssetDatabase.CreateAsset(manifestConfig, assetPath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                Debug.Log($"已创建资源清单配置: {assetPath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"创建资源清单配置时出错: {ex.Message}");
                Debug.LogError($"堆栈跟踪: {ex.StackTrace}");
            }
            
            // 调用完成回调
            onComplete?.Invoke();
        }
        
        internal Group CreateGroup(string note, string filter, string bundleName, BundleMode bundleMode,UnityEngine.Object target)
        {
            var group = new Group();
            group.note = note;
            group.bundleMode = bundleMode;
            group.buildTargetList = new BuildTarget[] {};
            group.filter = filter;
            group.target = target;
            group.assetBundleFileName = bundleName;
            group.isNeedHandleDependencies = true;
            return group;
        }
        
        /// <summary>
        /// 确保路径目录存在并返回对应的Object引用
        /// </summary>
        /// <param name="path">资源路径</param>
        /// <returns>资源对象</returns>
        private UnityEngine.Object GetOrCreateAssetAtPath(string path)
        {
            // 确保目录存在
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
       
            return AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
        }

        public override void Uninstall(System.Action onComplete = null)
        {
            Debug.Log("GooAssetManifestCreator: 执行卸载操作");
            
            // 卸载逻辑（如果需要）
            // 例如：删除创建的资源清单配置文件
            AssetDatabase.DeleteAsset(assetPath);
            // 调用完成回调
            onComplete?.Invoke();
        }
    }
}