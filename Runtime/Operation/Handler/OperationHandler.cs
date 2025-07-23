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

using System.Collections.Generic;

namespace HooAsset
{
    /// <summary>
    /// Operation对象的管理类，由它负责统一调度所有的操作对象实例
    /// </summary>
    internal static class OperationHandler
    {
        /// <summary>
        /// 处理中的Operation对象列表
        /// </summary>
        static readonly List<Operation> _operations = new();

        /// <summary>
        /// 添加Operation对象
        /// </summary>
        internal static void AddOperation(Operation operation)
        {
            _operations.Add(operation);
        }

        /// <summary>
        /// Update所有Operation对象
        /// </summary>
        internal static void UpdateAllOperations()
        {
            // 需要按顺序Update, 所以虽然要Remove也不倒着来遍历
            for (int n = 0; n < _operations.Count; ++n)
            {
                if (AssetDispatcher.Instance.IsBusy)
                    return;

                Operation operation = _operations[n];
                operation.Update();

                if (operation.IsDone)
                {
                    _operations.RemoveAt(n);
                    --n;

                    if (operation.Status == OperationStatus.Failed)
                        Logger.Warning($"操作{operation.GetType().Name}未完成, 失败原因：{operation.Error}！");

                    operation.Complete();
                }
            }
        }
    }
}
