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

namespace HooAsset
{
    /// <summary>
    /// 资源清单文件信息
    /// </summary>
    [System.Serializable]
    public class ManifestVersion
    {
        #region json存储和写入字段

        /// <summary>
        /// 清单名字
        /// </summary>
        public string n;

        /// <summary>
        /// 清单文件的文件名
        /// </summary>
        public string f;

        /// <summary>
        /// 清单文件的大小(单位:字节(B))
        /// </summary>
        public long s;

        /// <summary>
        /// 清单文件的Hash
        /// </summary>
        public string h;

        #endregion

        #region 代码使用属性, 方便维护

        /// <summary>
        /// 清单名字
        /// </summary>
        public string Name
        {
            get => n;
            set => n = value;
        }

        /// <summary>
        /// 清单文件的文件名(带Hash,即保持唯一)
        /// </summary>
        public string FileName
        {
            get => f;
            set => f = value;
        }

        /// <summary>
        /// 清单文件的大小(单位:字节(B))
        /// </summary>
        public long Size
        {
            get => s;
            set => s = value;
        }

        /// <summary>
        /// 清单文件的Hash
        /// </summary>
        public string Hash
        {
            get => h;
            set => h = value;
        }

        #endregion

        /// <summary>
        /// 覆盖
        /// </summary>
        public void Overwrite(ManifestVersion newManifestVersion)
        {
            Name = newManifestVersion.Name;
            FileName = newManifestVersion.FileName;
            Size = newManifestVersion.Size;
            Hash = newManifestVersion.Hash;
        }
    }
}
