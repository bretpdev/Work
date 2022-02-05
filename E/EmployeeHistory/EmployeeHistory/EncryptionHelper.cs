using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeHistory
{
    public class EncryptionHelper
    {
        byte[] KEY = new byte[] { 39, 127, 57, 248, 241, 192, 55, 21, 9, 209, 87, 114, 99, 112, 54, 31, 174, 29, 66, 123, 236, 171, 60, 255, 109, 194, 185, 70, 178, 132, 172, 255 };
        byte[] IV = new byte[] { 139, 127, 57, 244, 241, 192, 255, 21, 9, 209, 87, 14, 99, 112, 54, 31 };
        public string Encrypt(string plainText)
        {
            using (var aes = new AesManaged())
            {
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform encryptor = aes.CreateEncryptor(KEY, IV);
                using (MemoryStream memory = new MemoryStream())
                {
                    using (CryptoStream crypto = new CryptoStream(memory, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter writer = new StreamWriter(crypto))
                    {
                        writer.Write(plainText);
                    }
                    string encryptedText = Convert.ToBase64String(memory.ToArray());
                    return encryptedText;
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            using (var aes = new AesManaged())
            {
                aes.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = aes.CreateDecryptor(KEY, IV);
                using (MemoryStream memory = new MemoryStream(Convert.FromBase64String(encryptedText)))
                using (CryptoStream crypto = new CryptoStream(memory, decryptor, CryptoStreamMode.Read))
                using (StreamReader reader = new StreamReader(crypto))
                {
                    string decrypted = reader.ReadToEnd();
                    return decrypted;
                }
            }
        }
    }
}
