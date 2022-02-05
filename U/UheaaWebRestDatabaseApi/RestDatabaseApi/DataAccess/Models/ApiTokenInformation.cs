using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestDatabaseApi
{
    public class ApiTokenInformation
    {
        public int ApiTokenId { get; set; }
		public Guid GeneratedToken { get; set; }
		public string Description { get; set; }
        public DateTime AddedAt { get; set; }
		public string AddedBy { get; set; }
		public DateTime? InactivatedAt { get; set; }
    }
}