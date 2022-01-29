using System;
using Uheaa.Common.Scripts;

namespace BATCHESP
{
    /// <summary>
    /// Data comprised by scraping Session defer/forb info. 
    /// Used for identifying defer/forb to be removed, updated, or worked around.
    /// </summary>
    public class Tsx31Data
    {
        public DateTime Begin { get; set; }
        public DateTime End { get; set; }
        public DateTime? Cert { get; set; }
        public bool IsDdb { get; set; }
        public string DfType { get; set; }
        public int Row { get; set; }
        public int CurrentItem { get; set; }
        public PageHelper.IterationSettings Settings { get; set; }
    }
}
