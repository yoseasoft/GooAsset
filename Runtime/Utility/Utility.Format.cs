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

using System.Collections.Generic;

using SystemStringBuilder = System.Text.StringBuilder;
using SystemEncoding = System.Text.Encoding;
using SystemFile = System.IO.File;
using SystemFileStream = System.IO.FileStream;
using SystemMD5 = System.Security.Cryptography.MD5;

namespace GooAsset
{
    /// <summary>
    /// 资源模块的辅助工具类，整合所有常用的工具函数
    /// </summary>
    public static partial class Utility
    {
        /// <summary>
        /// 数据格式化的辅助接口函数
        /// </summary>
        public static class Format
        {
            /// <summary>
            /// 每个数据单位对应的字节数
            /// </summary>
            static readonly long[] _byteUnits =
            {
                1073741824, 1048576, 1024, 1
            };

            /// <summary>
            /// 每个数据单位的名字
            /// </summary>
            static readonly string[] _byteUnitsNames =
            {
                "GB", "MB", "KB", "B"
            };

            /// <summary>
            /// 格式化成数据大小形式(GB, MB, KB, B)显示
            /// </summary>
            public static string FormatBytes(long bytes)
            {
                string size = "0 B";
                if (bytes == 0)
                    return size;

                for (var i = 0; i < _byteUnits.Length; i++)
                {
                    long unit = _byteUnits[i];
                    if (bytes >= unit)
                    {
                        size = $"{(double) bytes / unit:0.00} {_byteUnitsNames[i]}";
                        break;
                    }
                }

                return size;
            }

            /// <summary>
            /// 转换为标准Hash格式的字符串
            /// 这里用的ToString("X2")为C#中的字符串格式控制符(X为十六进制, 2为每次都是两位数)
            /// </summary>
            /// <param name="data">字节流</param>
            static string ToHash(IEnumerable<byte> data)
            {
                SystemStringBuilder sb = new SystemStringBuilder();
                foreach (byte b in data)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }

            /// <summary>
            /// 计算文件流的Hash值
            /// </summary>
            public static string ComputeHashFromFile(SystemFileStream stream)
            {
                return ToHash(SystemMD5.Create().ComputeHash(stream));
            }

            /// <summary>
            /// 计算指定文件的Hash值
            /// </summary>
            public static string ComputeHashFromFile(string filePath)
            {
                if (!SystemFile.Exists(filePath))
                    return string.Empty;

                SystemFileStream stream = SystemFile.OpenRead(filePath);
                string hash = ComputeHashFromFile(stream);
                stream.Close();
                return hash;
            }

            /// <summary>
            /// 计算指定字符串的Hash值
            /// </summary>
            public static string ComputeHashFromString(string content)
            {
                return ToHash(SystemMD5.Create().ComputeHash(SystemEncoding.UTF8.GetBytes(content)));
            }
        }
    }
}
