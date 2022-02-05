using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace MDIntermediary
{
    class AlternateAddress
    {
        [PrimaryKey]
        public int AlternateAddressId { get; set; }
        [InsertOnly]
        public string AccountNumber{ get; set;}
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public AlternateAddress()
        {
            AccountNumber = Address1 = Address2 = City = State = Zip = Country = "";
        }

        public static List<AlternateAddress> GetAltAddresses(DataAccessHelper.Region region, string accountNumber)
        {
            return DataAccessHelper.ExecuteList<AlternateAddress>("[altaddress].GetAltAddresses", Db(region), SqlParams.Single("AccountNumber", accountNumber));
        }

        public static AlternateAddress AddNew(DataAccessHelper.Region region, AlternateAddress address)
        {
            return DataAccessHelper.ExecuteSingle<AlternateAddress>("[altaddress].AddAltAddress", Db(region), SqlParams.Insert(address));
        }

        public static void Update(DataAccessHelper.Region region, AlternateAddress address)
        {
            DataAccessHelper.Execute("[altaddress].UpdateAltAddress", Db(region), SqlParams.Update(address));
        }

        public static void Delete(DataAccessHelper.Region region, int alternateAddressId)
        {
            DataAccessHelper.Execute("[altaddress].DeleteAltAddress", Db(region), SqlParams.Single("AlternateAddressId", alternateAddressId));
        }

        private static DataAccessHelper.Database Db(DataAccessHelper.Region region)
        {
            return DataAccessHelper.Database.Udw;
        }
    }
}
