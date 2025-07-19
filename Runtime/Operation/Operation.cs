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
using UnityEngine;
using System.Threading.Tasks;

namespace HooAsset
{
    /// <summary>
    /// 操作基类
    /// </summary>
    public class Operation : CustomYieldInstruction
    {
        /// <summary>
        /// 协程等待标志
        /// </summary>
        public override bool keepWaiting => !IsDone;

        /// <summary>
        /// 操作完成回调
        /// </summary>
        public Action<Operation> completed;

        /// <summary>
        /// 当前状态
        /// </summary>
        public OperationStatus Status { get; protected set; } = OperationStatus.Init;

        /// <summary>
        /// 进度(取值范围:0~1)
        /// </summary>
        public float Progress { get; protected set; }

        /// <summary>
        /// 操作是否完成
        /// </summary>
        public bool IsDone => Status is OperationStatus.Successful or OperationStatus.Failed;

        /// <summary>
        /// 错误原因
        /// </summary>
        public string Error { get; protected set; }

        /// <summary>
        /// 开始操作
        /// </summary>
        internal void Start()
        {
            Status = OperationStatus.Processing;
            OperationHandler.AddOperation(this);
            OnStart();
        }

        internal void Update()
        {
            OnUpdate();
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        public void Cancel()
        {
            Finish("取消");
        }

        /// <summary>
        /// 完成操作(操作成功和失败都需由基类调用此接口)
        /// </summary>
        /// <param name="errorCode">操作失败原因</param>
        protected void Finish(string errorCode = null)
        {
            Error = errorCode;
            Status = string.IsNullOrEmpty(errorCode) ? OperationStatus.Successful : OperationStatus.Failed;
            Progress = 1;
        }

        /// <summary>
        /// 完成通知, 基类调用Finish后, Complete就会被Handler调用
        /// </summary>
        internal void Complete()
        {
            if (completed == null)
                return;

            var func = completed;
            completed = null;
            func.Invoke(this);
        }

        /// <summary>
        /// 开始通知, 由子类继承实现
        /// </summary>
        protected virtual void OnStart()
        {
        }

        /// <summary>
        /// Update通知, 由子类继承实现
        /// </summary>
        protected virtual void OnUpdate()
        {
        }

        /// <summary>
        /// 提供给await使用
        /// </summary>
        public Task<Operation> Task
        {
            get
            {
                TaskCompletionSource<Operation> tcs = new();
                completed += _ => tcs.SetResult(this);
                return tcs.Task;
            }
        }
    }
}
