using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using Uheaa.Common.DataAccess;

namespace PHNCNUPFED
{
    class DataAccess
    {
        public static List<QueueInformation> GetQueueInfo()
        {
            return DataAccessHelper.GetContext(DataAccessHelper.Database.Cls).ExecuteQuery<QueueInformation>("spGetPhoneConsentQueues").ToList();
        }
    }
}
