using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFTPXML
{
    class ErrorFiles
    {
        public int FailCount { get; set; }
        public string FileName { get; set; }
        public Exception Ex { get; set; }
    }
}
