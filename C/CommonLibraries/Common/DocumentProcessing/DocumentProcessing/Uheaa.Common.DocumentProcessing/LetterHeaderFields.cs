using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace Uheaa.Common.DocumentProcessing
{
    public class LetterHeaderFields
    {
        public List<string> Name { get; set; }
        public List<string> AddressLine { get; set; }
        public List<string> CityStateZip { get; set; }
        public List<string> ForeignFields { get; set; }
        public List<string> DataTable { get; set; }
        private List<string> MergeFieldKeys { get; set; }
        public Dictionary<string, string> MergeFields { get; set; }
        public List<MutliLineMergeField> MultiLineFields { get; set; }

        public LetterHeaderFields()
        {
            MultiLineFields = new List<MutliLineMergeField>();
        }

        /// <summary>
        /// Gets all of the fields that are needed for the letter.
        /// </summary>
        /// <param name="letterId">Letter Tracking Id</param>
        /// <returns>Object with fields required for the letter.</returns>
        public static LetterHeaderFields Populate(string letterId)
        {
            LetterHeaderFields fields = new LetterHeaderFields();
            Func<string, List<string>> getField = new Func<string, List<string>>(s =>
            {
                return DataAccessHelper.ExecuteList<string>("LTDB_GetHeader", DataAccessHelper.Database.Bsys,
                       letterId.ToSqlParameter("Letterid"), s.ToSqlParameter("Type"));
            });

            fields.Name = getField("Name");
            fields.AddressLine = getField("AddressLine");
            fields.CityStateZip = getField("CityStateZip");
            fields.ForeignFields = getField("ForeignAddressFields");
            fields.DataTable = getField("DataTable"); 
            fields.MergeFields = DataAccessHelper.ExecuteList<string>("LTDB_GetHeader", DataAccessHelper.Database.Bsys,
                       letterId.ToSqlParameter("Letterid"), "FormField".ToSqlParameter("Type")).ToDictionary(x => x, x => "");

            fields.MultiLineFields = GetMultiLineFields(letterId);

            return fields;
        }

        private static List<MutliLineMergeField> GetMultiLineFields(string letterId)
        {
            List<string> fields = DataAccessHelper.ExecuteList<string>("LTDB_GetAllMultiLineFieldsForLetter", DataAccessHelper.Database.Bsys, letterId.ToSqlParameter("LetterId"));
            List<MutliLineMergeField> multiLineFields = new List<MutliLineMergeField>();

            foreach (string field in fields)
                multiLineFields.Add(GetMultiLineField(letterId, field));

            return multiLineFields;
        }

        private static MutliLineMergeField GetMultiLineField(string letterId, string field)
        {
            return MutliLineMergeField.Populate(letterId, field);
        }
    }
}
