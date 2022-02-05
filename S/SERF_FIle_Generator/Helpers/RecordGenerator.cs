using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Uheaa.Common.DataAccess;
using Uheaa.Common;

namespace SERF_File_Generator
{
    public class RecordGenerator
    {
        public static void CreateFiles(string ssn, string owner, ClassFileWriter fileWriter)
        {
            Queue<string> records = new Queue<string>();
            ProcessRecords<BorrowerData>(ssn, "01", ref records, owner, ModifyBorrowerData);
            ProcessRecords<ReferenceData>(ssn, "02", ref records, owner, ModifyReferenceInfo);
            ProcessRecords<FinancialData>(ssn, "03", ref records, owner, ModifyFinancialData);
            AddDisbRecord(ssn, owner, ref records);
            ProcessRecords<RepaymentData>(ssn, "05", ref records, owner, ModifyRepaymentData);
            ProcessRecords<EndorserSpouse>(ssn, "07", ref records, owner, ModifyEndorserSpouseInfo);
            ProcessRecords<StudentData>(ssn, "08", ref records, owner, ModifyStudentData);
            ProcessRecords<DefermentData>(ssn, "09", ref records, owner, ModifyDefermentData);
            ProcessRecords<ForbearanceData>(ssn, "10", ref records, owner);

            if (records.Where(p => p.Substring(32, 2) == "03").Count() > 0)
                fileWriter.AddBorrower(records);
        }

        private static void ProcessRecords<T>(string ssn, string record, ref Queue<string> records, string owner, RecordFormatter<T> formatter = null) where T : SerfFileBase
        {
            foreach (string line in RecordPopulator.PopulateRecord<T>(ssn, record, owner, formatter))
                records.Enqueue(line);
        }

        private static void AddDisbRecord(string ssn, string owner, ref Queue<string> records)
        {
            List<DisbData> data = DataAccessHelper.ExecuteList<DisbData>("GetDisbData", DataAccessHelper.Database.AlignImport, new SqlParameter("SSN", ssn));
            int disbSeqNum = 1;
            int seqNumWithinLoan = 1;
            int currentLoanSeq = 0;
            int prevoiusLoanSeq = 0;
            for (int index = 0; index < data.Count; index++)
            {
                DisbData record = data[index];
                currentLoanSeq = record.LN_SEQ.ToInt();

                if (currentLoanSeq != prevoiusLoanSeq)
                    seqNumWithinLoan = 1;

                record.ListOfLON18_DAT = DataAccessHelper.ExecuteList<DisbData.LON18_DAT>("GetLon18_Dat", DataAccessHelper.Database.AlignImport, new SqlParameter("DisbDataId", record.DisbDataId));
                record = ModifyDisbData(record, disbSeqNum, seqNumWithinLoan);

                int occ = record.GetType().GetProperties().Where(p => p.Name.StartsWith("ListOf")).First().GetCustomAttribute<OccursAttribute>().Times;
                for (int i = record.ListOfLON18_DAT.Count; i < occ; i++)
                    record.ListOfLON18_DAT.Add(new DisbData.LON18_DAT());

                records.Enqueue(RecordCompiler.CompileRecord(RecordPopulator.GetHeaderInformation(ssn, "04", (index + 1).ToString().PadLeft(4, '0'), owner), record));
                prevoiusLoanSeq = currentLoanSeq;
                seqNumWithinLoan++;
                disbSeqNum++;
            }
        }

        private static void ModifyDefermentData(DefermentData data, int seqNum = 0)
        {
            data.LF_DFR_CTL_NUM = (data.LF_DFR_CTL_NUM ?? "").PadLeft(3, '0');
        }

        private static void ModifyRepaymentData(RepaymentData data, int seqNum = 0)
        {
            DateTime sentDate = new DateTime();
            DateTime.TryParse(data.LD_SNT_RPD_DIS, out sentDate);

            if (data.RS05_DAT_ENT_IND != "Y")
            {
                data.BD_CRT_RS05 = "";
                data.BC_IRS_TAX_FIL_STA = "";
                data.BA_AGI = "";
                data.BN_MEM_HSE_HLD = "";
                data.RS20_DAT_ENT_IND = "";
                data.LA_PFH_PAY = "";
                data.LA_PMN_STD_PAY = "";
            }

            if (sentDate > DateTime.Today)
            {
                data = new RepaymentData();
                data.RPST10_DAT_ENT_IND = "N";
                return;
            }


            if (data.RS05_DAT_ENT_IND == "Y")
                data.BD_CRT_RS05 = DateTime.Now.AddMonths(data.BD_CRT_RS05.ToInt() * -1).ToString("MM/dd/yyyy");

            data.LN_LON66_DAT_OCC_CNT = data.ListOfLON66_DAT.Where(p => p.LA_RPS_ISL != null).Count().ToString();
            foreach (RepaymentData.LON66_DAT item in data.ListOfLON66_DAT)
                if (item.LA_RPS_ISL != null)
                {
                    item.LA_RPS_ISL = item.LA_RPS_ISL.Replace(".", "");
                }
        }

        private static void ModifyFinancialData(FinancialData data, int seqNum = 0)
        {
            if ((data.LA_CUR_PRI.ToIntNullable() ?? 0) <= 0)
                throw new Exception("Found loan with zero-balance.");
            if (data.LA_PT_PAY_PCV != null)
                data.LA_PT_PAY_PCV = data.LA_PT_PAY_PCV.Remove(data.LA_PT_PAY_PCV.LastIndexOf(".") + 3).Replace(".", "");

            DateTime temp = new DateTime();
            if (DateTime.TryParse(data.LD_END_GRC_PRD, out temp) && temp > new DateTime(1902, 1, 1))
                data.LD_END_GRC_PRD = temp.AddDays(-1).ToString();
            else
                data.LD_END_GRC_PRD = "";

        }

        private static DisbData ModifyDisbData(DisbData data, int disbSeqNum, int seqNumWithinLoan)
        {
            data.LN_BR_DSB_SEQ = disbSeqNum.ToString();
            data.LN_LON_DSB_SEQ = seqNumWithinLoan.ToString();

            List<DisbData.LON18_DAT> disbItemsToRemove = new List<DisbData.LON18_DAT>();

            int checkTest = 0;
            if (!int.TryParse(data.LF_DSB_CHK, out checkTest))
                data.LF_DSB_CHK = "";

            //REMOVE ANY FEES THAT ARE ALL 0'S
            data.ListOfLON18_DAT.RemoveAll(x => x.LA_DSB_FEE == "0000000");

            data.LN_LON18_DAT_OCC_CNT = data.ListOfLON18_DAT.Count.ToString();

            return data;
        }

        private static void ModifyData(dynamic data)
        {
            if (data.DF_PRS_ID.Replace(" ", "") != "" && !System.Text.RegularExpressions.Regex.IsMatch(data.DF_PRS_ID, @"^(?!219-09-9999|078-05-1120)(?!666|000|9\d{2})\d{3}(?!00)\d{2}(?!0{4})\d{4}$"))
            {
                data.DF_PRS_ID = "";
                Console.WriteLine(data.DF_PRS_ID);
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(data.DM_PRS_1, "[^a-zA-Z0-9 -]"))
                data.DM_PRS_1 = System.Text.RegularExpressions.Regex.Replace(data.DM_PRS_1, "[^0-9a-zA-Z -]+", " ");

            if (System.Text.RegularExpressions.Regex.IsMatch(data.DM_PRS_LST, "[^a-zA-Z0-9 -]"))
                data.DM_PRS_LST = System.Text.RegularExpressions.Regex.Replace(data.DM_PRS_1, "[^0-9a-zA-Z -]+", " ");

            if (data.DF_ZIP_CDE.Contains("-"))
                data.DF_ZIP_CDE = data.DF_ZIP_CDE.Replace("-", "");

            if (data.DF_DRV_LIC.Contains("-"))
                data.DF_DRV_LIC = data.DF_DRV_LIC.Replace("-", "");

            if (data.DX_ADR_EML_TXT == null || string.IsNullOrEmpty(data.DX_ADR_EML_TXT.Trim()))
            {
                data.DC_ADR_EML = string.Empty;
                data.DD_VER_ADR_EML = string.Empty;
                data.DC_SRC_ADR_EML = string.Empty;
                data.DI_VLD_ADR_EML = string.Empty;
                data.DX_ADR_EML_TXT = string.Empty;
            }
            else
            {
                bool isEmail = System.Text.RegularExpressions.Regex.IsMatch(data.DX_ADR_EML_TXT.Trim().ToLower(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
                if (!isEmail)
                {
                    data.DC_ADR_EML = string.Empty;
                    data.DD_VER_ADR_EML = string.Empty;
                    data.DC_SRC_ADR_EML = string.Empty;
                    data.DI_VLD_ADR_EML = string.Empty;
                    data.DX_ADR_EML_TXT = string.Empty;
                }
            }

            int val = 0;
            if (!int.TryParse(data.DN_PHN_XTN, out val))
                data.DN_PHN_XTN = string.Empty;

            if (data.DN_DOM_PHN_LCL == "0000" && data.DN_DOM_PHN_XCH == "000")
            {
                data.DC_PHN = string.Empty;
                data.DD_PHN_VER = string.Empty;
                data.DI_PHN_VLD = string.Empty;
                data.DN_DOM_PHN_ARA = string.Empty;
                data.DN_DOM_PHN_LCL = string.Empty;
                data.DN_DOM_PHN_XCH = string.Empty;
                data.DN_PHN_XTN = string.Empty;
                data.DC_ALW_ADL_PHN = string.Empty;
            }
        }

        private static void ModifyEndorserSpouseInfo(EndorserSpouse data, int seqNumber = 0)
        {
            ModifyData(data);
        }

        private static void ModifyStudentData(StudentData data, int seqNumber = 0)
        {
            if (data.STU10_DAT_ENT_IND != "Y")
            {
                data.LC_REA_SCL_SPR = "";
                data.LC_SCR_SCL_SPR = "";
                data.LC_STA_STU10 = "";
                data.LD_NTF_SCL_SPR = "";
                data.LD_SCL_SPR = "";
                data.LD_STA_STU10 = "";
                data.IF_HSP = "";
                data.LF_DOE_SCL_ENR_CUR = "";
            }
            ModifyData(data);
        }

        private static void ModifyReferenceInfo(ReferenceData data, int seqNumber = 0)
        {
            data.BN_SEQ_RFR = seqNumber.ToString();
            ModifyData(data);
        }


        private static void ModifyBorrowerData(BorrowerData data, int seqNumber = 0)
        {

            if (data.DD_BRT == null)
                data.DD_BRT = "";

            ModifyData(data);
        }

        public static string GetHeaderInformation(string owner, string ssn, string recordType, string seqNumber)
        {
            return string.Format("{0}{1}00000{2}{3}{4}", "".PadLeft(10, ' '), owner, ssn, recordType, seqNumber);
        }
    }
}
