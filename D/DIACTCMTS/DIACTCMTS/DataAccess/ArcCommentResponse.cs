using System;
using System.Reflection;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace DIACTCMTS
{
    class ArcCommentResponse
    {
        public string Arc { get; set; }
        public string Comment { get; set; }
        public string ResponseCode { get; set; }
    }
}