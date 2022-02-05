using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRBRIDGE
{
    class Parking
    {
        [JsonProperty("table")]
        public string Table { get; set; }
        [JsonProperty("employees")]
        public Dictionary<string, ParkingRecordUpdated> Employees { get; set; }
    }

    public class ParkingRecordUpdated
    {
        [JsonProperty("lastChanged")]
        public DateTime LastChanged { get; set; }
        [JsonProperty("rows")]
        public List<ParkingRecord> Rows { get; set; }
    }

    public class ParkingRecord
    {
        [JsonProperty("customGarage")]
        public string Garage { get; set; }
        [JsonProperty("customType")]
        public string Type { get; set; }
        [JsonProperty("customFobID")]
        public string FobID { get; set; }

    }
}
