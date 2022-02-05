using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace LOCATE
{
    public class LocateTypes
    {
        public string LocateType { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", LocateType, ShortDescription);
        }

        [UsesSproc(DataAccessHelper.Database.Bsys, "GetLocateTypes")]
        public static List<LocateTypes> GetLocateTypes()
        {
            return DataAccessHelper.ExecuteList<LocateTypes>("GetLocateTypes", DataAccessHelper.Database.Bsys);
        }
    }
}
