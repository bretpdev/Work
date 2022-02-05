using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace _Starter
{
    class Program
    {
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;

            ReflectionInterface ri = new ReflectionInterface();
            BatchProcessingHelper LoginHelper = BatchProcessingHelper.GetNextAvailableId("BTCHLTRSFD", "BatchCornerstone");
            ri.Login(LoginHelper.UserName, LoginHelper.Password);
            new BTCHLTRSFD.BatchLettersFed(ri).Main();
        }
    }
}
