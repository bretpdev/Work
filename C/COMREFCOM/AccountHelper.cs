using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace COMREFCOM
{
    public static class AccountHelper
    {
        private const string AccountLocation = @"Q:\Operational Accounting\COMPASS Refund Comments\";
        private const string TestAccountLocation = @"Q:\Support Services\Test Files\COMREFCOM";
        private const string fileName = "encrypted_bank_account.txt";
        public static bool LoadAccountNumber()
        {
            string path = Path.Combine(DataAccessHelper.TestMode ? TestAccountLocation : AccountLocation, fileName);
            string error = "Please verify " + path + " exists and that you have access to it.";
            if (!File.Exists(path))
            {
                Dialog.Warning.Ok(error);
                return false;
            }
            string encryptedText = "";
            try
            {
                encryptedText = File.ReadAllText(path);
            }
            catch (Exception)
            {
                Dialog.Warning.Ok(error);
                return false;
            }
            AccountNumber = LegacyCryptography.Decrypt(encryptedText, LegacyCryptography.Keys.COMREFCOM);
            return true;
        }

        public static string AccountNumber { get; private set; }
    }
}
