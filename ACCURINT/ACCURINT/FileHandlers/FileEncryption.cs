using System.Diagnostics;
using System.IO;
using System.Threading;
using Uheaa.Common.DataAccess;

namespace ACCURINT
{
	public class FileEncryption
	{
		public DataAccess DA { get; set; }

		public FileEncryption(DataAccess da)
        {
			DA = da;
        }

		//Note that we're using the command-line GnuPG rather than an API like BouncyCastle,
		//because anyone running the script already has GnuPG (for importing keys, if nothing else).
		//the gpg.exe intalls in different locations based upon which OS is running and which
		//version it is.
		private static string GPG_EXECUTABLE = GetGpgExeLocation();

		public bool EncryptionSoftwareIsInstalled { get { return File.Exists(GPG_EXECUTABLE); } }

		private static string GetGpgExeLocation()
        {
			string filePath = "";
			if (File.Exists(@"C:\Program Files\GNU\GnuPG\gpg.exe"))
				filePath = @"C:\Program Files\GNU\GnuPG\gpg.exe";
			else if (File.Exists(@"C:\Program Files (x86)\GNU\GnuPG\gpg2.exe"))
				filePath = @"C:\Program Files (x86)\GNU\GnuPG\gpg2.exe";
			else if (File.Exists(@"C:\Program Files (x86)\GnuPG\bin\gpg.exe"))
				filePath = @"C:\Program Files (x86)\GnuPG\bin\gpg.exe";

			return filePath;
		}

		public bool DecryptFile(string encryptedFile, string outputFile)
		{
            if (DataAccessHelper.CurrentMode != DataAccessHelper.Mode.Live)
            {
                File.Copy(encryptedFile, outputFile);
                return true;
            }
            else
            {
                BatchProcessingHelper login = BatchProcessingHelper.GetNextAvailableId("ACURINTFED", "AccurintPGP");
                string gpgArgs = string.Format("--output {0} --decrypt --recipient \"Marty Hamilton\" --passphrase {2} --batch {1}", outputFile, encryptedFile, DA.GetBatchProcessingPassword(login.UserName));
                using (Process gpg = Process.Start(GPG_EXECUTABLE, gpgArgs)) { gpg.WaitForExit(); }
                BatchProcessingHelper.CloseConnection(login);
                return File.Exists(outputFile);
            }
		}

		public bool EncryptFile(string unencryptedFile, string outputFile)
		{
            string gpgArgs = string.Format("--output {0} --encrypt --recipient \"{2}\" {1}", outputFile, unencryptedFile, EnterpriseFileSystem.GetPath("AccurintFile"));
			using (Process gpg = Process.Start(GPG_EXECUTABLE, gpgArgs))
			{
				for (int seconds = 0; seconds < 30 && !gpg.HasExited; seconds++) { Thread.Sleep(1000); }
			}
			return File.Exists(outputFile);
		}
	}
}
