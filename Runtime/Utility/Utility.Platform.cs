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

using RuntimePlatform = UnityEngine.RuntimePlatform;

namespace GooAsset
{
    /// <summary>
    /// 资源模块的辅助工具类，整合所有常用的工具函数
    /// </summary>
    public static partial class Utility
    {
        /// <summary>
        /// 平台相关的辅助接口函数
        /// </summary>
        public static class Platform
        {
            /// <summary>
            /// 不支持平台提示信息
            /// </summary>
            private const string UnsupportedPlatform = "Unsupported";

            /// <summary>
            /// 获取当前平台的类型
            /// </summary>
            public static RuntimePlatform CurrentPlatform
            {
                get
                {
#if UNITY_EDITOR
                    if (!UnityEngine.Application.isEditor)
                    {
#endif
                        return UnityEngine.Application.platform switch
                        {
                            RuntimePlatform.WindowsPlayer => RuntimePlatform.WindowsPlayer,
                            RuntimePlatform.OSXPlayer => RuntimePlatform.OSXPlayer,
                            RuntimePlatform.Android => RuntimePlatform.Android,
                            RuntimePlatform.IPhonePlayer => RuntimePlatform.IPhonePlayer,
                            RuntimePlatform.WebGLPlayer => RuntimePlatform.WebGLPlayer,
                            _ => throw new System.Exception("Unsupported Platform")
                        };
#if UNITY_EDITOR
                    }

                    return UnityEditor.EditorUserBuildSettings.activeBuildTarget switch
                    {
                        UnityEditor.BuildTarget.StandaloneWindows or UnityEditor.BuildTarget.StandaloneWindows64 => RuntimePlatform.WindowsPlayer,
                        UnityEditor.BuildTarget.StandaloneOSX => RuntimePlatform.OSXPlayer,
                        UnityEditor.BuildTarget.Android => RuntimePlatform.Android,
                        UnityEditor.BuildTarget.iOS => RuntimePlatform.IPhonePlayer,
                        UnityEditor.BuildTarget.WebGL => RuntimePlatform.WebGLPlayer,
                        _ => throw new System.Exception("Unsupported Platform")
                    };

#endif
                }
            }

            /// <summary>
            /// 获取当前平台的名称
            /// </summary>
            public static string CurrentPlatformName
            {
                get
                {
                    return CurrentPlatform switch
                    {
                        RuntimePlatform.WindowsPlayer => "Windows",
                        RuntimePlatform.OSXPlayer => "MacOS",
                        RuntimePlatform.Android => "Android",
                        RuntimePlatform.IPhonePlayer => "iOS",
                        RuntimePlatform.WebGLPlayer => "WebGL",
                        _ => UnsupportedPlatform
                    };
                }
            }
        }
    }
}
