/// -------------------------------------------------------------------------------
/// HooAsset Framework
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

namespace HooAsset
{
    /// <summary>
    /// 资源模块的配置参数管理类
    /// </summary>
    public static partial class Configure
    {
        /// <summary>
        /// 资源文件及目录定义类
        /// </summary>
        public static class File
        {
            #region 版本文件相关参数及接口定义

            /// <summary>
            /// 默认版本文件名字
            /// </summary>
            public const string DefaultVersionFileName = "version";
            /// <summary>
            /// 默认白名单版本文件名字
            /// </summary>
            public const string DefaultWhiteListVersionFileName = "white_list_version";
            /// <summary>
            /// 加密版本文件名字
            /// </summary>
            public const string EncryptVersionFileName = "d4a27b33a023fdabc304433a38a64e11";
            /// <summary>
            /// 加密白名单版本文件名字
            /// </summary>
            public const string EncryptWhiteListVersionFileName = "33a38a6422e11d4a27b33a023fdabc28";

            /// <summary>
            /// 默认版本文件的扩展名
            /// </summary>
            public const string DefaultVersionFileExtensionName = ".json";
            /// <summary>
            /// 加密版本文件的扩展名
            /// </summary>
            public const string EncryptVersionFileExtensionName = ".unity3d";

            /// <summary>
            /// 获取版本文件名称
            /// </summary>
            /// <param name="version">版本号</param>
            /// <returns>返回版本文件名称</returns>
            public static string GetVersionFileName(int version = 0)
            {
                if (version > 0)
                {
                    if (IsEncryptManifestFile)
                    {
                        return $"{Utility.Format.ComputeHashFromString($"{EncryptVersionFileName}_v{version}")}{EncryptVersionFileExtensionName}";
                    }
                    else
                    {
                        return $"{DefaultVersionFileName}_v{version}{DefaultVersionFileExtensionName}";
                    }
                }
                else
                {
                    if (IsEncryptManifestFile)
                    {
                        return $"{EncryptVersionFileName}{EncryptVersionFileExtensionName}";
                    }
                    else
                    {
                        return $"{DefaultVersionFileName}{DefaultVersionFileExtensionName}";
                    }
                }
            }

            /// <summary>
            /// 获取白名单版本文件名称
            /// </summary>
            /// <returns>返回白名单版本文件名称</returns>
            public static string GetWhiteListVersionFileName()
            {
                if (IsEncryptManifestFile)
                {
                    return $"{EncryptWhiteListVersionFileName}{EncryptVersionFileExtensionName}";
                }
                else
                {
                    return $"{DefaultWhiteListVersionFileName}{DefaultVersionFileExtensionName}";
                }
            }

            #endregion

            /// <summary>
            /// 打包目录
            /// </summary>
            public const string BuildPath = "Bundles";
        }
    }
}
