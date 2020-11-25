using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DPTPWebAPI.IndusbankAPI
{
    public class EncryptionsUtility
    {
        public static string AES_ENCRYPT(string text, string hexPassword)
        {
            var result = string.Join("", text.Select(c => ((int)c).ToString("X2")));
            byte[] originalBytes = HexStringToByte(result);
            byte[] passwordBytes = HexStringToByte(hexPassword);
            byte[] encryptedBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.Padding = PaddingMode.Zeros;
                    AES.Key = passwordBytes;
                    AES.Mode = CipherMode.ECB;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(originalBytes, 0, originalBytes.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }

            }
            return BitConverter.ToString(encryptedBytes).Replace("-", "");
        }


        public static string AES_DECRYPT(string text, string hexPassword)
        {
            byte[] originalBytes = HexStringToByte(text);
            byte[] passwordBytes = HexStringToByte(hexPassword);
            byte[] encryptedBytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.Padding = PaddingMode.Zeros;
                    AES.Key = passwordBytes;
                    AES.Mode = CipherMode.ECB;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(originalBytes, 0, originalBytes.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }
            string input = ASCIIEncoding.UTF8.GetString(encryptedBytes).Trim();
            int index = input.LastIndexOf("}");
            if (index > 0)
            {
                input = input.Substring(0, index); // or index + 1 to keep slash
                input = input + "}";
            }
            return input;
        }

        private static byte[] HexStringToByte(string hexString)
        {
            try
            {
                int bytesCount = (hexString.Length) / 2;
                byte[] bytes = new byte[bytesCount];
                for (int x = 0; x < bytesCount; ++x)
                {
                    bytes[x] = Convert.ToByte(hexString.Substring(x * 2, 2), 16);
                }
                return bytes;
            }
            catch
            {
                throw;
            }
        }
        private static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

    }
}