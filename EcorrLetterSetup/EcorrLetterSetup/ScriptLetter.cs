using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace EcorrLetterSetup
{
    class ScriptLetter : Letter
    {
        public List<ScriptLetterData> Data { get; set; }
        private string LetterId { get; set; }

        public ScriptLetter(LetterSearchData letter)
            :base(letter)
        {
            LetterId = letter.LetterId;
            GetDBData();
        }

        /// <summary>
        /// Gets the Database Data.
        /// </summary>
        private void GetDBData()
        {
           Data = DataAccessHelper.ExecuteList<ScriptLetterData>("GetScriptLetterData", DataAccessHelper.Database.Bsys, LetterId.ToSqlParameter("LetterId"));
        }

        /// <summary>
        /// Shows the AddRecord form
        /// </summary>
        /// <param name="letterId">Letter Id</param>
        public override void AddRecord(string letterId)
        {
            ShowAddChangeForm(letterId);
        }

        /// <summary>
        /// Shows the change record form
        /// </summary>
        /// <param name="letterId">Letter Id</param>
        /// <param name="selectedData">Selected letter data</param>
        public override void ChangeRecord(string letterId, dynamic selectedData)
        {
            ShowAddChangeForm(letterId, selectedData);
        }

        /// <summary>
        /// Gets the Data Object. This is needed so that the LetterSetup form can access the Data Property.
        /// </summary>
        /// <returns></returns>
        public override dynamic GetData()
        {
            GetDBData();
            return Data.Select(p => new {p.LetterHeaderMappingId, p.Header, p.HeaderType, p.Order, p.Active }).ToList();
        }

        public override dynamic GetDataForPromotion()
        {
            GetDBData();
            return Data;
        }

        /// <summary>
        /// Shows the AddChangeForm.
        /// </summary>
        /// <param name="letterId"></param>
        /// <param name="selectedData"></param>
        private void ShowAddChangeForm(string letterId, dynamic selectedData = null)
        {
            using (AddChangeScriptLetter scrLtr = new AddChangeScriptLetter(letterId, selectedData))
            {
                if (scrLtr.ShowDialog() == DialogResult.OK)
                    GetDBData();
            }
        }
    }
}
