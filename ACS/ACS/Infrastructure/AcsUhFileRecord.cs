using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace ACS
{
    public class AcsUhFileRecord
    {
        public int UheaaDemographicsId { get; set; }
        public char PersonType { get; set; }
        [DbIgnore]
        public PersonTypeEnum PType { get => (PersonTypeEnum)PersonType; }
        public string SSN { get; set; }
        public string AccountNumber { get; set; }
        [DbName("AddrDate")]
        public string POAddressDate { get; set; }
        [DbName("AddrType")]
        public string AddressType { get; set; }
        [DbIgnore]
        public string FullName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        [DbIgnore]
        public Address NewAddress { get; set; } = new Address();
        [DbIgnore]
        public FormattedAddress ConcatenatedData { get; set; } = new FormattedAddress();
        public string NewAddressFull { get; set; }
        public string OldAddressFull { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Valid { get; set; }
        public string FileId { get; set; }
    }
}
