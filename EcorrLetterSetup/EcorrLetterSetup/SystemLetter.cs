using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace EcorrLetterSetup
{
    class SystemLetter : Letter
    {
        public List<SystemSprocs> Data { get; set; }
        private string LetterId { get; set; }
        private LetterSearchData Letter { get; set; }

        public SystemLetter(LetterSearchData letter)
            : base(letter)
        {
            Letter = letter;
        }

        /// <summary>
        /// Shows the Add Record Form.
        /// </summary>
        /// <param name="letterId"></param>
        public override void AddRecord(string letterId)
        {
            using (AddChangeSystemLetter addLetter = new AddChangeSystemLetter())
            {
                if (addLetter.ShowDialog() == DialogResult.OK)
                {
                    DataAccessHelper.Execute("AddSprocToAGivenLetter", DataAccessHelper.Database.Bsys, new SqlParameter("DocDetailId", Letter.DocDetailId),
                        new SqlParameter("SprocName", addLetter.Sproc.StoredProcedureName), new SqlParameter("ReturnType", addLetter.Sproc.ReturnType));
                }
            }
        }

        /// <summary>
        /// Shows the Change Letter Form.
        /// </summary>
        /// <param name="letterId"></param>
        /// <param name="selectedData"></param>
        public override void ChangeRecord(string letterId, dynamic selectedData)
        {
            using (AddChangeSystemLetter addLetter = new AddChangeSystemLetter(selectedData))
            {
                if (addLetter.ShowDialog() == DialogResult.OK)
                {
                    DataAccessHelper.Execute("LTDB_UpdateSystemSproc", DataAccessHelper.Database.Bsys, new SqlParameter("Id", selectedData.SystemLettersStoredProcedureId)
                        , new SqlParameter("StoredProcedureName", selectedData.StoredProcedureName), new SqlParameter("ReturnType", selectedData.ReturnType), 
                            new SqlParameter("Active", selectedData.Active) );
                }
            }
        }

        /// <summary>
        /// Gets the Data Object.  This is used in the LetterSetup class
        /// </summary>
        /// <returns></returns>
        public override dynamic GetData()
        {
            return Data = DataAccessHelper.ExecuteList<SystemSprocs>("GetSprocsForGivenLetter", DataAccessHelper.Database.Bsys, Letter.DocDetailId.ToSqlParameter("LetterId"));
        }

        public override dynamic GetDataForPromotion()
        {
            return GetData();
        }
    }
}
