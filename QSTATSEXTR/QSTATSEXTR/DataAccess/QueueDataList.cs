using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSTATSEXTR
{
    class QueueDataList : List<QueueData>
    {
        readonly object locker = new object();
        public new void Add(QueueData data)
        {
            lock (locker)
            {
                var match = this.SingleOrDefault(o => o.Queue == data.Queue);
                if (match == null)
                {
                    base.Add(data);
                }
                else
                {
                    match.Cancelled += data.Cancelled;
                    match.Complete += data.Complete;
                    match.Critical += data.Critical;
                    match.Late += data.Late;
                    match.Outstanding += data.Outstanding;
                    match.Problem += data.Problem;
                    match.Total += data.Total;
                    match.ScrapedUserData.AddRange(data.ScrapedUserData);
                }
            }
        }

        public new void AddRange(IEnumerable<QueueData> data)
        {
            foreach (var datum in data)
                Add(datum);
        }
    }
}
