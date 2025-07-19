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

using System.IO;

namespace HooAsset.Editor.Simulation
{
    /// <summary>
    /// 编辑器下原始文件加载
    /// </summary>
    public class EditorRawFile : RawFile
    {
        /// <summary>
        /// 创建EditorRawFile
        /// </summary>
        internal static EditorRawFile Create(string filePath)
        {
            return new EditorRawFile { filePath = filePath };
        }

        /// <summary>
        /// 设置文件保存目录并完成加载
        /// </summary>
        void SetRawFileSavePathAndFinish()
        {
            ManifestHandler.GetMainBundleInfoAndDependencies(filePath, out ManifestBundleInfo bundleInfo, out _);
            if (bundleInfo != null && !string.IsNullOrEmpty(bundleInfo.Name)) // 若获取到的bundle信息名字不为空，证明是外部目录资源, Name就是文件加载路径, 具体可查看EditorManifestAsset
                savePath = bundleInfo.Name;
            else
                savePath = Path.Combine(System.Environment.CurrentDirectory, filePath);
            address = savePath;
            Finish(File.Exists(savePath) ? null : "加载失败, 文件不存在");
        }

        protected override void OnLoad()
        {
            // 因编辑器下原始文件无需下载，直接读取即可, 故此处不加载, 放到OnUpdate中加载，模拟异步
        }

        protected override void OnLoadImmediately()
        {
            SetRawFileSavePathAndFinish();
        }

        protected override void OnUpdate()
        {
            if (Status == LoadableStatus.Loading)
                SetRawFileSavePathAndFinish();
        }
    }
}
