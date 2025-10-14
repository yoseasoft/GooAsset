/// -------------------------------------------------------------------------------
/// GooAsset Framework
///
/// Copyright (C) 2025, Hainan Yuanyou Information Technology Co., Ltd. Guangzhou Branch
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

            /**
             * 密钥（Key）和初始化向量（IV）是确保加密安全性的两个核心要素。
             * 两者需配合使用：密钥保证加密解密一致性，IV增强加密安全性。
             * 在CBC模式下，IV与首个明文块结合生成初始密文，后续明文块则与前一个密文块异或运算后再加密。
             */

            /// <summary>
            /// 密钥（Key）<br/>
            /// 用于加密和解密过程，双方需共享同一密钥。<br/>
            /// 支持128位（16字节）、192位（24字节）和256位（32字节）三种长度，根据安全需求选择。<br/>
            /// 密钥必须严格保密，泄露会导致加密数据被破解。
            /// </summary>
            internal const string Gd4H = "diKj530tJ6gzqRhAsMIu5YbxPee8H4dg";

            /// <summary>
            /// 初始化向量（IV）<br/>
            /// 与密钥结合增强加密随机性，避免相同明文重复加密产生相同密文。<br/>
            /// 通常为128位（16字节），与AES数据块长度一致。<br/>
            /// 初始化向量必须随机生成且不可预测，否则易受重放攻击和统计分析攻击。
            /// </summary>
            internal const string ZNfR = "xyN8sJI7IMRfNzD5";
        }
    }
}
