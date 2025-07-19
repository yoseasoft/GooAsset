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
    /// 加载状态
    /// </summary>
    public enum LoadableStatus
    {
        /// <summary>
        /// 初始状态
        /// </summary>
        Init,

        /// <summary>
        /// 检查版本中
        /// </summary>
        CheckingVersion,

        /// <summary>
        /// 依赖加载中
        /// </summary>
        DependentLoading,

        /// <summary>
        /// 解压中
        /// </summary>
        Unpacking,

        /// <summary>
        /// 加载中
        /// </summary>
        Loading,

        /// <summary>
        /// 加载成功
        /// </summary>
        LoadSuccessful,

        /// <summary>
        /// 加载失败
        /// </summary>
        LoadFailed,

        /// <summary>
        /// 已卸载
        /// </summary>
        Unloaded
    }
}
