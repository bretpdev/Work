//------------------------------------------------------------------------------
// <copyright file="CSSqlFunction.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{

    static readonly string EncryptionString = "To use OR nOt To use tHe OpS web";
    static readonly byte[] iv = new byte[] { 121, 9, 10, 1, 31, 74, 11, 39, 255, 91, 45, 78, 55, 66, 22, 7 };

    [SqlFunction()]
    public static SqlString Encrypt(string text)
    {
        string key = EncryptionString.Trim().PadRight(32, 'X').Substring(0, 32);
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

    [SqlFunction()]
    public static SqlString Decrypt(string text)
    {
        string key = EncryptionString.Trim().PadRight(32, 'X').Substring(0, 32);
        byte[] textArray = Convert.FromBase64String(text); ;
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
}
