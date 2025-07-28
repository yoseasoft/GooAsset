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

using System;
using System.IO;
using GooAsset.Editor.Build;
using System.Collections.Generic;

namespace GooAsset.Editor.GUI
{
    /// <summary>
    /// 版本对比工具类
    /// </summary>
    internal static class VersionContrastUtils
    {
        /// <summary>
        /// 获取排序后的版本名字列表
        /// </summary>
        internal static void GetVersionFileNameList(List<string> versionFileNameList)
        {
            versionFileNameList.Clear();

            string buildRecordFolderPath = BuildUtils.TranslateToBuildPath(BuildUtils.BuildRecordFolderName);
            DirectoryInfo directoryInfo = new DirectoryInfo(buildRecordFolderPath);
            if (!directoryInfo.Exists)
                return;

            FileInfo[] fileInfoList = directoryInfo.GetFiles("*.json");
            foreach (FileInfo fileInfo in fileInfoList)
            {
                string fileName = fileInfo.Name;
                if (!fileName.StartsWith(BuildUtils.BuildRecordFilePrefix) || !fileName.EndsWith(".json"))
                    continue;

                string replaceFileName = fileName.Replace(".json", string.Empty);
                replaceFileName = replaceFileName.Replace(BuildUtils.BuildRecordFilePrefix, string.Empty);
                string[] sep = replaceFileName.Split('_');
                if (sep.Length == 2 && int.TryParse(sep[0], out int _) && long.TryParse(sep[1], out long _))
                    versionFileNameList.Add(fileName);
            }

            // 按最新时间排序
            versionFileNameList.Sort((a, b) =>
            {
                long aTime = long.Parse(a.Replace(".json", string.Empty).Replace(BuildUtils.BuildRecordFilePrefix, string.Empty).Split('_')[1]);
                long bTime = long.Parse(b.Replace(".json", string.Empty).Replace(BuildUtils.BuildRecordFilePrefix, string.Empty).Split('_')[1]);
                return aTime < bTime ? 1 : -1;
            });
        }

        /// <summary>
        /// 获取当前打包目录下的版本记录文件数量
        /// </summary>
        internal static int GetVersionFileCount()
        {
            string buildRecordFolderPath = BuildUtils.TranslateToBuildPath(BuildUtils.BuildRecordFolderName);
            DirectoryInfo directoryInfo = new DirectoryInfo(buildRecordFolderPath);
            if (!directoryInfo.Exists)
                return 0;

            int fileCount = 0;
            FileInfo[] fileInfoList = directoryInfo.GetFiles("*.json");
            foreach (FileInfo fileInfo in fileInfoList)
            {
                string fileName = fileInfo.Name;
                if (!fileName.StartsWith(BuildUtils.BuildRecordFilePrefix) || !fileName.EndsWith(".json"))
                    continue;

                string replaceFileName = fileName.Replace(".json", string.Empty);
                replaceFileName = replaceFileName.Replace(BuildUtils.BuildRecordFilePrefix, string.Empty);
                string[] sep = replaceFileName.Split('_');
                if (sep.Length == 2 && int.TryParse(sep[0], out int _) && long.TryParse(sep[1], out long _))
                    fileCount++;
            }

            return fileCount;
        }

        /// <summary>
        /// 文件名字转换成显示名字
        /// </summary>
        internal static string ToShowName(string fileName, Dictionary<string, string> fileNameToComment = null)
        {
            string comment = null;
            fileNameToComment?.TryGetValue(fileName, out comment);
            fileName = fileName.Replace(".json", string.Empty);
            fileName = fileName.Replace(BuildUtils.BuildRecordFilePrefix, string.Empty);
            string[] sep = fileName.Split('_');
            DateTime dateTime = DateTime.FromFileTime(long.Parse(sep[1])).ToLocalTime();
            if (string.IsNullOrEmpty(comment))
                return $"v{int.Parse(sep[0])}({dateTime:yyyy-MM-dd HH:mm:ss})";

            return $"v{int.Parse(sep[0])}({dateTime:yyyy-MM-dd HH:mm:ss})({comment})";
        }

        /// <summary>
        /// 根据记录文件名获取版本号
        /// </summary>
        internal static int GetBuildVersion(string fileName)
        {
            fileName = fileName.Replace(".json", string.Empty);
            fileName = fileName.Replace(BuildUtils.BuildRecordFilePrefix, string.Empty);
            return int.Parse(fileName.Split('_')[0]);
        }

        /// <summary>
        /// 根据记录文件名获取版本构建时间
        /// </summary>
        internal static long GetVersionBuildTime(string fileName)
        {
            fileName = fileName.Replace(".json", string.Empty);
            fileName = fileName.Replace(BuildUtils.BuildRecordFilePrefix, string.Empty);
            return long.Parse(fileName.Split('_')[1]);
        }

        /// <summary>
        /// 根据构建版本和时间戳转换成记录文件名字
        /// </summary>
        internal static string TranslateToVersionFileName(int version, long timestamp)
        {
            return $"{BuildUtils.BuildRecordFilePrefix}{version}_{timestamp}.json";
        }

        /// <summary>
        /// 加载注释文件并初始化备注字典
        /// </summary>
        internal static void LoadCommentDataAndRefreshCommentDictionary(Dictionary<string, string> recordFileNameToComment)
        {
            recordFileNameToComment.Clear();

            RecordComment recordComment = RecordComment.LoadRecordComment();
            if (recordComment == null)
                return;

            string[] nameList = recordComment.recordFileNameList;
            string[] commentList = recordComment.recordCommentList;
            for (int i = 0; i < nameList.Length; i++)
                recordFileNameToComment.Add(nameList[i], commentList[i]);
        }
    }
}
