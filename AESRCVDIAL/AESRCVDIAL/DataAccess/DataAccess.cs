using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace AESRCVDIAL
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(LogDataAccess lda) =>
            LDA = lda;

        [UsesSproc(NobleCalls, "aesrcvdial.GetDialerFiles")]
        public List<DialerFiles> GetFiles() =>
            LDA.ExecuteList<DialerFiles>("aesrcvdial.GetDialerFiles", NobleCalls).Result;

        [UsesSproc(NobleCalls, "aesrcvdial.InsertSingleRecord")]
        public int InsertSingleRecord(FileData data) =>
            LDA.ExecuteId<int>("aesrcvdial.InsertSingleRecord", NobleCalls,
                Sp("FileName", data.FileName),
                Sp("TargetsId", data.TargetsId),
                Sp("QueueRegion", data.QueueRegion),
                Sp("CriticalTaskIndicator", data.CriticalTaskIndicator),
                Sp("BorrowersName", data.BorrowersName),
                Sp("BorrowersPaymentAmount", data.BorrowersPaymentAmount),
                Sp("BorrowersOutstandingBalance", data.BorrowersOutstandingBalance),
                Sp("BorrowersAccountNumber", data.BorrowersAccountNumber),
                Sp("TargetsDateLastAttempt", data.TargetsDateLastAttempt),
                Sp("TargetsDateLastContact", data.TargetsDateLastContact),
                Sp("TargetsRelationshipToBorrower", data.TargetsRelationshipToBorrower),
                Sp("TargetsName", data.TargetsName),
                Sp("TargetsZip", data.TargetsZip),
                Sp("TargetsHomePhoneType", data.TargetsHomePhoneType),
                Sp("TargetsHomePhone", data.TargetsHomePhone),
                Sp("TargetsAltPhoneType", data.TargetsAltPhoneType),
                Sp("TargetsAltPhone", data.TargetsAltPhone),
                Sp("TargetsOtherPhoneType", data.TargetsOtherPhoneType),
                Sp("TargetsOtherPhone", data.TargetsOtherPhone),
                Sp("TargetsTCPAConsentForHomePhone", data.TargetsTCPAConsentForHomePhone),
                Sp("TargetsTCPAConsentForAltPhone", data.TargetsTCPAConsentForAltPhone),
                Sp("TargetsTCPAConsentForOtherPHone", data.TargetsTCPAConsentForOtherPhone),
                Sp("RegardsToNumberOfDaysDelinquent", data.RegardsToNumberOfDaysDelinquent),
                Sp("RegardsToName", data.RegardsToName),
                Sp("RegardsToSkipStartDate", data.RegardsToSkipStartDate),
                Sp("PreviouslyRehabbedIndicator", data.PreviouslyRehabbedIndicator));

        /// <summary>
        /// Gets a count of the number of records added for current date
        /// </summary>
        [UsesSproc(NobleCalls, "aesrcvdial.GetRecordCountForFile")]
        public int GetRecordCountForFile(string fileName) =>
        LDA.ExecuteSingle<int>("aesrcvdial.GetRecordCountForFile", NobleCalls,
            Sp("FileName", fileName)).Result;

        /// <summary>
        /// Gets a count of all the records added with the file name
        /// </summary>
        [UsesSproc(NobleCalls, "aesrcvdial.GetRecordCount")]
        public int GetRecordCount(string fileName) =>
            LDA.ExecuteSingle<int>("aesrcvdial.GetRecordCount", NobleCalls,
                Sp("FileName", fileName)).Result;

        /// <summary>
        /// Gets all the records loaded to the database for the given date
        /// </summary>
        [UsesSproc(NobleCalls, "aesrcvdial.GetRecordsByDate")]
        public List<FileData> GetRecordsByDate(DateTime date) =>
            LDA.ExecuteList<FileData>("aesrcvdial.GetRecordsByDate", NobleCalls,
                Sp("AddedAt", date)).Result;

        public SqlParameter Sp(string name, object value) =>
            SqlParams.Single(name, value);

    }
}