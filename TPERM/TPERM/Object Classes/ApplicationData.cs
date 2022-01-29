using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.Scripts;

namespace TPERM
{
    public class ApplicationData
    {
        public enum ApplicationResults
        {
            NewAuth,
            AuthAlreadyExists,
            NoSign,
            WrongSign,
            InProcess,
            Illegible,
            NewAuthEndDate,
            NoAuthAddedRef,
            NoMatch,
            WrongForm
        }

        public SystemBorrowerDemographics BorrowerInfo { get; set; }
        public List<CoMakerData> CoMaker { get; set; }
        public CoMakerData SelectedCoMaker { get; set; }
        public ReferenceData ReferenceInfo { get; set; }
        public ApplicationResults Result { get; set; }
        public TaskData TaskInfo { get; set; }
        public bool AddMultiple { get; set; }
        public PossibleReferenceMatch SelectedReference { get; set; }
        public DateTime? AuthEndDate { get; set; }
        public List<TaskData> DupTasks { get; set; }
        public bool HasNoLoans { get; set; }

        public void Populate(ReflectionInterface ri, string ssn)
        {
            BorrowerInfo = ri.GetDemographicsFromTx1j(ssn);
            ri.Hit(ReflectionInterface.Key.F12);
            ri.Hit(ReflectionInterface.Key.F4);
            CoMaker = CheckForCoMaker(ri);
        }

        public void Clear()
        {
            SelectedCoMaker = null;
            ReferenceInfo = null;
            Result = ApplicationResults.InProcess;
            AddMultiple = false;
            SelectedReference = null;
            AuthEndDate = null;
        }

        private List<CoMakerData> CheckForCoMaker(ReflectionInterface ri)
        {
            List<CoMakerData> allCoMakers = new List<CoMakerData>();
            for (int row = 10; ri.MessageCode != "90007"; row++)
            {
                if (ri.CheckForText(row, 3, " ") || row > 21)
                {
                    ri.Hit(ReflectionInterface.Key.F8);
                    row = 9;
                    continue;
                }
                if (ri.CheckForText(row, 5, "E"))
                {
                    string comakerSsn = ri.GetText(row, 13, 9);
                    if (allCoMakers.Where(p => p.Ssn == comakerSsn).Any())//Comaker is already in the list we will need to add to the loan seq
                    {
                        var data = allCoMakers.Where(p => p.Ssn == comakerSsn).SingleOrDefault();
                        allCoMakers.Remove(data);//remove from the list
                        data.LoanSeqs.Add(ri.GetText(row, 9, 3).ToIntNullable().Value);
                        allCoMakers.Add(data);//readd to the list
                    }
                    else
                    {
                        var data = new CoMakerData() { Ssn = ri.GetText(row, 13, 9) };
                        data.LoanSeqs.Add(ri.GetText(row, 9, 3).ToIntNullable().Value);
                        allCoMakers.Add(data);
                    }
                }
            }

            foreach (var comaker in allCoMakers)
            {
                SystemBorrowerDemographics demos = ri.GetDemographicsFromTx1j(comaker.Ssn);
                comaker.FirstName = demos.FirstName;
                comaker.LastName = demos.LastName;
                comaker.AccountNumber = demos.AccountNumber;
            }
            return allCoMakers;
        }

        public string GetComment(bool noLoans)
        {
            switch (Result)
            {
                case ApplicationResults.NewAuth:
                    return string.Format("{2} AUTHORIZED {0}, {1}. ADDED AUTH{3}.", ReferenceInfo.ConcatName(), ReferenceInfo.Relationship, SelectedCoMaker != null ? "COSIGNER" : "BRWR",
                        SelectedCoMaker != null ? string.Format(", AUTH ONLY VALID FOR LOAN SEQ {0}", SelectedCoMaker.GetLoanSeq()) : string.Empty);
                case ApplicationResults.AuthAlreadyExists:
                    return string.Format("{1} AUTHORIZED {0}. AUTH ALREADY EXISTS. NO ADJ NEC.", ReferenceInfo.ConcatName(), SelectedCoMaker != null ? "COSIGNER" : "BRWR");
                case ApplicationResults.WrongSign:
                    return string.Format("{3} AUTHORIZED {0}, {1}. AUTH NOT ADDED DUE TO {3} SGND NM AS {2}. SNT NM DSCPNCY LTTR, SNT GG90L", ReferenceInfo.ConcatName(), ReferenceInfo.Relationship, ReferenceInfo.BorrowerSignatureName, 
                        SelectedCoMaker != null ? "COSIGNER" : "BRWR");
                case ApplicationResults.NoSign:
                    return string.Format("{2} AUTHORIZED {0}, {1}. AUTH NOT ADDED DUE TO {2} DID NOT SIGN FORM", ReferenceInfo.ConcatName(), ReferenceInfo.Relationship, SelectedCoMaker != null ? "COSIGNER" : "BRWR",
                        SelectedCoMaker != null ? "COSIGNER" : "BRWR");
                case ApplicationResults.Illegible:
                    return string.Format("RECVD THIRD PARTY LETTER, AUTH NOT ADDED DUE TO FORM ILLEGIBLE. {0}", noLoans ? " SNT GG90L" : "SNT ILLEGIBLE FAX DNL LTTR");
                case ApplicationResults.NewAuthEndDate:
                   return string.Format("{2} AUTHORIZED {0}, {1} ADDED AUTH{3}. AUTH VALID UNTIL {4:MM-dd-yyyy}", ReferenceInfo.ConcatName(), ReferenceInfo.Relationship, SelectedCoMaker != null ? "COSIGNER" : "BRWR",
                        SelectedCoMaker != null ? string.Format(", AUTH ONLY VALID FOR LOAN SEQ {0}", SelectedCoMaker.GetLoanSeq()) : string.Empty, AuthEndDate.Value);
                case ApplicationResults.NoMatch:
                    return string.Format("{2} RQST TO AUTH {0}, {1}. AUTH NOT ADDED DUE TO NM DISCREP. G727M SBMTTD, SNT DNL LTTR.", ReferenceInfo.ConcatName(), ReferenceInfo.Relationship, SelectedCoMaker != null ? "COSIGNER" : "BRWR");
                case ApplicationResults.WrongForm:
                    return string.Format("RECVD THIRD PARTY LETTER, AUTH NOT ADDED, TPERM CLOSED W/OUT BEING PROCESSD, SBMTTD RVW3P FR RVW", TaskInfo.ActivitySeq);
                default:
                    return null;
            }
        }
    }
}
