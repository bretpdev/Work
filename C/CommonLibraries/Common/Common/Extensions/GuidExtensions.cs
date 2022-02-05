using System;

namespace Uheaa.Common
{
    public static class  GuidExtensions
    {
        public static string ToBase64String(this Guid g)
        {
            string enc = Convert.ToBase64String(g.ToByteArray());
            enc = enc.Replace("/", "_");
            enc = enc.Replace("+", "-");
            return enc.Substring(0, 22);
        }
    }
}
