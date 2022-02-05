/*UTLWE02 BANKONE MR50 MONTH END FILE*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
/*%LET RPTLIB = /sas/whse/progrevw   ; */
/*FILENAME REPORT2 "&RPTLIB/ULWE02.LWE02R2";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE BKONEDF AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT  A.BF_SSN,
		A.LN_SEQ,
		A.LD_END_GRC_PRD,
		C.LN_MTH_GRC_PRD_DSC,
		B.LD_TRM_END,
		B.LD_TRM_BEG,
		B.LF_GTR_RFR,
		A.LF_DOE_SCL_ORG,
		C.LC_SCY_PGA,
		C.WX_SCY_PGA,
		A.IC_LON_PGM,
		C.LF_RGL_CAT_LP06,
		B.IF_DOE_LDR,
		A.IF_GTR,
		A.LF_STU_SSN,
		A.LD_LON_1_DSB,
		B.LC_ACA_GDE_LEV,
		C.WX_ACA_GDE_LEV,
		A.LC_SCY_PGA_PGM_YR,
		C.WX_SCY_PGA_PGM_YR,
		C.IC_HSP_CSE,
		C.WX_HSP_CSE,
		A.IF_OWN,
		B.LD_LON_EFF_ADD,
		C.LA_R78_INT_MAX,
		A.WN_DAY_GRC_RMN,
		A.WN_DAY_ENR_ELP,
		A.WD_RPY_BEG,
		A.WN_DAY_RPD_ELP,
		A.WN_MTH_RPD_ELP,
		C.IM_PGA_SHO,
		C.IM_GTR_SHO,
		A.WA_CUR_PRI,
		A.WA_CUR_BR_INT,
		A.WA_CUR_GOV_INT,
		A.WA_CUR_OTH_CHR,
		A.WA_AVG_DAY_BAL,
		A.WA_PRV_MTH_PRI,
		A.WA_PRV_MTH_BR_INT,
		A.WA_PRV_MTH_GOV_INT,
		A.WA_PRV_MTH_OTH_CHR,
		A.WM_BR_1,WM_BR_MID,
		A.WM_BR_LST,
		A.WM_BR_LST_SFX,
		B.DD_BRT,
		B.WM_STU_1,
		B.WM_STU_MID,
		B.WM_STU_LST,
		B.WM_STU_LST_SFX,
		B.WX_STR_ADR_1,
		B.WX_STR_ADR_2,
		B.WX_STR_ADR_3,
		B.WM_CT,
		B.DC_DOM_ST,
		B.DF_ZIP_CDE,
		B.DI_VLD_ADR,
		B.DD_STA_PDEM30,
		B.DM_FGN_CNY,
		B.DM_FGN_ST,
		B.DN_PHN_XTN,
		B.DN_DOM_PHN_LCL,
		B.DN_DOM_PHN_XCH,
		B.DN_DOM_PHN_ARA,
		B.DN_FGN_PHN_INL,
		B.DN_FGN_PHN_CNY,
		B.DN_FGN_PHN_CT,
		B.DN_FGN_PHN_LCL,
		B.DI_PHN_VLD,
		C.DD_SKP_BEG,
		B.WI_LON_COS,
		B.WI_LON_CMK,
		B.WI_LON_CBR,
		B.WI_OTH_EDS_TYP,
		A.WM_DOE_SCL_ORG,
		A.WC_TYP_SCL_ORG,
		B.WX_TYP_SCL_ORG,
		A.WI_PPR_SCL_ORG,
		B.LF_DOE_SCL_ENR_CUR,
		A.LD_SCL_SPR,
		A.WN_DAY_GRC_ELP,
		A.WN_DAY_ENR_RMN,
		B.WC_TYP_SCL_CUR,
		B.WX_TYP_SCL_CUR,
		C.WI_PPR_SCL_CUR,
		C.IF_GTR_RPT_SCL,
		C.WC_SCL_CUR_DOM_ST,
		C.WC_SCL_ORG_DOM_ST,
		B.LD_RPS_1_PAY_DU,
		B.LD_SNT_RPD_DIS,
		B.LD_BIL_DU,
		B.WN_MTH_INT_CAP_FRQ,
		C.WX_INT_CAP_FRQ,
		C.LC_TYP_SCH_DIS,
		C.WX_TYP_SCH_DIS,
		B.WA_RPS_ISL_1,
		B.WA_RPS_ISL_2,
		B.WA_RPS_ISL_3,
		B.WA_RPS_ISL_4,
		B.WA_RPS_ISL_5,
		B.WA_RPS_ISL_6,
		B.WA_RPS_ISL_7,
		B.WN_RPS_TRM_1,
		B.WN_RPS_TRM_2,
		B.WN_RPS_TRM_3,
		B.WN_RPS_TRM_4,
		B.WN_RPS_TRM_5,
		B.WN_RPS_TRM_6,
		B.WN_RPS_TRM_7,
		B.WN_RPS_TRM_INI,
		B.WA_1_DSB,
		B.WA_LON_TOT_DSB,
		B.WA_LON_TOT_INS_PRM,
		B.WA_LON_TOT_ORG_FEE,
		B.WA_LON_TOT_OTH_FEE,
		B.WA_ORG_PRI,
		B.WI_LON_FUL_DSB,
		B.LD_DSB,
		B.WA_LST_DSB,
		A.LD_DFR_BEG,
		A.LD_DFR_END,
		A.WN_DAY_DFR_RMN,
		A.LC_DFR_TYP,
		C.WX_DFR_TYP,
		A.WN_MTH_IN_DFR,
		A.LD_FOR_BEG,
		A.LD_FOR_END,
		B.WN_DAY_FOR_RMN,
		A.LC_FOR_TYP,
		C.WX_FOR_TYP,
		A.WN_MTH_IN_FOR,
		A.WI_INI_FOR_APL,
		A.WN_SQ_FOR_APL,
		B.WD_SBM_PCL,
		B.WD_INI_CLM_SBM,
		A.LC_REA_CLP_LON,
		C.WX_REA_CLP_LON,
		B.LD_CLM_REJ_RTN_ACL,
		B.WD_INI_CLM_PD,
		B.WA_INI_CLM_INT_PD,
		B.WA_INI_CLM_PRI_PD,
		A.WN_DAY_CLM_AGE,
		B.WD_RS_INI_CLM,
		B.WD_SUP_CLM_SBM,
		B.WD_SUP_CLM_PD,
		B.WA_SUP_CLM_INT_PD,
		B.WA_SUP_CLM_PRI_PD,
		B.WA_FAT_NSI_AT_PR,
		B.WA_PRI_AT_PR,
		B.WN_DAY_RPD_AFT_CVN,
		B.WD_FAT_APL_LST_CAP,
		B.WA_FAT_NSI_LST_CAP,
		B.WA_TOT_INT_CAP,
		B.WN_DAY_INT_CAP,
		B.WA_INT_WOF,
		B.WA_PRI_WOF,
		B.WD_LST_BR_PAY,
		B.WA_LST_PRI_PAY,
		B.WA_LST_INT_PAY,
		A.WC_REA_ZRO_BAL,
		C.WX_REA_ZRO_BAL,
		C.WC_SUB_REA_ZRO_BAL,
		C.WX_SUB_REA_ZRO_BAL,
		B.WD_ZRO_BAL_APL,
		B.WD_ZRO_BAL_EFF,
		A.WN_DAY_DLQ_ISL,
		B.WD_DLQ_DCO_ISL,
		A.WN_DAY_DLQ_INT,
		B.WD_DLQ_DCO_INT,
		A.WC_LON_STA,
		C.WX_LON_STA,
		A.WC_LON_SUB_STA,
		C.WX_LON_SUB_STA,
		B.WN_RPS_TRM_RMN,
		B.WA_CUR_ISL,
		A.WD_XPC_POF,
		A.WN_DAY_RPD_RMN,
		A.IM_OWN_SHO,
		A.IF_OWN_PRN,
		A.WM_OWN_PRN_SHO,
		A.IF_BND_ISS,
		A.LF_CUR_POR,
		A.LD_OWN_EFF_SR,
		B.II_TX_BND,
		A.ID_LON_SLE,
		B.IF_SLL_OWN,
		C.IC_OWN_DOM_ST,
		A.WC_ITR_TYP_1,
		A.WC_ITR_TYP_2,
		A.WC_ITR_TYP_3,
		A.WC_ITR_TYP_4,
		C.WX_ITR_TYP_1,
		C.WX_ITR_TYP_2,
		C.WX_ITR_TYP_3,
		C.WX_ITR_TYP_4,
		C.WD_ITR_EFF_BEG_1,
		C.WD_ITR_EFF_BEG_2,
		C.WD_ITR_EFF_BEG_3,
		C.WD_ITR_EFF_BEG_4,
		C.WD_ITR_EFF_END_1,
		C.WD_ITR_EFF_END_2,
		C.WD_ITR_EFF_END_3,
		C.WD_ITR_EFF_END_4,
		C.WD_ITR_APL_1,
		C.WD_ITR_APL_2,
		C.WD_ITR_APL_3,
		C.WD_ITR_APL_4,
		A.WR_ITR_1,
		A.WR_ITR_2,
		A.WR_ITR_3,
		A.WR_ITR_4,
		C.WI_SPC_ITR_1,
		C.WI_SPC_ITR_2,
		C.WI_SPC_ITR_3,
		C.WI_SPC_ITR_4,
		A.WC_ELG_SIN_1,
		A.WC_ELG_SIN_2,
		A.WC_ELG_SIN_3,
		A.WC_ELG_SIN_4,
		C.WX_ELG_SIN_1,
		C.WX_ELG_SIN_2,
		C.WX_ELG_SIN_3,
		C.WX_ELG_SIN_4,
		B.WN_PAY_RPD,
		C.WD_FNL_DMD_BR,
		C.WD_FNL_DMD_COS,
		A.WF_POR_AGE_LN_1,
		A.WF_POR_AGE_LN_2,
		A.WF_POR_RPT_LN_1,
		A.WF_POR_RPT_LN_2,
		A.WF_POR_RPT_LN_3,
		A.WF_POR_RPT_LN_4,
		A.WF_POR_RPT_LN_5,
		A.WF_POR_TME_LN,
		A.WD_MR50_CRT,
		C.LC_LIT_STA,
		C.LD_LIT_BEG,
		A.WI_NEW_LON,
		C.WI_LON_1_OWN_MR50,
		A.WF_TIR_PCE_LN35,
		B.LA_LON_AMT_GTR,
		B.LD_LON_GTR,
		A.WN_MTH_GRC_RMN,
		A.WN_MTH_DFR_RMN,
		A.WN_MTH_FOR_RMN,
		A.WN_MTH_ENR_RMN,
		A.WN_MTH_RPD_RMN,
		A.WN_MTH_ZRO_ELP,
		A.WN_MTH_GRC_ELP,
		A.WN_MTH_ENR_ELP,
		A.WN_MTH_LIT_ELP,
		C.LC_RPD_SLE,
		C.WX_RPD_SLE,
		C.LC_ST_BR_RSD_APL,
		C.WA_LON_SLE_TRF_PRI,
		C.WA_LON_SLE_TRF_INT,
		A.WC_SUB_STA_RPT,
		C.WC_REA_ZRO_NEW,
		C.WX_REA_ZRO_NEW,
		C.WC_SUB_REA_ZRO_NEW,
		C.WX_SUB_REA_ZRO_NEW,
		C.WD_ZRO_BAL_APL_NEW,
		C.WD_ZRO_BAL_EFF_NEW,
		B.DC_ADR_EML,
		B.DD_VER_ADR_EML,
		B.DI_VLD_ADR_EML,
		B.WX_ADR_EML,
		B.LC_MPN_TYP,
		B.LD_MPN_EXP,
		B.LC_MPN_SRL_LON,
		B.LC_MPN_REV_REA,
		B.LF_ORG_RGN,
		B.AF_LON_ALT,
		B.AN_SEQ_COM_LN_APL,
		C.LD_PNT_SIG

FROM	 OLWHRM1.MR5A_MR_LON_MTH_01 A
LEFT OUTER JOIN OLWHRM1.MR5B_MR_LON_MTH_02 B
		 ON A.BF_SSN = B.BF_SSN
		 AND A.LN_SEQ = B.LN_SEQ
		 AND A.IF_OWN = B.IF_OWN
LEFT OUTER JOIN OLWHRM1.MR5C_MR_LON_MTH_03 C
		 ON A.BF_SSN = C.BF_SSN
		 AND A.LN_SEQ = C.LN_SEQ
		 AND A.IF_OWN = C.IF_OWN
WHERE A.IF_OWN = '819628'
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA BKONEDF;
SET WORKLOCL.BKONEDF;
RUN;

PROC SORT DATA = BKONEDF NODUPKEY;
BY BF_SSN LN_SEQ;
RUN;

data _null_;
set  WORK.Bkonedf;
file 'T:\SAS\ULWE02.LWE02R2' delimiter=',' DSD DROPOVER lrecl=32767;
*FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
format BF_SSN $9. ;
format LN_SEQ 6. ;
format LD_END_GRC_PRD MMDDYY10. ;
format LN_MTH_GRC_PRD_DSC 4. ;
format LD_TRM_END MMDDYY10. ;
format LD_TRM_BEG MMDDYY10. ;
format LF_GTR_RFR $12. ;
format LF_DOE_SCL_ORG $8. ;
format LC_SCY_PGA $2. ;
format WX_SCY_PGA $20. ;
format IC_LON_PGM $6. ;
format LF_RGL_CAT_LP06 $7. ;
format IF_DOE_LDR $8. ;
format IF_GTR $6. ;
format LF_STU_SSN $9. ;
format LD_LON_1_DSB MMDDYY10. ;
format LC_ACA_GDE_LEV $2. ;
format WX_ACA_GDE_LEV $20. ;
format LC_SCY_PGA_PGM_YR $1. ;
format WX_SCY_PGA_PGM_YR $20. ;
format IC_HSP_CSE $3. ;
format WX_HSP_CSE $20. ;
format IF_OWN $8. ;
format LD_LON_EFF_ADD MMDDYY10. ;
format LA_R78_INT_MAX 9.2 ;
format WN_DAY_GRC_RMN 5. ;
format WN_DAY_ENR_ELP 5. ;
format WD_RPY_BEG MMDDYY10. ;
format WN_DAY_RPD_ELP 6. ;
format WN_MTH_RPD_ELP 4. ;
format IM_PGA_SHO $20. ;
format IM_GTR_SHO $20. ;
format WA_CUR_PRI 10.2 ;
format WA_CUR_BR_INT 10.2 ;
format WA_CUR_GOV_INT 9.2 ;
format WA_CUR_OTH_CHR 9.2 ;
format WA_AVG_DAY_BAL 10.2 ;
format WA_PRV_MTH_PRI 10.2 ;
format WA_PRV_MTH_BR_INT 10.2 ;
format WA_PRV_MTH_GOV_INT 9.2 ;
format WA_PRV_MTH_OTH_CHR 9.2 ;
format WM_BR_1 $13. ;
format WM_BR_MID $13. ;
format WM_BR_LST $23. ;
format WM_BR_LST_SFX $4. ;
format DD_BRT MMDDYY10. ;
format WM_STU_1 $13. ;
format WM_STU_MID $13. ;
format WM_STU_LST $23. ;
format WM_STU_LST_SFX $4. ;
format WX_STR_ADR_1 $30. ;
format WX_STR_ADR_2 $30. ;
format WX_STR_ADR_3 $30. ;
format WM_CT $20. ;
format DC_DOM_ST $2. ;
format DF_ZIP_CDE $17. ;
format DI_VLD_ADR $1. ;
format DD_STA_PDEM30 MMDDYY10. ;
format DM_FGN_CNY $25. ;
format DM_FGN_ST $15. ;
format DN_PHN_XTN $5. ;
format DN_DOM_PHN_LCL $4. ;
format DN_DOM_PHN_XCH $3. ;
format DN_DOM_PHN_ARA $3. ;
format DN_FGN_PHN_INL $3. ;
format DN_FGN_PHN_CNY $3. ;
format DN_FGN_PHN_CT $5. ;
format DN_FGN_PHN_LCL $11. ;
format DI_PHN_VLD $1. ;
format DD_SKP_BEG MMDDYY10. ;
format WI_LON_COS $1. ;
format WI_LON_CMK $1. ;
format WI_LON_CBR $1. ;
format WI_OTH_EDS_TYP $1. ;
format WM_DOE_SCL_ORG $20. ;
format WC_TYP_SCL_ORG $2. ;
format WX_TYP_SCL_ORG $20. ;
format WI_PPR_SCL_ORG $1. ;
format LF_DOE_SCL_ENR_CUR $8. ;
format LD_SCL_SPR MMDDYY10. ;
format WN_DAY_GRC_ELP 5. ;
format WN_DAY_ENR_RMN 5. ;
format WC_TYP_SCL_CUR $2. ;
format WX_TYP_SCL_CUR $20. ;
format WI_PPR_SCL_CUR $1. ;
format IF_GTR_RPT_SCL $8. ;
format WC_SCL_CUR_DOM_ST $2. ;
format WC_SCL_ORG_DOM_ST $2. ;
format LD_RPS_1_PAY_DU MMDDYY10. ;
format LD_SNT_RPD_DIS MMDDYY10. ;
format LD_BIL_DU MMDDYY10. ;
format WN_MTH_INT_CAP_FRQ 3. ;
format WX_INT_CAP_FRQ $15. ;
format LC_TYP_SCH_DIS $2. ;
format WX_TYP_SCH_DIS $20. ;
format WA_RPS_ISL_1 9.2 ;
format WA_RPS_ISL_2 9.2 ;
format WA_RPS_ISL_3 9.2 ;
format WA_RPS_ISL_4 9.2 ;
format WA_RPS_ISL_5 9.2 ;
format WA_RPS_ISL_6 9.2 ;
format WA_RPS_ISL_7 9.2 ;
format WN_RPS_TRM_1 4. ;
format WN_RPS_TRM_2 4. ;
format WN_RPS_TRM_3 4. ;
format WN_RPS_TRM_4 4. ;
format WN_RPS_TRM_5 4. ;
format WN_RPS_TRM_6 4. ;
format WN_RPS_TRM_7 4. ;
format WN_RPS_TRM_INI 4. ;
format WA_1_DSB 10.2 ;
format WA_LON_TOT_DSB 10.2 ;
format WA_LON_TOT_INS_PRM 9.2 ;
format WA_LON_TOT_ORG_FEE 9.2 ;
format WA_LON_TOT_OTH_FEE 9.2 ;
format WA_ORG_PRI 10.2 ;
format WI_LON_FUL_DSB $1. ;
format LD_DSB MMDDYY10. ;
format WA_LST_DSB 10.2 ;
format LD_DFR_BEG MMDDYY10. ;
format LD_DFR_END MMDDYY10. ;
format WN_DAY_DFR_RMN 5. ;
format LC_DFR_TYP $2. ;
format WX_DFR_TYP $20. ;
format WN_MTH_IN_DFR 4. ;
format LD_FOR_BEG MMDDYY10. ;
format LD_FOR_END MMDDYY10. ;
format WN_DAY_FOR_RMN 5. ;
format LC_FOR_TYP $2. ;
format WX_FOR_TYP $20. ;
format WN_MTH_IN_FOR 4. ;
format WI_INI_FOR_APL $1. ;
format WN_SQ_FOR_APL 4. ;
format WD_SBM_PCL MMDDYY10. ;
format WD_INI_CLM_SBM MMDDYY10. ;
format LC_REA_CLP_LON $2. ;
format WX_REA_CLP_LON $20. ;
format LD_CLM_REJ_RTN_ACL MMDDYY10. ;
format WD_INI_CLM_PD MMDDYY10. ;
format WA_INI_CLM_INT_PD 9.2 ;
format WA_INI_CLM_PRI_PD 10.2 ;
format WN_DAY_CLM_AGE 5. ;
format WD_RS_INI_CLM MMDDYY10. ;
format WD_SUP_CLM_SBM MMDDYY10. ;
format WD_SUP_CLM_PD MMDDYY10. ;
format WA_SUP_CLM_INT_PD 9.2 ;
format WA_SUP_CLM_PRI_PD 10.2 ;
format WA_FAT_NSI_AT_PR 9.2 ;
format WA_PRI_AT_PR 10.2 ;
format WN_DAY_RPD_AFT_CVN 6. ;
format WD_FAT_APL_LST_CAP MMDDYY10. ;
format WA_FAT_NSI_LST_CAP 9.2 ;
format WA_TOT_INT_CAP 9.2 ;
format WN_DAY_INT_CAP 5. ;
format WA_INT_WOF 9.2 ;
format WA_PRI_WOF 10.2 ;
format WD_LST_BR_PAY MMDDYY10. ;
format WA_LST_PRI_PAY 10.2 ;
format WA_LST_INT_PAY 9.2 ;
format WC_REA_ZRO_BAL $2. ;
format WX_REA_ZRO_BAL $20. ;
format WC_SUB_REA_ZRO_BAL $2. ;
format WX_SUB_REA_ZRO_BAL $20. ;
format WD_ZRO_BAL_APL MMDDYY10. ;
format WD_ZRO_BAL_EFF MMDDYY10. ;
format WN_DAY_DLQ_ISL 5. ;
format WD_DLQ_DCO_ISL MMDDYY10. ;
format WN_DAY_DLQ_INT 5. ;
format WD_DLQ_DCO_INT MMDDYY10. ;
format WC_LON_STA $2. ;
format WX_LON_STA $10. ;
format WC_LON_SUB_STA $2. ;
format WX_LON_SUB_STA $12. ;
format WN_RPS_TRM_RMN 4. ;
format WA_CUR_ISL 9.2 ;
format WD_XPC_POF MMDDYY10. ;
format WN_DAY_RPD_RMN 6. ;
format IM_OWN_SHO $20. ;
format IF_OWN_PRN $8. ;
format WM_OWN_PRN_SHO $20. ;
format IF_BND_ISS $8. ;
format LF_CUR_POR $20. ;
format LD_OWN_EFF_SR MMDDYY10. ;
format II_TX_BND $1. ;
format ID_LON_SLE MMDDYY10. ;
format IF_SLL_OWN $8. ;
format IC_OWN_DOM_ST $2. ;
format WC_ITR_TYP_1 $2. ;
format WC_ITR_TYP_2 $2. ;
format WC_ITR_TYP_3 $2. ;
format WC_ITR_TYP_4 $2. ;
format WX_ITR_TYP_1 $20. ;
format WX_ITR_TYP_2 $20. ;
format WX_ITR_TYP_3 $20. ;
format WX_ITR_TYP_4 $20. ;
format WD_ITR_EFF_BEG_1 MMDDYY10. ;
format WD_ITR_EFF_BEG_2 MMDDYY10. ;
format WD_ITR_EFF_BEG_3 MMDDYY10. ;
format WD_ITR_EFF_BEG_4 MMDDYY10. ;
format WD_ITR_EFF_END_1 MMDDYY10. ;
format WD_ITR_EFF_END_2 MMDDYY10. ;
format WD_ITR_EFF_END_3 MMDDYY10. ;
format WD_ITR_EFF_END_4 MMDDYY10. ;
format WD_ITR_APL_1 MMDDYY10. ;
format WD_ITR_APL_2 MMDDYY10. ;
format WD_ITR_APL_3 MMDDYY10. ;
format WD_ITR_APL_4 MMDDYY10. ;
format WR_ITR_1 7.3 ;
format WR_ITR_2 7.3 ;
format WR_ITR_3 7.3 ;
format WR_ITR_4 7.3 ;
format WI_SPC_ITR_1 $1. ;
format WI_SPC_ITR_2 $1. ;
format WI_SPC_ITR_3 $1. ;
format WI_SPC_ITR_4 $1. ;
format WC_ELG_SIN_1 $1. ;
format WC_ELG_SIN_2 $1. ;
format WC_ELG_SIN_3 $1. ;
format WC_ELG_SIN_4 $1. ;
format WX_ELG_SIN_1 $20. ;
format WX_ELG_SIN_2 $20. ;
format WX_ELG_SIN_3 $20. ;
format WX_ELG_SIN_4 $20. ;
format WN_PAY_RPD 4. ;
format WD_FNL_DMD_BR MMDDYY10. ;
format WD_FNL_DMD_COS MMDDYY10. ;
format WF_POR_AGE_LN_1 $3. ;
format WF_POR_AGE_LN_2 $3. ;
format WF_POR_RPT_LN_1 $3. ;
format WF_POR_RPT_LN_2 $3. ;
format WF_POR_RPT_LN_3 $3. ;
format WF_POR_RPT_LN_4 $3. ;
format WF_POR_RPT_LN_5 $3. ;
format WF_POR_TME_LN $3. ;
format WD_MR50_CRT MMDDYY10. ;
format LC_LIT_STA $2. ;
format LD_LIT_BEG MMDDYY10. ;
format WI_NEW_LON $1. ;
format WI_LON_1_OWN_MR50 $1. ;
format WF_TIR_PCE_LN35 $3. ;
format LA_LON_AMT_GTR 10.2 ;
format LD_LON_GTR MMDDYY10. ;
format WN_MTH_GRC_RMN 4. ;
format WN_MTH_DFR_RMN 4. ;
format WN_MTH_FOR_RMN 4. ;
format WN_MTH_ENR_RMN 4. ;
format WN_MTH_RPD_RMN 4. ;
format WN_MTH_ZRO_ELP 4. ;
format WN_MTH_GRC_ELP 4. ;
format WN_MTH_ENR_ELP 4. ;
format WN_MTH_LIT_ELP 4. ;
format LC_RPD_SLE $1. ;
format WX_RPD_SLE $20. ;
format LC_ST_BR_RSD_APL $2. ;
format WA_LON_SLE_TRF_PRI 10.2 ;
format WA_LON_SLE_TRF_INT 10.2 ;
format WC_SUB_STA_RPT $2. ;
format WC_REA_ZRO_NEW $2. ;
format WX_REA_ZRO_NEW $20. ;
format WC_SUB_REA_ZRO_NEW $2. ;
format WX_SUB_REA_ZRO_NEW $20. ;
format WD_ZRO_BAL_APL_NEW MMDDYY10. ;
format WD_ZRO_BAL_EFF_NEW MMDDYY10. ;
format DC_ADR_EML $1. ;
format DD_VER_ADR_EML MMDDYY10. ;
format DI_VLD_ADR_EML $1. ;
format WX_ADR_EML $75. ;
format LC_MPN_TYP $1. ;
format LD_MPN_EXP MMDDYY10. ;
format LC_MPN_SRL_LON $1. ;
format LC_MPN_REV_REA $2. ;
format LF_ORG_RGN $8. ;
format AF_LON_ALT $17. ;
format AN_SEQ_COM_LN_APL 6. ;
format LD_PNT_SIG MMDDYY10. ;
if _n_ = 1 then        /* write column names */
do;
put
'BF_SSN'
','
'LN_SEQ'
','
'LD_END_GRC_PRD'
','
'LN_MTH_GRC_PRD_DSC'
','
'LD_TRM_END'
','
'LD_TRM_BEG'
','
'LF_GTR_RFR'
','
'LF_DOE_SCL_ORG'
','
'LC_SCY_PGA'
','
'WX_SCY_PGA'
','
'IC_LON_PGM'
','
'LF_RGL_CAT_LP06'
','
'IF_DOE_LDR'
','
'IF_GTR'
','
'LF_STU_SSN'
','
'LD_LON_1_DSB'
','
'LC_ACA_GDE_LEV'
','
'WX_ACA_GDE_LEV'
','
'LC_SCY_PGA_PGM_YR'
','
'WX_SCY_PGA_PGM_YR'
','
'IC_HSP_CSE'
','
'WX_HSP_CSE'
','
'IF_OWN'
','
'LD_LON_EFF_ADD'
','
'LA_R78_INT_MAX'
','
'WN_DAY_GRC_RMN'
','
'WN_DAY_ENR_ELP'
','
'WD_RPY_BEG'
','
'WN_DAY_RPD_ELP'
','
'WN_MTH_RPD_ELP'
','
'IM_PGA_SHO'
','
'IM_GTR_SHO'
','
'WA_CUR_PRI'
','
'WA_CUR_BR_INT'
','
'WA_CUR_GOV_INT'
','
'WA_CUR_OTH_CHR'
','
'WA_AVG_DAY_BAL'
','
'WA_PRV_MTH_PRI'
','
'WA_PRV_MTH_BR_INT'
','
'WA_PRV_MTH_GOV_INT'
','
'WA_PRV_MTH_OTH_CHR'
','
'WM_BR_1'
','
'WM_BR_MID'
','
'WM_BR_LST'
','
'WM_BR_LST_SFX'
','
'DD_BRT'
','
'WM_STU_1'
','
'WM_STU_MID'
','
'WM_STU_LST'
','
'WM_STU_LST_SFX'
','
'WX_STR_ADR_1'
','
'WX_STR_ADR_2'
','
'WX_STR_ADR_3'
','
'WM_CT'
','
'DC_DOM_ST'
','
'DF_ZIP_CDE'
','
'DI_VLD_ADR'
','
'DD_STA_PDEM30'
','
'DM_FGN_CNY'
','
'DM_FGN_ST'
','
'DN_PHN_XTN'
','
'DN_DOM_PHN_LCL'
','
'DN_DOM_PHN_XCH'
','
'DN_DOM_PHN_ARA'
','
'DN_FGN_PHN_INL'
','
'DN_FGN_PHN_CNY'
','
'DN_FGN_PHN_CT'
','
'DN_FGN_PHN_LCL'
','
'DI_PHN_VLD'
','
'DD_SKP_BEG'
','
'WI_LON_COS'
','
'WI_LON_CMK'
','
'WI_LON_CBR'
','
'WI_OTH_EDS_TYP'
','
'WM_DOE_SCL_ORG'
','
'WC_TYP_SCL_ORG'
','
'WX_TYP_SCL_ORG'
','
'WI_PPR_SCL_ORG'
','
'LF_DOE_SCL_ENR_CUR'
','
'LD_SCL_SPR'
','
'WN_DAY_GRC_ELP'
','
'WN_DAY_ENR_RMN'
','
'WC_TYP_SCL_CUR'
','
'WX_TYP_SCL_CUR'
','
'WI_PPR_SCL_CUR'
','
'IF_GTR_RPT_SCL'
','
'WC_SCL_CUR_DOM_ST'
','
'WC_SCL_ORG_DOM_ST'
','
'LD_RPS_1_PAY_DU'
','
'LD_SNT_RPD_DIS'
','
'LD_BIL_DU'
','
'WN_MTH_INT_CAP_FRQ'
','
'WX_INT_CAP_FRQ'
','
'LC_TYP_SCH_DIS'
','
'WX_TYP_SCH_DIS'
','
'WA_RPS_ISL_1'
','
'WA_RPS_ISL_2'
','
'WA_RPS_ISL_3'
','
'WA_RPS_ISL_4'
','
'WA_RPS_ISL_5'
','
'WA_RPS_ISL_6'
','
'WA_RPS_ISL_7'
','
'WN_RPS_TRM_1'
','
'WN_RPS_TRM_2'
','
'WN_RPS_TRM_3'
','
'WN_RPS_TRM_4'
','
'WN_RPS_TRM_5'
','
'WN_RPS_TRM_6'
','
'WN_RPS_TRM_7'
','
'WN_RPS_TRM_INI'
','
'WA_1_DSB'
','
'WA_LON_TOT_DSB'
','
'WA_LON_TOT_INS_PRM'
','
'WA_LON_TOT_ORG_FEE'
','
'WA_LON_TOT_OTH_FEE'
','
'WA_ORG_PRI'
','
'WI_LON_FUL_DSB'
','
'LD_DSB'
','
'WA_LST_DSB'
','
'LD_DFR_BEG'
','
'LD_DFR_END'
','
'WN_DAY_DFR_RMN'
','
'LC_DFR_TYP'
','
'WX_DFR_TYP'
','
'WN_MTH_IN_DFR'
','
'LD_FOR_BEG'
','
'LD_FOR_END'
','
'WN_DAY_FOR_RMN'
','
'LC_FOR_TYP'
','
'WX_FOR_TYP'
','
'WN_MTH_IN_FOR'
','
'WI_INI_FOR_APL'
','
'WN_SQ_FOR_APL'
','
'WD_SBM_PCL'
','
'WD_INI_CLM_SBM'
','
'LC_REA_CLP_LON'
','
'WX_REA_CLP_LON'
','
'LD_CLM_REJ_RTN_ACL'
','
'WD_INI_CLM_PD'
','
'WA_INI_CLM_INT_PD'
','
'WA_INI_CLM_PRI_PD'
','
'WN_DAY_CLM_AGE'
','
'WD_RS_INI_CLM'
','
'WD_SUP_CLM_SBM'
','
'WD_SUP_CLM_PD'
','
'WA_SUP_CLM_INT_PD'
','
'WA_SUP_CLM_PRI_PD'
','
'WA_FAT_NSI_AT_PR'
','
'WA_PRI_AT_PR'
','
'WN_DAY_RPD_AFT_CVN'
','
'WD_FAT_APL_LST_CAP'
','
'WA_FAT_NSI_LST_CAP'
','
'WA_TOT_INT_CAP'
','
'WN_DAY_INT_CAP'
','
'WA_INT_WOF'
','
'WA_PRI_WOF'
','
'WD_LST_BR_PAY'
','
'WA_LST_PRI_PAY'
','
'WA_LST_INT_PAY'
','
'WC_REA_ZRO_BAL'
','
'WX_REA_ZRO_BAL'
','
'WC_SUB_REA_ZRO_BAL'
','
'WX_SUB_REA_ZRO_BAL'
','
'WD_ZRO_BAL_APL'
','
'WD_ZRO_BAL_EFF'
','
'WN_DAY_DLQ_ISL'
','
'WD_DLQ_DCO_ISL'
','
'WN_DAY_DLQ_INT'
','
'WD_DLQ_DCO_INT'
','
'WC_LON_STA'
','
'WX_LON_STA'
','
'WC_LON_SUB_STA'
','
'WX_LON_SUB_STA'
','
'WN_RPS_TRM_RMN'
','
'WA_CUR_ISL'
','
'WD_XPC_POF'
','
'WN_DAY_RPD_RMN'
','
'IM_OWN_SHO'
','
'IF_OWN_PRN'
','
'WM_OWN_PRN_SHO'
','
'IF_BND_ISS'
','
'LF_CUR_POR'
','
'LD_OWN_EFF_SR'
','
'II_TX_BND'
','
'ID_LON_SLE'
','
'IF_SLL_OWN'
','
'IC_OWN_DOM_ST'
','
'WC_ITR_TYP_1'
','
'WC_ITR_TYP_2'
','
'WC_ITR_TYP_3'
','
'WC_ITR_TYP_4'
','
'WX_ITR_TYP_1'
','
'WX_ITR_TYP_2'
','
'WX_ITR_TYP_3'
','
'WX_ITR_TYP_4'
','
'WD_ITR_EFF_BEG_1'
','
'WD_ITR_EFF_BEG_2'
','
'WD_ITR_EFF_BEG_3'
','
'WD_ITR_EFF_BEG_4'
','
'WD_ITR_EFF_END_1'
','
'WD_ITR_EFF_END_2'
','
'WD_ITR_EFF_END_3'
','
'WD_ITR_EFF_END_4'
','
'WD_ITR_APL_1'
','
'WD_ITR_APL_2'
','
'WD_ITR_APL_3'
','
'WD_ITR_APL_4'
','
'WR_ITR_1'
','
'WR_ITR_2'
','
'WR_ITR_3'
','
'WR_ITR_4'
','
'WI_SPC_ITR_1'
','
'WI_SPC_ITR_2'
','
'WI_SPC_ITR_3'
','
'WI_SPC_ITR_4'
','
'WC_ELG_SIN_1'
','
'WC_ELG_SIN_2'
','
'WC_ELG_SIN_3'
','
'WC_ELG_SIN_4'
','
'WX_ELG_SIN_1'
','
'WX_ELG_SIN_2'
','
'WX_ELG_SIN_3'
','
'WX_ELG_SIN_4'
','
'WN_PAY_RPD'
','
'WD_FNL_DMD_BR'
','
'WD_FNL_DMD_COS'
','
'WF_POR_AGE_LN_1'
','
'WF_POR_AGE_LN_2'
','
'WF_POR_RPT_LN_1'
','
'WF_POR_RPT_LN_2'
','
'WF_POR_RPT_LN_3'
','
'WF_POR_RPT_LN_4'
','
'WF_POR_RPT_LN_5'
','
'WF_POR_TME_LN'
','
'WD_MR50_CRT'
','
'LC_LIT_STA'
','
'LD_LIT_BEG'
','
'WI_NEW_LON'
','
'WI_LON_1_OWN_MR50'
','
'WF_TIR_PCE_LN35'
','
'LA_LON_AMT_GTR'
','
'LD_LON_GTR'
','
'WN_MTH_GRC_RMN'
','
'WN_MTH_DFR_RMN'
','
'WN_MTH_FOR_RMN'
','
'WN_MTH_ENR_RMN'
','
'WN_MTH_RPD_RMN'
','
'WN_MTH_ZRO_ELP'
','
'WN_MTH_GRC_ELP'
','
'WN_MTH_ENR_ELP'
','
'WN_MTH_LIT_ELP'
','
'LC_RPD_SLE'
','
'WX_RPD_SLE'
','
'LC_ST_BR_RSD_APL'
','
'WA_LON_SLE_TRF_PRI'
','
'WA_LON_SLE_TRF_INT'
','
'WC_SUB_STA_RPT'
','
'WC_REA_ZRO_NEW'
','
'WX_REA_ZRO_NEW'
','
'WC_SUB_REA_ZRO_NEW'
','
'WX_SUB_REA_ZRO_NEW'
','
'WD_ZRO_BAL_APL_NEW'
','
'WD_ZRO_BAL_EFF_NEW'
','
'DC_ADR_EML'
','
'DD_VER_ADR_EML'
','
'DI_VLD_ADR_EML'
','
'WX_ADR_EML'
','
'LC_MPN_TYP'
','
'LD_MPN_EXP'
','
'LC_MPN_SRL_LON'
','
'LC_MPN_REV_REA'
','
'LF_ORG_RGN'
','
'AF_LON_ALT'
','
'AN_SEQ_COM_LN_APL'
','
'LD_PNT_SIG'
;
end;
do;
put BF_SSN $ @;
put LN_SEQ @;
put LD_END_GRC_PRD @;
put LN_MTH_GRC_PRD_DSC @;
put LD_TRM_END @;
put LD_TRM_BEG @;
put LF_GTR_RFR $ @;
put LF_DOE_SCL_ORG $ @;
put LC_SCY_PGA $ @;
put WX_SCY_PGA $ @;
put IC_LON_PGM $ @;
put LF_RGL_CAT_LP06 $ @;
put IF_DOE_LDR $ @;
put IF_GTR $ @;
put LF_STU_SSN $ @;
put LD_LON_1_DSB @;
put LC_ACA_GDE_LEV $ @;
put WX_ACA_GDE_LEV $ @;
put LC_SCY_PGA_PGM_YR $ @;
put WX_SCY_PGA_PGM_YR $ @;
put IC_HSP_CSE $ @;
put WX_HSP_CSE $ @;
put IF_OWN $ @;
put LD_LON_EFF_ADD @;
put LA_R78_INT_MAX @;
put WN_DAY_GRC_RMN @;
put WN_DAY_ENR_ELP @;
put WD_RPY_BEG @;
put WN_DAY_RPD_ELP @;
put WN_MTH_RPD_ELP @;
put IM_PGA_SHO $ @;
put IM_GTR_SHO $ @;
put WA_CUR_PRI @;
put WA_CUR_BR_INT @;
put WA_CUR_GOV_INT @;
put WA_CUR_OTH_CHR @;
put WA_AVG_DAY_BAL @;
put WA_PRV_MTH_PRI @;
put WA_PRV_MTH_BR_INT @;
put WA_PRV_MTH_GOV_INT @;
put WA_PRV_MTH_OTH_CHR @;
put WM_BR_1 $ @;
put WM_BR_MID $ @;
put WM_BR_LST $ @;
put WM_BR_LST_SFX $ @;
put DD_BRT @;
put WM_STU_1 $ @;
put WM_STU_MID $ @;
put WM_STU_LST $ @;
put WM_STU_LST_SFX $ @;
put WX_STR_ADR_1 $ @;
put WX_STR_ADR_2 $ @;
put WX_STR_ADR_3 $ @;
put WM_CT $ @;
put DC_DOM_ST $ @;
put DF_ZIP_CDE $ @;
put DI_VLD_ADR $ @;
put DD_STA_PDEM30 @;
put DM_FGN_CNY $ @;
put DM_FGN_ST $ @;
put DN_PHN_XTN $ @;
put DN_DOM_PHN_LCL $ @;
put DN_DOM_PHN_XCH $ @;
put DN_DOM_PHN_ARA $ @;
put DN_FGN_PHN_INL $ @;
put DN_FGN_PHN_CNY $ @;
put DN_FGN_PHN_CT $ @;
put DN_FGN_PHN_LCL $ @;
put DI_PHN_VLD $ @;
put DD_SKP_BEG @;
put WI_LON_COS $ @;
put WI_LON_CMK $ @;
put WI_LON_CBR $ @;
put WI_OTH_EDS_TYP $ @;
put WM_DOE_SCL_ORG $ @;
put WC_TYP_SCL_ORG $ @;
put WX_TYP_SCL_ORG $ @;
put WI_PPR_SCL_ORG $ @;
put LF_DOE_SCL_ENR_CUR $ @;
put LD_SCL_SPR @;
put WN_DAY_GRC_ELP @;
put WN_DAY_ENR_RMN @;
put WC_TYP_SCL_CUR $ @;
put WX_TYP_SCL_CUR $ @;
put WI_PPR_SCL_CUR $ @;
put IF_GTR_RPT_SCL $ @;
put WC_SCL_CUR_DOM_ST $ @;
put WC_SCL_ORG_DOM_ST $ @;
put LD_RPS_1_PAY_DU @;
put LD_SNT_RPD_DIS @;
put LD_BIL_DU @;
put WN_MTH_INT_CAP_FRQ @;
put WX_INT_CAP_FRQ $ @;
put LC_TYP_SCH_DIS $ @;
put WX_TYP_SCH_DIS $ @;
put WA_RPS_ISL_1 @;
put WA_RPS_ISL_2 @;
put WA_RPS_ISL_3 @;
put WA_RPS_ISL_4 @;
put WA_RPS_ISL_5 @;
put WA_RPS_ISL_6 @;
put WA_RPS_ISL_7 @;
put WN_RPS_TRM_1 @;
put WN_RPS_TRM_2 @;
put WN_RPS_TRM_3 @;
put WN_RPS_TRM_4 @;
put WN_RPS_TRM_5 @;
put WN_RPS_TRM_6 @;
put WN_RPS_TRM_7 @;
put WN_RPS_TRM_INI @;
put WA_1_DSB @;
put WA_LON_TOT_DSB @;
put WA_LON_TOT_INS_PRM @;
put WA_LON_TOT_ORG_FEE @;
put WA_LON_TOT_OTH_FEE @;
put WA_ORG_PRI @;
put WI_LON_FUL_DSB $ @;
put LD_DSB @;
put WA_LST_DSB @;
put LD_DFR_BEG @;
put LD_DFR_END @;
put WN_DAY_DFR_RMN @;
put LC_DFR_TYP $ @;
put WX_DFR_TYP $ @;
put WN_MTH_IN_DFR @;
put LD_FOR_BEG @;
put LD_FOR_END @;
put WN_DAY_FOR_RMN @;
put LC_FOR_TYP $ @;
put WX_FOR_TYP $ @;
put WN_MTH_IN_FOR @;
put WI_INI_FOR_APL $ @;
put WN_SQ_FOR_APL @;
put WD_SBM_PCL @;
put WD_INI_CLM_SBM @;
put LC_REA_CLP_LON $ @;
put WX_REA_CLP_LON $ @;
put LD_CLM_REJ_RTN_ACL @;
put WD_INI_CLM_PD @;
put WA_INI_CLM_INT_PD @;
put WA_INI_CLM_PRI_PD @;
put WN_DAY_CLM_AGE @;
put WD_RS_INI_CLM @;
put WD_SUP_CLM_SBM @;
put WD_SUP_CLM_PD @;
put WA_SUP_CLM_INT_PD @;
put WA_SUP_CLM_PRI_PD @;
put WA_FAT_NSI_AT_PR @;
put WA_PRI_AT_PR @;
put WN_DAY_RPD_AFT_CVN @;
put WD_FAT_APL_LST_CAP @;
put WA_FAT_NSI_LST_CAP @;
put WA_TOT_INT_CAP @;
put WN_DAY_INT_CAP @;
put WA_INT_WOF @;
put WA_PRI_WOF @;
put WD_LST_BR_PAY @;
put WA_LST_PRI_PAY @;
put WA_LST_INT_PAY @;
put WC_REA_ZRO_BAL $ @;
put WX_REA_ZRO_BAL $ @;
put WC_SUB_REA_ZRO_BAL $ @;
put WX_SUB_REA_ZRO_BAL $ @;
put WD_ZRO_BAL_APL @;
put WD_ZRO_BAL_EFF @;
put WN_DAY_DLQ_ISL @;
put WD_DLQ_DCO_ISL @;
put WN_DAY_DLQ_INT @;
put WD_DLQ_DCO_INT @;
put WC_LON_STA $ @;
put WX_LON_STA $ @;
put WC_LON_SUB_STA $ @;
put WX_LON_SUB_STA $ @;
put WN_RPS_TRM_RMN @;
put WA_CUR_ISL @;
put WD_XPC_POF @;
put WN_DAY_RPD_RMN @;
put IM_OWN_SHO $ @;
put IF_OWN_PRN $ @;
put WM_OWN_PRN_SHO $ @;
put IF_BND_ISS $ @;
put LF_CUR_POR $ @;
put LD_OWN_EFF_SR @;
put II_TX_BND $ @;
put ID_LON_SLE @;
put IF_SLL_OWN $ @;
put IC_OWN_DOM_ST $ @;
put WC_ITR_TYP_1 $ @;
put WC_ITR_TYP_2 $ @;
put WC_ITR_TYP_3 $ @;
put WC_ITR_TYP_4 $ @;
put WX_ITR_TYP_1 $ @;
put WX_ITR_TYP_2 $ @;
put WX_ITR_TYP_3 $ @;
put WX_ITR_TYP_4 $ @;
put WD_ITR_EFF_BEG_1 @;
put WD_ITR_EFF_BEG_2 @;
put WD_ITR_EFF_BEG_3 @;
put WD_ITR_EFF_BEG_4 @;
put WD_ITR_EFF_END_1 @;
put WD_ITR_EFF_END_2 @;
put WD_ITR_EFF_END_3 @;
put WD_ITR_EFF_END_4 @;
put WD_ITR_APL_1 @;
put WD_ITR_APL_2 @;
put WD_ITR_APL_3 @;
put WD_ITR_APL_4 @;
put WR_ITR_1 @;
put WR_ITR_2 @;
put WR_ITR_3 @;
put WR_ITR_4 @;
put WI_SPC_ITR_1 $ @;
put WI_SPC_ITR_2 $ @;
put WI_SPC_ITR_3 $ @;
put WI_SPC_ITR_4 $ @;
put WC_ELG_SIN_1 $ @;
put WC_ELG_SIN_2 $ @;
put WC_ELG_SIN_3 $ @;
put WC_ELG_SIN_4 $ @;
put WX_ELG_SIN_1 $ @;
put WX_ELG_SIN_2 $ @;
put WX_ELG_SIN_3 $ @;
put WX_ELG_SIN_4 $ @;
put WN_PAY_RPD @;
put WD_FNL_DMD_BR @;
put WD_FNL_DMD_COS @;
put WF_POR_AGE_LN_1 $ @;
put WF_POR_AGE_LN_2 $ @;
put WF_POR_RPT_LN_1 $ @;
put WF_POR_RPT_LN_2 $ @;
put WF_POR_RPT_LN_3 $ @;
put WF_POR_RPT_LN_4 $ @;
put WF_POR_RPT_LN_5 $ @;
put WF_POR_TME_LN $ @;
put WD_MR50_CRT @;
put LC_LIT_STA $ @;
put LD_LIT_BEG @;
put WI_NEW_LON $ @;
put WI_LON_1_OWN_MR50 $ @;
put WF_TIR_PCE_LN35 $ @;
put LA_LON_AMT_GTR @;
put LD_LON_GTR @;
put WN_MTH_GRC_RMN @;
put WN_MTH_DFR_RMN @;
put WN_MTH_FOR_RMN @;
put WN_MTH_ENR_RMN @;
put WN_MTH_RPD_RMN @;
put WN_MTH_ZRO_ELP @;
put WN_MTH_GRC_ELP @;
put WN_MTH_ENR_ELP @;
put WN_MTH_LIT_ELP @;
put LC_RPD_SLE $ @;
put WX_RPD_SLE $ @;
put LC_ST_BR_RSD_APL $ @;
put WA_LON_SLE_TRF_PRI @;
put WA_LON_SLE_TRF_INT @;
put WC_SUB_STA_RPT $ @;
put WC_REA_ZRO_NEW $ @;
put WX_REA_ZRO_NEW $ @;
put WC_SUB_REA_ZRO_NEW $ @;
put WX_SUB_REA_ZRO_NEW $ @;
put WD_ZRO_BAL_APL_NEW @;
put WD_ZRO_BAL_EFF_NEW @;
put DC_ADR_EML $ @;
put DD_VER_ADR_EML @;
put DI_VLD_ADR_EML $ @;
put WX_ADR_EML $ @;
put LC_MPN_TYP $ @;
put LD_MPN_EXP @;
put LC_MPN_SRL_LON $ @;
put LC_MPN_REV_REA $ @;
put LF_ORG_RGN $ @;
put AF_LON_ALT $ @;
put AN_SEQ_COM_LN_APL @;
put LD_PNT_SIG ;
;
end;
run;

/*THESE DATA NULL STATEMENTS WERE USED IN TESTING AND WILL SPLIT THE ATTRIBUTES INTO TWO FILES*/
/*data _null_;*/
/*set  WORK.Bkonedf;*/
/*file 'T:\SAS\ULWE02.LWE02R2_T2' delimiter=',' DSD DROPOVER lrecl=32767;*/
/*format WD_INI_CLM_SBM MMDDYY10. ;*/
/*format LC_REA_CLP_LON $2. ;*/
/*format WX_REA_CLP_LON $20. ;*/
/*format LD_CLM_REJ_RTN_ACL MMDDYY10. ;*/
/*format WD_INI_CLM_PD MMDDYY10. ;*/
/*format WA_INI_CLM_INT_PD 9.2 ;*/
/*format WA_INI_CLM_PRI_PD 10.2 ;*/
/*format WN_DAY_CLM_AGE 5. ;*/
/*format WD_RS_INI_CLM MMDDYY10. ;*/
/*format WD_SUP_CLM_SBM MMDDYY10. ;*/
/*format WD_SUP_CLM_PD MMDDYY10. ;*/
/*format WA_SUP_CLM_INT_PD 9.2 ;*/
/*format WA_SUP_CLM_PRI_PD 10.2 ;*/
/*format WA_FAT_NSI_AT_PR 9.2 ;*/
/*format WA_PRI_AT_PR 10.2 ;*/
/*format WN_DAY_RPD_AFT_CVN 6. ;*/
/*format WD_FAT_APL_LST_CAP MMDDYY10. ;*/
/*format WA_FAT_NSI_LST_CAP 9.2 ;*/
/*format WA_TOT_INT_CAP 9.2 ;*/
/*format WN_DAY_INT_CAP 5. ;*/
/*format WA_INT_WOF 9.2 ;*/
/*format WA_PRI_WOF 10.2 ;*/
/*format WD_LST_BR_PAY MMDDYY10. ;*/
/*format WA_LST_PRI_PAY 10.2 ;*/
/*format WA_LST_INT_PAY 9.2 ;*/
/*format WC_REA_ZRO_BAL $2. ;*/
/*format WX_REA_ZRO_BAL $20. ;*/
/*format WC_SUB_REA_ZRO_BAL $2. ;*/
/*format WX_SUB_REA_ZRO_BAL $20. ;*/
/*format WD_ZRO_BAL_APL MMDDYY10. ;*/
/*format WD_ZRO_BAL_EFF MMDDYY10. ;*/
/*format WN_DAY_DLQ_ISL 5. ;*/
/*format WD_DLQ_DCO_ISL MMDDYY10. ;*/
/*format WN_DAY_DLQ_INT 5. ;*/
/*format WD_DLQ_DCO_INT MMDDYY10. ;*/
/*format WC_LON_STA $2. ;*/
/*format WX_LON_STA $10. ;*/
/*format WC_LON_SUB_STA $2. ;*/
/*format WX_LON_SUB_STA $12. ;*/
/*format WN_RPS_TRM_RMN 4. ;*/
/*format WA_CUR_ISL 9.2 ;*/
/*format WD_XPC_POF MMDDYY10. ;*/
/*format WN_DAY_RPD_RMN 6. ;*/
/*format IM_OWN_SHO $20. ;*/
/*format IF_OWN_PRN $8. ;*/
/*format WM_OWN_PRN_SHO $20. ;*/
/*format IF_BND_ISS $8. ;*/
/*format LF_CUR_POR $20. ;*/
/*format LD_OWN_EFF_SR MMDDYY10. ;*/
/*format II_TX_BND $1. ;*/
/*format ID_LON_SLE MMDDYY10. ;*/
/*format IF_SLL_OWN $8. ;*/
/*format IC_OWN_DOM_ST $2. ;*/
/*format WC_ITR_TYP_1 $2. ;*/
/*format WC_ITR_TYP_2 $2. ;*/
/*format WC_ITR_TYP_3 $2. ;*/
/*format WC_ITR_TYP_4 $2. ;*/
/*format WX_ITR_TYP_1 $20. ;*/
/*format WX_ITR_TYP_2 $20. ;*/
/*format WX_ITR_TYP_3 $20. ;*/
/*format WX_ITR_TYP_4 $20. ;*/
/*format WD_ITR_EFF_BEG_1 MMDDYY10. ;*/
/*format WD_ITR_EFF_BEG_2 MMDDYY10. ;*/
/*format WD_ITR_EFF_BEG_3 MMDDYY10. ;*/
/*format WD_ITR_EFF_BEG_4 MMDDYY10. ;*/
/*format WD_ITR_EFF_END_1 MMDDYY10. ;*/
/*format WD_ITR_EFF_END_2 MMDDYY10. ;*/
/*format WD_ITR_EFF_END_3 MMDDYY10. ;*/
/*format WD_ITR_EFF_END_4 MMDDYY10. ;*/
/*format WD_ITR_APL_1 MMDDYY10. ;*/
/*format WD_ITR_APL_2 MMDDYY10. ;*/
/*format WD_ITR_APL_3 MMDDYY10. ;*/
/*format WD_ITR_APL_4 MMDDYY10. ;*/
/*format WR_ITR_1 7.3 ;*/
/*format WR_ITR_2 7.3 ;*/
/*format WR_ITR_3 7.3 ;*/
/*format WR_ITR_4 7.3 ;*/
/*format WI_SPC_ITR_1 $1. ;*/
/*format WI_SPC_ITR_2 $1. ;*/
/*format WI_SPC_ITR_3 $1. ;*/
/*format WI_SPC_ITR_4 $1. ;*/
/*format WC_ELG_SIN_1 $1. ;*/
/*format WC_ELG_SIN_2 $1. ;*/
/*format WC_ELG_SIN_3 $1. ;*/
/*format WC_ELG_SIN_4 $1. ;*/
/*format WX_ELG_SIN_1 $20. ;*/
/*format WX_ELG_SIN_2 $20. ;*/
/*format WX_ELG_SIN_3 $20. ;*/
/*format WX_ELG_SIN_4 $20. ;*/
/*format WN_PAY_RPD 4. ;*/
/*format WD_FNL_DMD_BR MMDDYY10. ;*/
/*format WD_FNL_DMD_COS MMDDYY10. ;*/
/*format WF_POR_AGE_LN_1 $3. ;*/
/*format WF_POR_AGE_LN_2 $3. ;*/
/*format WF_POR_RPT_LN_1 $3. ;*/
/*format WF_POR_RPT_LN_2 $3. ;*/
/*format WF_POR_RPT_LN_3 $3. ;*/
/*format WF_POR_RPT_LN_4 $3. ;*/
/*format WF_POR_RPT_LN_5 $3. ;*/
/*format WF_POR_TME_LN $3. ;*/
/*format WD_MR50_CRT MMDDYY10. ;*/
/*format LC_LIT_STA $2. ;*/
/*format LD_LIT_BEG MMDDYY10. ;*/
/*format WI_NEW_LON $1. ;*/
/*format WI_LON_1_OWN_MR50 $1. ;*/
/*format WF_TIR_PCE_LN35 $3. ;*/
/*format LA_LON_AMT_GTR 10.2 ;*/
/*format LD_LON_GTR MMDDYY10. ;*/
/*format WN_MTH_GRC_RMN 4. ;*/
/*format WN_MTH_DFR_RMN 4. ;*/
/*format WN_MTH_FOR_RMN 4. ;*/
/*format WN_MTH_ENR_RMN 4. ;*/
/*format WN_MTH_RPD_RMN 4. ;*/
/*format WN_MTH_ZRO_ELP 4. ;*/
/*format WN_MTH_GRC_ELP 4. ;*/
/*format WN_MTH_ENR_ELP 4. ;*/
/*format WN_MTH_LIT_ELP 4. ;*/
/*format LC_RPD_SLE $1. ;*/
/*format WX_RPD_SLE $20. ;*/
/*format LC_ST_BR_RSD_APL $2. ;*/
/*format WA_LON_SLE_TRF_PRI 10.2 ;*/
/*format WA_LON_SLE_TRF_INT 10.2 ;*/
/*format WC_SUB_STA_RPT $2. ;*/
/*format WC_REA_ZRO_NEW $2. ;*/
/*format WX_REA_ZRO_NEW $20. ;*/
/*format WC_SUB_REA_ZRO_NEW $2. ;*/
/*format WX_SUB_REA_ZRO_NEW $20. ;*/
/*format WD_ZRO_BAL_APL_NEW MMDDYY10. ;*/
/*format WD_ZRO_BAL_EFF_NEW MMDDYY10. ;*/
/*format DC_ADR_EML $1. ;*/
/*format DD_VER_ADR_EML MMDDYY10. ;*/
/*format DI_VLD_ADR_EML $1. ;*/
/*format WX_ADR_EML $75. ;*/
/*format LC_MPN_TYP $1. ;*/
/*format LD_MPN_EXP MMDDYY10. ;*/
/*format LC_MPN_SRL_LON $1. ;*/
/*format LC_MPN_REV_REA $2. ;*/
/*format LF_ORG_RGN $8. ;*/
/*format AF_LON_ALT $17. ;*/
/*format AN_SEQ_COM_LN_APL 6. ;*/
/*format LD_PNT_SIG MMDDYY10. ;*/
/*if _n_ = 1 then        /* write column names */*/
/*do;*/
/*put*/
/*'WD_INI_CLM_SBM'*/
/*','*/
/*'LC_REA_CLP_LON'*/
/*','*/
/*'WX_REA_CLP_LON'*/
/*','*/
/*'LD_CLM_REJ_RTN_ACL'*/
/*','*/
/*'WD_INI_CLM_PD'*/
/*','*/
/*'WA_INI_CLM_INT_PD'*/
/*','*/
/*'WA_INI_CLM_PRI_PD'*/
/*','*/
/*'WN_DAY_CLM_AGE'*/
/*','*/
/*'WD_RS_INI_CLM'*/
/*','*/
/*'WD_SUP_CLM_SBM'*/
/*','*/
/*'WD_SUP_CLM_PD'*/
/*','*/
/*'WA_SUP_CLM_INT_PD'*/
/*','*/
/*'WA_SUP_CLM_PRI_PD'*/
/*','*/
/*'WA_FAT_NSI_AT_PR'*/
/*','*/
/*'WA_PRI_AT_PR'*/
/*','*/
/*'WN_DAY_RPD_AFT_CVN'*/
/*','*/
/*'WD_FAT_APL_LST_CAP'*/
/*','*/
/*'WA_FAT_NSI_LST_CAP'*/
/*','*/
/*'WA_TOT_INT_CAP'*/
/*','*/
/*'WN_DAY_INT_CAP'*/
/*','*/
/*'WA_INT_WOF'*/
/*','*/
/*'WA_PRI_WOF'*/
/*','*/
/*'WD_LST_BR_PAY'*/
/*','*/
/*'WA_LST_PRI_PAY'*/
/*','*/
/*'WA_LST_INT_PAY'*/
/*','*/
/*'WC_REA_ZRO_BAL'*/
/*','*/
/*'WX_REA_ZRO_BAL'*/
/*','*/
/*'WC_SUB_REA_ZRO_BAL'*/
/*','*/
/*'WX_SUB_REA_ZRO_BAL'*/
/*','*/
/*'WD_ZRO_BAL_APL'*/
/*','*/
/*'WD_ZRO_BAL_EFF'*/
/*','*/
/*'WN_DAY_DLQ_ISL'*/
/*','*/
/*'WD_DLQ_DCO_ISL'*/
/*','*/
/*'WN_DAY_DLQ_INT'*/
/*','*/
/*'WD_DLQ_DCO_INT'*/
/*','*/
/*'WC_LON_STA'*/
/*','*/
/*'WX_LON_STA'*/
/*','*/
/*'WC_LON_SUB_STA'*/
/*','*/
/*'WX_LON_SUB_STA'*/
/*','*/
/*'WN_RPS_TRM_RMN'*/
/*','*/
/*'WA_CUR_ISL'*/
/*','*/
/*'WD_XPC_POF'*/
/*','*/
/*'WN_DAY_RPD_RMN'*/
/*','*/
/*'IM_OWN_SHO'*/
/*','*/
/*'IF_OWN_PRN'*/
/*','*/
/*'WM_OWN_PRN_SHO'*/
/*','*/
/*'IF_BND_ISS'*/
/*','*/
/*'LF_CUR_POR'*/
/*','*/
/*'LD_OWN_EFF_SR'*/
/*','*/
/*'II_TX_BND'*/
/*','*/
/*'ID_LON_SLE'*/
/*','*/
/*'IF_SLL_OWN'*/
/*','*/
/*'IC_OWN_DOM_ST'*/
/*','*/
/*'WC_ITR_TYP_1'*/
/*','*/
/*'WC_ITR_TYP_2'*/
/*','*/
/*'WC_ITR_TYP_3'*/
/*','*/
/*'WC_ITR_TYP_4'*/
/*','*/
/*'WX_ITR_TYP_1'*/
/*','*/
/*'WX_ITR_TYP_2'*/
/*','*/
/*'WX_ITR_TYP_3'*/
/*','*/
/*'WX_ITR_TYP_4'*/
/*','*/
/*'WD_ITR_EFF_BEG_1'*/
/*','*/
/*'WD_ITR_EFF_BEG_2'*/
/*','*/
/*'WD_ITR_EFF_BEG_3'*/
/*','*/
/*'WD_ITR_EFF_BEG_4'*/
/*','*/
/*'WD_ITR_EFF_END_1'*/
/*','*/
/*'WD_ITR_EFF_END_2'*/
/*','*/
/*'WD_ITR_EFF_END_3'*/
/*','*/
/*'WD_ITR_EFF_END_4'*/
/*','*/
/*'WD_ITR_APL_1'*/
/*','*/
/*'WD_ITR_APL_2'*/
/*','*/
/*'WD_ITR_APL_3'*/
/*','*/
/*'WD_ITR_APL_4'*/
/*','*/
/*'WR_ITR_1'*/
/*','*/
/*'WR_ITR_2'*/
/*','*/
/*'WR_ITR_3'*/
/*','*/
/*'WR_ITR_4'*/
/*','*/
/*'WI_SPC_ITR_1'*/
/*','*/
/*'WI_SPC_ITR_2'*/
/*','*/
/*'WI_SPC_ITR_3'*/
/*','*/
/*'WI_SPC_ITR_4'*/
/*','*/
/*'WC_ELG_SIN_1'*/
/*','*/
/*'WC_ELG_SIN_2'*/
/*','*/
/*'WC_ELG_SIN_3'*/
/*','*/
/*'WC_ELG_SIN_4'*/
/*','*/
/*'WX_ELG_SIN_1'*/
/*','*/
/*'WX_ELG_SIN_2'*/
/*','*/
/*'WX_ELG_SIN_3'*/
/*','*/
/*'WX_ELG_SIN_4'*/
/*','*/
/*'WN_PAY_RPD'*/
/*','*/
/*'WD_FNL_DMD_BR'*/
/*','*/
/*'WD_FNL_DMD_COS'*/
/*','*/
/*'WF_POR_AGE_LN_1'*/
/*','*/
/*'WF_POR_AGE_LN_2'*/
/*','*/
/*'WF_POR_RPT_LN_1'*/
/*','*/
/*'WF_POR_RPT_LN_2'*/
/*','*/
/*'WF_POR_RPT_LN_3'*/
/*','*/
/*'WF_POR_RPT_LN_4'*/
/*','*/
/*'WF_POR_RPT_LN_5'*/
/*','*/
/*'WF_POR_TME_LN'*/
/*','*/
/*'WD_MR50_CRT'*/
/*','*/
/*'LC_LIT_STA'*/
/*','*/
/*'LD_LIT_BEG'*/
/*','*/
/*'WI_NEW_LON'*/
/*','*/
/*'WI_LON_1_OWN_MR50'*/
/*','*/
/*'WF_TIR_PCE_LN35'*/
/*','*/
/*'LA_LON_AMT_GTR'*/
/*','*/
/*'LD_LON_GTR'*/
/*','*/
/*'WN_MTH_GRC_RMN'*/
/*','*/
/*'WN_MTH_DFR_RMN'*/
/*','*/
/*'WN_MTH_FOR_RMN'*/
/*','*/
/*'WN_MTH_ENR_RMN'*/
/*','*/
/*'WN_MTH_RPD_RMN'*/
/*','*/
/*'WN_MTH_ZRO_ELP'*/
/*','*/
/*'WN_MTH_GRC_ELP'*/
/*','*/
/*'WN_MTH_ENR_ELP'*/
/*','*/
/*'WN_MTH_LIT_ELP'*/
/*','*/
/*'LC_RPD_SLE'*/
/*','*/
/*'WX_RPD_SLE'*/
/*','*/
/*'LC_ST_BR_RSD_APL'*/
/*','*/
/*'WA_LON_SLE_TRF_PRI'*/
/*','*/
/*'WA_LON_SLE_TRF_INT'*/
/*','*/
/*'WC_SUB_STA_RPT'*/
/*','*/
/*'WC_REA_ZRO_NEW'*/
/*','*/
/*'WX_REA_ZRO_NEW'*/
/*','*/
/*'WC_SUB_REA_ZRO_NEW'*/
/*','*/
/*'WX_SUB_REA_ZRO_NEW'*/
/*','*/
/*'WD_ZRO_BAL_APL_NEW'*/
/*','*/
/*'WD_ZRO_BAL_EFF_NEW'*/
/*','*/
/*'DC_ADR_EML'*/
/*','*/
/*'DD_VER_ADR_EML'*/
/*','*/
/*'DI_VLD_ADR_EML'*/
/*','*/
/*'WX_ADR_EML'*/
/*','*/
/*'LC_MPN_TYP'*/
/*','*/
/*'LD_MPN_EXP'*/
/*','*/
/*'LC_MPN_SRL_LON'*/
/*','*/
/*'LC_MPN_REV_REA'*/
/*','*/
/*'LF_ORG_RGN'*/
/*','*/
/*'AF_LON_ALT'*/
/*','*/
/*'AN_SEQ_COM_LN_APL'*/
/*','*/
/*'LD_PNT_SIG'*/
/*;*/
/*end;*/
/*do;*/
/*put WD_INI_CLM_SBM @;*/
/*put LC_REA_CLP_LON $ @;*/
/*put WX_REA_CLP_LON $ @;*/
/*put LD_CLM_REJ_RTN_ACL @;*/
/*put WD_INI_CLM_PD @;*/
/*put WA_INI_CLM_INT_PD @;*/
/*put WA_INI_CLM_PRI_PD @;*/
/*put WN_DAY_CLM_AGE @;*/
/*put WD_RS_INI_CLM @;*/
/*put WD_SUP_CLM_SBM @;*/
/*put WD_SUP_CLM_PD @;*/
/*put WA_SUP_CLM_INT_PD @;*/
/*put WA_SUP_CLM_PRI_PD @;*/
/*put WA_FAT_NSI_AT_PR @;*/
/*put WA_PRI_AT_PR @;*/
/*put WN_DAY_RPD_AFT_CVN @;*/
/*put WD_FAT_APL_LST_CAP @;*/
/*put WA_FAT_NSI_LST_CAP @;*/
/*put WA_TOT_INT_CAP @;*/
/*put WN_DAY_INT_CAP @;*/
/*put WA_INT_WOF @;*/
/*put WA_PRI_WOF @;*/
/*put WD_LST_BR_PAY @;*/
/*put WA_LST_PRI_PAY @;*/
/*put WA_LST_INT_PAY @;*/
/*put WC_REA_ZRO_BAL $ @;*/
/*put WX_REA_ZRO_BAL $ @;*/
/*put WC_SUB_REA_ZRO_BAL $ @;*/
/*put WX_SUB_REA_ZRO_BAL $ @;*/
/*put WD_ZRO_BAL_APL @;*/
/*put WD_ZRO_BAL_EFF @;*/
/*put WN_DAY_DLQ_ISL @;*/
/*put WD_DLQ_DCO_ISL @;*/
/*put WN_DAY_DLQ_INT @;*/
/*put WD_DLQ_DCO_INT @;*/
/*put WC_LON_STA $ @;*/
/*put WX_LON_STA $ @;*/
/*put WC_LON_SUB_STA $ @;*/
/*put WX_LON_SUB_STA $ @;*/
/*put WN_RPS_TRM_RMN @;*/
/*put WA_CUR_ISL @;*/
/*put WD_XPC_POF @;*/
/*put WN_DAY_RPD_RMN @;*/
/*put IM_OWN_SHO $ @;*/
/*put IF_OWN_PRN $ @;*/
/*put WM_OWN_PRN_SHO $ @;*/
/*put IF_BND_ISS $ @;*/
/*put LF_CUR_POR $ @;*/
/*put LD_OWN_EFF_SR @;*/
/*put II_TX_BND $ @;*/
/*put ID_LON_SLE @;*/
/*put IF_SLL_OWN $ @;*/
/*put IC_OWN_DOM_ST $ @;*/
/*put WC_ITR_TYP_1 $ @;*/
/*put WC_ITR_TYP_2 $ @;*/
/*put WC_ITR_TYP_3 $ @;*/
/*put WC_ITR_TYP_4 $ @;*/
/*put WX_ITR_TYP_1 $ @;*/
/*put WX_ITR_TYP_2 $ @;*/
/*put WX_ITR_TYP_3 $ @;*/
/*put WX_ITR_TYP_4 $ @;*/
/*put WD_ITR_EFF_BEG_1 @;*/
/*put WD_ITR_EFF_BEG_2 @;*/
/*put WD_ITR_EFF_BEG_3 @;*/
/*put WD_ITR_EFF_BEG_4 @;*/
/*put WD_ITR_EFF_END_1 @;*/
/*put WD_ITR_EFF_END_2 @;*/
/*put WD_ITR_EFF_END_3 @;*/
/*put WD_ITR_EFF_END_4 @;*/
/*put WD_ITR_APL_1 @;*/
/*put WD_ITR_APL_2 @;*/
/*put WD_ITR_APL_3 @;*/
/*put WD_ITR_APL_4 @;*/
/*put WR_ITR_1 @;*/
/*put WR_ITR_2 @;*/
/*put WR_ITR_3 @;*/
/*put WR_ITR_4 @;*/
/*put WI_SPC_ITR_1 $ @;*/
/*put WI_SPC_ITR_2 $ @;*/
/*put WI_SPC_ITR_3 $ @;*/
/*put WI_SPC_ITR_4 $ @;*/
/*put WC_ELG_SIN_1 $ @;*/
/*put WC_ELG_SIN_2 $ @;*/
/*put WC_ELG_SIN_3 $ @;*/
/*put WC_ELG_SIN_4 $ @;*/
/*put WX_ELG_SIN_1 $ @;*/
/*put WX_ELG_SIN_2 $ @;*/
/*put WX_ELG_SIN_3 $ @;*/
/*put WX_ELG_SIN_4 $ @;*/
/*put WN_PAY_RPD @;*/
/*put WD_FNL_DMD_BR @;*/
/*put WD_FNL_DMD_COS @;*/
/*put WF_POR_AGE_LN_1 $ @;*/
/*put WF_POR_AGE_LN_2 $ @;*/
/*put WF_POR_RPT_LN_1 $ @;*/
/*put WF_POR_RPT_LN_2 $ @;*/
/*put WF_POR_RPT_LN_3 $ @;*/
/*put WF_POR_RPT_LN_4 $ @;*/
/*put WF_POR_RPT_LN_5 $ @;*/
/*put WF_POR_TME_LN $ @;*/
/*put WD_MR50_CRT @;*/
/*put LC_LIT_STA $ @;*/
/*put LD_LIT_BEG @;*/
/*put WI_NEW_LON $ @;*/
/*put WI_LON_1_OWN_MR50 $ @;*/
/*put WF_TIR_PCE_LN35 $ @;*/
/*put LA_LON_AMT_GTR @;*/
/*put LD_LON_GTR @;*/
/*put WN_MTH_GRC_RMN @;*/
/*put WN_MTH_DFR_RMN @;*/
/*put WN_MTH_FOR_RMN @;*/
/*put WN_MTH_ENR_RMN @;*/
/*put WN_MTH_RPD_RMN @;*/
/*put WN_MTH_ZRO_ELP @;*/
/*put WN_MTH_GRC_ELP @;*/
/*put WN_MTH_ENR_ELP @;*/
/*put WN_MTH_LIT_ELP @;*/
/*put LC_RPD_SLE $ @;*/
/*put WX_RPD_SLE $ @;*/
/*put LC_ST_BR_RSD_APL $ @;*/
/*put WA_LON_SLE_TRF_PRI @;*/
/*put WA_LON_SLE_TRF_INT @;*/
/*put WC_SUB_STA_RPT $ @;*/
/*put WC_REA_ZRO_NEW $ @;*/
/*put WX_REA_ZRO_NEW $ @;*/
/*put WC_SUB_REA_ZRO_NEW $ @;*/
/*put WX_SUB_REA_ZRO_NEW $ @;*/
/*put WD_ZRO_BAL_APL_NEW @;*/
/*put WD_ZRO_BAL_EFF_NEW @;*/
/*put DC_ADR_EML $ @;*/
/*put DD_VER_ADR_EML @;*/
/*put DI_VLD_ADR_EML $ @;*/
/*put WX_ADR_EML $ @;*/
/*put LC_MPN_TYP $ @;*/
/*put LD_MPN_EXP @;*/
/*put LC_MPN_SRL_LON $ @;*/
/*put LC_MPN_REV_REA $ @;*/
/*put LF_ORG_RGN $ @;*/
/*put AF_LON_ALT $ @;*/
/*put AN_SEQ_COM_LN_APL @;*/
/*put LD_PNT_SIG ;*/
/*;*/
/*end;*/
/*run;*/
/**/
/*data _null_;*/
/*set  WORK.Bkonedf;*/
/*file 'T:\SAS\ULWE02.LWE02R2_T1' delimiter=',' DSD DROPOVER lrecl=32767;*/
/*format BF_SSN $9. ;*/
/*format LN_SEQ 6. ;*/
/*format LD_END_GRC_PRD MMDDYY10. ;*/
/*format LN_MTH_GRC_PRD_DSC 4. ;*/
/*format LD_TRM_END MMDDYY10. ;*/
/*format LD_TRM_BEG MMDDYY10. ;*/
/*format LF_GTR_RFR $12. ;*/
/*format LF_DOE_SCL_ORG $8. ;*/
/*format LC_SCY_PGA $2. ;*/
/*format WX_SCY_PGA $20. ;*/
/*format IC_LON_PGM $6. ;*/
/*format LF_RGL_CAT_LP06 $7. ;*/
/*format IF_DOE_LDR $8. ;*/
/*format IF_GTR $6. ;*/
/*format LF_STU_SSN $9. ;*/
/*format LD_LON_1_DSB MMDDYY10. ;*/
/*format LC_ACA_GDE_LEV $2. ;*/
/*format WX_ACA_GDE_LEV $20. ;*/
/*format LC_SCY_PGA_PGM_YR $1. ;*/
/*format WX_SCY_PGA_PGM_YR $20. ;*/
/*format IC_HSP_CSE $3. ;*/
/*format WX_HSP_CSE $20. ;*/
/*format IF_OWN $8. ;*/
/*format LD_LON_EFF_ADD MMDDYY10. ;*/
/*format LA_R78_INT_MAX 9.2 ;*/
/*format WN_DAY_GRC_RMN 5. ;*/
/*format WN_DAY_ENR_ELP 5. ;*/
/*format WD_RPY_BEG MMDDYY10. ;*/
/*format WN_DAY_RPD_ELP 6. ;*/
/*format WN_MTH_RPD_ELP 4. ;*/
/*format IM_PGA_SHO $20. ;*/
/*format IM_GTR_SHO $20. ;*/
/*format WA_CUR_PRI 10.2 ;*/
/*format WA_CUR_BR_INT 10.2 ;*/
/*format WA_CUR_GOV_INT 9.2 ;*/
/*format WA_CUR_OTH_CHR 9.2 ;*/
/*format WA_AVG_DAY_BAL 10.2 ;*/
/*format WA_PRV_MTH_PRI 10.2 ;*/
/*format WA_PRV_MTH_BR_INT 10.2 ;*/
/*format WA_PRV_MTH_GOV_INT 9.2 ;*/
/*format WA_PRV_MTH_OTH_CHR 9.2 ;*/
/*format WM_BR_1 $13. ;*/
/*format WM_BR_MID $13. ;*/
/*format WM_BR_LST $23. ;*/
/*format WM_BR_LST_SFX $4. ;*/
/*format DD_BRT MMDDYY10. ;*/
/*format WM_STU_1 $13. ;*/
/*format WM_STU_MID $13. ;*/
/*format WM_STU_LST $23. ;*/
/*format WM_STU_LST_SFX $4. ;*/
/*format WX_STR_ADR_1 $30. ;*/
/*format WX_STR_ADR_2 $30. ;*/
/*format WX_STR_ADR_3 $30. ;*/
/*format WM_CT $20. ;*/
/*format DC_DOM_ST $2. ;*/
/*format DF_ZIP_CDE $9. ;*/
/*format DI_VLD_ADR $1. ;*/
/*format DD_STA_PDEM30 MMDDYY10. ;*/
/*format DM_FGN_CNY $15. ;*/
/*format DM_FGN_ST $15. ;*/
/*format DN_PHN_XTN $4. ;*/
/*format DN_DOM_PHN_LCL $4. ;*/
/*format DN_DOM_PHN_XCH $3. ;*/
/*format DN_DOM_PHN_ARA $3. ;*/
/*format DN_FGN_PHN_INL $3. ;*/
/*format DN_FGN_PHN_CNY $3. ;*/
/*format DN_FGN_PHN_CT $4. ;*/
/*format DN_FGN_PHN_LCL $7. ;*/
/*format DI_PHN_VLD $1. ;*/
/*format DD_SKP_BEG MMDDYY10. ;*/
/*format WI_LON_COS $1. ;*/
/*format WI_LON_CMK $1. ;*/
/*format WI_LON_CBR $1. ;*/
/*format WI_OTH_EDS_TYP $1. ;*/
/*format WM_DOE_SCL_ORG $20. ;*/
/*format WC_TYP_SCL_ORG $2. ;*/
/*format WX_TYP_SCL_ORG $20. ;*/
/*format WI_PPR_SCL_ORG $1. ;*/
/*format LF_DOE_SCL_ENR_CUR $8. ;*/
/*format LD_SCL_SPR MMDDYY10. ;*/
/*format WN_DAY_GRC_ELP 5. ;*/
/*format WN_DAY_ENR_RMN 5. ;*/
/*format WC_TYP_SCL_CUR $2. ;*/
/*format WX_TYP_SCL_CUR $20. ;*/
/*format WI_PPR_SCL_CUR $1. ;*/
/*format IF_GTR_RPT_SCL $8. ;*/
/*format WC_SCL_CUR_DOM_ST $2. ;*/
/*format WC_SCL_ORG_DOM_ST $2. ;*/
/*format LD_RPS_1_PAY_DU MMDDYY10. ;*/
/*format LD_SNT_RPD_DIS MMDDYY10. ;*/
/*format LD_BIL_DU MMDDYY10. ;*/
/*format WN_MTH_INT_CAP_FRQ 3. ;*/
/*format WX_INT_CAP_FRQ $15. ;*/
/*format LC_TYP_SCH_DIS $2. ;*/
/*format WX_TYP_SCH_DIS $20. ;*/
/*format WA_RPS_ISL_1 9.2 ;*/
/*format WA_RPS_ISL_2 9.2 ;*/
/*format WA_RPS_ISL_3 9.2 ;*/
/*format WA_RPS_ISL_4 9.2 ;*/
/*format WA_RPS_ISL_5 9.2 ;*/
/*format WA_RPS_ISL_6 9.2 ;*/
/*format WA_RPS_ISL_7 9.2 ;*/
/*format WN_RPS_TRM_1 4. ;*/
/*format WN_RPS_TRM_2 4. ;*/
/*format WN_RPS_TRM_3 4. ;*/
/*format WN_RPS_TRM_4 4. ;*/
/*format WN_RPS_TRM_5 4. ;*/
/*format WN_RPS_TRM_6 4. ;*/
/*format WN_RPS_TRM_7 4. ;*/
/*format WN_RPS_TRM_INI 4. ;*/
/*format WA_1_DSB 10.2 ;*/
/*format WA_LON_TOT_DSB 10.2 ;*/
/*format WA_LON_TOT_INS_PRM 9.2 ;*/
/*format WA_LON_TOT_ORG_FEE 9.2 ;*/
/*format WA_LON_TOT_OTH_FEE 9.2 ;*/
/*format WA_ORG_PRI 10.2 ;*/
/*format WI_LON_FUL_DSB $1. ;*/
/*format LD_DSB MMDDYY10. ;*/
/*format WA_LST_DSB 10.2 ;*/
/*format LD_DFR_BEG MMDDYY10. ;*/
/*format LD_DFR_END MMDDYY10. ;*/
/*format WN_DAY_DFR_RMN 5. ;*/
/*format LC_DFR_TYP $2. ;*/
/*format WX_DFR_TYP $20. ;*/
/*format WN_MTH_IN_DFR 4. ;*/
/*format LD_FOR_BEG MMDDYY10. ;*/
/*format LD_FOR_END MMDDYY10. ;*/
/*format WN_DAY_FOR_RMN 5. ;*/
/*format LC_FOR_TYP $2. ;*/
/*format WX_FOR_TYP $20. ;*/
/*format WN_MTH_IN_FOR 4. ;*/
/*format WI_INI_FOR_APL $1. ;*/
/*format WN_SQ_FOR_APL 4. ;*/
/*format WD_SBM_PCL MMDDYY10. ;*/
/*if _n_ = 1 then        /* write column names */*/
/*do;*/
/*put*/
/*'BF_SSN'*/
/*','*/
/*'LN_SEQ'*/
/*','*/
/*'LD_END_GRC_PRD'*/
/*','*/
/*'LN_MTH_GRC_PRD_DSC'*/
/*','*/
/*'LD_TRM_END'*/
/*','*/
/*'LD_TRM_BEG'*/
/*','*/
/*'LF_GTR_RFR'*/
/*','*/
/*'LF_DOE_SCL_ORG'*/
/*','*/
/*'LC_SCY_PGA'*/
/*','*/
/*'WX_SCY_PGA'*/
/*','*/
/*'IC_LON_PGM'*/
/*','*/
/*'LF_RGL_CAT_LP06'*/
/*','*/
/*'IF_DOE_LDR'*/
/*','*/
/*'IF_GTR'*/
/*','*/
/*'LF_STU_SSN'*/
/*','*/
/*'LD_LON_1_DSB'*/
/*','*/
/*'LC_ACA_GDE_LEV'*/
/*','*/
/*'WX_ACA_GDE_LEV'*/
/*','*/
/*'LC_SCY_PGA_PGM_YR'*/
/*','*/
/*'WX_SCY_PGA_PGM_YR'*/
/*','*/
/*'IC_HSP_CSE'*/
/*','*/
/*'WX_HSP_CSE'*/
/*','*/
/*'IF_OWN'*/
/*','*/
/*'LD_LON_EFF_ADD'*/
/*','*/
/*'LA_R78_INT_MAX'*/
/*','*/
/*'WN_DAY_GRC_RMN'*/
/*','*/
/*'WN_DAY_ENR_ELP'*/
/*','*/
/*'WD_RPY_BEG'*/
/*','*/
/*'WN_DAY_RPD_ELP'*/
/*','*/
/*'WN_MTH_RPD_ELP'*/
/*','*/
/*'IM_PGA_SHO'*/
/*','*/
/*'IM_GTR_SHO'*/
/*','*/
/*'WA_CUR_PRI'*/
/*','*/
/*'WA_CUR_BR_INT'*/
/*','*/
/*'WA_CUR_GOV_INT'*/
/*','*/
/*'WA_CUR_OTH_CHR'*/
/*','*/
/*'WA_AVG_DAY_BAL'*/
/*','*/
/*'WA_PRV_MTH_PRI'*/
/*','*/
/*'WA_PRV_MTH_BR_INT'*/
/*','*/
/*'WA_PRV_MTH_GOV_INT'*/
/*','*/
/*'WA_PRV_MTH_OTH_CHR'*/
/*','*/
/*'WM_BR_1'*/
/*','*/
/*'WM_BR_MID'*/
/*','*/
/*'WM_BR_LST'*/
/*','*/
/*'WM_BR_LST_SFX'*/
/*','*/
/*'DD_BRT'*/
/*','*/
/*'WM_STU_1'*/
/*','*/
/*'WM_STU_MID'*/
/*','*/
/*'WM_STU_LST'*/
/*','*/
/*'WM_STU_LST_SFX'*/
/*','*/
/*'WX_STR_ADR_1'*/
/*','*/
/*'WX_STR_ADR_2'*/
/*','*/
/*'WX_STR_ADR_3'*/
/*','*/
/*'WM_CT'*/
/*','*/
/*'DC_DOM_ST'*/
/*','*/
/*'DF_ZIP_CDE'*/
/*','*/
/*'DI_VLD_ADR'*/
/*','*/
/*'DD_STA_PDEM30'*/
/*','*/
/*'DM_FGN_CNY'*/
/*','*/
/*'DM_FGN_ST'*/
/*','*/
/*'DN_PHN_XTN'*/
/*','*/
/*'DN_DOM_PHN_LCL'*/
/*','*/
/*'DN_DOM_PHN_XCH'*/
/*','*/
/*'DN_DOM_PHN_ARA'*/
/*','*/
/*'DN_FGN_PHN_INL'*/
/*','*/
/*'DN_FGN_PHN_CNY'*/
/*','*/
/*'DN_FGN_PHN_CT'*/
/*','*/
/*'DN_FGN_PHN_LCL'*/
/*','*/
/*'DI_PHN_VLD'*/
/*','*/
/*'DD_SKP_BEG'*/
/*','*/
/*'WI_LON_COS'*/
/*','*/
/*'WI_LON_CMK'*/
/*','*/
/*'WI_LON_CBR'*/
/*','*/
/*'WI_OTH_EDS_TYP'*/
/*','*/
/*'WM_DOE_SCL_ORG'*/
/*','*/
/*'WC_TYP_SCL_ORG'*/
/*','*/
/*'WX_TYP_SCL_ORG'*/
/*','*/
/*'WI_PPR_SCL_ORG'*/
/*','*/
/*'LF_DOE_SCL_ENR_CUR'*/
/*','*/
/*'LD_SCL_SPR'*/
/*','*/
/*'WN_DAY_GRC_ELP'*/
/*','*/
/*'WN_DAY_ENR_RMN'*/
/*','*/
/*'WC_TYP_SCL_CUR'*/
/*','*/
/*'WX_TYP_SCL_CUR'*/
/*','*/
/*'WI_PPR_SCL_CUR'*/
/*','*/
/*'IF_GTR_RPT_SCL'*/
/*','*/
/*'WC_SCL_CUR_DOM_ST'*/
/*','*/
/*'WC_SCL_ORG_DOM_ST'*/
/*','*/
/*'LD_RPS_1_PAY_DU'*/
/*','*/
/*'LD_SNT_RPD_DIS'*/
/*','*/
/*'LD_BIL_DU'*/
/*','*/
/*'WN_MTH_INT_CAP_FRQ'*/
/*','*/
/*'WX_INT_CAP_FRQ'*/
/*','*/
/*'LC_TYP_SCH_DIS'*/
/*','*/
/*'WX_TYP_SCH_DIS'*/
/*','*/
/*'WA_RPS_ISL_1'*/
/*','*/
/*'WA_RPS_ISL_2'*/
/*','*/
/*'WA_RPS_ISL_3'*/
/*','*/
/*'WA_RPS_ISL_4'*/
/*','*/
/*'WA_RPS_ISL_5'*/
/*','*/
/*'WA_RPS_ISL_6'*/
/*','*/
/*'WA_RPS_ISL_7'*/
/*','*/
/*'WN_RPS_TRM_1'*/
/*','*/
/*'WN_RPS_TRM_2'*/
/*','*/
/*'WN_RPS_TRM_3'*/
/*','*/
/*'WN_RPS_TRM_4'*/
/*','*/
/*'WN_RPS_TRM_5'*/
/*','*/
/*'WN_RPS_TRM_6'*/
/*','*/
/*'WN_RPS_TRM_7'*/
/*','*/
/*'WN_RPS_TRM_INI'*/
/*','*/
/*'WA_1_DSB'*/
/*','*/
/*'WA_LON_TOT_DSB'*/
/*','*/
/*'WA_LON_TOT_INS_PRM'*/
/*','*/
/*'WA_LON_TOT_ORG_FEE'*/
/*','*/
/*'WA_LON_TOT_OTH_FEE'*/
/*','*/
/*'WA_ORG_PRI'*/
/*','*/
/*'WI_LON_FUL_DSB'*/
/*','*/
/*'LD_DSB'*/
/*','*/
/*'WA_LST_DSB'*/
/*','*/
/*'LD_DFR_BEG'*/
/*','*/
/*'LD_DFR_END'*/
/*','*/
/*'WN_DAY_DFR_RMN'*/
/*','*/
/*'LC_DFR_TYP'*/
/*','*/
/*'WX_DFR_TYP'*/
/*','*/
/*'WN_MTH_IN_DFR'*/
/*','*/
/*'LD_FOR_BEG'*/
/*','*/
/*'LD_FOR_END'*/
/*','*/
/*'WN_DAY_FOR_RMN'*/
/*','*/
/*'LC_FOR_TYP'*/
/*','*/
/*'WX_FOR_TYP'*/
/*','*/
/*'WN_MTH_IN_FOR'*/
/*','*/
/*'WI_INI_FOR_APL'*/
/*','*/
/*'WN_SQ_FOR_APL'*/
/*','*/
/*'WD_SBM_PCL'*/
/*;*/
/*end;*/
/*do;*/
/*put BF_SSN $ @;*/
/*put LN_SEQ @;*/
/*put LD_END_GRC_PRD @;*/
/*put LN_MTH_GRC_PRD_DSC @;*/
/*put LD_TRM_END @;*/
/*put LD_TRM_BEG @;*/
/*put LF_GTR_RFR $ @;*/
/*put LF_DOE_SCL_ORG $ @;*/
/*put LC_SCY_PGA $ @;*/
/*put WX_SCY_PGA $ @;*/
/*put IC_LON_PGM $ @;*/
/*put LF_RGL_CAT_LP06 $ @;*/
/*put IF_DOE_LDR $ @;*/
/*put IF_GTR $ @;*/
/*put LF_STU_SSN $ @;*/
/*put LD_LON_1_DSB @;*/
/*put LC_ACA_GDE_LEV $ @;*/
/*put WX_ACA_GDE_LEV $ @;*/
/*put LC_SCY_PGA_PGM_YR $ @;*/
/*put WX_SCY_PGA_PGM_YR $ @;*/
/*put IC_HSP_CSE $ @;*/
/*put WX_HSP_CSE $ @;*/
/*put IF_OWN $ @;*/
/*put LD_LON_EFF_ADD @;*/
/*put LA_R78_INT_MAX @;*/
/*put WN_DAY_GRC_RMN @;*/
/*put WN_DAY_ENR_ELP @;*/
/*put WD_RPY_BEG @;*/
/*put WN_DAY_RPD_ELP @;*/
/*put WN_MTH_RPD_ELP @;*/
/*put IM_PGA_SHO $ @;*/
/*put IM_GTR_SHO $ @;*/
/*put WA_CUR_PRI @;*/
/*put WA_CUR_BR_INT @;*/
/*put WA_CUR_GOV_INT @;*/
/*put WA_CUR_OTH_CHR @;*/
/*put WA_AVG_DAY_BAL @;*/
/*put WA_PRV_MTH_PRI @;*/
/*put WA_PRV_MTH_BR_INT @;*/
/*put WA_PRV_MTH_GOV_INT @;*/
/*put WA_PRV_MTH_OTH_CHR @;*/
/*put WM_BR_1 $ @;*/
/*put WM_BR_MID $ @;*/
/*put WM_BR_LST $ @;*/
/*put WM_BR_LST_SFX $ @;*/
/*put DD_BRT @;*/
/*put WM_STU_1 $ @;*/
/*put WM_STU_MID $ @;*/
/*put WM_STU_LST $ @;*/
/*put WM_STU_LST_SFX $ @;*/
/*put WX_STR_ADR_1 $ @;*/
/*put WX_STR_ADR_2 $ @;*/
/*put WX_STR_ADR_3 $ @;*/
/*put WM_CT $ @;*/
/*put DC_DOM_ST $ @;*/
/*put DF_ZIP_CDE $ @;*/
/*put DI_VLD_ADR $ @;*/
/*put DD_STA_PDEM30 @;*/
/*put DM_FGN_CNY $ @;*/
/*put DM_FGN_ST $ @;*/
/*put DN_PHN_XTN $ @;*/
/*put DN_DOM_PHN_LCL $ @;*/
/*put DN_DOM_PHN_XCH $ @;*/
/*put DN_DOM_PHN_ARA $ @;*/
/*put DN_FGN_PHN_INL $ @;*/
/*put DN_FGN_PHN_CNY $ @;*/
/*put DN_FGN_PHN_CT $ @;*/
/*put DN_FGN_PHN_LCL $ @;*/
/*put DI_PHN_VLD $ @;*/
/*put DD_SKP_BEG @;*/
/*put WI_LON_COS $ @;*/
/*put WI_LON_CMK $ @;*/
/*put WI_LON_CBR $ @;*/
/*put WI_OTH_EDS_TYP $ @;*/
/*put WM_DOE_SCL_ORG $ @;*/
/*put WC_TYP_SCL_ORG $ @;*/
/*put WX_TYP_SCL_ORG $ @;*/
/*put WI_PPR_SCL_ORG $ @;*/
/*put LF_DOE_SCL_ENR_CUR $ @;*/
/*put LD_SCL_SPR @;*/
/*put WN_DAY_GRC_ELP @;*/
/*put WN_DAY_ENR_RMN @;*/
/*put WC_TYP_SCL_CUR $ @;*/
/*put WX_TYP_SCL_CUR $ @;*/
/*put WI_PPR_SCL_CUR $ @;*/
/*put IF_GTR_RPT_SCL $ @;*/
/*put WC_SCL_CUR_DOM_ST $ @;*/
/*put WC_SCL_ORG_DOM_ST $ @;*/
/*put LD_RPS_1_PAY_DU @;*/
/*put LD_SNT_RPD_DIS @;*/
/*put LD_BIL_DU @;*/
/*put WN_MTH_INT_CAP_FRQ @;*/
/*put WX_INT_CAP_FRQ $ @;*/
/*put LC_TYP_SCH_DIS $ @;*/
/*put WX_TYP_SCH_DIS $ @;*/
/*put WA_RPS_ISL_1 @;*/
/*put WA_RPS_ISL_2 @;*/
/*put WA_RPS_ISL_3 @;*/
/*put WA_RPS_ISL_4 @;*/
/*put WA_RPS_ISL_5 @;*/
/*put WA_RPS_ISL_6 @;*/
/*put WA_RPS_ISL_7 @;*/
/*put WN_RPS_TRM_1 @;*/
/*put WN_RPS_TRM_2 @;*/
/*put WN_RPS_TRM_3 @;*/
/*put WN_RPS_TRM_4 @;*/
/*put WN_RPS_TRM_5 @;*/
/*put WN_RPS_TRM_6 @;*/
/*put WN_RPS_TRM_7 @;*/
/*put WN_RPS_TRM_INI @;*/
/*put WA_1_DSB @;*/
/*put WA_LON_TOT_DSB @;*/
/*put WA_LON_TOT_INS_PRM @;*/
/*put WA_LON_TOT_ORG_FEE @;*/
/*put WA_LON_TOT_OTH_FEE @;*/
/*put WA_ORG_PRI @;*/
/*put WI_LON_FUL_DSB $ @;*/
/*put LD_DSB @;*/
/*put WA_LST_DSB @;*/
/*put LD_DFR_BEG @;*/
/*put LD_DFR_END @;*/
/*put WN_DAY_DFR_RMN @;*/
/*put LC_DFR_TYP $ @;*/
/*put WX_DFR_TYP $ @;*/
/*put WN_MTH_IN_DFR @;*/
/*put LD_FOR_BEG @;*/
/*put LD_FOR_END @;*/
/*put WN_DAY_FOR_RMN @;*/
/*put LC_FOR_TYP $ @;*/
/*put WX_FOR_TYP $ @;*/
/*put WN_MTH_IN_FOR @;*/
/*put WI_INI_FOR_APL $ @;*/
/*put WN_SQ_FOR_APL @;*/
/*put WD_SBM_PCL ;*/
/*;*/
/*end;*/
/*run;*/
