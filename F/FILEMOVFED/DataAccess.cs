using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Q;

namespace FILEMOVFED
{
    class DataAccess : DataAccessBase
    {
        //get a list of files to be moved
        public static List<FileData> GetFileData(bool testMode)
        {
            return CSYSDataContext(testMode).ExecuteQuery<FileData>("EXEC spFILE_GetFilesToMove").ToList();
        }

        //update the last processed timestamp for the file
        public static void UpdateLastProcessedTimeStamp(bool testMode, string fileName)
        {
            CSYSDataContext(testMode).ExecuteCommand(string.Format("EXEC spFILE_UpdateLastProcessed {0}", fileName));
        }

        //get the name of the script from Sacker
        public static string GetScriptName(string scriptId)
        {
            return Q.DataAccess.GetScriptName(scriptId);
        }
    } 
}


//spFILE_GetFilesToMove
//FILE_DAT_FilesToMove
//FILE_DAT_WhenToCheck
//spFILE_UpdateLastProcessed