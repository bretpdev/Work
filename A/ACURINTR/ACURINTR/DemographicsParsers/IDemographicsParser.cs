using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace ACURINTR.DemographicsParsers
{
    abstract class DemographicsParserBase
    {
        public ReflectionInterface RI { get; set; }
        public DataAccess DA { get; set; }
        public DemographicsParserBase(ReflectionInterface ri, DataAccess da)
        {
            RI = ri;
            DA = da;
        }
        public abstract QueueTask Parse();
    }
}
