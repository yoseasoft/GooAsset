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
using System.Text;
using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace HooAsset
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static partial class Utility
    {
        /// <summary>
        /// 每个数据单位对应的字节数
        /// </summary>
        static readonly long[] s_byteUnits =
        {
            1073741824, 1048576, 1024, 1
        };

        /// <summary>
        /// 每个数据单位的名字
        /// </summary>
        static readonly string[] s_byteUnitsNames =
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

            for (var i = 0; i < s_byteUnits.Length; i++)
            {
                long unit = s_byteUnits[i];
                if (bytes >= unit)
                {
                    size = $"{(double)bytes / unit:0.00} {s_byteUnitsNames[i]}";
                    break;
                }
            }

            return size;
        }
    }
}
