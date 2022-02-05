using System.Collections.Generic;
using System.IO;

namespace SftpCoordinator
{
    public class FileOpResults
    {
        public string InitialFilepath { get; private set; }
        public string InitialFilename
        {
            get { return Path.GetFileName(InitialFilepath); }
        }
        public string ChangedFilename { get; set; }

        public string FilePath { get; private set; }
        public string ErrorMessage { get; private set; }
        public bool? ProcessSuccessful { get; private set; }
        public string PreDecryptionArchiveLocation { get; set; }
        public string PreEncryptionArchiveLocation { get; set; }
        public List<string> CleanupPaths = new List<string>();
        private FileOpResults() { }

        #region Static Methods
        public static FileOpResults Initial(string filepath)
        {
            var results = new FileOpResults() { FilePath = filepath, InitialFilepath = filepath };
            results.ChangedFilename = results.InitialFilename;
            results.CleanupPaths.Add(results.InitialFilepath);
            return results;
        }
        public static FileOpResults Fail(FileOpResults previous, string errorMessage, params string[] formatParameters)
        {
            return Generate(previous, previous.FilePath, false, string.Format(errorMessage, formatParameters));
        }
        public static FileOpResults NotApplicable(FileOpResults previous)
        {
            return Generate(previous, null);
        }
        public static FileOpResults Success(FileOpResults previous, string newFilePath)
        {
            return Generate(previous, newFilePath, true);
        }
        private static FileOpResults Generate(FileOpResults previous, bool? successful)
        {
            return Generate(previous, previous.FilePath, successful);
        }
        private static FileOpResults Generate(FileOpResults previous, string newFilePath, bool? successful, string errorMessage = null)
        {
            //set new settings, keep other settings
            previous.ProcessSuccessful = successful;
            previous.FilePath = newFilePath;
            previous.ErrorMessage = errorMessage;
            return previous;
        }
        #endregion
    }
}
