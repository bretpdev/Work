using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;
using Uheaa.Common.WinForms;

namespace XmlGeneratorECorr
{
    class TagAttributeValues
    {
        [Hidden, PrimaryKey]
        public int TagAttributeValueId { get; set; }
        [Required, MaxLength(250)]
        public string Attribute { get; set; }
        [MaxLength(250)]
        public string Value { get; set; }

        [UsesSproc(DataAccessHelper.Database.ECorrFed, "UpdateAttributes")]
        public static void UpdateDb(TagAttributeValues value)
        {
            DataAccessHelper.Execute("UpdateAttributes", DataAccessHelper.Database.ECorrFed, SqlParams.Update(value));
        }

        [UsesSproc(DataAccessHelper.Database.ECorrFed, "InsertAttribute")]
        public static void AddRecordDb(TagAttributeValues attributeData)
        {
            DataAccessHelper.Execute("InsertAttribute", DataAccessHelper.Database.ECorrFed, SqlParams.Insert(attributeData));
        }
    }
}
