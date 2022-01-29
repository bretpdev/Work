using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDRUSERPRO
{
    public static class CalculatorSerializer
    {
        public static LpcInput Deserialize(string serializedData)
        {
            return JsonConvert.DeserializeObject<LpcInput>(serializedData);
        }

        public static string Serialize(LpcInput ci)
        {
            return JsonConvert.SerializeObject(ci, Formatting.Indented);
        }

        public static string Serialize(LpcResults cr)
        {
            return JsonConvert.SerializeObject(cr, Formatting.Indented);
        }
    }
}
