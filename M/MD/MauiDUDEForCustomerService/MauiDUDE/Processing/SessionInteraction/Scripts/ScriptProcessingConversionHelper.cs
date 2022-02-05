using Q;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace MauiDUDE
{
    public class ScriptProcessingConversionHelper
    {
        public static Q.MDBorrowerDemographics GetQBorrowerDemographics(Uheaa.Common.Scripts.MDBorrowerDemographics uBor, int depth = 0)
        {
            Q.MDBorrowerDemographics qBor = new DeprecatedDemographics();
            qBor.AccountNumber = uBor.AccountNumber;
            qBor.Addr1 = uBor.Addr1;
            qBor.Addr2 = uBor.Addr2;
            qBor.Addr3 = uBor.Addr3;
            qBor.AltPhone = uBor.AltPhone;
            qBor.City = uBor.City;
            qBor.CLAccNum = uBor.CLAccNum;
            qBor.CompassDemos = depth == 0 && uBor.CompassDemos != null ? GetQBorrowerDemographics(uBor.CompassDemos, depth++) : null; //Any depth of CompassDemos nesting deeper than 1 is set to null
            qBor.Country = uBor.Country;
            qBor.DefaultLetterFormat = uBor.DefaultLetterFormat;
            qBor.DemographicsVerified = uBor.DemographicsVerified;
            qBor.DOB = uBor.DOB;
            qBor.EcorrBilling = uBor.EcorrBilling;
            qBor.EcorrCorrespondence = uBor.EcorrCorrespondence;
            qBor.EcorrTax = uBor.EcorrTax;
            qBor.Email = uBor.Email;
            qBor.FirstName = uBor.FirstName;
            qBor.FName = uBor.FName;
            qBor.ForeignState = uBor.ForeignState;
            qBor.FoundOnSystem = uBor.FoundOnSystem;
            qBor.HomePhoneConsent = uBor.HomePhoneConsent;
            qBor.HomePhoneExt = uBor.HomePhoneExt;
            qBor.HomePhoneForeignCity = uBor.HomePhoneForeignCity;
            qBor.HomePhoneForeignCountry = uBor.HomePhoneForeignCountry;
            qBor.HomePhoneForeignLocalNumber = uBor.HomePhoneForeignLocalNumber;
            qBor.HomePhoneMBL = uBor.HomePhoneMBL;
            qBor.HomePhoneNum = uBor.HomePhoneValidityIndicator;
            qBor.HomePhoneValidityIndicator = uBor.HomePhoneValidityIndicator;
            qBor.HomePhoneVerificationDate = uBor.HomePhoneVerificationDate;
            qBor.IsForeignAddress = uBor.IsForeignAddress;
            qBor.LastName = uBor.LastName;
            qBor.LName = uBor.LName;
            qBor.MI = uBor.MI;
            qBor.Name = uBor.Name;
            qBor.OtherEmail = uBor.OtherEmail;
            qBor.OtherEmail2 = uBor.OtherEmail2;
            qBor.OtherPhone2Consent = uBor.OtherPhone2Consent;
            qBor.OtherPhone2Ext = uBor.OtherPhone2Ext;
            qBor.OtherPhone2ForeignCity = uBor.OtherPhone2ForeignCity;
            qBor.OtherPhone2ForeignCountry = uBor.OtherPhone2ForeignCountry;
            qBor.OtherPhone2ForeignLocalNumber = uBor.OtherPhone2ForeignLocalNumber;
            qBor.OtherPhone2MBL = uBor.OtherPhone2MBL;
            qBor.OtherPhone2Num = uBor.OtherPhone2Num;
            qBor.OtherPhone2ValidityIndicator = uBor.OtherPhone2ValidityIndicator;
            qBor.OtherPhone2VerificationDate = uBor.OtherPhone2VerificationDate;
            qBor.OtherPhone3Consent = uBor.OtherPhone3Consent;
            qBor.OtherPhone3Ext = uBor.OtherPhone3Ext;
            qBor.OtherPhone3ForeignCity = uBor.OtherPhone3ForeignCity;
            qBor.OtherPhone3ForeignCountry = uBor.OtherPhone3ForeignCountry;
            qBor.OtherPhone3ForeignLocalNumber = uBor.OtherPhone3ForeignLocalNumber;
            qBor.OtherPhone3MBL = uBor.OtherPhone3MBL;
            qBor.OtherPhone3Num = uBor.OtherPhone3Num;
            qBor.OtherPhone3ValidityIndicator = uBor.OtherPhone3ValidityIndicator;
            qBor.OtherPhone3VerificationDate = uBor.OtherPhone3VerificationDate;
            qBor.OtherPhoneConsent = uBor.OtherPhoneConsent;
            qBor.OtherPhoneExt = uBor.OtherPhoneExt;
            qBor.OtherPhoneForeignCity = uBor.OtherPhoneForeignCity;
            qBor.OtherPhoneForeignCountry = uBor.OtherPhoneForeignCountry;
            qBor.OtherPhoneForeignLocalNumber = uBor.OtherPhoneForeignLocalNumber;
            qBor.OtherPhoneMBL = uBor.OtherPhoneMBL;
            qBor.OtherPhoneNum = uBor.OtherPhoneNum;
            qBor.OtherPhoneValidityIndicator = uBor.OtherPhoneValidityIndicator;
            qBor.OtherPhoneVerificationDate = uBor.OtherPhoneVerificationDate;
            qBor.Phone = uBor.Phone;
            qBor.POBoxAllowed = uBor.POBoxAllowed.ToString();
            qBor.SPAddrInd = uBor.SPAddrInd;
            qBor.SPAddrVerDt = uBor.SPAddrVerDt;
            qBor.SPEmailInd = uBor.SPEmailInd;
            qBor.SPEmailVerDt = uBor.SPEmailVerDt;
            qBor.SPOt2EmailInd = uBor.SPOt2EmailInd;
            qBor.SPOt2EmailVerDt = uBor.SPOt2EmailVerDt;
            qBor.SPOtEmailInd = uBor.SPOtEmailInd;
            qBor.SPOtEmailVerDt = uBor.SPOtEmailVerDt;
            qBor.SSN = uBor.SSN;
            qBor.State = uBor.State;
            //qBor.TheSystem Not set
            qBor.UPAddrVal = uBor.UPAddrVal;
            qBor.UPAddrVer = uBor.UPAddrVer;
            qBor.UpdatedAlternateFormat = uBor.UpdatedAlternateFormat;
            qBor.UpdatedAlternateFormatId = uBor.UpdatedAlternateFormatId;
            qBor.UPEmailOther2Val = uBor.UPEmailOther2Val;
            qBor.UPEMailOther2Ver = uBor.UPEMailOther2Ver;
            qBor.UPEmailOtherVal = uBor.UPEmailOtherVal;
            qBor.UPEMailOtherVer = uBor.UPEMailOtherVer;
            qBor.UPEmailVal = uBor.UPEmailVal;
            qBor.UPEmailVer = uBor.UPEmailVer;
            qBor.UPOther2Consent = uBor.UPOther2Consent;
            qBor.UPOther2MBL = uBor.UPOther2MBL;
            qBor.UPOther2Val = uBor.UPOther2Val;
            qBor.UPOther2Ver = uBor.UPOther2Ver;
            qBor.UPOtherConsent = uBor.UPOtherConsent;
            qBor.UPOtherMBL = uBor.UPOtherMBL;
            qBor.UPOtherVal = uBor.UPOtherVal;
            qBor.UPOtherVer = uBor.UPOtherVer;
            qBor.UPPhoneConsent = uBor.UPPhoneConsent;
            qBor.UPPhoneMBL = uBor.UPPhoneMBL;
            qBor.UPPhoneNumVer = uBor.UPPhoneNumVer;
            qBor.UPPhoneVal = uBor.UPPhoneVal;
            qBor.Zip = uBor.Zip;

            return qBor;
        }

        public static Q.MDBorrower GetQBorrower(Borrower borrower)
        {
            Q.MDBorrower bor = new Q.MDBorrower();
            bor.SSN = borrower.SSN;
            bor.CLAccNum = borrower.AccountNumber;
            bor.Name = borrower.FullName;
            bor.FirstName = borrower.FirstName;
            bor.LastName = borrower.LastName;
            bor.MI = borrower.MI;
            bor.DOB = borrower.DOB;
            bor.CompassDemos = GetQBorrowerDemographics(borrower.CompassDemographics);
            bor.UserProvidedDemos = GetQBorrowerDemographics(borrower.UpdatedDemographics);
            bor.UserProvidedDemos.SSN = borrower.SSN;
            bor.UserProvidedDemos.FirstName = borrower.FirstName;
            bor.UserProvidedDemos.LastName = borrower.LastName;
            bor.UserProvidedDemos.MI = borrower.MI;
            bor.Principal = borrower.Principal;
            bor.Interest = borrower.Interest;
            bor.DailyInterest = borrower.DailyInterest;
            bor.LoanProgramsDistinctList = new List<string>(borrower.LoanProgramsDistinctList);
            bor.AmountPastDue = borrower.AmountPastDue.ToString().ToDouble();
            bor.ScriptInfoToGenericBusinessUnit = new MDScriptInfoSpecificToCustomerService();
            ((MDScriptInfoSpecificToCustomerService)bor.ScriptInfoToGenericBusinessUnit).CurrentAmountDue = borrower.CurrentAmountDue.ToString().ToDouble();
            ((MDScriptInfoSpecificToCustomerService)bor.ScriptInfoToGenericBusinessUnit).OutstandingLateFees = (borrower.TotalAmountPlusLateFees - borrower.TotalAmountDue).ToString().ToDouble();
            //monthly payment amount from the warehouse is a comma delimited string so the amounts must be split out and summed
            decimal tempMonthlyPaymentAmount = 0;
            if (borrower.MonthlyPaymentAmount.Length != 0)
            {
                string mpaString = borrower.MonthlyPaymentAmount.Replace("$", "").Replace(" ", "");
                List<string> tempMPAs = mpaString.Split(',').ToList();
                foreach (string mpa in tempMPAs)
                {
                    tempMonthlyPaymentAmount += mpa.Replace("RPF=", "").ToDecimal();
                }
            }
            ((MDScriptInfoSpecificToCustomerService)bor.ScriptInfoToGenericBusinessUnit).MonthlyPaymentAmount = tempMonthlyPaymentAmount.ToString().ToDouble();
            ((MDScriptInfoSpecificToCustomerService)bor.ScriptInfoToGenericBusinessUnit).HasRepaymentSchedule = borrower.LastRPSPrintDate.Length != 0 ? "N" : "Y";
            return bor;
        }

        public static Uheaa.Common.Scripts.MDBorrower GetUheaaCommonBorrower(Borrower borrower)
        {
            Uheaa.Common.Scripts.MDBorrower bor = new Uheaa.Common.Scripts.MDBorrower();
            bor.Ssn = borrower.SSN;
            bor.AccountNumber = borrower.AccountNumber;
            bor.Name = borrower.FullName;
            bor.FirstName = borrower.FirstName;
            bor.LastName = borrower.LastName;
            bor.MI = borrower.MI;
            bor.DOB = borrower.DOB;
            bor.CompassDemos = borrower.CompassDemographics;
            bor.UserProvidedDemos = borrower.UpdatedDemographics;
            bor.UserProvidedDemos.SSN = borrower.SSN;
            bor.UserProvidedDemos.FirstName = borrower.FirstName;
            bor.UserProvidedDemos.LastName = borrower.LastName;
            bor.UserProvidedDemos.MI = borrower.MI;
            bor.Principal = borrower.Principal;
            bor.Interest = borrower.Interest;
            bor.DailyInterest = borrower.DailyInterest;
            bor.LoanProgramsDistinctList = new List<string>(borrower.LoanProgramsDistinctList);
            bor.AmountPastDue = borrower.AmountPastDue;
            bor.CurrentAmountDue = borrower.CurrentAmountDue;
            bor.OutstandingLateFees = borrower.TotalAmountPlusLateFees - borrower.TotalAmountDue;
            //monthly payment amount from the warehouse is a comma delimited string so the amounts must be split out and summed
            decimal tempMonthlyPaymentAmount = 0;
            if (borrower.MonthlyPaymentAmount.Length != 0)
            {
                string mpaString = borrower.MonthlyPaymentAmount.Replace("$", "").Replace(" ", "");
                List<string> tempMPAs = mpaString.Split(',').ToList();
                foreach (string mpa in tempMPAs)
                {
                    tempMonthlyPaymentAmount += mpa.Replace("RPF=", "").ToDecimal();
                }
            }
            bor.MonthlyPaymentAmount = tempMonthlyPaymentAmount;
            bor.HasRepaymentSchedule = borrower.LastRPSPrintDate.Length != 0;
            return bor;
        }
    }
}
