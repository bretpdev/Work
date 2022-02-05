using System;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using DB = Uheaa.Common.DataAccess.DataAccessHelper.Database;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using ACS.Infrastructure;
using Uheaa.Common;

namespace ACS
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }
        public string ScriptId { get { return "ACS"; } }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = logRun.LDA;
        }

        #region onelink
        [UsesSproc(DB.Ols, "[acs].GetOneLINKDemographics")]
        public Name_Spec OneLINKDemos(string ssn)
        {
            Name_Spec demos = LDA.ExecuteSingle<Name_Spec>("[acs].GetOneLINKDemographics", DB.Ols, new SqlParameter("Ssn", ssn)).Result;
            return demos;
        }
        
        [UsesSproc(DB.Ols, "[acs].MarkRecordProcessed")]
        public void MarkOneLinkRecordProcessed(int oneLinkDemographicsId, int arcId)
        {
            LDA.Execute("[acs].MarkRecordProcessed", DB.Ols,
            SqlParams.Single("OneLinkDemographicsId", oneLinkDemographicsId), SqlParams.Single("ArcId", arcId));
        }

        [UsesSproc(DB.Ols, "[acs].GetUnprocessedRecords")]
        public List<AcsOlFileRecord> GetUnprocessedOneLINKRecords()
        {
            return LDA.ExecuteList<AcsOlFileRecord>("[acs].GetUnprocessedRecords", DB.Ols).Result;
        }
        #endregion

        #region uheaa
        [UsesSproc(DB.Uls, "[acs].MarkRecordProcessed")]
        public void MarkUheaaRecordProcessed(int uheaaDemographicsId, int arcId)
        {
            LDA.Execute("[acs].MarkRecordProcessed", DB.Uls,
            SqlParams.Single("UheaaDemographicsId", uheaaDemographicsId), SqlParams.Single("ArcId", arcId));
        }

        [UsesSproc(DB.Uls, "[acs].LoadRecord")]
        public bool LoadRecord(AcsOlFileRecord acs)
        {
            return LDA.ExecuteSingle<bool>("[acs].LoadRecord", DB.Uls, 
                SqlParams.Single("PType", (char)acs.PType), 
                SqlParams.Single("SSN", acs.SSN), 
                SqlParams.Single("AddrDt", acs.POAddressDate), 
                SqlParams.Single("FirstFour", acs.FirstName.SafeSubString(0,4)),
                SqlParams.Single("BorName", acs.ConcatenatedData.FullName),
                SqlParams.Single("Addr1", acs.NewAddress.Addr1),
                SqlParams.Single("City", acs.NewAddress.City),
                SqlParams.Single("State", acs.NewAddress.State),
                SqlParams.Single("Zip", acs.NewAddress.Zip),
                SqlParams.Single("FileId", acs.FileId),
                SqlParams.Single("NewAddressFull", acs.ConcatenatedData.NewAddress),
                SqlParams.Single("OldAddressFull", acs.ConcatenatedData.OldAddress)).Result;
        }

        [UsesSproc(DB.Uls, "[acs].GetUnprocessedRecords")]
        public List<AcsUhFileRecord> GetUnprocessedUheaaRecords()
        {
            return LDA.ExecuteList<AcsUhFileRecord>("[acs].GetUnprocessedRecords", DB.Uls).Result;
        }
        #endregion

        public string applyStandardAbbreviations(string addressPart)
        {
            if (addressPart == null)
                return "";

            addressPart = addressPart.Replace("STREET", "ST");
            addressPart = addressPart.Replace("AVENUE", "AVE");
            addressPart = addressPart.Replace("ROAD", "RD");
            addressPart = addressPart.Replace("LANE", "LN");
            addressPart = addressPart.Replace("DRIVE", "DR");
            addressPart = addressPart.Replace("HIGHWAY", "HWY");
            addressPart = addressPart.Replace("FLOOR", "FL");
            addressPart = addressPart.Replace("P O BOX", "PO BOX");
            addressPart = addressPart.Replace("P O BX", "PO BOX");
            addressPart = addressPart.Replace("-", " ");
            return addressPart;
        }
    }
}
