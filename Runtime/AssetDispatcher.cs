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

using UnityEngine;

namespace GooAsset
{
    /// <summary>
    /// 资源管理调度器
    /// </summary>
    internal static /*sealed*/ class AssetDispatcher // : MonoBehaviour
    {
        /**
        /// <summary>
        /// 进入游戏时先找到或创建Updater
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        static void InitializeOnLoad()
        {
            AssetDispatcher dispatcher = FindObjectOfType<AssetDispatcher>();
            if (dispatcher)
            {
                return;
            }

            dispatcher = new GameObject(nameof(AssetDispatcher)).AddComponent<AssetDispatcher>();
            DontDestroyOnLoad(dispatcher);
        }
        */

        /// <summary>
        /// 调度实例
        /// </summary>
        // public static AssetDispatcher Instance { get; private set; }

        /// <summary>
        /// 每次Update最大时间, 大于等于此时间视为繁忙, 不再做处理
        /// </summary>
        static float BusyTime => Application.backgroundLoadingPriority != ThreadPriority.High ? 0.01f : 0.06f;

        /// <summary>
        /// 当前时间
        /// </summary>
        static float _curTime = 0f;

        /// <summary>
        /// 程序是否繁忙(用于资源加载分帧处理, 保证流畅)
        /// </summary>
        public static bool IsBusy => Time.realtimeSinceStartup - _curTime >= BusyTime;

        // void Awake()
        public static void Start()
        {
            // Instance = this;
        }

        // void Update()
        public static void Update()
        {
            _curTime = Time.realtimeSinceStartup;

            LoadableHandler.UpdateAllLoadables();
            OperationHandler.UpdateAllOperations();
            DownloadHandler.Update();
        }

        // void OnDestroy()
        public static void Stop()
        {
            Clear();
        }

        /// <summary>
        /// 清理所有下载和加载内容
        /// </summary>
        static void Clear()
        {
            DownloadHandler.ClearAllDownloads();
            LoadableHandler.ClearAllLoadables();
        }
    }
}
