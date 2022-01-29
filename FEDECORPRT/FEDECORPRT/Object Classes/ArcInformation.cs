using System.Collections.Generic;
using Uheaa.Common.DataAccess;

namespace FEDECORPRT
{
    public class ArcInformation
    {
        public int ArcScriptDataMappingId { get; set; }
        public string Arc { get; set; }
        public string Comment { get; set; }
        public ArcData.ArcType Type { get; set; }
        public List<string> HeaderNames { get; set; }

        public static List<ArcInformation> Populate(int id, DataAccess da)
        {
            List<ArcInformation> data = da.GetDataForArc(id);
            foreach (ArcInformation info in data)
                info.HeaderNames = da.GetHeadersForArcs(info.ArcScriptDataMappingId);

            return data;
        }
    }
}