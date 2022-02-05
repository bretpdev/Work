using System;

namespace Uheaa.Common.DocumentProcessing
{
    class CostCenterFileData
    {
        public string CostCenterCode { get; set; }
        public string DataFileName { get; set; }
        public int DomesticAddressCount { get; set; }
        public int ForeignAddressCount { get; set; }
    }
}
