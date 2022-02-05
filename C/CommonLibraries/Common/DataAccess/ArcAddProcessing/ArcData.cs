using System;
using System.Data.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Uheaa.Common.DataAccess
{
    public class ArcData
    {
        public enum ArcType
        {
            Atd22ByLoan,
            Atd22AllLoans,
            Atd22ByBalance,
            Atd22ByLoanProgram,
            Atd22AllLoansRegards,
            Atd22ByLoanRegards,
            OneLINK
        }

        public ArcType ArcTypeSelected { get; set; }
        public string AccountNumber { get; set; }
        public string RecipientId { get; set; }
        public string Arc { get; set; }
        public string DelinquencyArc { get; set; }
        public DateTime? ProcessOn { get; set; }
        public string Comment { get; set; }
        public string ScriptId { get; set; }
        public bool IsReference { get; set; }
        public bool IsEndorser { get; set; }
        public DateTime? ProcessTo { get; set; }
        public DateTime? ProcessFrom { get; set; }
        public DateTime? NeedBy { get; set; }
        public string RegardsCode { get; set; }
        public string RegardsTo { get; set; }
        public List<int> LoanSequences { get; set; }
        public List<string> LoanPrograms { get; set; }
        public string ResponseCode { get; set; }
        public string ActivityType { get; set; }
        public string ActivityContact { get; set; }
        public string RunBy { get; set; }

        private readonly DataAccessHelper.Database DataBase;
        private const string AccountNumberError = "Account number is a required field";
        private const string ArcError = "ARC is required";
        private const string ScriptIdError = "ScriptId is required";
        private const string ActivityTypeError = "Activity Type is required";
        private const string ActivityContactError = "Activity Contact is required";
        private SqlConnection Conn { get; set; }
        private ArcAddResults Result { get; set; }

        public ArcData(DataAccessHelper.Region region)
        {
            DataBase = region == DataAccessHelper.Region.CornerStone ? DataAccessHelper.Database.Cls : DataAccessHelper.Database.Uls;
            Conn = DataAccessHelper.GetManagedConnection(DataBase, DataAccessHelper.CurrentMode);
        }

        /// <summary>
        /// Adds the arc record to the database
        /// </summary>
        /// <returns>ArcAddResults object with ArcAdded bool and Error Results</returns>
        public ArcAddResults AddArc()
        {
            Result = ValidateArcRecord();
            if (Result.Errors.Count > 0)
                return Result;

            switch (ArcTypeSelected)
            {
                case ArcType.Atd22AllLoans:
                    Result.ArcAddProcessingId = AddComment(AccountNumber, Arc, RecipientId, ScriptId, Comment, ArcTypeSelected, ResponseCode, ProcessOn, RegardsCode, RegardsTo, IsReference, IsEndorser, ProcessFrom, ProcessTo, NeedBy, RunBy);
                    Result.ArcAdded = Result.ArcAddProcessingId > 0;
                    break;
                case ArcType.Atd22AllLoansRegards:
                    Result.ArcAddProcessingId = AddComment(AccountNumber, Arc, RecipientId, ScriptId, Comment, ArcTypeSelected, ResponseCode, ProcessOn, RegardsCode, RegardsTo, IsReference, IsEndorser, ProcessFrom, ProcessTo, NeedBy, RunBy);
                    Result.ArcAdded = Result.ArcAddProcessingId > 0;
                    break;
                case ArcType.Atd22ByBalance:
                    Result.ArcAddProcessingId = AddComment(AccountNumber, Arc, RecipientId, ScriptId, Comment, ArcTypeSelected, ResponseCode, ProcessOn, RegardsCode, RegardsTo, IsReference, IsEndorser, ProcessFrom, ProcessTo, NeedBy, RunBy);
                    Result.ArcAdded = Result.ArcAddProcessingId > 0;
                    break;
                case ArcType.Atd22ByLoan:
                    Result.ArcAddProcessingId = AddLoanSequenceComment(AccountNumber, Arc, RecipientId, ScriptId, Comment, ArcTypeSelected, LoanSequences.Count > 0 ? LoanSequences.ToLoanSequenceList().ToDataTable() : null, ResponseCode, ProcessOn, RegardsCode, RegardsTo, IsReference, IsEndorser, ProcessFrom, ProcessTo, NeedBy, RunBy);
                    Result.ArcAdded = Result.ArcAddProcessingId > 0;
                    break;
                case ArcType.Atd22ByLoanProgram:
                    Result.ArcAddProcessingId = AddLoanProgramComment(AccountNumber, Arc, RecipientId, ScriptId, Comment, ArcTypeSelected, LoanPrograms.Count > 0 ? LoanPrograms.ToLoanProgramList().ToDataTable() : null, ResponseCode, ProcessOn, RegardsCode, RegardsTo, IsReference, IsEndorser, ProcessFrom, ProcessTo, NeedBy, RunBy);
                    Result.ArcAdded = Result.ArcAddProcessingId > 0;
                    break;
                case ArcType.Atd22ByLoanRegards:
                    Result.ArcAddProcessingId = AddLoanSequenceComment(AccountNumber, Arc, RecipientId, ScriptId, Comment, ArcTypeSelected, LoanSequences.Count > 0 ? LoanSequences.ToLoanSequenceList().ToDataTable() : null, ResponseCode, ProcessOn, RegardsCode, RegardsTo, IsReference, IsEndorser, ProcessFrom, ProcessTo, NeedBy, RunBy);
                    Result.ArcAdded = Result.ArcAddProcessingId > 0;
                    break;
                case ArcType.OneLINK:
                    Result.ArcAddProcessingId = AddOneLinkComment(AccountNumber, Arc, RecipientId, ScriptId, Comment, ArcTypeSelected, ResponseCode, ProcessOn, RegardsCode, RegardsTo, IsReference, IsEndorser, ProcessFrom, ProcessTo, NeedBy, RunBy);
                    Result.ArcAdded = Result.ArcAddProcessingId > 0;
                    break;
                default:
                    Result.ArcAdded = false;
                    break;
            }
            return Result;
        }


        private ArcAddResults ValidateArcRecord()
        {
            ArcAddResults Result = new ArcAddResults();
            switch (ArcTypeSelected)
            {
                case ArcType.Atd22AllLoans:
                    return (ValidateAllLoans());
                case ArcType.Atd22AllLoansRegards:
                    return ValidateAllLoansRegards();
                case ArcType.Atd22ByBalance:
                    return ValidateByBalance();
                case ArcType.Atd22ByLoan:
                    return ValidateByLoan();
                case ArcType.Atd22ByLoanProgram:
                    return ValidateLoanProgram();
                case ArcType.Atd22ByLoanRegards:
                    return ValidateByLoanRegards();
                case ArcType.OneLINK:
                    return ValidateOneLINK();
                default:
                    return new ArcAddResults() { ArcAdded = false };
            }
        }

        private ArcAddResults ValidateOneLINK()
        {
            ArcAddResults Result = new ArcAddResults();
            if (AccountNumber.IsNullOrEmpty())
                Result.Errors.Add(AccountNumberError);
            if (Arc.IsNullOrEmpty())
                Result.Errors.Add(ArcError);
            if (ScriptId.IsNullOrEmpty())
                Result.Errors.Add(ScriptIdError);
            if (ActivityType.IsNullOrEmpty())
                Result.Errors.Add(ActivityTypeError);
            if (ActivityContact.IsNullOrEmpty())
                Result.Errors.Add(ActivityContactError);
            return Result;
        }

        private ArcAddResults ValidateAllLoans()
        {
            ArcAddResults Result = new ArcAddResults();
            if (AccountNumber.IsNullOrEmpty())
                Result.Errors.Add(AccountNumberError);
            if (Arc.IsNullOrEmpty())
                Result.Errors.Add(ArcError);
            if (ScriptId.IsNullOrEmpty())
                Result.Errors.Add(ScriptIdError);
            return Result;
        }

        private ArcAddResults ValidateAllLoansRegards()
        {
            ArcAddResults Result = new ArcAddResults();
            if (AccountNumber.IsNullOrEmpty())
                Result.Errors.Add(AccountNumberError);
            if (Arc.IsNullOrEmpty())
                Result.Errors.Add(ArcError);
            if (ScriptId.IsNullOrEmpty())
                Result.Errors.Add(ScriptIdError);
            if (RegardsTo.IsNullOrEmpty())
                Result.Errors.Add("RegardsTo was not provided");
            if (RegardsCode.IsNullOrEmpty())
                Result.Errors.Add("RegardsCode was not provided");
            return Result;
        }

        private ArcAddResults ValidateByBalance()
        {
            ArcAddResults Result = new ArcAddResults();
            if (AccountNumber.IsNullOrEmpty())
                Result.Errors.Add(AccountNumberError);
            if (Arc.IsNullOrEmpty())
                Result.Errors.Add(ArcError);
            if (ScriptId.IsNullOrEmpty())
                Result.Errors.Add(ScriptIdError);
            return Result;
        }

        private ArcAddResults ValidateByLoan()
        {
            ArcAddResults Result = new ArcAddResults();
            if (AccountNumber.IsNullOrEmpty())
                Result.Errors.Add(AccountNumberError);
            if (Arc.IsNullOrEmpty())
                Result.Errors.Add(ArcError);
            if (ScriptId.IsNullOrEmpty())
                Result.Errors.Add(ScriptIdError);
            if (!LoanSequences.Any())
                Result.Errors.Add("No Loan Sequences were provided.");
            return Result;
        }

        private ArcAddResults ValidateLoanProgram()
        {
            ArcAddResults Result = new ArcAddResults();
            if (AccountNumber.IsNullOrEmpty())
                Result.Errors.Add(AccountNumberError);
            if (Arc.IsNullOrEmpty())
                Result.Errors.Add(ArcError);
            if (ScriptId.IsNullOrEmpty())
                Result.Errors.Add(ScriptIdError);
            if (!LoanPrograms.Any())
                Result.Errors.Add("No Loan Programs were provided.");
            return Result;
        }

        private ArcAddResults ValidateByLoanRegards()
        {
            ArcAddResults Result = new ArcAddResults();
            if (AccountNumber.IsNullOrEmpty())
                Result.Errors.Add(AccountNumberError);
            if (Arc.IsNullOrEmpty())
                Result.Errors.Add(ArcError);
            if (ScriptId.IsNullOrEmpty())
                Result.Errors.Add(ScriptIdError);
            if (!LoanSequences.Any())
                Result.Errors.Add("No Loan Sequences were provided.");
            if (RegardsTo.IsNullOrEmpty())
                Result.Errors.Add("RegardsTo was not provided");
            if (RegardsCode.IsNullOrEmpty())
                Result.Errors.Add("RegardsCode was not provided");

            return Result;
        }

        private int AddOneLINKComment(string ssn, string arc, string comment, string activityType, string activityContact, string scriptId)
        {
            return 0;
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "ArcAdd_AddLoanProgramRecords")]
        [UsesSproc(DataAccessHelper.Database.Uls, "ArcAdd_AddLoanProgramRecords")]
        private int AddLoanProgramComment(string ssn, string arc, string recipientId, string scriptId, string comment, ArcType type, DataTable loanPrograms, string responseCode = "", DateTime? processOn = null, string regardsToCode = "", string regardsToId = "", bool isReference = false, bool isEndorser = false, DateTime? from = null, DateTime? to = null, DateTime? neededBy = null, string runBy = null)
        {
            try
            {
                return DataAccessHelper.ExecuteId("ArcAdd_AddLoanProgramRecords", Conn,
                        SqlParams.Single("AccountNumber", ssn),
                        SqlParams.Single("ArcTypeId", (int)type),
                        SqlParams.Single("RecipientId", recipientId),
                        SqlParams.Single("Arc", arc),
                        SqlParams.Single("ProcessOn", processOn),
                        SqlParams.Single("Comment", comment),
                        SqlParams.Single("ScriptId", scriptId),
                        SqlParams.Single("IsReference", isReference),
                        SqlParams.Single("IsEndorser", isEndorser),
                        SqlParams.Single("ProcessTo", to),
                        SqlParams.Single("ProcessFrom", from),
                        SqlParams.Single("NeededBy", neededBy),
                        SqlParams.Single("LoanProgram", loanPrograms),
                        SqlParams.Single("RegardsCode", regardsToCode),
                        SqlParams.Single("RegardsTo", regardsToId),
                        SqlParams.Single("ArcResponseCode", responseCode),
                        SqlParams.Single("RunBy", runBy));

            }
            catch (Exception ex)
            {
                Result.Ex = ex;
                return 0;
            }
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "ArcAdd_AddLoanSequenceRecords")]
        [UsesSproc(DataAccessHelper.Database.Uls, "ArcAdd_AddLoanSequenceRecords")]
        private int AddLoanSequenceComment(string ssn, string arc, string recipientId, string scriptId, string comment, ArcType type, DataTable loanSequences, string responseCode = "", DateTime? processOn = null, string regardsToCode = "", string regardsToId = "", bool isReference = false, bool isEndorser = false, DateTime? from = null, DateTime? to = null, DateTime? neededBy = null, string runBy = null)
        {
            try
            {
                return DataAccessHelper.ExecuteId("ArcAdd_AddLoanSequenceRecords", Conn,
                        SqlParams.Single("AccountNumber", ssn),
                        SqlParams.Single("ArcTypeId", (int)type),
                        SqlParams.Single("RecipientId", recipientId ?? ""),
                        SqlParams.Single("Arc", arc),
                        SqlParams.Single("ProcessOn", processOn),
                        SqlParams.Single("Comment", comment),
                        SqlParams.Single("ScriptId", scriptId),
                        SqlParams.Single("IsReference", isReference),
                        SqlParams.Single("IsEndorser", isEndorser),
                        SqlParams.Single("ProcessTo", to),
                        SqlParams.Single("ProcessFrom", from),
                        SqlParams.Single("NeededBy", neededBy),
                        SqlParams.Single("LoanSequence", loanSequences),
                        SqlParams.Single("RegardsCode", regardsToCode),
                        SqlParams.Single("RegardsTo", regardsToId),
                        SqlParams.Single("ArcResponseCode", responseCode),
                        SqlParams.Single("RunBy", runBy));

            }
            catch (Exception ex)
            {
                Result.Ex = ex;
                return 0;
            }
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "ArcAdd_AddRecord")]
        [UsesSproc(DataAccessHelper.Database.Uls, "ArcAdd_AddRecord")]
        private int AddComment(string ssn, string arc, string recipientId, string scriptId, string comment, ArcType type, string responseCode = "", DateTime? processOn = null, string regardsToCode = "", string regardsToId = "", bool isReference = false, bool isEndorser = false, DateTime? from = null, DateTime? to = null, DateTime? neededBy = null, string runBy = null)
        {
            try
            {
                return DataAccessHelper.ExecuteId("ArcAdd_AddRecord", Conn,
                        SqlParams.Single("AccountNumber", ssn),
                        SqlParams.Single("ArcTypeId", (int)type),
                        SqlParams.Single("RecipientId", recipientId),
                        SqlParams.Single("Arc", arc),
                        SqlParams.Single("ProcessOn", processOn),
                        SqlParams.Single("Comment", comment),
                        SqlParams.Single("ScriptId", scriptId),
                        SqlParams.Single("IsReference", isReference),
                        SqlParams.Single("IsEndorser", isEndorser),
                        SqlParams.Single("ProcessTo", to),
                        SqlParams.Single("ProcessFrom", from),
                        SqlParams.Single("NeededBy", neededBy),
                        SqlParams.Single("RegardsCode", regardsToCode),
                        SqlParams.Single("RegardsTo", regardsToId),
                        SqlParams.Single("ArcResponseCode", responseCode),
                        SqlParams.Single("RunBy", runBy));

            }
            catch (Exception ex)
            {
                Result.Ex = ex;
                return 0;
            }
        }

        [UsesSproc(DataAccessHelper.Database.Cls, "ArcAdd_AddRecord")]
        [UsesSproc(DataAccessHelper.Database.Uls, "ArcAdd_AddRecord")]
        private int AddOneLinkComment(string ssn, string arc, string recipientId, string scriptId, string comment, ArcType type, string responseCode = "", DateTime? processOn = null, string regardsToCode = "", string regardsToId = "", bool isReference = false, bool isEndorser = false, DateTime? from = null, DateTime? to = null, DateTime? neededBy = null, string runBy = null)
        {
            try
            {
                return DataAccessHelper.ExecuteId("ArcAdd_AddOneLinkRecord", Conn,
                        SqlParams.Single("AccountNumber", ssn),
                        SqlParams.Single("ArcTypeId", (int)type),
                        SqlParams.Single("RecipientId", recipientId),
                        SqlParams.Single("Arc", arc),
                        SqlParams.Single("ProcessOn", processOn),
                        SqlParams.Single("Comment", comment),
                        SqlParams.Single("ScriptId", scriptId),
                        SqlParams.Single("IsReference", isReference),
                        SqlParams.Single("IsEndorser", isEndorser),
                        SqlParams.Single("ProcessTo", to),
                        SqlParams.Single("ProcessFrom", from),
                        SqlParams.Single("NeededBy", neededBy),
                        SqlParams.Single("RegardsCode", regardsToCode),
                        SqlParams.Single("RegardsTo", regardsToId),
                        SqlParams.Single("ArcResponseCode", responseCode),
                        SqlParams.Single("ActivityType", ActivityType),
                        SqlParams.Single("ActivityContact", ActivityContact),
                        SqlParams.Single("RunBy", runBy));

            }
            catch (Exception ex)
            {
                Result.Ex = ex;
                return 0;
            }
        }
    }


}

public class LoanPrograms
{
    public string LoanProgram { get; set; }
}

public class LoanSequences
{
    public int LoanSequence { get; set; }
}
