using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSTATSEXTR
{
    class QueueErrors
    {
        public List<string> QueuesWithNoManagers { get; set; }
        public List<string> QueuesWithBlankManagers { get; set; }
        public List<string> QueuesWithNoDescription { get; set; }
        public List<string> BadNoPopulateQueues { get; set; }
        public List<string> QueuesNeedingDaysLatePopulated { get; set; }
        public List<string> ErrorEmailAddresses { get; set; }

        public string GetErrorMessage()
        {
            List<string> lines = new List<string>();
            var checkAdd = new Action<List<string>, string>((list, message) =>
            {
                if (list.Any())
                {
                    lines.Add(message);
                    foreach (var queue in list)
                        lines.Add(" - " + queue);
                    lines.Add("");
                }
            });
            checkAdd(QueuesWithNoManagers, "The following queues can't be found in the Queue Detail table:");
            checkAdd(QueuesWithBlankManagers, "The following queues have no Business Unit associated with them:");
            checkAdd(QueuesWithNoDescription, "The following queues have no documented description:");
            checkAdd(BadNoPopulateQueues, "The following queues have a population and should not:");
            checkAdd(QueuesNeedingDaysLatePopulated, "The following OneLINK queues need the number of days before it's a late task \"NumOfDaysLateTask\" field populated:");
            if (!lines.Any())
                return null;
            return string.Join(Environment.NewLine, lines);
        }
    }
}
