using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace UHECORPRT
{
    public class ArcInformation
    {
        public int ArcScriptDataMappingId { get; set; }
        public string Arc { get; set; } 
        public string Comment { get; set; }
        public ArcData.ArcType Type { get; set; }
        public List<string> HeaderNames { get; set; }
        public string ResponseCode { get; set; }
        public string ActivityType { get; set; }
        public string ActivityContact { get; set; }

        public static List<ArcInformation> Populate(DataAccess DA, int id)
        {
            List<ArcInformation> data = DA.GetDataForArc(id);
            foreach (ArcInformation info in data)
                info.HeaderNames = DA.GetHeadersForArcs(info.ArcScriptDataMappingId);

            return data;
        }
    }
}
