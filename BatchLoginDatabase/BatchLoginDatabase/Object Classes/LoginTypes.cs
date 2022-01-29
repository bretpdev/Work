using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace BatchLoginDatabase
{
    public class LoginTypes
    {
        public int LoginTypeId { get; set; }
        public string LoginType { get; set; }
        public int MaxInUse { get; set; }

        /// <summary>
        /// Get all Login Types
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.BatchProcessing, "GetAllLoginTypes")]
        public static List<LoginTypes> GetAllLoginTypes()
        {
            return DataAccessHelper.ExecuteList<LoginTypes>("GetAllLoginTypes", DataAccessHelper.Database.BatchProcessing);
        }
    }


}
