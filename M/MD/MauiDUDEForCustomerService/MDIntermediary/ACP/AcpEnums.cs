using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDIntermediary
{
    public enum CallType
    {
        IncomingCall,
        OutgoingCall,
        OfficeVisit
    }

    public enum CallRecipientTarget
    {
        Borrower,
        Endorser,
        ThirdParty,
        Reference
    }

    public enum CallStatusType
    {
        CallSuccessful,
        NoAnswer,
        AnsweringMachine,
        WrongNumber,
        PhoneBusy,
        OutOfService
    }

    public enum ContactCode
    {
        ToAttorney,
        FromAttorney,
        ToBorrower,
        FromBorrower,
        ToComaker,
        FromComaker,
        ToCreditBureau,
        FromCreditBureau,
        ToDmv,
        FromDmv,
        ToEmployer,
        FromEmployer,
        ToEndorser,
        FromEndorser,
        ToFamily,
        FromFamily,
        ToGuarantor,
        FromGuarantor,
        ToLender,
        FromLender,
        ToMisc,
        FromMisc,
        ToPostOffice,
        FromPostOffice,
        ToPrison,
        FromPrison,
        ToReference,
        FromReference,
        ToSchool,
        FromSchool,
        ToUheaa,
        FromUheaa,
        To3rdParty,
        From3rdParty
    }

    public enum ActivityCode
    {
        AccountMaintenance,
        CourtDocument,
        Claim,
        Email,
        Fax,
        Form,
        Letter,
        Misc,
        OfficeVisit,
        TelephoneContact,
        FailedCall
    }
}
