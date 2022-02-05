using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Q;
using System.Windows.Forms;

namespace PUTSUSPCOM
{
    public class DeleteSuspenseOptionProcessor : OptionProcessorBase
    {

        protected enum DealPart
        {
            Description,
            Number,
            Owner
        }

        private string EDTransmittalDeletedTextFileName = string.Format("{0}ED Transmittal Suspense Pmt Deleted.txt", DataAccessBase.PersonalDataDirectory);

        public List<Deal> UserSelectedDeals { get; set; }
        public List<string> UserSelectedLoanTypes { get; set; }

        public DeleteSuspenseOptionProcessor(ReflectionInterface ri, Suspense suspenseData)
            : base(ri, suspenseData)
        {
        }

        public override void Process()
        {
            //Delete Suspense
            DeleteSuspense();
            //Create comment string
            string assignedTo = (_systemSuspenseData.AssignedTo.StartsWith("U")?"":_systemSuspenseData.AssignedTo);
            string comment = string.Format("Delete Suspense Payment {0} - Effective {1} for {2}.  Forward Pmt to Fedloan Serv {3} {4}, {5}, {6}.", 
                                            assignedTo, _systemSuspenseData.EffectiveDate, _systemSuspenseData.Amount, DateTime.Today.ToString("MM/dd/yyyy"),
                                            CreateLoanTypeStringList().ToString(), CreateDealStringList(DealPart.Description).ToString(), Comments);
            //Add comments
            AddComments(comment);
            //Add record to the ED text file
            AddRecordToEDTransmittalDeletedFile();
        }

        /// <summary>
        /// Deletes suspense on TS3I
        /// </summary>
        protected void DeleteSuspense()
        {
            FastPath(string.Format("TX3ZDTS3I{0}",_systemSuspenseData.FastPathText));
            //check if the user is on target screen of TS3I
            while (Check4Text(1, 72, "TSX3G") == false)
            {
                MessageBox.Show("The script wasn't able to return to the suspense detail record to be deleted.  Please navigate to it again and hit <Insert> when you are done.","Detail Screen Needed",MessageBoxButtons.OK,MessageBoxIcon.Information);
                PauseForInsert();
            }
            Hit(ReflectionInterface.Key.Enter);
        }

        /// <summary>
        /// Adds record to ED Transmittal File
        /// </summary>
        private void AddRecordToEDTransmittalDeletedFile()
        {
            VbaStyleFileOpen(EDTransmittalDeletedTextFileName, 1, Common.MSOpenMode.Append);
            VbaStyleFileWriteLine(1, _systemSuspenseData.BorrowerDemos.SSN, "", string.Format("{0}, {1}, {2}", _systemSuspenseData.BorrowerDemos.LName, _systemSuspenseData.BorrowerDemos.FName, _systemSuspenseData.BorrowerDemos.MI), CreateLoanTypeStringList().ToString(), _systemSuspenseData.EffectiveDate, _systemSuspenseData.Amount, CreateDealStringList(DealPart.Number).ToString(), _systemSuspenseData.BatchNumber, _systemSuspenseData.TransactionType, _systemSuspenseData.SequenceNumber, _systemSuspenseData.AssignedTo, CreateDealStringList(DealPart.Owner).ToString(), Comments);
            VbaStyleFileClose(1);
        }

        /// <summary>
        /// Returns a ";" delimited list of deals 
        /// </summary>
        /// <returns></returns>
        protected StringBuilder CreateDealStringList(DealPart part)
        {
            StringBuilder dealData = new StringBuilder();
            foreach (Deal deal in UserSelectedDeals)
            {
                if (part == DealPart.Description)
                {
                    if (dealData.ToString() == string.Empty)
                    {
                        dealData.Append(deal.Description);
                    }
                    else
                    {
                        dealData.AppendFormat(";{0}", deal.Description);
                    }
                }
                else if (part == DealPart.Number)
                {
                    if (dealData.ToString() == string.Empty)
                    {
                        dealData.Append(deal.DealNumber);
                    }
                    else
                    {
                        dealData.AppendFormat(";{0}", deal.DealNumber);
                    }
                }
                else //owner
                {
                    if (dealData.ToString() == string.Empty)
                    {
                        dealData.Append(deal.Owner);
                    }
                    else
                    {
                        dealData.AppendFormat(";{0}", deal.Owner);
                    }
                }

            }
            return dealData;
        }

        /// <summary>
        /// Returns a ";" delimited list of loan types 
        /// </summary>
        /// <returns></returns>
        protected StringBuilder CreateLoanTypeStringList()
        {
            StringBuilder loanTypeData = new StringBuilder();
            foreach (string loanType in UserSelectedLoanTypes)
            {
                if (loanTypeData.ToString() == string.Empty)
                {
                    loanTypeData.Append(loanType);
                }
                else
                {
                    loanTypeData.AppendFormat(";{0}", loanType);
                }
            }
            return loanTypeData;
        }

    }
}
