using System;
using Uheaa.Common.ProcessLogger;

namespace SftpCoordinator
{
    public class EncryptionException : Exception
    {
        public EncryptionException(string error) : base("The following error was encountered when trying to encrypt: \n" + error) { Program.PLR.AddNotification($"The following error was encountered when trying to encrypt: \n {error}", NotificationType.ErrorReport, NotificationSeverityType.Critical); }
    }
}
