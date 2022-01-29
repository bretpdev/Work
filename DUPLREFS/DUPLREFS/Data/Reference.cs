using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace DUPLREFS
{
    public class Reference
    {
        public int? ReferenceQueueId { get; set; }
        public int? BorrowerQueueId { get; set; }
        public string RefId { get; set; }
        public string RefName { get; set; }
        public string RefStatus { get; set; }
        public string RefAddress1 { get; set; } //Ref address line 1
        public string RefAddress2 { get; set; } //Ref address line 2
        public string RefCity { get; set; }
        public string RefState { get; set; }
        public string RefPhone { get; set; }
        public string RefZip { get; set; }
        public string RefCountry { get; set; } 
        public bool? ValidAddress { get; set; }
        public bool? ValidPhone { get; set; }
        public bool? DemosChanged { get; set; } // Whether script updated the reference's demos
        public bool? ZipChanged { get; set; } // We only care about this for validation in order to accomodate session zip restrictions when it is changed
        public bool? ManuallyWorked { get; set; } // Whether user manually worked the reference rather than the script processing it
        public bool Duplicate { get; set; }
        public bool PossibleDuplicate { get; set; }
        public int? ArcAddProcessingId { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public DateTime? Lp2fProcessedAt { get; set; }

        public override string ToString()
        {
            string representation = "";
            foreach (PropertyInfo propertyInfo in Type.GetType("Reference").GetProperties())
            {
                string name = propertyInfo.Name;
                object value = propertyInfo.GetValue(this);
                representation += $"{name}: {value ?? ""},";
            }
            return representation.Remove(representation.Length - 1, 1); // Remove the last comma
        }

        public string GetRefNumber()
        {
            return RefId.Replace("RF@", "");
        }

        public bool HasDifferentDemos(Reference rfr)
        {
            bool differentDemos = (RefAddress1 != rfr.RefAddress1
                || RefAddress2 != rfr.RefAddress2
                || RefCity != rfr.RefCity
                || RefState != rfr.RefState
                || RefZip != rfr.RefZip
                || RefPhone != rfr.RefPhone
                || RefCountry != rfr.RefCountry
                );
            return differentDemos;
        }
    }
}
