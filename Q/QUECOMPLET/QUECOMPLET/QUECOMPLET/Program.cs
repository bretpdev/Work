using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using static System.Console;


namespace QUECOMPLET
{
    class Program
    {
        public static string ScriptId = "QUECOMPLET";
        public static Args ApplicationArgs { get; set; }

        /// <summary>
        /// ARGS:
        /// Mode (Required)
        /// ShowPrompts
        /// NumberOfThreads
        /// </summary>
        static int Main(string[] args)
        {
            WriteLine(Assembly.GetExecutingAssembly().GetName().Version);

            if (args.Length == 0)
            {
                Dialog.Error.Ok("Missing KVP Args.");
                return 1;
            }

            var results = KvpArgValidator.ValidateArguments<Args>(args);
            int returnVal;
            if (results.IsValid)
            {
                ApplicationArgs = new Args(args);
                DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
                DataAccessHelper.CurrentMode = ApplicationArgs.Mode.ToLower() == "live" ? DataAccessHelper.Mode.Live : DataAccessHelper.Mode.Dev;

                if (!DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), ApplicationArgs.ShowPrompts))
                    return 1;

                returnVal = new QueueProcessor().StartProcessing();
                DataAccessHelper.CloseAllManagedConnections();
            }
            else
            {
                WriteLine(results.ValidationMesssage);
                Dialog.Error.Ok(results.ValidationMesssage);
                return 1;
            }

            return returnVal;
        }
    }
}