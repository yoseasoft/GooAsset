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
using System.Collections;
using System.Threading.Tasks;
using Object = UnityEngine.Object;

namespace HooAsset
{
    /// <summary>
    /// 资源对象基类
    /// </summary>
    public class Asset : Loadable, IEnumerator
    {
        /// <summary>
        /// 加载完成对象
        /// </summary>
        public Object result;

        /// <summary>
        /// 加载类型
        /// </summary>
        public Type type;

        /// <summary>
        /// 加载完成回调
        /// </summary>
        public Action<Asset> completed;

        /// <summary>
        /// 提供给await使用
        /// </summary>
        public Task<Asset> Task
        {
            get
            {
                TaskCompletionSource<Asset> tcs = new();
                completed += _ => tcs.SetResult(this);
                return tcs.Task;
            }
        }

        /// <summary>
        /// 加载完成时由子类调用
        /// </summary>
        protected void OnAssetLoaded(Object obj)
        {
            result = obj;
            Finish(result == null ? "加载出的object为空??" : null);
        }

        protected override void OnComplete()
        {
            if (completed == null)
                return;

            Action<Asset> func = completed;
            completed = null;

            try
            {
                func(this);
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
        }

        protected override void OnUnused()
        {
            completed = null;
        }

        protected override void OnUnload()
        {
            AssetHandler.RemoveCache(address);
        }

        #region IEnumerator

        public object Current => null;

        public bool MoveNext()
        {
            return !IsDone;
        }

        public void Reset()
        {
        }

        #endregion
    }
}
