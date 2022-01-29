using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.DocumentProcessing;
using Uheaa.Common.ProcessLogger;

namespace ECORXMLGEN
{
    public class DataAccess
    {
        private LogDataAccess LDA { get; set; }
        private DataAccessHelper.Database DB { get; set; }
        public DataAccess(int plId)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, plId, false, true);
            if (DataAccessHelper.CurrentRegion == DataAccessHelper.Region.CornerStone)
                DB = DataAccessHelper.Database.ECorrFed;
            else
                DB = DataAccessHelper.Database.EcorrUheaa;
        }

        /// <summary>
        /// Gets the attributes for the XML file
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.EcorrUheaa, "GetAttributesAndValues")]
        public List<EcorrXmlAttributeValues> GetXMLAttributes(string tag)
        {
            return GetLDAResult(LDA.ExecuteList<EcorrXmlAttributeValues>("GetAttributesAndValues", DB, SqlParams.Single("Tag", tag)));
        }

        /// <summary>
        /// Update the zipfilename for all documentdetailsIds in the zip file
        /// </summary>t -
        /// <param name="ids"></param>
        /// <param name="zipFile"></param>
        [UsesSproc(DataAccessHelper.Database.EcorrUheaa, "UpdateZipFIleName")]
        public void UpdateZipFIleName(DataTable ids, string zipFile)
        {
            //No Need to do anything if this fails as it will be processed logged in the common code.
            LDA.Execute("UpdateZipFIleName", DB, SqlParams.Single("DDIds", ids), SqlParams.Single("ZipFileName", zipFile));
        }

        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "spGetDecrpytedPassword")]
        public string GetTestPassword(string userName)
        {
            return GetLDAResult(LDA.ExecuteSingle<string>("spGetDecrpytedPassword", DataAccessHelper.Database.BatchProcessing, SqlParams.Single("UserId", userName)));
        }

        /// <summary>
        /// Gets the next record to be processed.
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.EcorrUheaa, "GetNextUnprocessedRecord")]
        public DocumentProperties GetNextUnprocessedRecord()
        {
            return GetLDAResult(LDA.ExecuteSingle<DocumentProperties>("GetNextUnprocessedRecord", DB));
        }

        private T GetLDAResult<T>(ManagedDataResult<T> result)
        {
            if (result.DatabaseCallSuccessful)
                return result.Result;
            else
                return default(T);
        }
    }
}
