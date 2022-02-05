using System;
using Uheaa.Common.Scripts;

namespace LENDERLTRS
{
    public class LenderData
    {
        public string Mod { get; set; }
        public string LenderId { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Type { get; set; }
        public bool Valid { get; set; }
        public string AddedBy
        {
            get
            {
                return Environment.UserName;
            }
        }

        public LenderData()
        { }

        public LenderData(ReflectionInterface ri)
        {
            Mod = ri.ScreenCode == "TXX00" ? "A" : "C";
            if (ri.ScreenCode == "TXX04")
                ri.PutText(22, 18, "01", ReflectionInterface.Key.Enter, true);
            LenderId = ri.GetText(5, 19, 6);
            FullName = ri.GetText(6, 19, 40);
            ShortName = ri.GetText(7, 19, 20);
            Address1 = ri.GetText(11, 23, 30);
            Address2 = ri.GetText(12, 23, 30).Replace("_","");
            City = ri.GetText(14, 13, 20);
            State = ri.GetText(14, 53, 2);
            Zip = ri.GetText(14, 69, 9);
            Valid = ri.GetText(22, 10, 1) == "Y" ? true : false;
        }

    }
}
