using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using MDIntermediary;
using Uheaa.Common;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace MauiDUDE
{
    public class DataAccess
    {
        public static DataAccess DA { get; set; }

        private enum AccountIdentifierType
        {
            SSN,
            AccountNumber
        }

        public static string SETTINGS_FILE = Path.Combine(EnterpriseFileSystem.TempFolder,"MauiDUDEPref.dat");

        public LogDataAccess LDA { get; set; }
        public DataAccessHelper.Database DB { get; } = DataAccessHelper.Database.MauiDude;

        public DataAccess(ProcessLogRun logRun)
        {
            if(!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), true))
            {
                throw new Exception("No Sproc Access");
            }

            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, true, true);
            DA = this;
        }

        public SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "GetIVRResponseCode")]
        public string GetIVRResponse(string accountNumber)
        {
            return LDA.ExecuteList<string>("GetIVRResponseCode", DB, SP("AccountNumber", accountNumber)).Result.FirstOrDefault();
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "GetScriptAndServicesMenuOptions")]
        public DataTable GetScriptAndServicesMenuOptions(string homePage, string parentMenu)
        {
            return LDA.ExecuteDataTable("GetScriptAndServicesMenuOptions", DB, false, SP("HomePage", homePage), SP("ParentMenu", parentMenu)).Result;
        }

        [UsesSproc(DataAccessHelper.Database.MauiDude, "GetACPSelectionByDescription")]
        [UsesSproc(DataAccessHelper.Database.MauiDude, "GetACPSelection")]
        public AcpEntryInfo GetACPSelection(string activityCode, string contactCode, string attemptType = "")
        {
            AcpEntryInfo result = null;
            if(attemptType != "")
            {
                result = LDA.ExecuteSingle<AcpEntryInfo>("GetACPSelectionByDescription", DB, SP("ActivityCode", activityCode), SP("ContactCode", contactCode), SP("Description", attemptType)).Result;
            }
            else
            {
                result = LDA.ExecuteSingle<AcpEntryInfo>("GetACPSelection", DB, SP("ActivityCode", activityCode), SP("ContactCode", contactCode)).Result;
            }

            if(result == null)
            {
                //if the query return nothing then return a ACPEntryInformation object that will result in a "4" or a general comment being added
                result = new AcpEntryInfo() { AcpSelection = "4" };
            }

            return result;
        }

        #region Borrower Data Collection

        //Uses Sproc cannot be applied to the Warehouse tables since default uses MD for Uheaa
        public Borrower GetFFELPOneLINKBorrowerFromWarehouse(string accountIdentifier)
        {
            string identifierType;
            if(accountIdentifier.Length == 10)
            {
                identifierType = Enum.GetName(typeof(AccountIdentifierType), AccountIdentifierType.AccountNumber);
            }
            else
            {
                identifierType = Enum.GetName(typeof(AccountIdentifierType), AccountIdentifierType.SSN);
            }
            List<Borrower> results = LDA.ExecuteList<Borrower>("spMD_GetOneLINKBorrowerLevelData", DataAccessHelper.Database.Udw, SP("AccountIdentifier", accountIdentifier), SP("IdentifierType", identifierType)).Result;
            if(results.Count == 0)
            {
                throw new BorrowerNotFoundInWarehouseException("The borrower couldn't be found in the warehouse. Please try again.");
            }

            Borrower locatedBorrower = results.First();
            //get call forwarding information
            locatedBorrower.CallForwardingWarehouseData = GetCallForwardingData(locatedBorrower.AccountNumber, DataAccessHelper.Region.Uheaa);
            return locatedBorrower;
        }

        /// <summary>
        /// Gets uheaa borrower from data warehouse. Throws BorrowerNotFoundInWarehouseException if borrower is not found
        /// </summary>
        public UheaaBorrower GetMinimizedUheaaCompassBorrowerFromWarehouse(string accountIdentifier)
        {
            string identifierType;
            if (accountIdentifier.Length == 10)
            {
                identifierType = Enum.GetName(typeof(AccountIdentifierType), AccountIdentifierType.AccountNumber);
            }
            else
            {
                identifierType = Enum.GetName(typeof(AccountIdentifierType), AccountIdentifierType.SSN);
            }

            List<Borrower> results = LDA.ExecuteList<Borrower>("spMD_GetCompassBorrower", DataAccessHelper.Database.Udw, SP("AccountIdentifier", accountIdentifier)).Result;
            if (results.Count == 0)
            {
                throw new BorrowerNotFoundInWarehouseException("The borrower wasn't found in the UHEAA data warehouse.");
            }
            UheaaBorrower locatedBorrower = new UheaaBorrower(results.First());

            GetDemographicData(locatedBorrower.AccountNumber, DataAccessHelper.Region.Uheaa, locatedBorrower);

            //Get reference information
            locatedBorrower.References = GetReferenceDataFromWarehouse(locatedBorrower.AccountNumber, DataAccessHelper.Region.Uheaa);

            //Get call forwarding information
            locatedBorrower.CallForwardingWarehouseData = GetCallForwardingData(locatedBorrower.AccountNumber, DataAccessHelper.Region.Uheaa);

            //return built borrower
            return locatedBorrower;
        }

        /// <summary>
        /// Gets uheaa borrower from data warehouse. Throws BorrowerNotFoundInWarehouseException if borrower is not found
        /// </summary>
        public UheaaBorrower GetUheaaCompassBorrowerFromWarehouse(UheaaBorrower locatedBorrower, string accountIdentifier)
        {
            string identifierType;
            if (accountIdentifier.Length == 10)
            {
                identifierType = Enum.GetName(typeof(AccountIdentifierType), AccountIdentifierType.AccountNumber);
            }
            else
            {
                identifierType = Enum.GetName(typeof(AccountIdentifierType), AccountIdentifierType.SSN);
            }

            List<Borrower> results = LDA.ExecuteList<Borrower>("spMD_GetCompassBorrowerLevelData", DataAccessHelper.Database.Udw, SP("AccountIdentifier", accountIdentifier), SP("IdentifierType", identifierType)).Result;
            //This should never happen at this point unless the database changed/was disconnected
            if(results.Count == 0)
            {
                throw new BorrowerNotFoundInWarehouseException("The borrower wasn't found in the UHEAA data warehouse.");
            }
            locatedBorrower.Replicate(results.First());

            //Get loan information
            locatedBorrower.CompassLoans = GetCompassLoanLevelDataFromWarehouse(locatedBorrower.AccountNumber, DataAccessHelper.Region.Uheaa);
            locatedBorrower.LoanProgramsDistinctList = locatedBorrower.CompassLoans.Select(p => p.LoanType).Distinct().ToList();
            locatedBorrower.LoanStatusDistinctList = locatedBorrower.CompassLoans.Select(p => p.Status).Distinct().ToList();
            locatedBorrower.LoanStatusListForLegal = locatedBorrower.CompassLoans.Where(p => 
                p.StatusCode == "06" ||
                p.StatusCode == "16" ||
                p.StatusCode == "17" ||
                p.StatusCode == "18" ||
                p.StatusCode == "19" ||
                p.StatusCode == "20" ||
                p.StatusCode == "21").Select(p => p.Status).Distinct().ToList();

            //Get ACH information
            locatedBorrower.ACHData = GetACHFromWarehouse(locatedBorrower.AccountNumber, DataAccessHelper.Region.Uheaa);
            //Get employer information
            locatedBorrower.EmployerDemo = GetEmployerInformationFromWarehouse(locatedBorrower.AccountNumber, DataAccessHelper.Region.Uheaa);
            //Get financial transactions
            locatedBorrower.FinancialTransactionHistory = GetFinancialTransactionInformation(locatedBorrower.AccountNumber, DataAccessHelper.Region.Uheaa);
            //Get RPS Types
            locatedBorrower.RPSTypes = GetRPSTypes(locatedBorrower.AccountNumber, DataAccessHelper.Region.Uheaa);
            //Get Bill Types
            locatedBorrower.BillTypes = GetBillTypes(locatedBorrower.AccountNumber, DataAccessHelper.Region.Uheaa);
            //Get Bill data
            locatedBorrower.Bills = GetBillingInformation(locatedBorrower.AccountNumber, DataAccessHelper.Region.Uheaa);
            //Get deferment and forbearances
            locatedBorrower.DefermentsAndForbearences = GetDefermentsAndForbearances(locatedBorrower.AccountNumber, DataAccessHelper.Region.Uheaa);
            //Get dates 20-day letters were sent
            locatedBorrower.DatesOf20DayLettersSent = Get20DayLetterDates(locatedBorrower.AccountNumber, DataAccessHelper.Region.Uheaa);

            //return built borrower
            return locatedBorrower;
        }

        public DataAccessHelper.Database GetWarehouseDbFromRegion(DataAccessHelper.Region region)
        {
            if (region == DataAccessHelper.Region.Uheaa)
            {
                return DataAccessHelper.Database.Udw;
            }
            else
            {
                throw new RegionNotValidException();
            }
        }

        public void GetDemographicData(string accountNumber, DataAccessHelper.Region region, Borrower locatedBorrower)
        {
            DemographicsFromSQL results = LDA.ExecuteList<DemographicsFromSQL>("spMD_GetDemographicData", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber)).Result.SingleOrDefault();
            
            if(results == null)
            {
                throw new BorrowerNotFoundInWarehouseException($"The borrower was found in the {region} but no demographics were returned.  It might be because the borrower has a PD10 borrower record but no PD30 address information.  Again that should never happen so check the data.");
            }

            //borrower
            locatedBorrower.FirstName = results.FirstName;
            locatedBorrower.LastName = results.LastName;
            locatedBorrower.MI = results.MI;
            locatedBorrower.FullName = results.FullName;
            locatedBorrower.DOB = results.DOB;
            locatedBorrower.UpdatedDemographics = new Demographics();
            locatedBorrower.CompassDemographics = new Demographics();

            //compass demographics
            locatedBorrower.CompassDemographics.AccountNumber = results.AccountNumber;
            locatedBorrower.CompassDemographics.SSN = results.SSN;
            locatedBorrower.CompassDemographics.FirstName = results.FirstName;
            locatedBorrower.CompassDemographics.LastName = results.LastName;
            locatedBorrower.CompassDemographics.MI = results.MI;
            locatedBorrower.CompassDemographics.Name = results.FullName;
            locatedBorrower.CompassDemographics.DOB = results.DOB;
            locatedBorrower.CompassDemographics.FoundOnSystem = true;

            //address
            locatedBorrower.CompassDemographics.Addr1 = results.Address1;
            locatedBorrower.CompassDemographics.Addr2 = results.Address2;
            locatedBorrower.CompassDemographics.City = results.City;
            locatedBorrower.CompassDemographics.State = results.State;
            locatedBorrower.CompassDemographics.Zip = results.ZIP;

            //FOREIGN: add property to Demographics class and uncomment line below
            locatedBorrower.CompassDemographics.ForeignState = results.ForeignState;
            locatedBorrower.CompassDemographics.Country = results.Country;
            locatedBorrower.CompassDemographics.SPAddrVerDt = results.AddressVerifiedDate;
            locatedBorrower.CompassDemographics.SPAddrInd = results.AddressValidityIndicator;

            //home phone
            locatedBorrower.CompassDemographics.HomePhoneConsent = results.HomePhoneConsentIndicator;
            locatedBorrower.CompassDemographics.HomePhoneVerificationDate = results.HomePhoneVerifiedDate;
            locatedBorrower.CompassDemographics.HomePhoneValidityIndicator = results.HomePhoneValidityIndicator;
            locatedBorrower.CompassDemographics.HomePhoneNum = results.HomePhone;
            locatedBorrower.CompassDemographics.HomePhoneExt = results.HomePhoneExtension;
            locatedBorrower.CompassDemographics.HomePhoneConsent = results.HomePhoneConsentIndicator;
            locatedBorrower.CompassDemographics.HomePhoneMBL = results.HomePhoneMBLIndicator;
            locatedBorrower.CompassDemographics.HomePhoneForeignCountry = results.HomePhoneForeignCountry;
            locatedBorrower.CompassDemographics.HomePhoneForeignCity = results.HomePhoneForeignCity;
            locatedBorrower.CompassDemographics.HomePhoneForeignLocalNumber = results.HomePhoneForeignLocalNumber;
            //alternate (other) phone
            locatedBorrower.CompassDemographics.OtherPhoneMBL = results.AlternatePhoneMBLIndicator;
            locatedBorrower.CompassDemographics.OtherPhoneConsent = results.AlternatePhoneConsentIndicator;
            locatedBorrower.CompassDemographics.OtherPhoneVerificationDate = results.AlternatePhoneVerifiedDate;
            locatedBorrower.CompassDemographics.OtherPhoneValidityIndicator = results.AlternatePhoneValidityIndicator;
            locatedBorrower.CompassDemographics.OtherPhoneNum = results.AlternatePhone;
            locatedBorrower.CompassDemographics.OtherPhoneExt = results.AlternatePhoneExtension;
            locatedBorrower.CompassDemographics.OtherPhoneConsent = results.AlternatePhoneConsentIndicator;
            locatedBorrower.CompassDemographics.OtherPhoneMBL = results.AlternatePhoneMBLIndicator;
            locatedBorrower.CompassDemographics.OtherPhoneForeignCountry = results.AlternatePhoneForeignCountry;
            locatedBorrower.CompassDemographics.OtherPhoneForeignCity = results.AlternatePhoneForeignCity;
            locatedBorrower.CompassDemographics.OtherPhoneForeignLocalNumber = results.AlternatePhoneForeignLocalNumber;
            //work (other2) phone
            locatedBorrower.CompassDemographics.OtherPhone2MBL = results.WorkPhoneMBLIndicator;
            locatedBorrower.CompassDemographics.OtherPhone2Consent = results.WorkPhoneConsentIndicator;
            locatedBorrower.CompassDemographics.OtherPhone2VerificationDate = results.WorkPhoneVerifiedDate;
            locatedBorrower.CompassDemographics.OtherPhone2ValidityIndicator = results.WorkPhoneValidityIndicator;
            locatedBorrower.CompassDemographics.OtherPhone2Num = results.WorkPhone;
            locatedBorrower.CompassDemographics.OtherPhone2Ext = results.WorkPhoneExtension;
            locatedBorrower.CompassDemographics.OtherPhone2Consent = results.WorkPhoneConsentIndicator;
            locatedBorrower.CompassDemographics.OtherPhone2MBL = results.WorkPhoneMBLIndicator;
            locatedBorrower.CompassDemographics.OtherPhone2ForeignCountry = results.WorkPhoneForeignCountry;
            locatedBorrower.CompassDemographics.OtherPhone2ForeignCity = results.WorkPhoneForeignCity;
            locatedBorrower.CompassDemographics.OtherPhone2ForeignLocalNumber = results.WorkPhoneForeignLocalNumber;
            //other phone 3
            locatedBorrower.CompassDemographics.OtherPhone3MBL = results.MobilePhoneMBLIndicator;
            locatedBorrower.CompassDemographics.OtherPhone3Consent = results.MobilePhoneConsentIndicator;
            locatedBorrower.CompassDemographics.OtherPhone3VerificationDate = results.MobilePhoneVerifiedDate;
            locatedBorrower.CompassDemographics.OtherPhone3ValidityIndicator = results.MobilePhoneValidityIndicator;
            locatedBorrower.CompassDemographics.OtherPhone3Num = results.MobilePhone;
            locatedBorrower.CompassDemographics.OtherPhone3Ext = results.MobilePhoneExtension;
            locatedBorrower.CompassDemographics.OtherPhone3Consent = results.MobilePhoneConsentIndicator;
            locatedBorrower.CompassDemographics.OtherPhone3MBL = results.MobilePhoneMBLIndicator;
            locatedBorrower.CompassDemographics.OtherPhone3ForeignCountry = results.MobilePhoneForeignCountry;
            locatedBorrower.CompassDemographics.OtherPhone3ForeignCity = results.MobilePhoneForeignCity;
            locatedBorrower.CompassDemographics.OtherPhone3ForeignLocalNumber = results.MobilePhoneForeignLocalNumber;

            //home email
            locatedBorrower.CompassDemographics.Email = results.HomeEmail;
            locatedBorrower.CompassDemographics.SPEmailVerDt = results.HomeEmailVerifiedDate;
            locatedBorrower.CompassDemographics.SPEmailInd = results.HomeEmailValidityIndicator;
            //alternate (other) email
            locatedBorrower.CompassDemographics.OtherEmail = results.AlternateEmail;
            locatedBorrower.CompassDemographics.SPOtEmailVerDt = results.AlternateEmailVerifiedDate;
            locatedBorrower.CompassDemographics.SPOtEmailInd = results.AlternateEmailValidityIndicator;
            //work (other2) email
            locatedBorrower.CompassDemographics.OtherEmail2 = results.WorkEmail;
            locatedBorrower.CompassDemographics.SPOt2EmailVerDt = results.WorkEmailVerifiedDate;
            locatedBorrower.CompassDemographics.SPOt2EmailInd = results.WorkEmailValidityIndicator;

            locatedBorrower.CompassDemographics.EcorrCorrespondence = results.EcorrCorrespondence;
            locatedBorrower.CompassDemographics.EcorrBilling = results.EcorrBilling;
            locatedBorrower.CompassDemographics.EcorrTax = results.EcorrTax;

            locatedBorrower.HasPendingDisbursementCancellation = results.HasPendingDisbursementCancellation;
            locatedBorrower.HasSulaLoans = results.HasSulaLoans;
            locatedBorrower.NeedsDeconArc = results.NeedsDeconArc;
            locatedBorrower.RelationshipBeginDate = results.RelationshipStartDate;
            locatedBorrower.EndProcessingAfterDemosPage = results.EndProcessingAfterDemosPage;
        }

        public class CallForwardingData
        {
            public string FORWARDING { get; set; }
            public string IF_GTR { get; set; }
        }

        /// <summary>
        /// Gets call forwarding data from warehouse
        /// </summary>
        private List<CallForwardingData> GetCallForwardingData(string accountNumber, DataAccessHelper.Region region)
        {
            return LDA.ExecuteList<CallForwardingData>("spMD_GetCallForwardingData", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber)).Result;
        }

        //Get deferments and forbearances
        private List<DefermentForbearance> GetDefermentsAndForbearances(string accountNumber, DataAccessHelper.Region region)
        {
            return LDA.ExecuteList<DefermentForbearance>("spMD_GetDefermentAndForbearenceData", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber)).Result;
        }

        //Get bills
        private List<Bill> GetBillingInformation(string accountNumber, DataAccessHelper.Region region)
        {
            return LDA.ExecuteList<Bill>("spMD_GetBillingData", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber)).Result;
        }

        //Get loan and borrower benefits information
        private List<ServicingLoanDetail> GetCompassLoanLevelDataFromWarehouse(string accountNumber, DataAccessHelper.Region region)
        {
            return LDA.ExecuteList<ServicingLoanDetail>("spMD_GetCompassLoanData", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber)).Result;
        }

        //Get financial transaction information
        private List<FinancialTransaction> GetFinancialTransactionInformation(string accountNumber, DataAccessHelper.Region region)
        {
            return LDA.ExecuteList<FinancialTransaction>("spMD_GetFinancialTransactions", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber)).Result;
        }

        //Get distinct list of all Bill types
        private List<string> GetBillTypes(string accountNumber, DataAccessHelper.Region region)
        {
            return LDA.ExecuteList<string>("spMD_GetBillingTypeData", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber)).Result;
        }

        //Get distinct list of RPS types
        private List<string> GetRPSTypes(string accountNumber, DataAccessHelper.Region region)
        {
            return LDA.ExecuteList<string>("spMD_GetRPSTypeData", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber)).Result;
        }

        //Get all reference infomration from warehouse fo borrower
        private List<Reference> GetReferenceDataFromWarehouse(string accountNumber, DataAccessHelper.Region region)
        {
            return LDA.ExecuteList<Reference>("spMD_GetReferenceData", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber)).Result;
        }

        //Gets ACH data for borrower
        private ACHInformation GetACHFromWarehouse(string accountNumber, DataAccessHelper.Region region)
        {
            ACHInformation results = LDA.ExecuteList<ACHInformation>("spMD_GetACHData", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber)).Result.SingleOrDefault();

            //Get loan level data for ACH
            if(results != null)
            {
                results.LoansOnACH = GetACHLoanDataFromWarehouse(accountNumber, true, region);
                results.LoansNotOnACH = GetACHLoanDataFromWarehouse(accountNumber, false, region);
                return results;
            }
            else
            {
                return null;
            }
        }

        //Gets loan level information for ACH
        private List<ACHLoanData> GetACHLoanDataFromWarehouse(string accountNumber, bool onACHIndicator, DataAccessHelper.Region region)
        {
            return LDA.ExecuteList<ACHLoanData>("spMD_GetACHLoanLevelData", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber), SP("OnACHIndicator", onACHIndicator)).Result;
        }

        //Gets employer information from data warehouse
        private EmployerDemographics GetEmployerInformationFromWarehouse(string accountNumber, DataAccessHelper.Region region)
        {
            return LDA.ExecuteList<EmployerDemographics>("spMD_GetEmployerData", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber)).Result.SingleOrDefault();
        }
        
        //Get reference activity history from data warehouse
        public List<ActivityHistoryRecord> GetReferenceActivityHistory(string accountNumber, string recipientId, DataAccessHelper.Region region)
        {
            //To use the UHEAA region, the sproc would have to be created on UDW
            return LDA.ExecuteList<ActivityHistoryRecord>("spMD_GetReferenceActivityHistory", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber), SP("RecipientId", recipientId)).Result;
        }

        //Gets all reference information from warehouse for borrower
        private List<string> Get20DayLetterDates(string accountNumber, DataAccessHelper.Region region)
        {
            return LDA.ExecuteList<string>("spMD_Get20DayLetterDates", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber)).Result;
        }

        public bool HasAnticipatedDisbursement(string accountNumber, DataAccessHelper.Region region)
        {
            return LDA.ExecuteList<bool>("spMD_HasAnticipatedDisbursement", GetWarehouseDbFromRegion(region), SP("AccountNumber", accountNumber)).Result.SingleOrDefault();
        }

        public bool HasAP03Record(string ssn, DataAccessHelper.Region region)
        {
            return LDA.ExecuteList<bool>("spMD_HasAP03Record", GetWarehouseDbFromRegion(region), SP("Ssn", ssn)).Result.SingleOrDefault();
        }

        #endregion

        #region Call Categorization

        /// <summary>
        /// Insert Call Categorization record into the DB
        /// </summary>
        /// <param name="entry">A populated call categorization entry object</param> 
        [UsesSproc(DataAccessHelper.Database.MauiDude, "spCallCategorizationInsert")]
        public void AddCallCategorizationRecord(CallCategorizationEntry entry)
        {
            LDA.Execute("spCallCategorizationInsert", DB, SP("CAT", entry.Category), SP("REA", entry.Reason), SP("LID", entry.LetterID), SP("CMT", entry.Comments), SP("UID", entry.UserID), SP("Region", entry.Region));
        }

        public string GetSSNFromAccountNumber(string accountNumber, DataAccessHelper.Region region)
        {
            return LDA.ExecuteList<string>("spGetSSNFromAcctNumber", GetWarehouseDbFromRegion(region), SqlParams.Single("AccountNumber", accountNumber)).Result.FirstOrDefault();
        }

        #endregion
        
        #region Private Demographics from SQL Class
        private class DemographicsFromSQL
        {
            public string AccountNumber { get; set; }
            public string SSN { get; set; }
            public string FirstName { get; set; }
            public string MI { get; set; }
            public string LastName { get; set; }
            public string FullName { get; set; }
            public string DOB { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZIP { get; set; }
            public string ForeignState { get; set; }
            public string Country { get; set; }
            public string AddressVerifiedDate { get; set; }
            public string AddressValidityIndicator { get; set; }
            public string HomePhoneMBLIndicator { get; set; }
            public string HomePhoneConsentIndicator { get; set; }
            public string HomePhoneVerifiedDate { get; set; }
            public string HomePhoneValidityIndicator { get; set; }
            public string HomePhone { get; set; }
            public string HomePhoneExtension { get; set; }
            public string HomePhoneForeignCountry { get; set; }
            public string HomePhoneForeignCity { get; set; }
            public string HomePhoneForeignLocalNumber { get; set; }
            public string AlternatePhoneMBLIndicator { get; set; }
            public string AlternatePhoneConsentIndicator { get; set; }
            public string AlternatePhoneVerifiedDate { get; set; }
            public string AlternatePhoneValidityIndicator { get; set; }
            public string AlternatePhone { get; set; }
            public string AlternatePhoneExtension { get; set; }
            public string AlternatePhoneForeignCountry { get; set; }
            public string AlternatePhoneForeignCity { get; set; }
            public string AlternatePhoneForeignLocalNumber { get; set; }
            public string WorkPhoneMBLIndicator { get; set; }
            public string WorkPhoneConsentIndicator { get; set; }
            public string WorkPhoneVerifiedDate { get; set; }
            public string WorkPhoneValidityIndicator { get; set; }
            public string WorkPhone { get; set; }
            public string WorkPhoneExtension { get; set; }
            public string WorkPhoneForeignCountry { get; set; }
            public string WorkPhoneForeignCity { get; set; }
            public string WorkPhoneForeignLocalNumber { get; set; }
            public string MobilePhoneMBLIndicator { get; set; }
            public string MobilePhoneConsentIndicator { get; set; }
            public string MobilePhoneVerifiedDate { get; set; }
            public string MobilePhoneValidityIndicator { get; set; }
            public string MobilePhoneForeignCountry { get; set; }
            public string MobilePhoneForeignCity { get; set; }
            public string MobilePhoneForeignLocalNumber { get; set; }
            public string MobilePhone { get; set; }
            public string MobilePhoneExtension { get; set; }
            public string HomeEmail { get; set; }
            public string HomeEmailVerifiedDate { get; set; }
            public string HomeEmailValidityIndicator { get; set; }
            public string AlternateEmail { get; set; }
            public string AlternateEmailVerifiedDate { get; set; }
            public string AlternateEmailValidityIndicator { get; set; }
            public string WorkEmail { get; set; }
            public string WorkEmailVerifiedDate { get; set; }
            public string WorkEmailValidityIndicator { get; set; }
            public bool EcorrCorrespondence { get; set; }
            public bool EcorrBilling { get; set; }
            public bool EcorrTax { get; set; }
            public bool HasPendingDisbursementCancellation { get; set; }
            public bool HasAp03Record { get; set; }
            public bool HasSulaLoans { get; set; }
            public bool NeedsDeconArc { get; set; }
            public DateTime? RelationshipStartDate { get; set; }
            public bool EndProcessingAfterDemosPage { get; set; }

        }
        #endregion

    }
}
