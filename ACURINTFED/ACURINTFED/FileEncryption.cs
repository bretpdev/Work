using System.Diagnostics;
using System.IO;
using System;
using Uheaa.Common.DataAccess;

namespace ACURINTFED
{
	class FileEncryption
	{
		//Note that we're using the command-line GnuPG rather than an API like BouncyCastle,
		//because anyone running the script already has GnuPG (for importing keys, if nothing else).
		//the gpg.exe installs in different locations based upon which OS is running
        private static string GPG_EXECUTABLE = File.Exists(EnterpriseFileSystem.GetPath("GPG")) ? EnterpriseFileSystem.GetPath("GPG") : EnterpriseFileSystem.GetPath("GPG2");

		public static bool EncryptionSoftwareIsInstalled { get { return File.Exists(GPG_EXECUTABLE); } }

		public static bool DecryptFile(string encryptedFile, string outputFile)
		{
            BatchProcessingHelper login = BatchProcessingHelper.GetNextAvailableId("ACURINTFED", "AccurintPGP");
            string gpgArgs = string.Format("--output {0} --batch --passphrase {2} --decrypt {1}", outputFile, encryptedFile, login.Password);
			using (Process gpg = Process.Start(GPG_EXECUTABLE, gpgArgs)) { gpg.WaitForExit(); }
            BatchProcessingHelper.CloseConnection(login);
			return File.Exists(outputFile);
		}//DecryptFile()

		public static bool EncryptFile(string unencryptedFile, string outputFile)
		{
            string gpgArgs = string.Format("--output {0} --encrypt --recipient \"{2}\" {1}", outputFile, unencryptedFile, EnterpriseFileSystem.GetPath("AccurintFile"));
			using (Process gpg = Process.Start(GPG_EXECUTABLE, gpgArgs)) { gpg.WaitForExit(); }
			return File.Exists(outputFile);
		}//EncryptFile()
	}//class
}//namespace
