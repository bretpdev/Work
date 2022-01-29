using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;
using Uheaa.Common.Scripts;

namespace LOCATE
{
    class UheaaProcessing : SessionBase
    {
        private enum System
        {
            Compass,
            OneLink,
            Both,
            None
        }

        public UheaaProcessing(ReflectionInterface ri, string accountIdentifer, ProcessLogData logData)
            : base(ri, accountIdentifer, logData)
        {

        }

        public void Process()
        {
            System sys = GetBorrowersSystem();
            if (sys == System.None)
            {
                Dialog.Error.Ok("There are no loans on OneLink or Compass.");
                return;
            }

            SkipType compassType = IsCompassLocate();
            SkipType oneLinkType = IsOneLinkLocate();

            if (compassType == SkipType.None && oneLinkType == SkipType.None)
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

            ProcessLocates(compassType, oneLinkType, locateType);

            Dialog.Info.Ok("Processing Complete");
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "GetLocateQueues")]
        private void ProcessLocates(SkipType compassType, SkipType oneLinkType, LocateTypes locateType)
        {
            List<string> queues = DataAccessHelper.ExecuteList<string>("GetLocateQueues", DataAccessHelper.Database.Bsys);
            if (compassType != SkipType.None && oneLinkType != SkipType.None)
            {
                Dialog.Info.Ok("The locate will be added to both Systems");
                ProcessCompassLocate(locateType, compassType, queues);
                ProcessOneLinkLocate(locateType, oneLinkType, queues);
            }
            else if (compassType != SkipType.None)
            {
                Dialog.Info.Ok("The locate will be added to Compass");
                ProcessCompassLocate(locateType, compassType, queues);
            }
            else if (oneLinkType != SkipType.None)
            {
                Dialog.Info.Ok("The locate will be added to OneLink");
                ProcessOneLinkLocate(locateType, oneLinkType, queues);
            }
        }

        private System GetBorrowersSystem()
        {
            bool compass = HasOpenLoansOnCompass();
            bool oneLink = HasOpenLoansOnOneLink();

            if (compass && oneLink)
                return System.Both;
            else if (compass && !oneLink)
                return System.Compass;
            else if (!compass && oneLink)
                return System.OneLink;
            else
                return System.None;
        }


    }
}
