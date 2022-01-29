using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.Scripts;
using Uheaa.Common.DataAccess;

namespace PAYHISTFED
{
    static class Program
    {
        const string ScriptId = "PAYHISTFED";

        public static string DLODeal = "PSAOG";
        public static string LNCDeal = "PSAOH";
        public static string SaleDate = "20201109";

        [STAThread]
        static int Main(string[] args)
        {
            DataAccessHelper.StandardArgsCheck(args, ScriptId);
            if (args.Length > 1)
            {
                DLODeal = args[1];
                LNCDeal = args[2];
                SaleDate = args[3];
            }
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
            new PaymentHistoryReport().Main();
            return 0;
        }
    }
}
