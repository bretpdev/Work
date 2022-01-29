using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace TPERM
{
    public class ReferenceData
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string RelationshipCode { get; set; }
        public string Relationship { get; set; }
        public string BorrowerSignatureName { get; set; }
        public AddressDemographics RefAddress { get; set; }
        public ReferencePhone HomePhone { get; set; }
        public ReferencePhone AltPhone { get; set; }
        public ReferencePhone WorkPhone { get; set; }
        public ReferencePhone MobilePhone { get; set; }
        public bool NotAuthed { get; set; }

        public string ConcatName()
        {
            if (MiddleName.IsNullOrEmpty())
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            else
                return string.Format("{0} {1} {2}", this.FirstName,this.MiddleName, this.LastName);
        }
    }
}
