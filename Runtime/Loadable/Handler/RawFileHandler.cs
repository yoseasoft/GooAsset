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

namespace HooAsset
{
    /// <summary>
    /// 原始资源文件对象管理
    /// </summary>
    public static class RawFileHandler
    {
        /// <summary>
        /// 创建场景对象方法, 可进行自定义覆盖, 默认为DefaultCreateRawFileFunc
        /// </summary>
        public static Func<string, RawFile> CreateRawFileFunc { get; set; } = DefaultCreateRawFileFunc;

        /// <summary>
        /// 默认创建原始文件对象的方法
        /// </summary>
        static RawFile DefaultCreateRawFileFunc(string filePath)
        {
            return new RawFile { filePath = filePath };
        }

        /// <summary>
        /// 同步加载原始资源
        /// </summary>
        /// <param name="filePath">文件原打包路径('Assets/_Resources/......', 若为Assets外部文件则为:'Assets文件夹同级目录/...'或'Assets文件夹同级文件')</param>
        internal static RawFile Load(string filePath)
        {
            RawFile rawFile = LoadAsync(filePath);
            rawFile?.LoadImmediately();
            return rawFile;
        }

        /// <summary>
        /// 异步加载原始资源文件
        /// </summary>
        /// /// <param name="filePath">文件原打包路径('Assets/_Resources/......', 若为Assets外部文件则为:'Assets文件夹同级目录/...'或'Assets文件夹同级文件')</param>
        /// <param name="completed">完成回调</param>
        internal static RawFile LoadAsync(string filePath, Action<RawFile> completed = null)
        {
            if (!ManifestHandler.IsAssetContains(filePath))
            {
                Logger.Error($"原始资源文件加载失败, 因所有资源清单中都没有此文件的记录:{filePath}");
                return null;
            }

            RawFile rawFile = CreateRawFileFunc(filePath);

            if (completed != null)
                rawFile.completed += completed;

            rawFile.Load();
            return rawFile;
        }
    }
}
