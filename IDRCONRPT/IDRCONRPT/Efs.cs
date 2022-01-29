using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace IDRCONRPT
{
    public class Efs
    {
        const string SearchKey = Processor.ScriptId + "-Search";
        public string SearchDirectory { get { return EnterpriseFileSystem.GetPath(SearchKey); } }
        const string PatternKey = Processor.ScriptId + "-Pattern";
        public string SearchPattern { get { return EnterpriseFileSystem.GetPath(PatternKey); } }
        const string OutputKey = Processor.ScriptId + "-Output";
        public string OutputDirectory { get { return EnterpriseFileSystem.GetPath(OutputKey); } }
        const string ReviewKey = Processor.ScriptId + "-Review";
        public string ReviewDirectory { get { return EnterpriseFileSystem.GetPath(ReviewKey); } }
        const string EmailKey = "Consol Reject Report – FED";
        public string ErrorReportEmail { get { return EnterpriseFileSystem.GetPath(EmailKey); } }
        public void EnsureDirectoriesExist()
        {
            foreach (var dir in new string[] { OutputDirectory, SearchDirectory, ReviewDirectory })
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
        }
    }
}
