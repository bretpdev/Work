using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRIDRCRP
{
    public class Error
    {
        public int? BorrowerInformationId { get; set; }
        public string ErrorMessage { get; set; }

        public Error(int? borrowerInformationId, string errorMessage)
        {
            BorrowerInformationId = borrowerInformationId;
            ErrorMessage = errorMessage;
        }
    }
}
