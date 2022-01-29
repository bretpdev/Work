using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uheaa.Common.DocumentProcessing
{
    public class Forms
    {
        public string LetterId { get; set; }
        public string PathAndForm { get; set; }

        public Forms()
        {
            LetterId = string.Empty;
            PathAndForm = string.Empty;
        }
    }
}
