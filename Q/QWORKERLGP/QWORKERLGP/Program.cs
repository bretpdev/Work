using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using static System.Console;

namespace QWORKERLGP
{
    class Program
    {
        public const int SUCCESS = 0;
        public const int FAILURE = 1;
        public const string ScriptId = "QWORKERLGP";
        public static Args ApplicationArgs { get; set; }

        public static int Main(string[] args)
        {
            WriteLine(Assembly.GetExecutingAssembly().GetName().Version);

            if(args.Length == 0)
            {
                WriteLine("Missing KVP Args");
                Dialog.Error.Ok("Missing KVP Args");
                return FAILURE;
            }

            var results = KvpArgValidator.ValidateArguments<Args>(args);
            if(results.IsValid)
            {
                ApplicationArgs = new Args(args);
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
                DataAccessHelper.CurrentMode = ApplicationArgs.Mode.ToLower() == "live" ? DataAccessHelper.Mode.Live : DataAccessHelper.Mode.Dev;

                if(!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), ApplicationArgs.ShowPrompts))
                {
                    return FAILURE;
                }

                return new QueueProcessor().Process();
            }
            else
            {
                WriteLine(results.ValidationMesssage);
                Dialog.Error.Ok(results.ValidationMesssage);
                return FAILURE;
            }
        }
    }
}
