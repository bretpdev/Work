using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.Scripts;
using Uheaa.Common;
using Uheaa.Common.ProcessLogger;

namespace LOCATE
{
    class CornerStoneProcessing : SessionBase
    {
        public CornerStoneProcessing(ReflectionInterface ri, string accountIdentifier, ProcessLogData logData)
            : base(ri, accountIdentifier, logData)
        {
        }


        public void Process()
        {
            SkipType compassType = IsCompassLocate();

            if (compassType == SkipType.None)
            {
                Dialog.Error.Ok("The borrower is not a locate.");
                return;
            }

            LocateTypes locateType = null;
            using (LocateInfo locate = new LocateInfo())
            {
                locate.ShowDialog();
                locateType = locate.SelectedType;
            }

            List<string> queues = DataAccessHelper.ExecuteList<string>("GetLocateQueues", DataAccessHelper.Database.Bsys);
            Dialog.Info.Ok("The locate will be added to Compass");
            ProcessCompassLocate(locateType, compassType, queues);
            Dialog.Info.Ok("Processing Complete");
        }
    }
}
