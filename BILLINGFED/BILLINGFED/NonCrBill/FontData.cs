using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace BILLINGFED
{
    class FontData
    {
        public int FontTypeId { get; set; }
        public string FontType { get; set; }
        public int EnumValue { get; set; }
        public float FontSize { get; set; }
        public bool IsBold { get; set; }

        public FontData()
        { }

        [UsesSproc(DataAccessHelper.Database.Cls, "[billing].GetFontFromId")]
        public static FontData GetFontFromId(int id)
        {
            return DataAccessHelper.ExecuteSingle<FontData>("billing.GetFontFromId", DataAccessHelper.Database.Cls, SqlParams.Single("FontTypeId", id));
        }
    }
}
