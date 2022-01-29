using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace IDRXMLDATA
{
    public class ExistingApp
    {
        [DbName("AccountNumber")]
        public string AccountNumber { get; set; }
        [DbName("FirstName")]
        public string FirstName { get; set; }
        [DbName("LastName")]
        public string LastName { get; set; }
        [DbName("EappId")]
        public string EappId { get; set; }
    }
}
