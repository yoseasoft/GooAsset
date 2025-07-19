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

namespace HooAsset.Editor.Build
{
    /// <summary>
    /// 参与打包的资源信息对象
    /// </summary>
    [Serializable]
    public class BuildAssetInfo
    {
        /// <summary>
        /// 资源打包时的真实路径
        /// </summary>
        public string path;

        /// <summary>
        /// 被分配到的打包文件名字(由BuildUtils.GetAssetPackedBundleName()赋值)
        /// 1.若是ab包则是ab包文件名字(此处不带hash, 故还不是最终文件名字),
        /// 2.若是原始文件则是打包复制后的最终文件名字
        /// </summary>
        public string buildFileName;

        #region 原始文件扩展字段

        /// <summary>
        /// 是否外部目录文件(仅原始文件使用)
        /// </summary>
        public bool isExternalFile;

        /// <summary>
        /// 原始外部目录
        /// </summary>
        public string originalExternalPath;

        /// <summary>
        /// 资源存放的文件夹名(仅原始文件使用)
        /// </summary>
        public string placeFolderName;

        #endregion
    }
}
