using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookImagingAddin
{
    class Document
    {
        public string DocId { get; set; }
        public string DocType { get; set; }
        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(DocId))
                    return "";
                return DocId + " (" + DocType + ")";
            }
        }
    }
}
