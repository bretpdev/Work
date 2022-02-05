using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using static Uheaa.Common.DataAccess.DataAccessHelper.Database;

namespace TCPAPNS
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }
        public bool OneLink { get; set; }

        public DataAccess(LogDataAccess lda, bool oneLink)
        {
            LDA = lda;
            OneLink = oneLink;
        }

        [UsesSproc(Uls, "[tcpapns].Delete_BulkLoad")]
        [UsesSproc(Uls, "[tcpapns].Delete_OneLinkBulkLoad")]
        public void ClearBulkLoad() => LDA.Execute(OneLink ? "[tcpapns].Delete_OneLinkBulkLoad" : "[tcpapns].Delete_BulkLoad", Uls);

        [UsesSproc(Uls, "[tcpapns].GetBulkLoadCount")]
        [UsesSproc(Uls, "[tcpapns].OneLinkGetBulkLoadCount")]
        public int GetBulkLoadCount() => LDA.ExecuteSingle<int>(OneLink ? "[tcpapns].OneLinkGetBulkLoadCount" : "[tcpapns].GetBulkLoadCount", Uls).Result;

        [UsesSproc(Uls, "[tcpapns].LoadData")]
        [UsesSproc(Uls, "[tcpapns].OneLinkLoadData")]
        public void LoadData(string sourceFile) => LDA.Execute(OneLink ? "[tcpapns].OneLinkLoadData" : "[tcpapns].LoadData", Uls,
                SP("SourceFile", sourceFile));

        [UsesSproc(Uls, "[tcpapns].InactivateInvalidRecords")]
        [UsesSproc(Uls, "[tcpapns].OneLinkInactivateInvalidRecords")]
        public void InactivateInvalidRecords() => LDA.Execute(OneLink ? "[tcpapns].OneLinkInactivateInvalidRecords" : "[tcpapns].InactivateInvalidRecords", Uls);

        [UsesSproc(Uls, "[tcpapns].MarkProcessed")]
        [UsesSproc(Uls, "[tcpapns].OneLinkMarkProcessed")]
        public bool MarkProcessed(int fileProcessingId, long? arcAddProcessingId)
        {
            if (OneLink)
                return LDA.Execute("[tcpapns].OneLinkMarkProcessed", Uls,
                    SP("FileProcessingId", fileProcessingId));
            else
                return LDA.Execute("[tcpapns].MarkProcessed", Uls,
                    SP("FileProcessingId", fileProcessingId),
                    SP("ArcAddProcessingId", arcAddProcessingId.HasValue ? (object)arcAddProcessingId.Value : DBNull.Value));
        }

        [UsesSproc(Uls, "[tcpapns].GetUnprocessedRecords")]
        [UsesSproc(Uls, "[tcpapns].OneLinkGetUnprocessedRecords")]
        public List<FileProcessingRecord> GetUnprocessedRecords() => LDA.ExecuteList<FileProcessingRecord>(OneLink ? "[tcpapns].OneLinkGetUnprocessedRecords" : "[tcpapns].GetUnprocessedRecords", Uls).Result;

        [UsesSproc(Uls, "[tcpapns].MarkDeleted")]
        [UsesSproc(Uls, "[tcpapns].OneLinkMarkDeleted")]
        public bool MarkDeleted(int fileProcessingId) => LDA.Execute(OneLink ? "[tcpapns].OneLinkMarkDeleted" : "[tcpapns].MarkDeleted", Uls,
            SP("FileProcessingId", fileProcessingId));

        [UsesSproc(Uls, "[tcpapns].GetPhoneData")]
        public List<string> GetPhoneData(string accountIdentifier, string phone, bool mobileIndicator, bool hasConsentArc) => LDA.ExecuteList<string>("[tcpapns].GetPhoneData", Uls,
            SP("AccountIdentifier", accountIdentifier),
            SP("Phone", phone),
            SP("MobileIndicator", mobileIndicator),
            SP("HasConsentArc", hasConsentArc)).Result;

        [UsesSproc(Uls, "[tcpapns].[GetAccountNumberFromSsn]")]
        public string GetAccountNumber(string ssn) => LDA.ExecuteSingle<string>("[tcpapns].[GetAccountNumberFromSsn]", Uls,
            SP("Ssn", ssn)).Result;

        private SqlParameter SP(string name, object value)
        {
            return SqlParams.Single(name, value);
        }
    }
}