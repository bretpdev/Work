using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIntermediary
{
    public class AcpSelectionResult
    {
        public ActivityCode ActivityCodeSelection { get; set; }
        public ContactCode ContactCodeSelection { get; set; }

        public string ActivityCodeValue { get; set; }
        public string ContactCodeValue { get; set; }
        public string DescriptionValue { get; set; }

        public string Lp22SourceValue { get; set; }
        public string Tx1jSourceValue { get; set; }

        public CallType CallType { get; set; }
        public CallRecipientTarget? RecipientTarget { get; set; }
        public Reference RecipientTargetReference { get; set; }
        public CallStatusType? CallStatusType { get; set; }
        public AcpRecipientInfo RecipientInfo { get; set; }
    }

}
