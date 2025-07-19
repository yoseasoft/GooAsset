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
using HooAsset.Editor.Build;
using System.Collections.Generic;

namespace HooAsset.Editor.Simulation
{
    /// <summary>
    /// 编辑器下清单初始化
    /// </summary>
    public class EditorInitManifestOperation : InitManifestOperation
    {
        /// <summary>
        /// 创建编辑器初始化清单操作方法
        /// </summary>
        public static EditorInitManifestOperation Create()
        {
            return new EditorInitManifestOperation();
        }

        protected override void OnStart()
        {
            foreach (ManifestConfig config in BuildUtils.GetAllManifestConfigs())
            {
                Manifest manifest = ScriptableObject.CreateInstance<Manifest>();
                manifest.name = config.name;
                foreach (Group group in config.groups)
                {
                    if (!group.IsNeedBuild)
                        continue;

                    foreach (BuildAssetInfo assetInfo in group.CollectAssets())
                    {
                        if (assetInfo.isExternalFile)
                        {
                            // 外部原始文件放入加载路径，提供给EditorRawFile加载
                            string loadPath = BuildUtils.GetExternalRawFileLoadPath(assetInfo.path, assetInfo.originalExternalPath, assetInfo.placeFolderName);
                            ManifestBundleInfo bundleInfo = new ManifestBundleInfo
                            {
                                Name = assetInfo.path,
                                AssetPathList = new List<string> { loadPath }
                            };
                            manifest.RecordAssetButUseEmptyBundleInfo(loadPath, bundleInfo);
                        }
                        else
                            manifest.RecordAssetButUseEmptyBundleInfo(assetInfo.path);
                    }
                }

                ManifestHandler.RefreshGlobalManifest(manifest);
            }

            Finish();
        }

        protected override void OnUpdate()
        {
            // do nothing
        }
    }
}
