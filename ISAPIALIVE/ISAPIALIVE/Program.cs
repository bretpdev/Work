using System;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace ISAPIALIVE
{
    public class Program
    {

        static int Main(string[] args)
        {
            var result = KvpArgValidator.ValidateArguments<Args>(args);

            if (!result.IsValid)
            {
                Console.WriteLine(result.ValidationMesssage);
                return 1;
            }
            else
            {
                var parsedArgs = new Args(args);
                DataAccessHelper.CurrentMode = parsedArgs.Mode;
                DataAccessHelper.CurrentRegion = parsedArgs.Region;

                return new ProcessAPI().Process(parsedArgs.AccountNumber);
            }
        }
    }
}