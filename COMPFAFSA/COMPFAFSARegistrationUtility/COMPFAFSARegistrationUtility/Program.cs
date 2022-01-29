using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;

namespace COMPFAFSARegistrationUtility
{
    class Program
    {
        /// <summary>
        /// args[0]: mode == "dev", "live", string
        /// args[1]: email, string
        /// args[2]: password, string
        /// args[3]: fullName, string
        /// args[4]: admin, bool
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            DataAccessHelper.Mode mode = DataAccessHelper.Mode.Dev;
            if(args[0].ToLower() == "live")
            {
                mode = DataAccessHelper.Mode.Live;
            }
            string email = args[1];
            string password = args[2];
            string fullName = args[3];
            bool admin = args[4].ToLower() == "true";
            DataAccessHelper helper = new DataAccessHelper(mode);
            var result = helper.Register(email, password, fullName, admin);
            Console.WriteLine("Registration result: " + result.ToString());
            Console.ReadKey();

        }
    }
}
