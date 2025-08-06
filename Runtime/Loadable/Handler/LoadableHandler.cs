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

namespace GooAsset
{
    /// <summary>
    /// Loadable对象管理
    /// </summary>
    public static class LoadableHandler
    {
        /// <summary>
        /// 加载中的Loadable对象列表
        /// </summary>
        static readonly List<Loadable> LoadingList = new();

        /// <summary>
        /// 不再使用的Loadable对象列表
        /// </summary>
        static readonly List<Loadable> UnusedList = new();

        /// <summary>
        /// 将loadable对象添加到Loading列表进行管理
        /// </summary>
        internal static void AddLoadable(Loadable loadable)
        {
            LoadingList.Add(loadable);
        }

        /// <summary>
        /// 将loadable对象添加到不再使用的列表进行管理
        /// </summary>
        internal static void AddUnused(Loadable loadable)
        {
            UnusedList.Add(loadable);
        }

        /// <summary>
        /// 从不再使用的列表移除指定的loadable对象
        /// </summary>
        internal static void RemoveUnused(Loadable loadable)
        {
            UnusedList.Remove(loadable);
        }

        /// <summary>
        /// Update所有Loadable对象
        /// </summary>
        internal static void UpdateAllLoadables()
        {
            // 需要按顺序Update, 所以虽然要Remove也不倒着来遍历
            // Loading列表处理
            for (int i = 0, loadingCount = LoadingList.Count; i < loadingCount; i++)
            {
                if (AssetDispatcher.IsBusy)
                    return;

                Loadable loadable = LoadingList[i];
                loadable.Update();

                if (loadable.IsDone)
                {
                    LoadingList.RemoveAt(i);
                    i--;
                    loadingCount--;
                    loadable.Complete();
                }
            }

            // 有正在加载中或卸载中的场景时不处理任何未使用的Loadable对象
            // 主要原因:异步卸载场景(UnloadSceneAsync)时, 若卸载AssetBundle, 会导致卸载中的场景材质丢失, 故此处直接统一不卸载任何Loadable对象
            // 次要原因:一个小的性能优化点, 加载场景时尽量少作其他操作
            if (SceneHandler.UpdateAsyncOperationListAndGetHasLoadingOrUnloadingScene())
                return;

            // Unused列表处理
            for (int i = 0, unusedCount = UnusedList.Count; i < unusedCount; i++)
            {
                if (AssetDispatcher.IsBusy)
                    break;

                Loadable loadable = UnusedList[i];
                if (!loadable.IsDone)
                    continue;

                UnusedList.RemoveAt(i);
                i--;
                unusedCount--;

                // 虽然在Unused列表里遍历, 但是资源有可能被重新引用, 所以需判断当前是否Unused
                if (loadable.reference.IsUnused)
                    loadable.Unload();
            }
        }

        /// <summary>
        /// 清除所有Loadable对象
        /// </summary>
        internal static void ClearAllLoadables()
        {
            AssetHandler.ClearCache();
            DependencyHandler.ClearCache();
            BundleHandler.ClearCache();

            LoadingList.Clear();
            UnusedList.Clear();
        }
    }
}
