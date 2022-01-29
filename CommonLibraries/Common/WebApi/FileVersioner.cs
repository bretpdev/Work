using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Uheaa.Common;

namespace Uheaa.Common.WebApi
{
    public class FileVersioner
    {
        private static Dictionary<string, string> computedHashes = new Dictionary<string, string>();
        private static string GetHash(string relativePath)
        {
            lock (computedHashes)
            {
                var localPath = HostingEnvironment.MapPath(relativePath).ToLower();
#if DEBUG
                computedHashes.Clear();
#endif
                if (!computedHashes.ContainsKey(localPath))
                    using (var hasher = SHA256.Create())
                    using (var stream = FS.OpenRead(localPath))
                    {
                        var hash = hasher.ComputeHash(stream);
                        computedHashes[localPath] = BitConverter.ToString((byte[])hash).Replace("-", "").ToLowerInvariant();
                    }
                return computedHashes[localPath];
            }
        }

        public static MvcHtmlString GenerateStylesheetLink(string relativePath)
        {
            string output = "<link rel=\"stylesheet\" href=\"{0}\">";
            output = string.Format(output, VirtualPathUtility.ToAbsolute(relativePath) + "?cache=" + GetHash(relativePath));
            return new MvcHtmlString(output);
        }

        public static MvcHtmlString GenerateScriptLink(string relativePath)
        {
            string output = "<script type=\"text/javascript\" src=\"{0}\"></script>";
            output = string.Format(output, VirtualPathUtility.ToAbsolute(relativePath) + "?cache=" + GetHash(relativePath));
            return new MvcHtmlString(output);
        }
    }
}