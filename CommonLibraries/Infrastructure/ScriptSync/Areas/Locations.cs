using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace Uheaa
{
    public static class LocalLocations
    {
        public const string EnterpriseProgramFiles = @"C:\Enterprise Program Files\";
        public const string NexusLive = EnterpriseProgramFiles + @"Nexus\";
        public const string NexusTest = NexusLive + @"Test\";
        public static string Nexus(DataAccessHelper.Mode mode)
        {
            return mode == DataAccessHelper.Mode.Test ? NexusTest : NexusLive;
        }
        public const string FedScriptsLive = EnterpriseProgramFiles + @"FedScripts\";
        public const string FedScriptsTest = FedScriptsLive + @"Test\";
        public static string FedScripts(DataAccessHelper.Mode mode)
        {
            return mode == DataAccessHelper.Mode.Test ? FedScriptsTest : FedScriptsLive;
        }
        public const string FfelScriptsLive = EnterpriseProgramFiles + @"FfelScripts\";
        public const string FfelScriptsTest = FfelScriptsLive + @"Test\";
        public static string FfelScripts(DataAccessHelper.Mode mode)
        {
            return mode == DataAccessHelper.Mode.Test ? FfelScriptsTest : FfelScriptsLive;
        }
    }
    public static class NetworkLocations
    {
        public const string FfelLive = @"X:\Sessions\UHEAA Codebase";
        public const string FfelTest = @"X:\PADU\UHEAACodeBase";
        public static string Ffel(DataAccessHelper.Mode mode)
        {
            return mode == DataAccessHelper.Mode.Test ? FfelTest : FfelLive;
        }
        public const string FedLive = @"Z:\Codebase\Scripts";
        public const string FedTest = @"Y:\Codebase\Scripts";
        public static string Fed(DataAccessHelper.Mode mode)
        {
            return mode == DataAccessHelper.Mode.Test ? FedTest : FedLive;
        }
    }
}
