using System;
using System.Collections.Generic;
using System.IO;
using Uheaa.Common;
using Uheaa.Common.DataAccess;

namespace SftpCoordinator
{
    public class Settings
    {
        public static string PgpLocation { get; internal set; }
        public static string ErrorReportLocation { get; internal set; }
        public static string ErrorReportFormatString { get; internal set; }
        public static string TempFolderLocation { get; internal set; }
        public static string UheaaPublicKeyring { get; internal set; }
        public static string UheaaPrivateKeyring { get; internal set; }
        public static string AesPublicKey { get; internal set; }
        public static string UheaaPassphraseLocation { get; internal set; }
        public static string UheaaPassphrase
        {
            get
            {
                if (File.Exists(UheaaPassphraseLocation))
                    return File.ReadAllText(UheaaPassphraseLocation);
                return null;
            }
        }
        public static string UheaaPreDecryptionArchiveLocation { get; internal set; }
        public static string UheaaPreEncryptionArchiveLocation { get; internal set; }
        public static bool AutoResolveNamingConflicts { get; internal set; }
        public static int UheaaPreDecryptionRetentionDays { get; internal set; }
        public static int UheaaPreEncryptionRetentionDays { get; internal set; }

        static Settings()
        {
            LoadSettings();
        }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "GetSettings")]
        public static List<Setting> GetAll()
        {
            return Program.PLR.LDA.ExecuteList<Setting>("GetSettings", DataAccessHelper.Database.SftpCoordinator).Result;
        }

        [UsesSproc(DataAccessHelper.Database.SftpCoordinator, "UpdateSetting")]
        public static void SaveSetting<T>(int settingId, T newValue)
        {
            Program.PLR.LDA.Execute("UpdateSetting", DataAccessHelper.Database.SftpCoordinator, SqlParams.Generate(new { SettingId = settingId, Value = newValue }));
        }
        
        public static void LoadSettings()
        {
            foreach (Setting s in GetAll())
            {
                string value = s.Value.ToString();
                switch (s.SettingId)
                {
                    case SettingId.Pgp:
                        PgpLocation = Environment.ExpandEnvironmentVariables(value.UpdatePath());
                        break;
                    case SettingId.ErrorReportLocation:
                        ErrorReportLocation = value.UpdatePath();
                        break;
                    case SettingId.ErrorReportFormatString:
                        ErrorReportFormatString = value.UpdatePath();
                        break;
                    case SettingId.TempFolderLocation:
                        TempFolderLocation = string.Format(value.UpdatePath(), Environment.UserName); //insert username at {0}
                        break;
                    case SettingId.UheaaPublicKeyring:
                        UheaaPublicKeyring = value.UpdatePath();
                        break;
                    case SettingId.UheaaPrivateKeyring:
                        UheaaPrivateKeyring = value.UpdatePath();
                        break;
                    case SettingId.AesPublicKey:
                        AesPublicKey = value.UpdatePath();
                        break;
                    case SettingId.UheaaPassphrase:
                        UheaaPassphraseLocation = value.UpdatePath();
                        break;
                    case SettingId.UheaaPreDecryptionArchiveLocation:
                        UheaaPreDecryptionArchiveLocation = value.UpdatePath();
                        break;
                    case SettingId.UheaaPreEncryptionArchiveLocation:
                        UheaaPreEncryptionArchiveLocation = value.UpdatePath();
                        break;
                    case SettingId.AutoResolveNamingConflicts:
                        AutoResolveNamingConflicts = (bool)s.Value; //bit
                        break;
                    case SettingId.UheaaPreDecryptionRetentionDays:
                        UheaaPreDecryptionRetentionDays = value.ToIntNullable() ?? 7;
                        break;
                    case SettingId.UheaaPreEncryptionRetentionDays:
                        UheaaPreEncryptionRetentionDays = value.ToIntNullable() ?? 7;
                        break;
                }
            }
        }

        public static IEnumerable<SettingId> ArchiveLocationSettings
        {
            get
            {
                yield return SettingId.UheaaPreDecryptionArchiveLocation;
                yield return SettingId.UheaaPreEncryptionArchiveLocation;
            }
        }

        public static bool IsValid
        {
            get
            {
                bool valid = true;
                Action<string> validate = new Action<string>(s => { if (!Directory.Exists(s)) valid = false; });
                validate(UheaaPreDecryptionArchiveLocation);
                validate(UheaaPreEncryptionArchiveLocation);
                return valid;
            }
        }
    }

    public class Setting
    {
        public SettingId SettingId { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
