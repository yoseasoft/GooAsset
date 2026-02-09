using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using NovaFramework.Editor.Manifest;
using GooAsset.Editor.Build;
using NovaFramework.Editor.Preference;
using UnityEditor;
using UnityEngine;

namespace GooAsset.Editor.Install
{
    /// <summary>
    /// GooAsset资源清单配置创建器，实现模块安装接口
    /// </summary>
    public class GooAssetManifestCreator : InstallationStep
    {
        public override void Install(System.Action onComplete = null)
        {
            Debug.Log("GooAssetManifestCreator: 开始创建资源清单配置");
            
            try
            {
                // 确保Resources目录存在
                string resourcesPath = Path.Combine(Application.dataPath, "Resources");
                if (!Directory.Exists(resourcesPath))
                {
                    Directory.CreateDirectory(resourcesPath);
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
                
                // 创建默认资源组
                UnityEngine.Object aotTarget = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(aotPath);
                UnityEngine.Object codeTarget = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(codePath);
                Group AotGroup = CreateGroup("AOT", "t:TextAsset", "AOT", aotTarget);
                Group CodeGroup = CreateGroup("Code", "t:TextAsset", "Code", codeTarget);
          

                // 将默认组添加到组列表
                manifestConfig.groups = new List<Group>() { AotGroup, CodeGroup };

                // 确保Resources/GooAsset目录存在
                string gooAssetPath = resourcesPath;
                if (!Directory.Exists(gooAssetPath))
                {
                    Directory.CreateDirectory(gooAssetPath);
                }

                // 保存ManifestConfig为Asset
                string assetPath = "Assets/Resources/SampleManifestConfig.asset";
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
        
        internal Group CreateGroup(string note, string filter, string bundleName, UnityEngine.Object target)
        {
            var group = new Group();
            group.note = note;
            group.bundleMode = BundleMode.整组打包;
            group.buildTargetList = new BuildTarget[] {BuildTarget.StandaloneWindows64 };
            group.filter = filter;
            group.target = target;
            group.assetBundleFileName = bundleName;
            group.isNeedHandleDependencies = true;
            return group;
        }

        public override void Uninstall(System.Action onComplete = null)
        {
            Debug.Log("GooAssetManifestCreator: 执行卸载操作");
            
            // 卸载逻辑（如果需要）
            // 例如：删除创建的资源清单配置文件
            
            // 调用完成回调
            onComplete?.Invoke();
        }
    }
}