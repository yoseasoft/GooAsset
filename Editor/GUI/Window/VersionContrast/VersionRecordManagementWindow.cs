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
    /// 版本记录管理窗口
    /// </summary>
    public class VersionRecordManagementWindow : EditorWindow
    {
        /// <summary>
        /// 打开此窗口
        /// </summary>
        public static void Open()
        {
            GetWindow<VersionRecordManagementWindow>("版本记录管理").minSize = new Vector2(460, 680);
        }

        /// <summary>
        /// 提示高度
        /// </summary>
        const float TipsHeight = 20;

        /// <summary>
        /// 提示图片和文本
        /// </summary>
        GUIContent _tipsContent;

        /// <summary>
        /// 提示的文本样式
        /// </summary>
        GUIStyle _tipsStyle;

        /// <summary>
        /// 备注列表
        /// </summary>
        VersionRecordManagementTreeView _recordCommentTreeView;

        void OnGUI()
        {
            // 提示
            if (_tipsContent == null)
            {
                _tipsStyle = new GUIStyle(UnityEngine.GUI.skin.label) { fontSize = 11 };
                GUIContent consoleInfoIconContent = EditorGUIUtility.IconContent("d_console.infoicon.sml");
                _tipsContent = new GUIContent("温馨提示:1.关闭窗口即可保存备注; 2.在对应版本右键可弹出删除按钮(可多选)", consoleInfoIconContent.image);
            }

            GUILayout.Label(_tipsContent, _tipsStyle, GUILayout.Height(TipsHeight));

            // 列表
            Rect treeViewRect = new Rect(0, TipsHeight, position.width, position.height - TipsHeight);
            _recordCommentTreeView ??= new VersionRecordManagementTreeView();
            _recordCommentTreeView.OnGUI(treeViewRect);
        }

        void OnDestroy()
        {
            _recordCommentTreeView?.SaveComment();
            if (HasOpenInstances<VersionContrastWindow>())
                GetWindow<VersionContrastWindow>().RefreshVersionFileNameList();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void Refresh()
        {
            _recordCommentTreeView?.RefreshCommentList();
        }
    }
}
