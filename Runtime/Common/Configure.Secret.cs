/// -------------------------------------------------------------------------------
/// GooAsset Framework
///
/// Copyright (C) 2025, Hainan Yuanyou Information Tecdhnology Co., Ltd. Guangzhou Branch
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

namespace GooAsset
{
    /// <summary>
    /// 资源模块的配置参数管理类
    /// </summary>
    public static partial class Configure
    {
        /// <summary>
        /// 资源秘钥管理定义类
        /// </summary>
        public static class Secret
        {
            /// <summary>
            /// 是否加密清单文件和版本文件并隐藏真实的文件名
            /// </summary>
            public static readonly bool ManifestFileEncryptEnabled = false;

            internal const string Gd4H = "diKj530tJ6gzqRhAsMIu5YbxPee8H4dg";

            internal const string ZNfR = "xyN8sJI7IMRfNzD5";
        }
    }
}
