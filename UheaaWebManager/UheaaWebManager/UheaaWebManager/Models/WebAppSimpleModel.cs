using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UheaaWebManager.Models
{
    public class WebAppSimpleModel
    {
        public int WebAppId { get; set; }
        public string AppName { get; set; }
        public string Url { get; set; }
        public int WebAppUserCount { get; set; }
    }
}