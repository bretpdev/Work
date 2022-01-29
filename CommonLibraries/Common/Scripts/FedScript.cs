using Uheaa.Common.DataAccess;

namespace Uheaa.Common.Scripts
{
    public class FedScript :ScriptBase
    {
        public FedScript(ReflectionInterface ri, string scriptId)
			: base(ri, scriptId, DataAccessHelper.Region.CornerStone)
		{
            DataAccessHelper.CurrentRegion = DataAccessHelper.Region.CornerStone;
        }
    }
}
