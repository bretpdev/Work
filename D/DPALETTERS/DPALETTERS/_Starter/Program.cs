using DPALETTERS.Confirmation;
using DPALETTERS.Cancellation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace _Starter
{
    class Program
    {
        public static string ScriptId { get; set; } = "DPALETTERS";

        static void Main(string[] args)
        {
//            var argResults = KvpArgValidator.ValidateArguments<Args>(args);
//            if (!argResults.IsValid)
//            {
//                //Console.WriteLine(argResults.ValidationMesssage);
//#if DEBUG
//                Console.ReadKey(); //when debugging stop the console so it displays the result
//#endif
//            }
//            else
//            {
                //Process the request
//                var parsedArgs = new Args(args);
                DataAccessHelper.CurrentMode = DataAccessHelper.Mode.Dev;
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
                ReflectionInterface ri = new ReflectionInterface();
                //CancellationProcessor processor = new CancellationProcessor(ri);
                ConfirmationProcessor processor = new ConfirmationProcessor(ri);
                processor.Main();
                //TODO
            //}
        }
    }
}
