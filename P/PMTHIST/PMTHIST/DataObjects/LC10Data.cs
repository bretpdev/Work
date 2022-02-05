using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace PMTHIST
{
    public class LC10Data
    {
        //TODO maybe rename later to make more sense
        //string of decimal types
        public decimal PrincPur { get; set; }
        public decimal PrincCol { get; set; }
        public decimal IntAcc { get; set; }
        public decimal IntCol { get; set; }
        public decimal LegalAcc { get; set; }
        public decimal LegalCol { get; set; }
        public decimal OtherAcc { get; set; }
        public decimal OtherCol { get; set; }
        public decimal CCAcc { get; set; }
        public decimal CCCol { get; set; }
        public decimal CCProj { get; set; }
        //Calculated fields
        public decimal PrincCur { get; set; }
        public decimal IntCur { get; set; }
        public decimal LegalCur { get; set; }
        public decimal OtherCur { get; set; }
        public decimal CCCur { get; set; }
        public decimal TotalCur { get; set; }

        public void GetValuesFromLC10(ReflectionInterface ri)
        {
            PrincPur = ri.GetText(7, 35, 11).ToDecimal();
            PrincCol = ri.GetText(8, 35, 11).ToDecimal();
            IntAcc = ri.GetText(9, 35, 11).ToDecimal();
            IntCol = ri.GetText(10, 35, 11).ToDecimal();
            LegalAcc = ri.GetText(11, 35, 11).ToDecimal();
            LegalCol = ri.GetText(12, 35, 11).ToDecimal();
            OtherAcc = ri.GetText(13, 35, 11).ToDecimal();
            OtherCol = ri.GetText(14, 35, 11).ToDecimal();
            CCAcc = ri.GetText(15, 35, 11).ToDecimal();
            CCCol = ri.GetText(16, 35, 11).ToDecimal();
            CCProj = ri.GetText(17, 35, 11).ToDecimal();
            CalculateFields();
        }

        public void CalculateFields()
        {
            PrincCur = PrincPur - PrincCol;
            IntCur = IntAcc - IntCol;
            LegalCur = LegalAcc - LegalCol;
            OtherCur = OtherAcc - OtherCol;
            CCCur = (CCAcc - CCCol);
            TotalCur = PrincCur + IntCur + LegalCur + OtherCur + CCCur + CCProj;
        }
    }
}
