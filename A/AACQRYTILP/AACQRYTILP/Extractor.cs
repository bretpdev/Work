using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Uheaa.Common;
using Uheaa.Common.DataAccess;
using Uheaa.Common.ProcessLogger;

namespace AACQRYTILP
{
    class Extractor
    {
        public ProcessLogRun logRun { get; set; }
        public DataAccess DA { get; set; }

        public Extractor(ProcessLogRun logRun)
        {
            this.logRun = logRun;
            this.DA = new DataAccess(logRun);
        }

        public void Process()
        {
			var data = DA.GetAACData();

			if(data == null || data.Rows.Count == 0)
            {
				Dialog.Info.Ok("No loans meeting criteria exist, Processing complete", "AACQRYTILP Processing Complete");
				return;
            }

			//Open data file for writing
			string dataPath = EnterpriseFileSystem.GetPath("AACQRYTILP_Data_File", DataAccessHelper.Region.Uheaa);
			var writer = new StreamW(dataPath);
			writer.WriteLine(GetHeaderRow());

			foreach(DataRow row in data.Rows)
            {
				string dataRow = "";
				//we want to skip the first column because it contains an identity not wanted in the file
				for(int i = 1; i <  data.Columns.Count; i++)
                {
					if(data.Columns[i].ColumnName == "_213BlankColsHere")
                    {
						for(int j = 0; j < 213; j++)
                        {
							dataRow += ",";
                        }
                    }
					else if(row[i] == null || row[i].GetType() == typeof(DBNull))
                    {
						dataRow += ",";
					}
					else
                    {
						dataRow += row[i].ToString() + ",";
					}
					
                }

				dataRow.Remove(dataRow.Length - 1, 1);
				writer.WriteLine(dataRow);
				writer.Flush();
            }
			writer.Close();
			var updateResult = DA.UpdateAfterExtraction();
			Dialog.Info.Ok(updateResult ? "Processing completed successfully." : $"Processing completed with errors. See process log id: {logRun.ProcessLogId.ToString()}.", "AACQRYTILP Processing Complete");
        }

        public string GetHeaderRow()
        {
			string headerRow = "BF_SSN,DM_PRS_1,DM_PRS_MID,DM_PRS_LST,DD_BRT,DX_STR_ADR_1,DX_STR_ADR_2,DM_CT,DC_DOM_ST," +
								"DF_ZIP,DM_FGN_CNY,DI_VLD_ADR,DN_PHN,DI_PHN_VLD,DN_ALT_PHN,DI_ALT_PHN_VLD,AC_LON_TYP," +
								"SUBSIDY,AD_PRC,AF_ORG_APL_OPS_LDR,AF_APL_ID,AF_APL_ID_SFX,AD_IST_TRM_BEG,AD_IST_TRM_END," +
								"AA_GTE_LON_AMT,AF_APL_OPS_SCL,AD_BR_SIG,LD_LFT_SCL,PR_RPD_FOR_ITR,LC_INT_TYP,LD_TRX_EFF," +
								"LA_TRX,IF_OPS_SCL_RPT,LC_STU_ENR_TYP,LD_ENR_CER,LD_LDR_NTF,AR_CON_ITR,AD_APL_RCV," +
								"AC_STU_DFR_REQ,AN_DISB_1,AC_DISB_1,AD_DISB_1,AA_DISB_1,ORG_1,CD_DISB_1,CA_DISB_1,GTE_1," +
								"AN_DISB_2,AC_DISB_2,AD_DISB_2,AA_DISB_2,ORG_2,CD_DISB_2,CA_DISB_2,GTE_2,AN_DISB_3," +
								"AC_DISB_3,AD_DISB_3,AA_DISB_3,ORG_3,CD_DISB_3,CA_DISB_3,GTE_3,AN_DISB_4,AC_DISB_4," +
								"AD_DISB_4,AA_DISB_4,ORG_4,CD_DISB_4,CA_DISB_4,GTE_4,AA_TOT_EDU_DET_PNT,LC_DFR_TYP1," +
								"LC_DFR_TYP2,LC_DFR_TYP3,LC_DFR_TYP4,LC_DFR_TYP5,LC_DFR_TYP6,LC_DFR_TYP7,LC_DFR_TYP8," +
								"LC_DFR_TYP9,LC_DFR_TYP10,LC_DFR_TYP11,LC_DFR_TYP12,LC_DFR_TYP13,LC_DFR_TYP14,LC_DFR_TYP15," +
								"LD_DFR_BEG1,LD_DFR_BEG2,LD_DFR_BEG3,LD_DFR_BEG4,LD_DFR_BEG5,LD_DFR_BEG6,LD_DFR_BEG7," +
								"LD_DFR_BEG8,LD_DFR_BEG9,LD_DFR_BEG10,LD_DFR_BEG11,LD_DFR_BEG12,LD_DFR_BEG13,LD_DFR_BEG14," +
								"LD_DFR_BEG15,LD_DFR_END1,LD_DFR_END2,LD_DFR_END3,LD_DFR_END4,LD_DFR_END5,LD_DFR_END6," +
								"LD_DFR_END7,LD_DFR_END8,LD_DFR_END9,LD_DFR_END10,LD_DFR_END11,LD_DFR_END12,LD_DFR_END13," +
								"LD_DFR_END14,LD_DFR_END15,LF_DOE_SCL_DFR1,LF_DOE_SCL_DFR2,LF_DOE_SCL_DFR3,LF_DOE_SCL_DFR4," +
								"LF_DOE_SCL_DFR5,LF_DOE_SCL_DFR6,LF_DOE_SCL_DFR7,LF_DOE_SCL_DFR8,LF_DOE_SCL_DFR9," +
								"LF_DOE_SCL_DFR10,LF_DOE_SCL_DFR11,LF_DOE_SCL_DFR12,LF_DOE_SCL_DFR13,LF_DOE_SCL_DFR14," +
								"LF_DOE_SCL_DFR15,LD_DFR_INF_CER1,LD_DFR_INF_CER2,LD_DFR_INF_CER3,LD_DFR_INF_CER4," +
								"LD_DFR_INF_CER5,LD_DFR_INF_CER6,LD_DFR_INF_CER7,LD_DFR_INF_CER8,LD_DFR_INF_CER9," +
								"LD_DFR_INF_CER10,LD_DFR_INF_CER11,LD_DFR_INF_CER12,LD_DFR_INF_CER13,LD_DFR_INF_CER14," +
								"LD_DFR_INF_CER15,AC_LON_STA_REA1,AC_LON_STA_REA2,AC_LON_STA_REA3,AC_LON_STA_REA4," +
								"AC_LON_STA_REA5,AC_LON_STA_REA6,AC_LON_STA_REA7,AC_LON_STA_REA8,AC_LON_STA_REA9," +
								"AC_LON_STA_REA10,AC_LON_STA_REA11,AC_LON_STA_REA12,AC_LON_STA_REA13,AC_LON_STA_REA14," +
								"AC_LON_STA_REA15,AD_DFR_BEG1,AD_DFR_BEG2,AD_DFR_BEG3,AD_DFR_BEG4,AD_DFR_BEG5,AD_DFR_BEG6," +
								"AD_DFR_BEG7,AD_DFR_BEG8,AD_DFR_BEG9,AD_DFR_BEG10,AD_DFR_BEG11,AD_DFR_BEG12,AD_DFR_BEG13," +
								"AD_DFR_BEG14,AD_DFR_BEG15,AD_DFR_END1,AD_DFR_END2,AD_DFR_END3,AD_DFR_END4,AD_DFR_END5," +
								"AD_DFR_END6,AD_DFR_END7,AD_DFR_END8,AD_DFR_END9,AD_DFR_END10,AD_DFR_END11,AD_DFR_END12," +
								"AD_DFR_END13,AD_DFR_END14,AD_DFR_END15,IF_OPS_SCL_RPT1,IF_OPS_SCL_RPT2,IF_OPS_SCL_RPT3," +
								"IF_OPS_SCL_RPT4,IF_OPS_SCL_RPT5,IF_OPS_SCL_RPT6,IF_OPS_SCL_RPT7,IF_OPS_SCL_RPT8," +
								"IF_OPS_SCL_RPT9,IF_OPS_SCL_RPT10,IF_OPS_SCL_RPT11,IF_OPS_SCL_RPT12,IF_OPS_SCL_RPT13," +
								"IF_OPS_SCL_RPT14,IF_OPS_SCL_RPT15,LD_ENR_CER1,LD_ENR_CER2,LD_ENR_CER3,LD_ENR_CER4," +
								"LD_ENR_CER5,LD_ENR_CER6,LD_ENR_CER7,LD_ENR_CER8,LD_ENR_CER9,LD_ENR_CER10,LD_ENR_CER11," +
								"LD_ENR_CER12,LD_ENR_CER13,LD_ENR_CER14,LD_ENR_CER15,STU_SSN,STU_DM_PRS_1,STU_DM_PRS_MID," +
								"STU_DM_PRS_LST,STU_DD_BRT,STU_DX_STR_ADR_1,STU_DX_STR_ADR_2,STU_DM_CT,STU_DC_DOM_ST," +
								"STU_DF_ZIP,STU_DM_FGN_CNY,STU_DI_VLD_ADR,STU_DN_PHN,STU_DI_PHN_VLD,STU_DN_ALT_PHN," +
								"STU_DI_ALT_PHN_VLD,EDSR_SSN,EDSR_DM_PRS_1,EDSR_DM_PRS_MID,EDSR_DM_PRS_LST,EDSR_DD_BRT," +
								"EDSR_DX_STR_ADR_1,EDSR_DX_STR_ADR_2,EDSR_DM_CT,EDSR_DC_DOM_ST,EDSR_DF_ZIP,EDSR_DM_FGN_CNY," +
								"EDSR_DI_VLD_ADR,EDSR_DN_PHN,EDSR_DI_PHN_VLD,EDSR_DN_ALT_PHN,EDSR_DI_ALT_PHN_VLD," +
								"AC_EDS_TYP,REF_IND,BM_RFR_1_1,BM_RFR_MID_1,BM_RFR_LST_1,BX_RFR_STR_ADR_1_1," +
								"BX_RFR_STR_ADR_2_1,BM_RFR_CT_1,BC_RFR_ST_1,BF_RFR_ZIP_1,BM_RFR_FGN_CNY_1,BI_VLD_ADR_1," +
								"BN_RFR_DOM_PHN_1,BI_DOM_PHN_VLD_1,BN_RFR_ALT_PHN_1,BI_ALT_PHN_VLD_1,BC_RFR_REL_BR_1," +
								"BM_RFR_1_2,BM_RFR_MID_2,BM_RFR_LST_2,BX_RFR_STR_ADR_1_2,BX_RFR_STR_ADR_2_2,BM_RFR_CT_2," +
								"BC_RFR_ST_2,BF_RFR_ZIP_2,BM_RFR_FGN_CNY_2,BI_VLD_ADR_2,BN_RFR_DOM_PHN_2,BI_DOM_PHN_VLD_2," +
								"BN_RFR_ALT_PHN_2,BI_ALT_PHN_VLD_2,BC_RFR_REL_BR_2,BONDID,AVE_REHB_PAY_AMT,RPY_LETTER_DT," +
								"RPY_INIT_BAL,RPY_INT_RATE,RPY_PAY_AMT,RPY_PAY_TERM,RPY_FIRST_DUE_DT,TOT_INT_TO_REPAY," +
								"TOT_REPAY_AMT,RPY_LAST_PAY_DT,FIT_INT_AMT,FIT_FEE_AMT,STH_STATUS,STH_EFF_DT,CACTUS_ID," +
								"RPY_NEXT_DUE_DT,RPY_OVERPAY,INT_ACCRUE_START_DT,BAT_ID,BAT_BR_CT,BAT_LN_CT,BAT_TOT," +
								"BAT_ITR,BAT_FEE";

			return headerRow;
		}
    }
}
