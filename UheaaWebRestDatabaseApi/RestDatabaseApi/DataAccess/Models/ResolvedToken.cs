using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestDatabaseApi
{
    public class ResolvedToken
    {
        public int TokenId { get; set; }
        public bool IsUserToken { get; set; }

        public int? ApiTokenId
        {
            get
            {
                return IsUserToken ? null : (int?)TokenId;
            }
        }

        public int? UserTokenId
        {
            get
            {
                return IsUserToken ? (int?)TokenId : null;
            }
        }
    }
}