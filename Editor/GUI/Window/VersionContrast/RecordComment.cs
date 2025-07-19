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

using System;
using System.IO;
using UnityEngine;
using HooAsset.Editor.Build;

namespace HooAsset.Editor.GUI
{
    /// <summary>
    /// 版本备注数据
    /// </summary>
    [Serializable]
    public class RecordComment
    {
        /// <summary>
        /// 已备注的版本文件名列表
        /// </summary>
        public string[] recordFileNameList;

        /// <summary>
        /// 对应的备注列表
        /// </summary>
        public string[] recordCommentList;

        /// <summary>
        /// 版本备注数据记录文件名
        /// </summary>
        public const string RecordCommentFileName = "recordComment.json";

        /// <summary>
        /// 加载备注数据
        /// </summary>
        public static RecordComment LoadRecordComment()
        {
            string recordCommentFilePath = BuildUtils.TranslateToBuildPath(AssetPath.CombinePath(BuildUtils.BuildRecordFolderName, RecordCommentFileName));
            if (!File.Exists(recordCommentFilePath))
                return null;

            RecordComment recordComment = JsonUtility.FromJson<RecordComment>(File.ReadAllText(recordCommentFilePath));
            return recordComment;
        }
    }
}
