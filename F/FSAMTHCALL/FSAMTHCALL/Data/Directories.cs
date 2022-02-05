using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace FSAMTHCALL
{
    class Directories
    {
        public string CallsDirectory =>
#if DEBUG
            EnterpriseFileSystem.TempFolder; //Developers don't have access to test folder
#else
            EnterpriseFileSystem.GetPath("FSACALLS");
#endif
        public string SupervisorDirectory =>
#if DEBUG
            EnterpriseFileSystem.TempFolder; //Developers don't have access to test folder
#else
            EnterpriseFileSystem.GetPath("FSACALLSUP");
#endif
        public string SelectedDirectory =>
#if DEBUG
            EnterpriseFileSystem.TempFolder; //Developers don't have access to test folder
#else
            EnterpriseFileSystem.GetPath("FSASELECTED");
#endif
    }
}