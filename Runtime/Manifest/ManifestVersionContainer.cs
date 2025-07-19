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
using System.Collections.Generic;

namespace HooAsset
{
    /// <summary>
    /// 包含所有资源清单的版本信息的容器
    /// </summary>
    public class ManifestVersionContainer : ScriptableObject
    {
        #region json存储和写入字段

        /// <summary>
        /// 版本号, 仅文件名使用
        /// </summary>
        public int v;

        /// <summary>
        /// 构建时的时间戳
        /// </summary>
        public long t;

        /// <summary>
        /// 所有资源清单的版本信息
        /// </summary>
        public List<ManifestVersion> a = new();

        #endregion

        #region 代码使用属性, 方便维护

        /// <summary>
        /// 版本号, 仅文件名使用
        /// </summary>
        public int Version
        {
            get => v;
            set => v = value;
        }

        /// <summary>
        /// 构建时的时间戳
        /// </summary>
        public long Timestamp
        {
            get => t;
            set => t = value;
        }

        /// <summary>
        /// 所有资源清单的版本信息
        /// </summary>
        public List<ManifestVersion> AllManifestVersions => a;

        #endregion
    }
}
