/*LPP SERVICING DATA FILE FOR FINANCE - MR5B TABLE*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*THIS DIRECTORY HAS BEEN HARD CODED AT THE REQUEST OF AES*/
/*%LET RPTLIB = /sas/whse/progrevw ;*/
/*FILENAME REPORT2 "/sas/whse/progrevw/ULWE12.LWE12R2";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE LPPFMR5B AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.IF_GTR, 
		A.IC_LON_PGM, 
		A.IF_OWN, 
		A.IF_BND_ISS, 
		A.BF_SSN, 
		A.LN_SEQ, 
		A.LD_TRM_END, 
		A.LD_TRM_BEG, 
		A.LF_GTR_RFR, 
		A.IF_DOE_LDR, 
		A.LC_ACA_GDE_LEV, 
		A.LD_LON_EFF_ADD, 
		A.DD_BRT, 
		A.WM_STU_1, 
		A.WM_STU_MID, 
		A.WM_STU_LST, 
		A.WM_STU_LST_SFX, 
		A.WX_STR_ADR_1, 
		A.WX_STR_ADR_2,
		A.WX_STR_ADR_3, 
		A.WM_CT, 
		A.DC_DOM_ST, 
		A.DF_ZIP_CDE, 
		A.DI_VLD_ADR, 
		A.DD_STA_PDEM30, 
		A.DM_FGN_CNY, 
		A.DM_FGN_ST, 
		A.DN_PHN_XTN, 
		A.DN_DOM_PHN_LCL, 
		A.DN_DOM_PHN_XCH, 
		A.DN_DOM_PHN_ARA, 
		A.DN_FGN_PHN_INL, 
		A.DN_FGN_PHN_CNY, 
		A.DN_FGN_PHN_CT,
		A.DN_FGN_PHN_LCL, 
		A.DI_PHN_VLD, 
		A.WI_LON_COS, 
		A.WI_LON_CMK, 
		A.WI_LON_CBR,
		A.WI_OTH_EDS_TYP, 
		A.WX_TYP_SCL_ORG, 
		A.LF_DOE_SCL_ENR_CUR, 
		A.WC_TYP_SCL_CUR, 
		A.WX_TYP_SCL_CUR, 
		A.LD_RPS_1_PAY_DU, 
		A.LD_SNT_RPD_DIS, 
		A.LD_BIL_DU, 
		A.WN_MTH_INT_CAP_FRQ, 
		A.WA_RPS_ISL_1, 
		A.WA_RPS_ISL_2, 
		A.WA_RPS_ISL_3, 
		A.WA_RPS_ISL_4, 
		A.WA_RPS_ISL_5, 
		A.WA_RPS_ISL_6, 
		A.WA_RPS_ISL_7, 
		A.WN_RPS_TRM_1, 
		A.WN_RPS_TRM_2, 
		A.WN_RPS_TRM_3, 
		A.WN_RPS_TRM_4, 
		A.WN_RPS_TRM_5, 
		A.WN_RPS_TRM_6, 
		A.WN_RPS_TRM_7, 
		A.WN_RPS_TRM_INI, 
		A.WA_1_DSB, 
		A.WA_LON_TOT_DSB, 
		A.WA_LON_TOT_INS_PRM, 
		A.WA_LON_TOT_ORG_FEE, 
		A.WA_LON_TOT_OTH_FEE, 
		A.WA_ORG_PRI, 
		A.WI_LON_FUL_DSB, 
		A.LD_DSB, 
		A.WA_LST_DSB, 
		A.WN_DAY_FOR_RMN, 
		A.WD_SBM_PCL, 
		A.WD_INI_CLM_SBM, 
		A.LD_CLM_REJ_RTN_ACL, 
		A.WD_INI_CLM_PD, 
		A.WA_INI_CLM_INT_PD, 
		A.WA_INI_CLM_PRI_PD, 
		A.WD_RS_INI_CLM, 
		A.WD_SUP_CLM_SBM, 
		A.WD_SUP_CLM_PD, 
		A.WA_SUP_CLM_INT_PD, 
		A.WA_SUP_CLM_PRI_PD, 
		A.WA_FAT_NSI_AT_PR, 
		A.WA_PRI_AT_PR, 
		A.WN_DAY_RPD_AFT_CVN, 
		A.WD_FAT_APL_LST_CAP, 
		A.WA_FAT_NSI_LST_CAP, 
		A.WA_TOT_INT_CAP, 
		A.WN_DAY_INT_CAP, 
		A.WA_INT_WOF, 
		A.WA_PRI_WOF, 
		A.WD_LST_BR_PAY, 
		A.WA_LST_PRI_PAY, 
		A.WA_LST_INT_PAY, 
		A.WD_ZRO_BAL_APL, 
		A.WD_ZRO_BAL_EFF, 
		A.WD_DLQ_DCO_ISL, 
		A.WD_DLQ_DCO_INT, 
		A.WN_RPS_TRM_RMN, 
		A.WA_CUR_ISL, 
		A.II_TX_BND, 
		A.IF_SLL_OWN, 
		A.WN_PAY_RPD, 
		A.LA_LON_AMT_GTR, 
		A.LD_LON_GTR, 
		A.DC_ADR_EML, 
		A.DD_VER_ADR_EML, 
		A.DI_VLD_ADR_EML, 
		A.WX_ADR_EML, 
		A.LC_MPN_TYP, 
		A.LD_MPN_EXP, 
		A.LC_MPN_SRL_LON, 
		A.LC_MPN_REV_REA, 
		A.LF_ORG_RGN, 
		A.AF_LON_ALT, 
		A.AN_SEQ_COM_LN_APL
FROM	OLWHRM1.MR5B_MR_LON_MTH_02 A

);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA LPPFMR5B;
SET WORKLOCL.LPPFMR5B;
RUN;

PROC SORT DATA=LPPFMR5B;
BY BF_SSN LN_SEQ;
RUN;

data _null_;
set  WORK.Lppfmr5b;
file 'T:\SAS\ULWE12.LWE12R2' delimiter=',' DSD DROPOVER LRECL=32767;
*FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
format IF_GTR $6. ;
format IC_LON_PGM $6. ;
format IF_OWN $8. ;
format IF_BND_ISS $8. ;
format BF_SSN $9. ;
format LN_SEQ 6. ;
format LD_TRM_END MMDDYY10. ;
format LD_TRM_BEG MMDDYY10. ;
format LF_GTR_RFR $12. ;
format IF_DOE_LDR $8. ;
format LC_ACA_GDE_LEV $2. ;
format LD_LON_EFF_ADD MMDDYY10. ;
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
format WI_LON_COS $1. ;
format WI_LON_CMK $1. ;
format WI_LON_CBR $1. ;
format WI_OTH_EDS_TYP $1. ;
format WX_TYP_SCL_ORG $20. ;
format LF_DOE_SCL_ENR_CUR $8. ;
format WC_TYP_SCL_CUR $2. ;
format WX_TYP_SCL_CUR $20. ;
format LD_RPS_1_PAY_DU MMDDYY10. ;
format LD_SNT_RPD_DIS MMDDYY10. ;
format LD_BIL_DU MMDDYY10. ;
format WN_MTH_INT_CAP_FRQ 3. ;
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
format WN_DAY_FOR_RMN 5. ;
format WD_SBM_PCL MMDDYY10. ;
format WD_INI_CLM_SBM MMDDYY10. ;
format LD_CLM_REJ_RTN_ACL MMDDYY10. ;
format WD_INI_CLM_PD MMDDYY10. ;
format WA_INI_CLM_INT_PD 9.2 ;
format WA_INI_CLM_PRI_PD 10.2 ;
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
format WD_ZRO_BAL_APL MMDDYY10. ;
format WD_ZRO_BAL_EFF MMDDYY10. ;
format WD_DLQ_DCO_ISL MMDDYY10. ;
format WD_DLQ_DCO_INT MMDDYY10. ;
format WN_RPS_TRM_RMN 4. ;
format WA_CUR_ISL 9.2 ;
format II_TX_BND $1. ;
format IF_SLL_OWN $8. ;
format WN_PAY_RPD 4. ;
format LA_LON_AMT_GTR 10.2 ;
format LD_LON_GTR MMDDYY10. ;
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
if _n_ = 1 then        /* write column names */
do;
put
'IF_GTR'
','
'IC_LON_PGM'
','
'IF_OWN'
','
'IF_BND_ISS'
','
'BF_SSN'
','
'LN_SEQ'
','
'LD_TRM_END'
','
'LD_TRM_BEG'
','
'LF_GTR_RFR'
','
'IF_DOE_LDR'
','
'LC_ACA_GDE_LEV'
','
'LD_LON_EFF_ADD'
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
'WI_LON_COS'
','
'WI_LON_CMK'
','
'WI_LON_CBR'
','
'WI_OTH_EDS_TYP'
','
'WX_TYP_SCL_ORG'
','
'LF_DOE_SCL_ENR_CUR'
','
'WC_TYP_SCL_CUR'
','
'WX_TYP_SCL_CUR'
','
'LD_RPS_1_PAY_DU'
','
'LD_SNT_RPD_DIS'
','
'LD_BIL_DU'
','
'WN_MTH_INT_CAP_FRQ'
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
'WN_DAY_FOR_RMN'
','
'WD_SBM_PCL'
','
'WD_INI_CLM_SBM'
','
'LD_CLM_REJ_RTN_ACL'
','
'WD_INI_CLM_PD'
','
'WA_INI_CLM_INT_PD'
','
'WA_INI_CLM_PRI_PD'
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
'WD_ZRO_BAL_APL'
','
'WD_ZRO_BAL_EFF'
','
'WD_DLQ_DCO_ISL'
','
'WD_DLQ_DCO_INT'
','
'WN_RPS_TRM_RMN'
','
'WA_CUR_ISL'
','
'II_TX_BND'
','
'IF_SLL_OWN'
','
'WN_PAY_RPD'
','
'LA_LON_AMT_GTR'
','
'LD_LON_GTR'
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
;
end;
do;
put IF_GTR $ @;
put IC_LON_PGM $ @;
put IF_OWN $ @;
put IF_BND_ISS $ @;
put BF_SSN $ @;
put LN_SEQ @;
put LD_TRM_END @;
put LD_TRM_BEG @;
put LF_GTR_RFR $ @;
put IF_DOE_LDR $ @;
put LC_ACA_GDE_LEV $ @;
put LD_LON_EFF_ADD @;
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
put WI_LON_COS $ @;
put WI_LON_CMK $ @;
put WI_LON_CBR $ @;
put WI_OTH_EDS_TYP $ @;
put WX_TYP_SCL_ORG $ @;
put LF_DOE_SCL_ENR_CUR $ @;
put WC_TYP_SCL_CUR $ @;
put WX_TYP_SCL_CUR $ @;
put LD_RPS_1_PAY_DU @;
put LD_SNT_RPD_DIS @;
put LD_BIL_DU @;
put WN_MTH_INT_CAP_FRQ @;
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
put WN_DAY_FOR_RMN @;
put WD_SBM_PCL @;
put WD_INI_CLM_SBM @;
put LD_CLM_REJ_RTN_ACL @;
put WD_INI_CLM_PD @;
put WA_INI_CLM_INT_PD @;
put WA_INI_CLM_PRI_PD @;
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
put WD_ZRO_BAL_APL @;
put WD_ZRO_BAL_EFF @;
put WD_DLQ_DCO_ISL @;
put WD_DLQ_DCO_INT @;
put WN_RPS_TRM_RMN @;
put WA_CUR_ISL @;
put II_TX_BND $ @;
put IF_SLL_OWN $ @;
put WN_PAY_RPD @;
put LA_LON_AMT_GTR @;
put LD_LON_GTR @;
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
put AN_SEQ_COM_LN_APL ;
;
end;
run;


