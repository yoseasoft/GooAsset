/// -------------------------------------------------------------------------------
/// HooAsset Framework
///
/// Copyright (C) 2020 - 2022, Guangzhou Xinyuan Technology Co., Ltd.
/// Copyright (C) 2022 - 2023, Shanghai Bilibili Technology Co., Ltd.
/// Copyright (C) 2023 - 2024, Guangzhou Shiyue Network Technology Co., Ltd.
/// Copyright (C) 2025, Hainan Yuanyou Information Tecdhnology Co., Ltd. Guangzhou Branch
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
    /// 资源管理模块的日志输出管理类
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// 调试信息可打印状态标识
        /// </summary>
        public static bool DebuggingInformationPrintable = true;

        /// <summary>
        /// 日志输出回调句柄
        /// </summary>
        /// <param name="format">格式文本</param>
        /// <param name="args">格式化参数</param>
        public delegate void LogOutputHandler(string format, params object[] args);

        private static LogOutputHandler _infoCallback = null;
        private static LogOutputHandler _warnCallback = null;
        private static LogOutputHandler _errorCallback = null;

        public static LogOutputHandler InfoCallback
        {
            get { return _infoCallback; }
            set { _infoCallback = value; }
        }

        public static LogOutputHandler WarnCallback
        {
            get { return _warnCallback; }
            set { _warnCallback = value; }
        }

        public static LogOutputHandler ErrorCallback
        {
            get { return _errorCallback; }
            set { _errorCallback = value; }
        }

        /// <summary>
        /// 调试模式下的日志输出接口函数
        /// </summary>
        /// <param name="format">格式文本</param>
        /// <param name="args">格式化参数</param>
        public static void Info(string format, params object[] args)
        {
            if (false == DebuggingInformationPrintable) return;

            if (null == _infoCallback)
            {
                UnityEngine.Debug.LogFormat(format, args);
            }
            else
            {
                _infoCallback(format, args);
            }
        }

        /// <summary>
        /// 警告模式下的日志输出接口函数
        /// </summary>
        /// <param name="format">格式文本</param>
        /// <param name="args">格式化参数</param>
        public static void Warning(string format, params object[] args)
        {
            if (null == _warnCallback)
            {
                UnityEngine.Debug.LogWarningFormat(format, args);
            }
            else
            {
                _warnCallback(format, args);
            }
        }

        /// <summary>
        /// 错误模式下的日志输出接口函数
        /// </summary>
        /// <param name="format">格式文本</param>
        /// <param name="args">格式化参数</param>
        public static void Error(string format, params object[] args)
        {
            if (null == _errorCallback)
            {
                UnityEngine.Debug.LogErrorFormat(format, args);
            }
            else
            {
                _errorCallback(format, args);
            }
        }

        /// <summary>
        /// 异常模式下的日志输出接口函数
        /// </summary>
        /// <param name="e">异常实例</param>
        public static void Exception(System.Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }
    }
}
