using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;

namespace XmlGeneratorECorr
{
    class LetterTypes
    {
        [PrimaryKey,  Hidden]
        public int LetterTypeId { get; set; }
        [Required, MaxLength(50)]
        public string LetterType { get; set; }

        [UsesSproc(DataAccessHelper.Database.ECorrFed, "UpdateLetterTypes")]
        public static void UpdateDb(LetterTypes letterTypeData)
        {
            DataAccessHelper.Execute("UpdateLetterTypes", DataAccessHelper.Database.ECorrFed, SqlParams.Update(letterTypeData));
        }

        [UsesSproc(DataAccessHelper.Database.ECorrFed, "InsertLetterType")]
        public static void AddRecordDb(LetterTypes letterTypeData)
        {
            DataAccessHelper.Execute("InsertLetterType", DataAccessHelper.Database.ECorrFed, SqlParams.Insert(letterTypeData));
        }
    }
}
