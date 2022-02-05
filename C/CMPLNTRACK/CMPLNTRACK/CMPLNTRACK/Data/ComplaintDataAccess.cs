using System.Collections.Generic;
using System.Data;
using System.Linq;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CMPLNTRACK
{
    public class ComplaintDataAccess
    {
        private DataAccessHelper.Database Db
        {
            get
            {
                return DataAccessHelper.Database.Uls;
            }
        }
        private LogDataAccess LDA { get; set; }

        public ComplaintDataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
        }

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.FlagsSelectAll")]
        public List<Flag> FlagsGetAll() => LDA.ExecuteList<Flag>("complaints.FlagsSelectAll", Db).Result;

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.FlagDelete")]
        public void FlagDelete(int flagId) => LDA.Execute("complaints.FlagDelete", Db, SqlParams.Single("FlagId", flagId));

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.FlagInsert")]
        public void FlagAdd(string flagName) => LDA.Execute("complaints.FlagInsert", Db, SqlParams.Single("FlagName", flagName), SqlParams.Single("EnablesControlMailFields", false));

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.ComplaintTypesSelectAll")]
        public List<ComplaintType> ComplaintTypesGetAll() => LDA.ExecuteList<ComplaintType>("complaints.ComplaintTypesSelectAll", Db).Result;

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.ComplaintTypeDelete")]
        public void ComplaintTypeDelete(int complaintTypeId) => LDA.Execute("complaints.ComplaintTypeDelete", Db, SqlParams.Single("ComplaintTypeId", complaintTypeId));

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.ComplaintTypeInsert")]
        public void ComplaintTypeAdd(string typeName) => LDA.Execute("complaints.ComplaintTypeInsert", Db, SqlParams.Single("ComplaintTypeName", typeName));

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.ComplaintGroupsSelectAll")]
        public List<ComplaintGroup> GetComplaintGroupsGetAll() => LDA.ExecuteList<ComplaintGroup>("complaints.ComplaintGroupsSelectAll", Db).Result;

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.ComplaintGroupDelete")]
        public void ComplaintGroupDelete(int complaintGroupId) => LDA.Execute("complaints.ComplaintGroupDelete", Db, SqlParams.Single("ComplaintGroupId", complaintGroupId));

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.ComplaintGroupInsert")]
        public void ComplaintGroupAdd(string groupName) => LDA.Execute("complaints.ComplaintGroupInsert", Db, SqlParams.Single("ComplaintGroupName", groupName));

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.ComplaintPartiesSelectAll")]
        public List<ComplaintParty> ComplaintPartiesGetAll() => LDA.ExecuteList<ComplaintParty>("complaints.ComplaintPartiesSelectAll", Db).Result;

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.ComplaintPartyDelete")]
        public void ComplaintPartyDelete(int complaintPartyId) => LDA.Execute("complaints.ComplaintPartyDelete", Db, SqlParams.Single("ComplaintPartyId", complaintPartyId));

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.ComplaintPartyInsert")]
        public void ComplaintPartyAdd(string partyName) => LDA.Execute("complaints.ComplaintPartyInsert", Db, SqlParams.Single("ComplaintPartyName", partyName));

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.ComplaintInsert")]
        public int ComplaintAdd(Complaint complaint) => LDA.ExecuteSingle<int>("complaints.ComplaintInsert", Db,
                SqlParams.Specifics(complaint, o => o.AccountNumber, o => o.BorrowerName, o => o.ComplaintDescription, o => o.ComplaintPartyId, o => o.ComplaintTypeId,
                    o => o.DaysToRespond, o => o.NeedHelpTicketNumber, o => o.ControlMailNumber, o => o.ComplaintDate, o => o.ComplaintGroupId)).Result;

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.ComplaintUpdate")]
        public void ComplaintSave(Complaint complaint) => LDA.Execute("complaints.ComplaintUpdate", Db,
                SqlParams.Specifics(complaint, o => o.ComplaintId, o => o.BorrowerName, o => o.ComplaintDescription, o => o.ComplaintPartyId, o => o.ComplaintTypeId,
                    o => o.DaysToRespond, o => o.NeedHelpTicketNumber, o => o.ControlMailNumber, o => o.ComplaintDate, o => o.ComplaintGroupId));

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.ComplaintSetFlags")]
        public void ComplaintSetFlags(int complaintId, int[] flagIds) => LDA.Execute("complaints.ComplaintSetFlags", Db, SqlParams.Single("ComplaintId", complaintId), SqlParams.Single("FlagIds", ToDataTableParameter(flagIds)));

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.Search")]
        public List<Complaint> Search(string accountNumber, bool openComplaints, bool closedComplaints, int? flagId, int? partyId, int? typeId, int? groupId) => LDA.ExecuteList<Complaint>("complaints.Search", Db,
                SqlParams.Single("AccountNumber", string.IsNullOrWhiteSpace(accountNumber) ? null : accountNumber),
                SqlParams.Single("IncludeOpenComplaints", openComplaints), SqlParams.Single("IncludeClosedComplaints", closedComplaints),
                SqlParams.Single("FlagId", flagId), SqlParams.Single("GroupId", groupId),
                SqlParams.Single("TypeId", typeId), SqlParams.Single("PartyId", partyId)).Result;

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.ComplaintAddHistory")]
        public int AddHistory(int complaintId, string historyText, bool updateResolvesComplaint) => LDA.ExecuteSingle<int>("complaints.ComplaintAddHistory", Db, SqlParams.Single("ComplaintId", complaintId), SqlParams.Single("HistoryDetail", historyText), SqlParams.Single("UpdateResolvesComplaint", updateResolvesComplaint)).Result;

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.ComplaintHistoriesGetByComplaintId")]
        public List<ComplaintHistory> GetHistories(int complaintId) => LDA.ExecuteList<ComplaintHistory>("complaints.ComplaintHistoriesGetByComplaintId", Db, SqlParams.Single("ComplaintId", complaintId)).Result;

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.FlagUpdate")]
        public void FlagSave(int flagId, bool enablesControlMailFields) => LDA.Execute("complaints.FlagUpdate", Db, SqlParams.Single("FlagId", flagId), SqlParams.Single("EnablesControlMailFields", enablesControlMailFields));

        [UsesSproc(DataAccessHelper.Database.Uls, "complaints.FlagsGetByComplaintId")]
        public List<Flag> FlagsGetByComplaintId(int complaintId) => LDA.ExecuteList<Flag>("complaints.FlagsGetByComplaintId", Db, SqlParams.Single("ComplaintId", complaintId)).Result;

        [UsesSproc(DataAccessHelper.Database.Csys, "spSYSA_GetSqlUsers")]
        public string GetUserName(string name) => LDA.ExecuteList<Users>("spSYSA_GetSqlUsers", DataAccessHelper.Database.Csys, SqlParams.Single("IncludeInactiveRecords", 1)).Result.Where(p => p.WindowsUserName == name).FirstOrDefault()?.FullName ?? "";

        public List<Complaint> OpenComplaintsByPartyId(int partyId) => Search(null, true, false, null, partyId, null, null);

        public List<Complaint> OpenComplaintsByGroupId(int groupId) => Search(null, true, false, null, null, null, groupId);

        public List<Complaint> OpenComplaintsByTypeId(int typeId) => Search(null, true, false, null, null, typeId, null);

        /// <summary>
        /// Returns the values in the form of a DataTable
        /// </summary>
        private DataTable ToDataTableParameter(IEnumerable<int> values)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            foreach (var val in values)
                dt.Rows.Add(val);
            return dt;
        }
    }
}