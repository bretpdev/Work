using Uheaa.Common.DataAccess;

namespace OLDEMOS
{
    public class Sources
    {
        [DbName("PX_CDE_VAL")]
        public string Code { get; set; }
        [DbName("PX_LNG_DSC")]
        public string Name { get; set; }
    }
}