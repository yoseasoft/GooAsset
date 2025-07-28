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

using System.IO;
using System.Security.Cryptography;

using SystemRandom = System.Random;
using SystemArray = System.Array;
using SystemConvert = System.Convert;
using SystemStringBuilder = System.Text.StringBuilder;
using SystemEncoding = System.Text.Encoding;

namespace GooAsset
{
    /// <summary>
    /// 资源模块的辅助工具类，整合所有常用的工具函数
    /// </summary>
    public static partial class Utility
    {
        /// <summary>
        /// 加密/解密相关的辅助接口函数
        /// 
        /// 在资源模块中主要使用的还是AES加密
        /// 参考:https://github.com/myloveCc/NETCore.Encrypt
        /// </summary>
        public static class Cryptography
        {
            #region 生成随机密钥算法, 可自行调用生成用作key

            /// <summary>
            /// The single Random Generator
            /// </summary>
            static SystemRandom _random;

            /// <summary>
            /// Generate a random key
            /// </summary>
            /// <param name="n">key length，IV is 16，Key is 32</param>
            /// <returns>return random value</returns>
            static string GetRandomStr(int length)
            {
                char[] keys =
                {
                    'a', 'b', 'd', 'c', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'p', 'r', 'q', 's', 't', 'u', 'v', 'w', 'z', 'y', 'x',
                    '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'Q', 'P', 'R', 'T', 'S', 'V', 'U', 'W', 'X', 'Y', 'Z'
                };

                SystemStringBuilder str = new SystemStringBuilder();

                // New stronger Random Generator
                _random ??= new SystemRandom();

                for (int i = 0; i < length; i++)
                    str.Append(keys[_random.Next(0, keys.Length)].ToString());

                return str.ToString();
            }

            /// <summary>
            /// 生成随机的Key和IV，并打印出来
            /// </summary>
            public static void PrintRandomAesKey()
            {
                Logger.Info("AesKey={0},AesIV={1}", GetRandomStr(32), GetRandomStr(16));
            }

            #endregion

            /// <summary>
            /// AES encrypt
            /// </summary>
            /// <param name="data">Raw data</param>
            /// <param name="key">Key, requires 32 bits</param>
            /// <param name="vector">IV, requires 16 bits</param>
            /// <returns>Encrypted string</returns>
            public static string Encrypt(string data, string key, string vector)
            {
                IsNotEmpty(data, nameof(data));

                IsNotEmpty(key, nameof(key));
                IsNotOutOfRange(key.Length, 32, 32, nameof(key));

                IsNotEmpty(vector, nameof(vector));
                IsNotOutOfRange(vector.Length, 16, 16, nameof(vector));

                byte[] plainBytes = SystemEncoding.UTF8.GetBytes(data);

                var encryptBytes = Encrypt(plainBytes, key, vector);
                return encryptBytes != null ? SystemConvert.ToBase64String(encryptBytes) : null;
            }

            /// <summary>
            /// AES encrypt
            /// </summary>
            /// <param name="data">Raw data</param>
            /// <param name="key">Key, requires 32 bits</param>
            /// <param name="vector">IV, requires 16 bits</param>
            /// <returns>Encrypted byte array</returns>
            public static byte[] Encrypt(byte[] data, string key, string vector)
            {
                IsNotEmpty(data, nameof(data));

                IsNotEmpty(key, nameof(key));
                IsNotOutOfRange(key.Length, 32, 32, nameof(key));

                IsNotEmpty(vector, nameof(vector));
                IsNotOutOfRange(vector.Length, 16, 16, nameof(vector));

                byte[] plainBytes = data;

                byte[] bKey = new byte[32];
                SystemArray.Copy(SystemEncoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

                byte[] bVector = new byte[16];
                SystemArray.Copy(SystemEncoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);

                byte[] encryptData = null; // encrypted data
                using Aes aes = Aes.Create();
                try
                {
                    using MemoryStream memory = new MemoryStream();
                    using CryptoStream encryptor = new CryptoStream(memory, aes.CreateEncryptor(bKey, bVector), CryptoStreamMode.Write);
                    encryptor.Write(plainBytes, 0, plainBytes.Length);
                    encryptor.FlushFinalBlock();
                    encryptData = memory.ToArray();
                }
                catch
                {
                    encryptData = null;
                }

                return encryptData;
            }

            /// <summary>
            ///  AES decrypt
            /// </summary>
            /// <param name="data">Encrypted data</param>
            /// <param name="key">Key, requires 32 bits</param>
            /// <param name="vector">IV, requires 16 bits</param>
            /// <returns>Decrypted string</returns>
            public static string Decrypt(string data, string key, string vector)
            {
                IsNotEmpty(data, nameof(data));

                IsNotEmpty(key, nameof(key));
                IsNotOutOfRange(key.Length, 32, 32, nameof(key));

                IsNotEmpty(vector, nameof(vector));
                IsNotOutOfRange(vector.Length, 16, 16, nameof(vector));

                byte[] encryptedBytes = SystemConvert.FromBase64String(data);

                byte[] decryptBytes = Decrypt(encryptedBytes, key, vector);

                return decryptBytes != null ? SystemEncoding.UTF8.GetString(decryptBytes) : null;
            }

            /// <summary>
            ///  AES decrypt
            /// </summary>
            /// <param name="data">Encrypted data</param>
            /// <param name="key">Key, requires 32 bits</param>
            /// <param name="vector">IV, requires 16 bits</param>
            /// <returns>Decrypted byte array</returns>
            public static byte[] Decrypt(byte[] data, string key, string vector)
            {
                IsNotEmpty(data, nameof(data));

                IsNotEmpty(key, nameof(key));
                IsNotOutOfRange(key.Length, 32, 32, nameof(key));

                IsNotEmpty(vector, nameof(vector));
                IsNotOutOfRange(vector.Length, 16, 16, nameof(vector));

                byte[] encryptedBytes = data;

                byte[] bKey = new byte[32];
                SystemArray.Copy(SystemEncoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

                byte[] bVector = new byte[16];
                SystemArray.Copy(SystemEncoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);

                byte[] decryptedData; // decrypted data

                using Aes aes = Aes.Create();
                try
                {
                    using MemoryStream memory = new MemoryStream(encryptedBytes);
                    using CryptoStream cryptoStream = new CryptoStream(memory, aes.CreateDecryptor(bKey, bVector), CryptoStreamMode.Read);
                    using MemoryStream tempMemory = new MemoryStream();
                    byte[] buffer = new byte[1024];
                    int readBytes = 0;
                    while ((readBytes = cryptoStream.Read(buffer, 0, buffer.Length)) > 0)
                        tempMemory.Write(buffer, 0, readBytes);

                    decryptedData = tempMemory.ToArray();
                }
                catch
                {
                    decryptedData = null;
                }

                return decryptedData;
            }

            static void IsNotEmpty(string argument, string argumentName)
            {
                if (string.IsNullOrEmpty((argument ?? string.Empty).Trim()))
                    throw new System.ArgumentException($"\"{argumentName}\" 不能为空.", argumentName);
            }

            static void IsNotEmpty(byte[] array, string argumentName)
            {
                if (array == null || array.Length == 0)
                    throw new System.ArgumentException("集合不能为空.", argumentName);
            }

            static void IsNotOutOfRange(int argument, int min, int max, string argumentName)
            {
                if (argument < min || argument > max)
                    throw new System.ArgumentOutOfRangeException(argumentName, $"{argumentName} 必须在此区间 \"{min}\"-\"{max}\".");
            }
        }
    }
}
