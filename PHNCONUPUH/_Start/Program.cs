using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace _Start
{
    public class Program
    {
        static void Main(string[] args)
        {
            DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Live;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;

            ReflectionInterface ri = new ReflectionInterface();
            ri.PauseForInsert();

            new PHNCONUPUH.PhoneConsentUpdate(ri).Main();
        }
    }
}