using System;
using System.Collections.Generic;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace CONPMTPST
{
    public class DataAccess
    {
        public LogDataAccess LDA { get; set; }

        public DataAccess(ProcessLogRun logRun)
        {
            LDA = new LogDataAccess(DataAccessHelper.CurrentMode, logRun.ProcessLogId, true, true);
        }

        /// <summary>
        /// Gets a list of all Payment Sources
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "[cpp].GetPaymentSources")]
        public List<PaymentSources> GetPaymentSources()
        {
            List<PaymentSources> sources = LDA.ExecuteList<PaymentSources>("[cpp].GetPaymentSources", DataAccessHelper.Database.Uls).Result;

            foreach (PaymentSources source in sources)
            {
                source.FileType = GetFileType(source.PaymentSourcesId);
            }
            return sources;
        }

        /// <summary>
        /// Gets a single file type for the selected payment source
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[cpp].GetFileTypeForPaymentSource")]
        public FileTypes GetFileType(int paymentSourceId)
        {
            return LDA.ExecuteSingle<FileTypes>("[cpp].GetFileTypeForPaymentSource", DataAccessHelper.Database.Uls,
                SqlParams.Single("PaymentSourceId", paymentSourceId)).Result;
        }

        /// <summary>
        /// Gets a list of all file types
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[cpp].GetFileTypes")]
        public List<FileTypes> GetFilesTypes()
        {
            return LDA.ExecuteList<FileTypes>("[cpp].GetFileTypes", DataAccessHelper.Database.Uls).Result;
        }

        /// <summary>
        /// Gets a list of all Payment Types
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Uls, "[cpp].GetPaymentTypes")]
        public List<PaymentTypes> GetPaymentTypes()
        {
            return LDA.ExecuteList<PaymentTypes>("[cpp].GetPaymentTypes", DataAccessHelper.Database.Uls).Result;
        }

        /// <summary>
        /// Inserts data into the Overpayment Transmittal table.
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[cpp].SetOverpaymentTramsmittal")]
        public void SetOverpayment(List<BorrowerData> borData)
        {
            foreach (BorrowerData bor in borData)
            {
                LDA.Execute("[cpp].SetOverpaymentTramsmittal", DataAccessHelper.Database.Uls,
                    SqlParams.Single("AccountNumber", bor.AccountNumber),
                    SqlParams.Single("LoanSequence", bor.LoanSequence),
                    SqlParams.Single("FirstDisbursementDate", bor.FirstDisbursement.ToShortDateString()),
                    SqlParams.Single("ManifestNumber", bor.ManifestNumber),
                    SqlParams.Single("LoanType", bor.LoanType),
                    SqlParams.Single("OriginatorLoanId", bor.OriginatorId),
                    SqlParams.Single("NsldsId", bor.NsldsId));
            }
        }

        /// <summary>
        /// Gets the active manager for the designated business unit
        /// </summary>
        /// <returns></returns>
        [UsesSproc(DataAccessHelper.Database.Csys, "Get_BU_Manager")]
        public bool CheckManagerAccess()
        {
            List<User> managers = LDA.ExecuteList<User>("Get_BU_Manager", DataAccessHelper.Database.Csys,
                SqlParams.Single("BusinessUnitId", 24)).Result;
            foreach (User manager in managers)
            {
                if (manager.WindowsUserName == Environment.UserName)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Inserts a new payment source
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[cpp].InsertPaymentSource")]
        public bool InsertPaymentSource(PaymentSources source)
        {
            return LDA.Execute("[cpp].InsertPaymentSource", DataAccessHelper.Database.Uls,
                SqlParams.Single("PaymentSource", source.PaymentSource),
                SqlParams.Single("InstitutionId", source.InstitutionId),
                SqlParams.Single("FileName", source.FileName),
                SqlParams.Single("FileTypeId", source.FileType.FileTypeId));
        }

        /// <summary>
        /// Deletes the current payment source
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[cpp].DeletePaymentSource")]
        public bool DeleteSource(int paymentSourceId)
        {
            return LDA.Execute("[cpp].DeletePaymentSource", DataAccessHelper.Database.Uls,
                    SqlParams.Single("PaymentSourceId", paymentSourceId));
        }

        /// <summary>
        /// Updates the payment source information
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[cpp].UpdatePaymentSource")]
        public bool UpdatePaymentSource(PaymentSources source)
        {
            return LDA.Execute("[cpp].UpdatePaymentSource", DataAccessHelper.Database.Uls,
                    SqlParams.Single("PaymentSourceId", source.PaymentSourcesId),
                    SqlParams.Single("InstitutionId", source.InstitutionId),
                    SqlParams.Single("FileName", source.FileName),
                    SqlParams.Single("FileTypeId", source.FileType.FileTypeId));
        }

        /// <summary>
        /// Inactivates the payment type
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[cpp].DeletePaymentType")]
        public bool DeletePaymentType(int paymentTypeId)
        {
            return LDA.Execute("[cpp].DeletePaymentType", DataAccessHelper.Database.Uls,
                 SqlParams.Single("PaymentTypeId", paymentTypeId));
        }

        /// <summary>
        /// Updates the payment type
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[cpp].UpdatePaymentType")]
        public bool UpdatePaymentType(PaymentTypes type)
        {
            return LDA.Execute("[cpp].UpdatePaymentType", DataAccessHelper.Database.Uls,
                SqlParams.Single("PaymentTypeId", type.PaymentTypeId),
                SqlParams.Single("CompassLoanType", type.CompassLoanType),
                SqlParams.Single("TivaFileLoanType", type.TivaFileLoanType));
        }

        /// <summary>
        /// Inserts a new payment type
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.Uls, "[cpp].InsertPaymentType")]
        public bool InsertPaymentType(PaymentTypes type)
        {
            return LDA.Execute("[cpp].InsertPaymentType", DataAccessHelper.Database.Uls,
                SqlParams.Single("CompassLoanType", type.CompassLoanType),
                SqlParams.Single("TivaFileLoanType", type.TivaFileLoanType));
        }
    }
}