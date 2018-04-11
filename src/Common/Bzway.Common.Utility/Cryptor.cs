using Bzway.Common.Utility.RSAHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bzway.Common.Utility
{
    public static class Cryptor
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

        #region Hash
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
                return Convert.ToBase64String(md5.ComputeHash(bytes));
            }
        }
        public static string EncryptSHA1(string input, string salt = "")
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            input += salt;
            using (var sha1 = SHA1.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                return Convert.ToBase64String(sha1.ComputeHash(bytes));
            }
        }
        #endregion

        #region RSA
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="publickey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string publickey, string content)
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromPublicString(publickey);
                var cipherbytes = rsa.Encrypt(Convert.FromBase64String(content), RSAEncryptionPadding.Pkcs1);
                rsa.Clear();
                return Convert.ToBase64String(cipherbytes);
            }
        }
        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RSADecrypt(string privatekey, string content)
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromPrivateString(privatekey);
                var cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), RSAEncryptionPadding.Pkcs1);
                rsa.Clear();
                return Convert.ToBase64String(cipherbytes);
            }
        }

        public static string RSASign(string privatekey, string content)
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromPrivateString(privatekey);
                var bytes = rsa.SignData(Encoding.UTF8.GetBytes(content), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                rsa.Clear();
                return Convert.ToBase64String(bytes);
            }
        }
        public static string ToXmlKey(string keyString, bool IsPrivateKey = true)
        {
            using (var rsa = RSA.Create())
            {
                if (IsPrivateKey)
                {
                    rsa.FromPrivateString(keyString);
                }
                else
                {
                    rsa.FromPublicString(keyString);
                }
                return rsa.ToXmlStringX(IsPrivateKey);
            }
        }

        public static bool RSAVerifySign(string publickey, string content, string sign)
        {
            using (var rsa = RSA.Create())
            {
                rsa.FromPublicString(publickey);
                var result = rsa.VerifyData(Encoding.UTF8.GetBytes(content), Convert.FromBase64String(sign), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
                rsa.Clear();
                return result;
            }
        }

        public static PublicAndPrivateKey GenerateKeys()
        {
            using (var rsa = RSA.Create())
            {
                var parameters = rsa.ExportParameters(true);

                var result = new PublicAndPrivateKey()
                {
                    PrivateKey = Convert.ToBase64String(AsnKeyBuilder.PrivateKeyToPKCS8(parameters).GetBytes()),
                    PublicKey = Convert.ToBase64String(AsnKeyBuilder.PublicKeyToX509(parameters).GetBytes())
                };
                rsa.Clear();
                return result;
            }
        }

        #region 导入密钥算法  
        #region XML
        internal static void FromXmlStringX(this RSA rsa, string xmlString)
        {
            RSAParameters parameters = new RSAParameters();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);

            if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
            {
                foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "Modulus": parameters.Modulus = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Exponent": parameters.Exponent = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "P": parameters.P = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "Q": parameters.Q = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DP": parameters.DP = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "DQ": parameters.DQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "InverseQ": parameters.InverseQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                        case "D": parameters.D = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    }
                }
            }
            else
            {
                throw new Exception("Invalid XML RSA key.");
            }

            rsa.ImportParameters(parameters);
        }
        internal static string ToXmlStringX(this RSA rsa, bool includePrivateParameters)
        {
            RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);
            return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
                  parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
                  parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
                  parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
                  parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
                  parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
                  parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
                  parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
                  parameters.D != null ? Convert.ToBase64String(parameters.D) : null);
        }
        #endregion

        #region Java
        internal static void FromPrivateString(this RSA rsa, string key)
        {
            var PKCS8 = Convert.FromBase64String(key);
            AsnKeyParser keyParser = new AsnKeyParser(PKCS8);
            RSAParameters rsaParameters = keyParser.ParseRSAPrivateKey();
            rsa.ImportParameters(rsaParameters);

        }
        internal static void FromPublicString(this RSA rsa, string key)
        {
            var x509Key = Convert.FromBase64String(key);
            AsnKeyParser keyParser = new AsnKeyParser(x509Key);
            RSAParameters rsaParameters = keyParser.ParseRSAPublicKey();
            rsa.ImportParameters(rsaParameters);
        }

        #endregion

        #region others
        private static void CreateDsaKeys()
        {
            CspParameters csp = new CspParameters
            {
                KeyContainerName = "DSA Test (OK to Delete)"
            };

            const int PROV_DSS_DH = 13;
            csp.ProviderType = PROV_DSS_DH;

            // Can't use AT_EXCHANGE for creation. This is
            //  a signature algorithm
            const int AT_SIGNATURE = 2;
            csp.KeyNumber = AT_SIGNATURE;

            DSACryptoServiceProvider dsa = new DSACryptoServiceProvider(1024, csp)
            {
                PersistKeyInCsp = false
            };

            // Encoded key
            AsnKeyBuilder.AsnMessage key = null;

            // Private Key
            DSAParameters privateKey = dsa.ExportParameters(true);
            key = AsnKeyBuilder.PrivateKeyToPKCS8(privateKey);

            using (BinaryWriter writer = new BinaryWriter(
                new FileStream("private.dsa.cs.ber", FileMode.Create,
                    FileAccess.ReadWrite)))
            {
                writer.Write(key.GetBytes());
            }

            // Public Key
            DSAParameters publicKey = dsa.ExportParameters(false);
            key = AsnKeyBuilder.PublicKeyToX509(publicKey);

            using (BinaryWriter writer = new BinaryWriter(
                new FileStream("public.dsa.cs.ber", FileMode.Create,
                    FileAccess.ReadWrite)))
            {
                writer.Write(key.GetBytes());
            }

            // See http://blogs.msdn.com/tess/archive/2007/10/31/
            //   asp-net-crash-system-security-cryptography-cryptographicexception.aspx
            dsa.Clear();
        }

        private static void LoadDsaPrivateKey()
        {
            //
            // Load the Private Key
            //   PKCS#8 Format
            //
            AsnKeyParser keyParser = new AsnKeyParser("private.dsa.cs.ber");

            DSAParameters privateKey = keyParser.ParseDSAPrivateKey();

            //
            // Initailize the CSP
            //   Supresses creation of a new key
            //
            CspParameters csp = new CspParameters
            {
                KeyContainerName = "DSA Test (OK to Delete)"
            };

            // Can't use PROV_DSS_DH for loading. We have lost
            //   parameters such as seed and j.
            // const int PROV_DSS_DH = 13;
            const int PROV_DSS = 3;
            csp.ProviderType = PROV_DSS;

            // const int AT_EXCHANGE = 1;
            const int AT_SIGNATURE = 2;
            csp.KeyNumber = AT_SIGNATURE;

            //
            // Initialize the Provider
            //
            DSACryptoServiceProvider dsa =
              new DSACryptoServiceProvider(csp)
              {
                  PersistKeyInCsp = false
              };

            //
            // The moment of truth...
            //
            dsa.ImportParameters(privateKey);

            // See http://blogs.msdn.com/tess/archive/2007/10/31/
            //   asp-net-crash-system-security-cryptography-cryptographicexception.aspx
            dsa.Clear();
        }

        private static void LoadDsaPublicKey()
        {
            //
            // Load the Public Key
            //   X.509 Format
            //
            AsnKeyParser keyParser = new AsnKeyParser("public.dsa.cs.ber");

            DSAParameters publicKey = keyParser.ParseDSAPublicKey();

            //
            // Initailize the CSP
            //   Supresses creation of a new key
            //
            CspParameters csp = new CspParameters();

            // const int PROV_DSS_DH = 13;
            const int PROV_DSS = 3;
            csp.ProviderType = PROV_DSS;

            const int AT_SIGNATURE = 2;
            csp.KeyNumber = AT_SIGNATURE;

            csp.KeyContainerName = "DSA Test (OK to Delete)";

            //
            // Initialize the Provider
            //
            DSACryptoServiceProvider dsa = new DSACryptoServiceProvider(csp)
            {
                PersistKeyInCsp = false
            };

            //
            // The moment of truth...
            //
            dsa.ImportParameters(publicKey);

            // See http://blogs.msdn.com/tess/archive/2007/10/31/
            //   asp-net-crash-system-security-cryptography-cryptographicexception.aspx
            dsa.Clear();
        }

        private static void LoadRsaPrivateKey()
        {
            //
            // Load the Private Key
            //   PKCS#8 Format
            //
            AsnKeyParser keyParser = new AsnKeyParser("private.rsa.cs.ber");

            RSAParameters privateKey = keyParser.ParseRSAPrivateKey();

            //
            // Initailize the CSP
            //   Supresses creation of a new key
            //
            CspParameters csp = new CspParameters
            {
                KeyContainerName = "RSA Test (OK to Delete)"
            };

            const int PROV_RSA_FULL = 1;
            csp.ProviderType = PROV_RSA_FULL;

            const int AT_KEYEXCHANGE = 1;
            // const int AT_SIGNATURE = 2;
            csp.KeyNumber = AT_KEYEXCHANGE;

            //
            // Initialize the Provider
            //
            RSACryptoServiceProvider rsa =               new RSACryptoServiceProvider(csp)
              {
                  PersistKeyInCsp = false
              };

            //
            // The moment of truth...
            //
            rsa.ImportParameters(privateKey);

            // See http://blogs.msdn.com/tess/archive/2007/10/31/
            //   asp-net-crash-system-security-cryptography-cryptographicexception.aspx
            rsa.Clear();
        }

        private static void LoadRsaPublicKey()
        {
            //
            // Load the Public Key
            //   X.509 Format
            //
            AsnKeyParser keyParser =
              new AsnKeyParser("public.rsa.cs.ber");

            RSAParameters publicKey = keyParser.ParseRSAPublicKey();

            //
            // Initailize the CSP
            //   Supresses creation of a new key
            //
            CspParameters csp = new CspParameters
            {
                KeyContainerName = "RSA Test (OK to Delete)"
            };

            const int PROV_RSA_FULL = 1;
            csp.ProviderType = PROV_RSA_FULL;

            const int AT_KEYEXCHANGE = 1;
            // const int AT_SIGNATURE = 2;
            csp.KeyNumber = AT_KEYEXCHANGE;

            //
            // Initialize the Provider
            //
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(csp)
            {
                PersistKeyInCsp = false
            };

            //
            // The moment of truth...
            //
            rsa.ImportParameters(publicKey);

            // See http://blogs.msdn.com/tess/archive/2007/10/31/
            //   asp-net-crash-system-security-cryptography-cryptographicexception.aspx
            rsa.Clear();
        }
        #endregion

        #endregion
        #endregion

        #region EC Key
        public class HashWithSaltResult
        {
            public string Salt { get; }
            public string Digest { get; set; }

            public HashWithSaltResult(string salt, string digest)
            {
                Salt = salt;
                Digest = digest;
            }
        }
        public class RNG
        {
            public string GenerateRandomCryptographicKey(int keyLength)
            {
                return Convert.ToBase64String(GenerateRandomCryptographicBytes(keyLength));
            }

            public byte[] GenerateRandomCryptographicBytes(int keyLength)
            {
                RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
                byte[] randomBytes = new byte[keyLength];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return randomBytes;
            }
        }
        public class PKCS
        {
            public HashWithSaltResult HashPasswordWithPkcs(string plainPassword, int roundOfHashIterations, int saltLengthBytes)
            {
                RNG rng = new RNG();
                byte[] saltBytes = rng.GenerateRandomCryptographicBytes(saltLengthBytes);
                Rfc2898DeriveBytes pbkdf = new Rfc2898DeriveBytes(plainPassword, saltBytes, roundOfHashIterations);
                byte[] derivedBytes = pbkdf.GetBytes(32);
                return new HashWithSaltResult(Convert.ToBase64String(saltBytes), Convert.ToBase64String(derivedBytes));
            }
        }
        #endregion
    }
}