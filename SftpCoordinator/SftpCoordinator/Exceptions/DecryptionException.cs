using System;
using Uheaa.Common.ProcessLogger;

namespace SftpCoordinator
{
    public class DecryptionException : Exception
    {
        public DecryptionException(string error) : base("The following error was encountered when trying to decrypt: \n" + error) { Program.PLR.AddNotification($"The following error was encountered when trying to decrypt: \n {error}", NotificationType.ErrorReport, NotificationSeverityType.Critical); }
    }
}
