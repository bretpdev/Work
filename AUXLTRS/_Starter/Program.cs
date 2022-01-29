using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace _Starter
{
    public class Program
    {
         public static int Main(string[] args)
        {
               DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            if (!DataAccessHelper.StandardArgsCheck(args, "AUXLTRS"))
                return 1;
            if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly()))
                return 1;

            ReflectionInterface RI = new ReflectionInterface();            
           
            
            new AUXLTRS.CollectorLetters(RI).Main();

            return 0;
        }
    }
}
