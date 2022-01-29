using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcorrLetterSetup
{
    public class SystemSprocs
    {
        public int SystemLettersStoredProcedureId { get; set; }
        public string StoredProcedureName { get; set; }
        public string ReturnType { get; set; }
        public bool Active { get; set; }
    }
}
