using System;
using Uheaa.Common.DataAccess;

namespace OLDEMOS
{
    public class WarehouseInfo
    {
        private WarehouseInfo() { }
        [UsesSproc(DataAccessHelper.Database.Udw, "GetLastWarehouseRefreshTime")]
        public static WarehouseInfo RetrieveWarehouseInfo()
        {
            var info = new WarehouseInfo();
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
    }
}