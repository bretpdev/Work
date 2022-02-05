using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RegentsApp
{
    public class StringEncryption
    {
        #region String Encryption/Decryption
        private string _encryptionKey = "Regents e muito legal, mas pode dar-lhe uma ulcera ou conduzi-lo a bebida.";
        private byte[] _salt = { 121, 9, 10, 1, 31, 74, 11, 39, 255, 91, 45, 78, 55, 66, 22, 7 };

        /// <summary>
        /// Encrypts a string such that it can be decrypted using the "string.Decrypt()" method.
        /// The encryption key, salt, and encryption mechanism match those used
        /// in the internal Regents' program's "reset password" function.
        /// </summary>
        public string Encrypt(string unencryptedString)
        {
            //Remove any null characters from the unencrypted string.
            unencryptedString = unencryptedString.Replace("\0", "");
            //Ensure the encryption key is 32 bytes. Pad with 'X' characters if needed.
            if (_encryptionKey.Length > 32)
            {
                _encryptionKey = _encryptionKey.Substring(0, 32);
            }
            else
            {
                _encryptionKey = _encryptionKey.PadRight(32, 'X');
            }

            //Create the encryptor and send it a byte array of the unencrypted string.
            try
            {
                string encryptedString = string.Empty;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    byte[] keyBytes = Encoding.ASCII.GetBytes(_encryptionKey.ToCharArray());
                    byte[] unencryptedBytes = Encoding.ASCII.GetBytes(unencryptedString.ToCharArray());
                    using (CryptoStream encryptionStream = new CryptoStream(memoryStream, new RijndaelManaged().CreateEncryptor(keyBytes, _salt), CryptoStreamMode.Write))
                    {
                        encryptionStream.Write(unencryptedBytes, 0, unencryptedBytes.Length);
                        encryptionStream.FlushFinalBlock();
                        encryptionStream.Close();
                    }
                    encryptedString = Convert.ToBase64String(memoryStream.ToArray());
                    memoryStream.Close();
                }
                return encryptedString;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }//Encrypt()

        /// <summary>
        /// Decrypts a string that was encrypted using the "string.Encrypt()" method.
        /// The encryption key, salt, and encryption mechanism match those used
        /// in the internal Regents' program's "reset password" function.
        /// </summary>
        public string Decrypt(string encryptedString)
        {
            //Ensure the encryption key is 32 bytes. Pad with 'X' characters if needed.
            if (_encryptionKey.Length > 32)
            {
                _encryptionKey = _encryptionKey.Substring(0, 32);
            }
            else
            {
                _encryptionKey = _encryptionKey.PadRight(32, 'X');
            }

            //Create the decryptor and send it a byte array of the encrypted string.
            try
            {
                string decryptedString = string.Empty;
                byte[] encryptedBytes = Convert.FromBase64String(encryptedString);
                using (MemoryStream memoryStream = new MemoryStream(encryptedBytes))
                {
                    byte[] keyBytes = Encoding.ASCII.GetBytes(_encryptionKey.ToCharArray());
                    byte[] decryptedBytes = new byte[encryptedBytes.Length];
                    RijndaelManaged encrypter = new RijndaelManaged();
                    using (CryptoStream decryptionStream = new CryptoStream(memoryStream, encrypter.CreateDecryptor(keyBytes, _salt), CryptoStreamMode.Read))
                    {
                        decryptionStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                        decryptionStream.Close();
                    }
                    decryptedString = Encoding.ASCII.GetString(decryptedBytes).Replace("\0", "");
                    memoryStream.Close();
                }
                return decryptedString;
            }
            catch (Exception ex)
            {
                return ex.ToString();
                //return string.Empty;
            }
        }//Decrypt()
        #endregion String Encryption/Decryption
    }//class
}//namespace