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

namespace HooAsset.Editor.GUI
{
    /// <summary>
    /// 资源运行分析窗口
    /// </summary>
    public class RuntimeAnalyzerWindow : EditorWindow
    {
        /// <summary>
        /// 打开此窗口
        /// </summary>
        public static void Open()
        {
            GetWindow<RuntimeAnalyzerWindow>("资源运行分析").minSize = new Vector2(800, 500);
        }

        /// <summary>
        /// 工具栏高度
        /// </summary>
        const int ToolBarHeight = 20;

        /// <summary>
        /// 资源列表
        /// </summary>
        RuntimeAnalyzerAssetTreeView _assetTreeView;

        /// <summary>
        /// 是否需要自动刷新
        /// </summary>
        bool _needAutoRefresh = true;

        void OnGUI()
        {
            _assetTreeView ??= new RuntimeAnalyzerAssetTreeView();

            Rect toolBarRect = new Rect(0, 0, position.width, ToolBarHeight);
            DrawToolBar(toolBarRect);
            _assetTreeView.OnGUI(new Rect(0, ToolBarHeight + 2, position.width, position.height - ToolBarHeight - 2));
        }

        void Update()
        {
            _assetTreeView?.Update();
        }

        /// <summary>
        /// 绘制最上方的工具栏
        /// </summary>
        void DrawToolBar(Rect toolBarRect)
        {
            GUILayout.BeginArea(toolBarRect, EditorStyles.toolbar);
            {
                GUILayout.BeginHorizontal();
                {
                    // 自动刷新开关位置
                    Rect autoRefreshBtnRect = new Rect(0, 0, 100, toolBarRect.height);
                    _needAutoRefresh = UnityEngine.GUI.Toggle(autoRefreshBtnRect, _needAutoRefresh, "自动刷新", EditorStyles.toolbarButton);
                    _assetTreeView.needAutoRefresh = _needAutoRefresh;
                    if (!_needAutoRefresh)
                    {
                        // 刷新按钮位置
                        Rect refreshBtnRect = new Rect(autoRefreshBtnRect.x + autoRefreshBtnRect.width, 0, 50, toolBarRect.height);
                        if (UnityEngine.GUI.Button(refreshBtnRect, "刷新", EditorStyles.toolbarButton))
                        {
                            if (UnityEngine.Application.isPlaying)
                                _assetTreeView.RefreshAssetList();
                            else
                                ShowNotification(new GUIContent("请在游戏运行中刷新资源列表"));
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndArea();
        }
    }
}
