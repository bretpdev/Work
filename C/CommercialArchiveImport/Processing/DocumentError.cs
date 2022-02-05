using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommercialArchiveImport
{
    public class DocumentError
    {
        public DocumentErrorType ErrorType { get; set; }
        public string ErrorMessage { get; set; }
        public DocumentError(string message, DocumentErrorType type = DocumentErrorType.Unspecified)
        {
            this.ErrorType = type;
            this.ErrorMessage = message;
        }
    }
}
