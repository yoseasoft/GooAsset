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

using System.Linq;
using System.Collections.Generic;

namespace HooAsset
{
    /// <summary>
    /// Dependency对象管理
    /// </summary>
    public static class DependencyHandler
    {
        /// <summary>
        /// 依赖对象缓存
        /// </summary>
        static readonly Dictionary<string, Dependency> Cache = new();

        /// <summary>
        /// 加载Dependency对象
        /// </summary>
        public static Dependency LoadAsync(string address)
        {
            if (!Cache.TryGetValue(address, out Dependency dependency))
            {
                dependency = new Dependency { address = address };
                Cache.Add(address, dependency);
            }

            dependency.Load();

            return dependency;
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        internal static void RemoveCache(string address)
        {
            Cache.Remove(address);
        }

        /// <summary>
        /// 清除缓存并全部卸载
        /// </summary>
        internal static void ClearCache()
        {
            Dependency[] dependencies = Cache.Values.ToArray();
            foreach (Dependency dependency in dependencies)
            {
                dependency.FullyRelease();
                dependency.Unload();
            }

            Cache.Clear();
        }
    }
}
