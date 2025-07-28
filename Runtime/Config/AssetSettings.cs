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
using Sirenix.OdinInspector;

namespace GooAsset
{
    /// <summary>
    /// 资源运行参数配置类
    /// </summary>
    // [CreateAssetMenu(menuName = "HooAsset/Create Asset Settings", fileName = "AssetSettings")] // (因每个工程只需要创建一个实例就足够, 创建后不再在菜单显示, 故屏蔽此行代码)
    public class AssetSettings : ScriptableObject
    {
#if UNITY_EDITOR
        [Title("配置(仅编辑器下使用)")]
        [LabelText("资源加载模式"), DrawWithUnity]
        public EditorAssetLoadMode editorAssetLoadMode;

        [LabelText("资源加载Log开关")]
        public bool isEnableLog;
#endif

        /// <summary>
        /// 离线模式
        /// </summary>
        [Title("配置")]
        [LabelText("离线模式"), InfoBox("离线模式字段仅在资源使用正式加载模式时生效, 其他情况默认为true")]
        public bool offlineMode;

        /// <summary>
        /// 内置资源包文件(包含已构建的原始文件)信息列表
        /// </summary>
        [LabelText("首包资源打包文件列表"), ReadOnly, ListDrawerSettings(DefaultExpandedState = true), Space(10)]
        public string[] buildInBundleFileNameList;
    }

#if UNITY_EDITOR
    /// <summary>
    /// 编辑器下的资源加载模式
    /// </summary>
    public enum EditorAssetLoadMode
    {
        使用资源目录原文件加载,

        使用打包目录的bundle加载,

        正式加载,
    }
#endif
}
