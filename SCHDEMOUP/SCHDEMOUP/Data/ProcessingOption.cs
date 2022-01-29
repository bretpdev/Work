using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCHDEMOUP
{
    class ProcessingOption
    {
        public enum Option
        {
            Update,
            Live,
            Test
        }

        public Option SelectedOption { get; set; }
    }
}
