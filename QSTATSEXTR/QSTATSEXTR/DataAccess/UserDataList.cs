using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSTATSEXTR
{
    class UserDataList : List<UserData>
    {
        readonly object locker = new object();
        public new void Add(UserData user)
        {
            lock (locker)
            {
                var match = this.SingleOrDefault(o => o.Queue == user.Queue && o.Status == user.Status && o.UserId == user.UserId);
                if (match == null)
                    base.Add(user);
                else
                {
                    match.CountInStatus++;
                    match.TotalTimeWorked += user.TotalTimeWorked;
                }
            }
        }

        public new void AddRange(IEnumerable<UserData> users)
        {
            foreach (var user in users)
                this.Add(user);
        }
    }
}
