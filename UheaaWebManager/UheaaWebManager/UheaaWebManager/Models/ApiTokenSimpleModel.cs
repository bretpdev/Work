using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UheaaWebManager.Models
{
    public class ApiTokenSimpleModel
    {
        public int ApiTokenId { get; set; }
        public string GeneratedTokenLast12 { get; set; }
        public string Notes { get; set; }
        public int ControllerCount { get; set; }
    }
}