using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMGCLIMPRT
{
    class BatchItem
    {
        public string LoanId { get; set; }
        public string Ssn { get; set; }
        //public string BorrowerFirstName { get; set; }
        //public string BorrowerLastName { get; set; }
        //public string DisbursementDate { get; set; }
        //public string DisbursementType { get; set; }
        public string DocumentType { get; set; }
        //public string UniqueDocumentId { get; set; }
        public string DocumentDate { get; set; }
        public string DocumentFileName { get; set; }

        public string DocId
        {
            get
            {
                return 
                    DocumentTypeHelper.GetDocId(DocumentType) 
                    ??
                    DocumentTypeHelper.GetDocId((DocumentType ?? "").Split('-')[0]);
            }
        }
    }
}
