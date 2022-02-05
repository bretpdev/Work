using Uheaa.Common;

namespace SftpCoordinator
{
    public static class StringExtensions
    {
        /// <summary>
        /// The Allowed Roots of the FS class is case sensitive. This should change the path to be the same case as the allowed cases.
        /// </summary>
        public static string UpdatePath(this string path)
        {
            if (path.IsPopulated())
            {
                if (path.ToUpper().StartsWith("C:\\ENTERPRISE PROGRAM FILES"))
                    return SubstringPath(path, "C:\\ENTERPRISE PROGRAM FILES", "C:\\Enterprise Program Files");
                if (path.ToUpper().StartsWith("C:\\INETPUB"))
                    return SubstringPath(path, "C:\\INETPUB", "C:\\inetpub");
                if (path.ToUpper().StartsWith("\\UHEAA-FS\\DOMAINUSERSDATA"))
                    return SubstringPath(path, "\\UHEAA-FS\\DOMAINUSERSDATA", "\\UHEAA-FS\\DomainUsersData");
                if (path.ToUpper().StartsWith("\\UHEAA-FS\\USERTEMP"))
                    return SubstringPath(path, path.ToUpper().Substring(0, path.ToUpper().IndexOf("\\TEMP\\") + 6), "T:");
                if (path.ToUpper().StartsWith("\\FSUHEAAXYZ\\SEAS"))
                    return SubstringPath(path, "\\FSUHEAAXYZ\\SEAS", "X:");
                if (path.ToUpper().StartsWith("\\FSUHEAAQ\\RESTRICTED"))
                    return SubstringPath(path, "\\FSUHEAAQ\\RESTRICTED", "Q:");
                if (path.ToUpper().StartsWith(@"\\UHEAA-FS\DOMAINUSERSDATA"))
                    return SubstringPath(path, @"\\UHEAA-FS\DOMAINUSERSDATA", @"\\UHEAA-FS\DomainUsersData");
                if (path.ToUpper().StartsWith(@"\\UHEAA-FS\USERTEMP"))
                    return SubstringPath(path, path.ToUpper().Substring(0, path.ToUpper().IndexOf("\\TEMP\\") + 6), "T:\\");
                if (path.ToUpper().StartsWith(@"\\FSUHEAAXYZ\DEVSEASCS"))
                    return SubstringPath(path, @"\\FSUHEAAXYZ\DEVSEASCS", "Y:\\");
                if (path.ToUpper().StartsWith(@"\\FSUHEAAXYZ\SEASCS"))//Make sure to check SEASCS before SEAS, it will always find SEAS first
                    return SubstringPath(path, @"\\FSUHEAAXYZ\SEASCS", "Z:\\");
                if (path.ToUpper().StartsWith(@"\\FSUHEAAXYZ\SEAS"))
                    return SubstringPath(path, @"\\FSUHEAAXYZ\SEAS", "X:\\");
                if (path.ToUpper().StartsWith(@"\\FSUHEAAQ\RESTRICTED"))
                    return SubstringPath(path, @"\\FSUHEAAQ\RESTRICTED", "Q:\\");
                if (path.ToUpper() == @"\\FSUHEAAXYZ" || path.ToUpper() == @"\\FSUHEAAQ" || path.ToUpper() == @"\\UHEAA-FS")
                    return "";
            }
            return path;
        }

        private static string SubstringPath(string path, string originalText, string replaceText)
        {
            return $"{replaceText}{path.Substring(originalText.Length, path.Length - originalText.Length)}";
        }
    }
}