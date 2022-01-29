using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;

namespace BTCBPUHEAA
{
    class NameHelper
    {
        public string AUTO_POST_FILE_PREFIX => "AUTOPOST";
        public string AUTO_POST_FILE_PREFIX_ARCHIVE => "AUTOPOSTARCHIVE";
        public string TEL_FILE_PREFIX => "telPay";
        public string TEL_FILE_PREFIX_ARCHIVE => "telPayARCHIVE";
        public string PAY_GOV_FILE_PREFIX => "PAYGOV";
        public string PAY_GOV_FILE_PREFIX_ARCHIVE => "PAYGOVARCHIVE";
        public string UheaaArchiveFolder { get; private set; }
        public string AutoPostFileName { get; private set; }
        public string AutoPostFileNameArchive { get; private set; }
        public string FileDateTimeStamp { get; private set; }
        public string TelPayFileName { get; private set; }
        public string TelPayFileNameArchive { get; private set; }
        public string PayGovFileName { get; private set; }
        public string PayGovFileNameArchive { get; private set; }
        public string UheaaUploadFolder { get; private set; }
        public NameHelper()
        {
            UheaaArchiveFolder = EnterpriseFileSystem.GetPath("Uheaa Check by Phone Archive");
            UheaaUploadFolder = EnterpriseFileSystem.GetPath("Uheaa Check by Phone Upload");
            FileDateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssff");
            AutoPostFileName = string.Format("{0}{1}.txt", AUTO_POST_FILE_PREFIX, FileDateTimeStamp);
            AutoPostFileNameArchive = string.Format("{0}{1}.txt", AUTO_POST_FILE_PREFIX_ARCHIVE, FileDateTimeStamp);
            TelPayFileName = string.Format("{0}{1}.txt", TEL_FILE_PREFIX, FileDateTimeStamp);
            TelPayFileNameArchive = string.Format("{0}{1}.txt", TEL_FILE_PREFIX_ARCHIVE, FileDateTimeStamp);
            PayGovFileName = string.Format("{0}{1}.txt", PAY_GOV_FILE_PREFIX, FileDateTimeStamp);
            PayGovFileNameArchive = string.Format("{0}{1}.txt", PAY_GOV_FILE_PREFIX_ARCHIVE, FileDateTimeStamp);
        }

    }
}
