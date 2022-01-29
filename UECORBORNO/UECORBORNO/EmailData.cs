using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace UECORBORNO
{
    class EmailData
    {
        public List<int> DocumentDetailIds { get; set; }
        public int EmailId { get; set; }
        public string AccountNumber { get; set; }
        public string EmailAddress { get; set; }
        public string EmailSubjectLine { get; set; }

        public EmailData()
        {
            DocumentDetailIds = new List<int>();
        }

        /// <summary>
        /// Updated the emailed indicator to the current date and time
        /// </summary>
        [UsesSproc(DataAccessHelper.Database.EcorrUheaa, "UpdateEmailedAt")]
        public void UpdateEmailedIndicator(DataAccess da)
        {
            Parallel.ForEach(DocumentDetailIds, new ParallelOptions() { MaxDegreeOfParallelism = int.MaxValue }, id =>
                {
                    da.UpdateEmailedIndicator(id);
                });
        }
    }
}
