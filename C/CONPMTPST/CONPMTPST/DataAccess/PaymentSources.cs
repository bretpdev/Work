using System;

namespace CONPMTPST
{
    public class PaymentSources
    {
        public int PaymentSourcesId { get; set; }
        public string PaymentSource { get; set; }
        public string InstitutionId { get; set; }
        public string FileName { get; set; }
        public FileTypes FileType { get; set; }
        public string FilePath { get; set; }
        public ConsolPaymentPosting.FileType Type
        {
            get
            {
                return (ConsolPaymentPosting.FileType)Enum.Parse(typeof(ConsolPaymentPosting.FileType), FileType.FileType, true);
            }
        }
    }
}