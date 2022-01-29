using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using ThrdPrtyAuth;

namespace _Starter
{
    class Program
    {
        public static int Main(string[] args)
        {
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, "3RDPRTAUTH"))
                return 1;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return 1;

            Uheaa.Common.Scripts.ReflectionInterface refInt = new Uheaa.Common.Scripts.ReflectionInterface();
            
            new ThrdPrtyAuth.ThirdPartyAuthorization(refInt).Main();
            return 0;
        }
    }
}
