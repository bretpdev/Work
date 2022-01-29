using System;
using System.Data.Linq;
using System.IO;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts.Exceptions;

namespace Uheaa.Common.Scripts
{
    public static class ScriptHelper
    {
        [Flags]
        public enum FileOptions
        {
            None,
            ErrorOnEmpty,
            ErrorOnMissing
        }

        [Flags]
        public enum DeleteActivityCommentsResults
        {
            ArcNotFoundOrInactive,
            SsnIsNotABorrower,
            SsnNotFount,
            Success
        }

        public static string DeleteOldFilesReturnMostCurrent(string path, string searchPattern, FileOptions options)
        {
            string[] foundFiles = Directory.GetFiles(path, searchPattern);

            //Check to see if the file is missing
            if (foundFiles.Length == 0)
            {
                if (options == FileOptions.ErrorOnMissing)
                    throw new FileNotFoundException();
                else
                    return string.Empty;
            }

            //Find the newest file
            string newestFile = foundFiles[0];
            
            //Remove the other files
            foreach (string file in foundFiles)
            {
                if (File.GetLastWriteTime(file) < File.GetLastWriteTime(newestFile))
                    FS.Delete(file);
                else if (File.GetLastWriteTime(file) > File.GetLastWriteTime(newestFile))
                {
                    FS.Delete(newestFile);
                    newestFile = file;
                }
            }

            //Check to see if the file is empty
            if (options == FileOptions.ErrorOnEmpty && File.Open(newestFile, FileMode.Open).Length == 0)
                throw new FileEmptyException("File Empty", newestFile);

            return newestFile;
        }
        //undone if you need this make it a sproc
        public static string GetScriptName(string scriptId)
        {
            DataContext context = DataAccessHelper.GetContext(DataAccessHelper.Database.Bsys);
            return context.ExecuteQuery<string>("SELECT Script FROM SCKR_DAT_Scripts WHERE ID = {0}", scriptId).SingleOrDefault();
        }


        public static DeleteActivityCommentsResults DeleteActivityComments(ReflectionInterface ri, string ssn, string arc, DateTime fromDate, DateTime toDate)
        {
            ri.FastPath(string.Format("TX3ZCTD2A{0}", ssn));
            ri.PutText(11, 65, arc);
            ri.PutText(21, 16, fromDate.ToString("MMddyy"));
            ri.PutText(21, 30, toDate.ToString("MMddyy"));
            ri.Hit(ReflectionInterface.Key.Enter);
            //Check for ARC error
            switch (ri.GetText(23, 2, 78))
            {
                case "01488":
                    return DeleteActivityCommentsResults.ArcNotFoundOrInactive;
                case "01080":
                    return DeleteActivityCommentsResults.SsnIsNotABorrower;
                case "01019":
                    return DeleteActivityCommentsResults.SsnIsNotABorrower;
                default:
                    if (ri.CheckForText(1, 72, "TDX2C"))
                    {
                        ri.PutText(5, 14, "X", ReflectionInterface.Key.Enter);
                        while (!ri.CheckForText(23, 2, "90007"))
                        {
                            ri.PutText(6, 18, "E", ReflectionInterface.Key.Enter);
                            ri.Hit(ReflectionInterface.Key.F8);
                        }
                    }
                    else
                        ri.PutText(6, 18, "E", ReflectionInterface.Key.Enter);
                    return DeleteActivityCommentsResults.Success;
            }//End Switch
        }// DeleteActivityCommentResults

    }//Class
}//Namespace
