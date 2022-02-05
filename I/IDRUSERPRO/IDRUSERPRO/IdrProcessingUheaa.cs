using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;

namespace IDRUSERPRO
{
    public class IDRProcessingUheaa : ScriptBase
    {
        public IDRProcessingUheaa(ReflectionInterface ri)
            : base(ri, "IDRUSERPRO", DataAccessHelper.Region.Uheaa)
        {
        }

        public override void Main()
        {
            new IDRProcessing(ScriptId, UserId, RI).Process();
        }
    }
}