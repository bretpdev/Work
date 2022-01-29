using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MD
{
    public class WarehouseInfo
    {
        private WarehouseInfo() { }
        //[UsesSproc(DataAccessHelper.Database.Cdw, "GetLastWarehouseRefreshTime")]
        [UsesSproc(DataAccessHelper.Database.Udw, "GetLastWarehouseRefreshTime")]
        public static WarehouseInfo RetrieveWarehouseInfo()
        {
            var info = new WarehouseInfo();
            try
            {
                info.CdwRefresh = DataAccessHelper.ExecuteSingle<DateTime>("GetLastWarehouseRefreshTime", DataAccessHelper.Database.Cdw);
            }
            catch (Exception)
            {
                //leave field null
            }
            try
            {
                info.UdwRefresh = DataAccessHelper.ExecuteSingle<DateTime>("GetLastWarehouseRefreshTime", DataAccessHelper.Database.Udw);
            }
            catch (Exception)
            {
                //leave field null
            }
            return info;
        }

        public DateTime? UdwRefresh { get; private set; }
        public DateTime? CdwRefresh { get; private set; }
    }
}
