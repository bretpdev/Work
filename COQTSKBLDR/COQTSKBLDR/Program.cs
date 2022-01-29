using System.Reflection;
using Uheaa.Common.DataAccess;

/// <summary>
/// Script launching point.
/// </summary>
namespace COQTSKBLDR
{
    class Program
    {
        static int Main(string[] args)
        {
            if (!DataAccessHelper.StandardArgsCheck(args, "COQTSKBLDR", false) || !DataAccessHelper.CheckSprocAccess(Assembly.GetExecutingAssembly(), false))
                return 1;
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.Uheaa;
            return new CompassQueueTaskBuilder().Run(args);
        }
    }
}
