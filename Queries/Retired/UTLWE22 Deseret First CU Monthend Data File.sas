/*UTLWE22 - MONTH-END DATA FILE FOR DESERET FIRST CU*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*FILENAME REPORT2 "&RPTLIB/ULWE22.LWE22R2";*/

FILENAME REPORT2 'T:\SAS\UTLWE22.txt';
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.BF_SSN
	,A.WD_RPY_BEG
	,A.WA_CUR_PRI
	,A.WA_CUR_BR_INT
	,A.WA_CUR_GOV_INT
	,A.WM_BR_1
	,A.WM_BR_MID
	,A.WM_BR_LST
	,B.WM_STU_1
	,B.WM_STU_MID
	,B.WM_STU_LST
	,B.WM_STU_LST_SFX
	,B.WX_STR_ADR_1
	,B.WX_STR_ADR_2
	,B.WX_STR_ADR_3
	,B.WM_CT
	,B.DC_DOM_ST
	,B.DF_ZIP_CDE
	,A.LD_SCL_SPR
	,B.LA_LON_AMT_GTR
	,B.LD_LON_GTR
	,A.WR_ITR_1
	,D.LD_DSB_CAN
	,D.LA_DSB_CAN
	,D.LD_DSB
	,D.LA_DSB
	,D.WX_DSB_FEE_1
	,D.LA_DSB_FEE_1
	,D.WX_DSB_FEE_2
	,D.LA_DSB_FEE_2
	,B.WD_LST_BR_PAY
	,B.WA_LST_PRI_PAY
	,B.WA_LST_INT_PAY
	,B.AF_LON_ALT
	,B.AN_SEQ_COM_LN_APL
FROM	OLWHRM1.MR5A_MR_LON_MTH_01 A
		LEFT OUTER JOIN OLWHRM1.MR5B_MR_LON_MTH_02 B ON
			A.BF_SSN = B.BF_SSN
			AND A.LN_SEQ = B.LN_SEQ
		LEFT OUTER JOIN OLWHRM1.MR52_MR_DSB_MTH D ON
			A.BF_SSN = D.BF_SSN
			AND A.LN_SEQ = D.LN_SEQ		
WHERE	A.IF_OWN = '833828'
		AND A.WA_CUR_PRI > 0
ORDER BY A.BF_SSN
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA DEMO;
SET WORKLOCL.DEMO;
RUN;

DATA _NULL_;
SET DEMO;
%let _EFIERR_ = 0; /* set the ERROR detection macro variable */
%let _EFIREC_ = 0;     /* clear export record count macro variable */
FILE REPORT2 DELIMITER=',' DSD LRECL=32767;

FORMAT BF_SSN $9. ;
FORMAT WD_RPY_BEG YYMMDDD10. ;
FORMAT WA_CUR_PRI DOLLAR10.2 ;
FORMAT WA_CUR_BR_INT DOLLAR10.2 ;
FORMAT WA_CUR_GOV_INT DOLLAR10.2 ;
FORMAT WM_BR_1 $13. ;
FORMAT WM_BR_MID $13. ;
FORMAT WM_BR_LST $23. ;
FORMAT WM_STU_1 $13. ;
FORMAT WM_STU_MID $13. ;
FORMAT WM_STU_LST $23. ;
FORMAT WX_STR_ADR_1 $30. ;
FORMAT WX_STR_ADR_2 $30. ;
FORMAT WX_STR_ADR_3 $30. ;
FORMAT WM_CT $20. ;
FORMAT DC_DOM_ST $2. ;
FORMAT DF_ZIP_CDE $17. ;
FORMAT LD_SCL_SPR YYMMDDD10. ;
FORMAT LA_LON_AMT_GTR DOLLAR10.2 ;
FORMAT LD_LON_GTR YYMMDDD10. ;
FORMAT WR_ITR_1 8.3;
FORMAT LD_DSB_CAN YYMMDDD10. ;
FORMAT LA_DSB_CAN DOLLAR10.2 ;
FORMAT LD_DSB YYMMDDD10. ;
FORMAT LA_DSB DOLLAR10.2 ;
FORMAT WX_DSB_FEE_1 $20. ;
FORMAT LA_DSB_FEE_1 DOLLAR10.2 ;
FORMAT WX_DSB_FEE_2 $20. ;
FORMAT LA_DSB_FEE_2 DOLLAR10.2 ;
FORMAT WD_LST_BR_PAY YYMMDDD10. ;
FORMAT WA_LST_PRI_PAY DOLLAR10.2 ;
FORMAT WA_LST_INT_PAY DOLLAR10.2 ;

IF _N_ = 1 THEN DO;       /* WRITE COLUMN NAMES */
	PUT
	'BF_SSN'			','
	'WD_RPY_BEG'		','
	'WA_CUR_PRI'		','
	'WA_CUR_BR_INT'		','
	'WA_CUR_GOV_INT'	','
	'WM_BR_1'			','
	'WM_BR_MID'			','
	'WM_BR_LST'			','
	'WM_STU_1'			','
	'WM_STU_MID'		','
	'WM_STU_LST'		','
	'WX_STR_ADR_1'		','
	'WX_STR_ADR_2'		','
	'WX_STR_ADR_3'		','
	'WM_CT'				','
	'DC_DOM_ST'			','
	'DF_ZIP_CDE'		','
	'LD_SCL_SPR'		','
	'LA_LON_AMT_GTR'	','
	'LD_LON_GTR'		','
	'WR_ITR_1'			','
	'LD_DSB_CAN'		','
	'LA_DSB_CAN'		','
	'LD_DSB'			','
	'LA_DSB'			','
	'WX_DSB_FEE_1'		','
	'LA_DSB_FEE_1'		','
	'WX_DSB_FEE_2'		','
	'LA_DSB_FEE_2'		','
	'WD_LST_BR_PAY'		','
	'WA_LST_PRI_PAY'	','
	'WA_LST_INT_PAY'
	;
	END;
DO;
	PUT BF_SSN $ @;
	PUT WD_RPY_BEG $ @;
	PUT WA_CUR_PRI $ @;
	PUT WA_CUR_BR_INT $ @;
	PUT WA_CUR_GOV_INT $ @;
	PUT WM_BR_1 $ @;
	PUT WM_BR_MID $ @;
	PUT WM_BR_LST $ @;
	PUT WM_STU_1 $ @;
	PUT WM_STU_MID $ @;
	PUT WM_STU_LST $ @;
	PUT WX_STR_ADR_1 $ @;
	PUT WX_STR_ADR_2 $ @;
	PUT WX_STR_ADR_3 $ @;
	PUT WM_CT $ @;
	PUT DC_DOM_ST $ @;
	PUT DF_ZIP_CDE $ @;
	PUT LD_SCL_SPR $ @;
	PUT LA_LON_AMT_GTR $ @;
	PUT LD_LON_GTR $ @;
	PUT WR_ITR_1 $ @;
	PUT LD_DSB_CAN $ @;
	PUT LA_DSB_CAN $ @;
	PUT LD_DSB $ @;
	PUT LA_DSB $ @;
	PUT WX_DSB_FEE_1 $ @;
	PUT LA_DSB_FEE_1 $ @;
	PUT WX_DSB_FEE_2 $ @;
	PUT LA_DSB_FEE_2 $ @;
	PUT WD_LST_BR_PAY $ @;
	PUT WA_LST_PRI_PAY $ @;
	PUT WA_LST_INT_PAY $ ;
	END;
if _ERROR_ then call symput('_EFIERR_',1);  /* set ERROR detection macro variable */
If EFIEOD then
   call symput('_EFIREC_',EFIOUT);
RUN;