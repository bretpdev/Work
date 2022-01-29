using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.WinForms;

namespace IMGEMAILAR
{
    class DataAccess
    {
        readonly ProcessLogRun PLR;
        public DataAccess(ProcessLogRun plr)
        {
            PLR = plr;
            UheaaDocuments = ExecuteListIgnore<Document>("imgemailar.GetDocIds", DataAccessHelper.Database.Uls);

            UheaaAvailableDocuments = GetAvailableDocuments(DataAccessHelper.Database.Uls);
            OnelinkAvailableDocuments = GetAvailableDocuments(DataAccessHelper.Database.Ols);
        }

        private List<T> ExecuteListIgnore<T>(string commandName, DataAccessHelper.Database db)
        {
            try
            {
                //using the static method purposefully here so exceptions aren't logged
                return DataAccessHelper.ExecuteList<T>(commandName, db);
            }
            catch
            {
                //exception indicates no access;
                return null;
            }
        }

        public List<Document> UheaaDocuments { get; private set; }

        public List<Letter> UheaaAvailableDocuments { get; private set; }
        public List<Letter> OnelinkAvailableDocuments { get; private set; }

        [UsesSproc(DataAccessHelper.Database.Uls, "imgemailar.GetAvailableDocuments")]
        public List<Letter> GetAvailableDocuments(DataAccessHelper.Database db)
        {
            return ExecuteListIgnore<Letter>("imgemailar.GetAvailableDocuments", db);
        }

        [UsesSproc(DataAccessHelper.Database.Csys, "acthist.GetRecordsByBorrower")]
        public string GetCsysAesId()
        {
            var result = PLR.LDA.ExecuteList<string>("[dbo].[GetAESIdFromWindowsUserName]", DataAccessHelper.Database.Csys, SqlParams.Single("WindowsUserId", Environment.UserName.Replace("UHEAA\\", "")));
            return result.Result.FirstOrDefault();
        }

        public bool HasUheaaAccess => UheaaAvailableDocuments?.Any() ?? false;
        public bool HasOnelinkAccess => OnelinkAvailableDocuments?.Any() ?? false;
    }
}
