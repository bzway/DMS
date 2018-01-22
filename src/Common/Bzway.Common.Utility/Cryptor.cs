using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bzway.Common.Utility
{
    public class Cryptor
    {
        #region AES
        public static string DecryptAES(string encryptedDataString, string Key, string IV)
        {
            byte[] byteKey = Encoding.UTF8.GetBytes(Key);
            byte[] byteIV = Encoding.UTF8.GetBytes(IV);
            var managed = Aes.Create();
            byte[] buffer = Convert.FromBase64String(encryptedDataString);
            ICryptoTransform transform = managed.CreateDecryptor(byteKey, byteIV);
            MemoryStream stream = new MemoryStream(buffer);
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            byte[] buffer2 = new byte[buffer.Length];
            int length = stream2.Read(buffer2, 0, buffer2.Length);
            byte[] destinationArray = new byte[length];
            Array.Copy(buffer2, destinationArray, length);
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetString(destinationArray);
        }
        public static string EncryptAES(string PlainText, string Key, string IV)
        {
            byte[] byteKey = Encoding.UTF8.GetBytes(Key);
            byte[] byteIV = Encoding.UTF8.GetBytes(IV);
            ICryptoTransform transform = Aes.Create().CreateEncryptor(byteKey, byteIV);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write);
            byte[] bytes = new ASCIIEncoding().GetBytes(PlainText);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            string str = Convert.ToBase64String(stream.ToArray());
            return str;
        }



        #endregion
        #region DES
        /// <summary>
        /// DES加密方法
        /// </summary>
        /// <param name="PlainText">明文</param>
        /// <param name="DESKey">密钥</param>
        /// <param name="DESIV">向量</param>
        /// <returns>密文</returns>
        public static string DESEncrypt(string PlainText, string DESKey, string DESIV)
        {
            byte[] btKey = Encoding.UTF8.GetBytes(DESKey);
            byte[] btIV = Encoding.UTF8.GetBytes(DESIV);
            var des = TripleDES.Create();
            byte[] inData = Encoding.UTF8.GetBytes(PlainText);
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// DES解密方法
        /// </summary>
        /// <param name="CipherText">密文</param>
        /// <param name="DESKey">密钥</param>
        /// <param name="DESIV">向量</param>
        /// <returns>明文</returns>
        public static string DESDecrypt(string CipherText, string DESKey, string DESIV)
        {
            byte[] btKey = Encoding.UTF8.GetBytes(DESKey);
            byte[] btIV = Encoding.UTF8.GetBytes(DESIV);
            var des = TripleDES.Create();
            byte[] inData = Convert.FromBase64String(CipherText);
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);
                        cs.FlushFinalBlock();
                    }
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        #endregion

        #region Other
        public static string EncryptMD5(string input, string salt = "")
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            input += salt;
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                return BitConverter.ToString(md5.ComputeHash(bytes));
            }
        }

        public static string EncryptSHA1(string input, string salt = "")
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            input += salt;
            using (var sha1 = SHA1.Create())
            {
                foreach (var item in sha1.ComputeHash(Encoding.UTF8.GetBytes(input)))
                {
                    stringBuilder.AppendFormat("{0:x2}", item);
                }
            }
            return stringBuilder.ToString();
        }
        #endregion
    }
}