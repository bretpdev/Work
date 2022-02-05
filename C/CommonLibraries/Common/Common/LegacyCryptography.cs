using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Uheaa.Common
{
    public static class LegacyCryptography
    {
        public enum Keys
        {
            /// <summary>
            /// Used to access the NoradSP login info at X:\PADU\Security Access\NORAD\NORAD LOGIN.txt"
            /// </summary>
            [Description("NoRaD is A Fine Name FOR a DB bu")]
            NoradCredentials,
            /// <summary>
            /// Used to encrypt/decrypt routing numbers in NORAD.dbo.CKPH_DAT_OPSCheckByPhone
            /// </summary>
            [Description("To use OR nOt To use tHe OpS web")]
            NoradOPS,
            /// <summary>
            /// used to encrypt/decrypt the bank account number used in COMREFCOM
            /// </summary>
            [Description("SE2*Q&ZMfG-7V&mhwVMgZeZ7A5ZvXZIv")]
            COMREFCOM
        }
        static readonly byte[] iv = new byte[] { 121, 9, 10, 1, 31, 74, 11, 39, 255, 91, 45, 78, 55, 66, 22, 7 };
        public static string Encrypt(string text, Keys legacyKey)
        {
            string key = GetPaddedKey(legacyKey);
            byte[] textArray = Encoding.ASCII.GetBytes(text.ToCharArray());
            byte[] keyArray = Encoding.ASCII.GetBytes(key.ToCharArray());

            using (var ms = new MemoryStream())
            using (var rijndael = new RijndaelManaged())
            using (var crypto = new CryptoStream(ms, rijndael.CreateEncryptor(keyArray, iv), CryptoStreamMode.Write))
            {
                crypto.Write(textArray, 0, textArray.Length);
                crypto.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static string Decrypt(string text, Keys legacyKey)
        {
            string key = GetPaddedKey(legacyKey);
            byte[] textArray = Convert.FromBase64String(text);;
            byte[] keyArray = Encoding.ASCII.GetBytes(key.ToCharArray());


            using (var ms = new MemoryStream(textArray))
            using (var rijndael = new RijndaelManaged())
            using (var crypto = new CryptoStream(ms, rijndael.CreateDecryptor(keyArray, iv), CryptoStreamMode.Read))
            {
                byte[] temp = new byte[textArray.Length];
                crypto.Read(temp, 0, temp.Length);
                return Encoding.ASCII.GetString(temp.ToArray()).Trim(' ', '\0');
            }
        }

        private static string GetPaddedKey(Keys legacyKey)
        {
            string key = legacyKey.GetDescription().Description;
            return key.Trim().PadRight(32, 'X').Substring(0, 32); //key must be exactly 32 characters, padded with X if string is too short
        }
    }
}
