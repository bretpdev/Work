using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace BILLINGFED
{
    [Serializable()] //Needed for a copy method
    public class BillData
    {
        public int BillDataId { get; set; }
        public string SASFieldName { get; set; }
        public float XCoord { get; set; }
        public float YCoord { get; set; }
        public int? VertialAlign { get; set; }
        public int? HorizontalAlign { get; set; }
        public string Value { get; set; }//This will be pulled from the file
        public int FontTypeId { get; set; }
        public string FontType { get; set; }
        public int EnumValue { get; set; }
        public float FontSize { get; set; }
        public bool IsBold { get; set; }
    }
}
