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

namespace GooAsset.Editor.GUI
{
    /// <summary>
    /// 编辑器UI垂直分割工具类
    /// </summary>
    public class VerticalSplitter
    {
        /// <summary>
        /// 显示区域
        /// </summary>
        public Rect rect;

        /// <summary>
        /// 提供给拖动大小的可操作高度
        /// </summary>
        const int OperateSize = 5;

        /// <summary>
        /// 默认预留原有区域的百分比
        /// </summary>
        float _percent = 0.8f;

        /// <summary>
        /// 最小高度限制
        /// </summary>
        public float MinHeight { get; set; }

        /// <summary>
        /// 是否拖动中
        /// </summary>
        public bool IsResizing { get; private set; }

        /// <summary>
        /// 处理显示区域, 传入需分割的原区域的Rect
        /// </summary>
        public void OnGUI(Rect position)
        {
            rect.y = (int)(position.yMin + position.height * _percent);
            rect.width = position.width;
            rect.height = OperateSize;
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.ResizeVertical);

            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
                IsResizing = true;

            if (IsResizing)
            {
                float mousePosInRect = Event.current.mousePosition.y - position.yMin;
                _percent = Mathf.Clamp(mousePosInRect / position.height, 0.2f, 0.9f);
                rect.y = (int)(position.yMin + position.height * _percent);

                // 检查最小高度
                if (rect.y > position.yMax - MinHeight)
                {
                    rect.y = position.yMax - MinHeight;
                    _percent = Mathf.Clamp((float)(rect.y - position.yMin) / position.height, 0.2f, 0.9f);
                    rect.y = (int)(position.yMin + position.height * _percent);
                }

                if (Event.current.type == EventType.MouseUp || !position.Contains(Event.current.mousePosition))
                    IsResizing = false;
            }
            else
                _percent = Mathf.Clamp(_percent, 0.2f, 0.9f);
        }
    }
}
