using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace SftpCoordinator
{
    public static class CryptographyHelper
    {
        public static bool KeysAvailable { get; internal set; }
        public static bool GpgAvailable { get { return File.Exists(EnterpriseFileSystem.GetPath(Settings.PgpLocation)); } }
        public const string Extension = ".pgp";
        public static string Encrypt(string filename)
        {
            string error = Pgp($"-e -r \"{("AES PHEAA PROD 20201116 (CLDATA.DOE.742)")}\" --trust-model always --output {filename + Extension} {filename}");
            if (!string.IsNullOrEmpty(error))
                throw new EncryptionException(error);
            return filename + Extension;
        }

        public static string Decrypt(string filename)
        {
            string newName = filename.Replace(Extension, "");
            string format = $"--output {filename.Replace(Extension, "")} --decrypt --recipient \"{("UHEAA (UHEAA's KEY RING) <noc@utahsbr.edu>")}\" --passphrase {Settings.UheaaPassphrase} --batch {filename}";
            string error = Pgp(format, (lines) => lines.First().StartsWith("gpg: encrypted with"));
            if (!string.IsNullOrEmpty(error))
                throw new DecryptionException(error);
            Repeater.TryRepeatedly(() => FS.Delete(filename.UpdatePath()));
            return newName;
        }

        public static string Pgp(string arguments, Func<IEnumerable<string>, bool> checkOutput = null)
        {
            using (Process gpg = Proc.Start(Settings.PgpLocation, arguments, false, true, true))
            {
                List<string> lines = new List<string>();
                while (!gpg.StandardError.EndOfStream)
                    lines.Add(gpg.StandardError.ReadLine());
                gpg.WaitForExit();
                if (checkOutput != null && !checkOutput(lines))
                    return string.Join(Environment.NewLine, lines.ToArray());
            }
            return null;
        }
        static CryptographyHelper()
        {
            if (GpgAvailable)
            {
                Pgp($"--import \"{Settings.UheaaPublicKeyring}\"");
                Pgp($"--import \"{Settings.UheaaPrivateKeyring}\"");
                Pgp($"--import \"{Settings.AesPublicKey}\"");
            }
            if (!File.Exists(Settings.UheaaPublicKeyring) || !File.Exists(Settings.UheaaPrivateKeyring) || !File.Exists(Settings.AesPublicKey) || Settings.UheaaPassphrase == null)
                KeysAvailable = false;
            else
                KeysAvailable = true;
        }
    }
}
