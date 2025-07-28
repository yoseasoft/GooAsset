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
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace GooAsset.Editor.Build
{
    /// <summary>
    /// 清单配置
    /// </summary>
    // [CreateAssetMenu(menuName = "资源管理/资源清单", fileName = "NewManifest")] // (因目前只创建两个就足够, 创建后不再在菜单显示, 故屏蔽此行代码)
    public class ManifestConfig : ScriptableObject
    {
        /// <summary>
        /// AssetBundle构建选项，主要用于设置压缩算法、加密保护等参数
        /// </summary>
        [LabelText("构建选项"), DrawWithUnity]
        public BuildAssetBundleOptions buildAssetBundleOptions = BuildAssetBundleOptions.ChunkBasedCompression;

        /// <summary>
        /// 资源组列表
        /// </summary>
        [LabelText("打包资源组列表"), ListDrawerSettings(DefaultExpandedState = true, OnBeginListElementGUI = "ElementTitle")]
        public List<Group> groups = new();

        void ElementTitle(int index)
        {
            EditorGUILayout.LabelField(groups[index].note, EditorStyles.boldLabel);
        }
    }
}
