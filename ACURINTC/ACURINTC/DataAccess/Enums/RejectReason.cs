using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACURINTC
{
    public enum RejectReason
    {
        InvalidAddressHistory = 1,
        SameAddress =2,
        InvalidDemographics = 3,
        WrongForwarding = 4,
        NoPdem = 5,
        None = 6,
        BlockedPhone = 7,
        InvalidPhoneHistory = 8,
        SamePhone = 9,
        NoStreet = 10,
        ForeignDemographics = 11,
        IncompleteAddress = 12,
        IncompletePhone = 13,
        InvalidAddressReturn = 14,
        InvalidPhoneReturn = 15,
        ValidAddress = 16,
        ValidPhones = 17,
        PhoneTooLong = 18
    }
}
