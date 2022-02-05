using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Q;
using System.IO;

namespace PUTSUSPCOM
{
    public class DeleteAndReapplyOptionProcessor : DeleteSuspenseOptionProcessor
    {

        private string DeleteAndReapplyTextFileName = string.Format("{0}Delete And Reapply Log.txt", DataAccessBase.PersonalDataDirectory);

        public List<DeleteAndReapplyRowData> EDServicerRowData { get; set; }
        public List<DeleteAndReapplyRowData> ReapplyRowData { get; set; }

        public DeleteAndReapplyOptionProcessor(ReflectionInterface ri, Suspense suspenseData)
            : base(ri,suspenseData)
        {
            EDServicerRowData = new List<DeleteAndReapplyRowData>();
            ReapplyRowData = new List<DeleteAndReapplyRowData>();
        }

        public override void Process()
        {
            //Delete Suspense
            DeleteSuspense();
            //print screen
            Hit(ReflectionInterface.Key.F9);
            //Create comment string
            string assignedTo = (_systemSuspenseData.AssignedTo.StartsWith("U") ? "" : string.Format("assigned {0}",_systemSuspenseData.AssignedTo));
            string comment = string.Format("Delete Trans Type {0} Susp Pmt {1}, Effective {2}, payment amt {3}, batch # {4}.  {5}, {6}, {7}, {8}",
                                            _systemSuspenseData.TransactionType, assignedTo, _systemSuspenseData.EffectiveDate, _systemSuspenseData.Amount, _systemSuspenseData.BatchNumber,
                                            RowDataToDelimitedString().ToString(), CreateLoanTypeStringList().ToString(), CreateDealStringList(DealPart.Description).ToString(), Comments);
            //Add comments
            AddComments(comment);
            //Add data to file
            AddRecordToDeleteAndReapplyTextFile();
        }

        //adds record to data file
        private void AddRecordToDeleteAndReapplyTextFile()
        {
            //create lists of data records, loan type lists and deal #s
            StringBuilder rowData = RowDataToDelimitedString();
            StringBuilder loanTypeData = CreateLoanTypeStringList();
            StringBuilder dealData = CreateDealStringList(DealPart.Description);
            VbaStyleFileOpen(DeleteAndReapplyTextFileName, 1, Common.MSOpenMode.Append);
            VbaStyleFileWriteLine(1, _systemSuspenseData.BorrowerDemos.SSN, _systemSuspenseData.BorrowerDemos.LName, _systemSuspenseData.Amount, _systemSuspenseData.EffectiveDate, _systemSuspenseData.TransactionType, rowData.ToString(), loanTypeData.ToString(), dealData.ToString(), Comments);
            VbaStyleFileClose(1);
        }

        private StringBuilder RowDataToDelimitedString()
        {
            StringBuilder rowData = new StringBuilder();
            foreach (DeleteAndReapplyRowData data in EDServicerRowData)
            {
                if (rowData.ToString() == string.Empty)
                {
                    rowData.AppendFormat("ED Servicer,{0},{1},{2},{3}", data.LoanSequenceNumber, data.Amount, data.DisbursementDate, data.TransactionType);
                }
                else
                {
                    rowData.AppendFormat(";ED Servicer,{0},{1},{2},{3}", data.LoanSequenceNumber, data.Amount, data.DisbursementDate, data.TransactionType);
                }
            }
            foreach (DeleteAndReapplyRowData data in ReapplyRowData)
            {
                rowData.AppendFormat(";Reapply,{0},{1},{2},{3}", data.LoanSequenceNumber, data.Amount, data.DisbursementDate, data.TransactionType);
            }
            return rowData;
        }

    }
}
