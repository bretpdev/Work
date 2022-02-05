using PMTHIST;
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
        static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "PMTHIST", false))
            {
                return 1;
            }
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            ReflectionInterface ri = new ReflectionInterface();

           new PaymentHistory(ri).Main();
            DataAccessHelper.CloseAllManagedConnections();
            return 0;
        }
    }
}
