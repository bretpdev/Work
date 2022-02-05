using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace BillingRemoveCRCode
{
    class BillData
    {
        public enum FontType
        {
            Arial = 1,
            ArialLoan = 2,
            ScanLine =3
        }

        public int BillDataId { get; set; }
        public string SASFieldName { get; set; }
        public float XCoord { get; set; }
        public float YCoord { get; set; }
        public int? VertialAlign { get; set; }
        public int? HorizontalAlign { get; set; }
        public object Value { get; set; }//This will be pulled from the file
        public FontType Font { get; set; }

        public static List<BillData> GetData()
        {
            return DataAccessHelper.ExecuteList<BillData>("[billing].GetBillData", DataAccessHelper.Database.Cls);
        }

    }
}
