/*UTLWQ12 - BORROWER ACCOUNTS IN WORKGROUP 15*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWQ12.LWQ12R2";
libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE WG15A AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	B.DF_SPE_ACC_ID
	,A.LD_DFL_LST_ATT_CNC
	,A.LF_DFL_CLR

	,SUM(
            A.LA_CLM_PRI + 
            A.LA_CLM_INT - 
            A.LA_PRI_COL +
            A.LA_INT_ACR +
            C.LA_CLM_INT_ACR -
            A.LA_INT_COL
            ) + SUM(    A.LA_LEG_CST_ACR -
                        A.LA_LEG_CST_COL +
                        A.LA_OTH_CHR_ACR -
                        A.LA_OTH_CHR_COL +
                        A.LA_COL_CST_ACR -
                        A.LA_COL_CST_COL +
                        C.LA_CLM_PRJ_COL_CST
                        ) AS TOTBL

FROM OLWHRM1.DC01_LON_CLM_INF A 
INNER JOIN OLWHRM1.PD01_PDM_INF B
	ON A.BF_SSN = B.DF_PRS_ID
INNER JOIN OLWHRM1.DC02_BAL_INT C
	ON A.BF_SSN = C.BF_SSN
	AND A.AF_APL_ID = C.AF_APL_ID
	AND A.AF_APL_ID_SFX = C.AF_APL_ID_SFX
	AND A.LF_CRT_DTS_DC10 = C.LF_CRT_DTS_DC10
WHERE A.LD_CLM_ASN_DOE IS NULL
AND A.LC_STA_DC10 = '03'
AND A.LF_DFL_CLR = 'WG000015'
GROUP BY B.DF_SPE_ACC_ID,A.LD_DFL_LST_ATT_CNC,A.LF_DFL_CLR
);
DISCONNECT FROM DB2;

PROC SQL;
CREATE TABLE WG15 AS
SELECT *
FROM WG15A
WHERE TOTBL > 25;
QUIT;

ENDRSUBMIT;
DATA WG15; 
SET WORKLOCL.WG15; 
RUN;

PROC SORT DATA=WG15;
BY DF_SPE_ACC_ID;
RUN;

DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;

DATA _NULL_;
	SET WG15 ;
	LENGTH DESCRIPTION $600.;

	USER = LF_DFL_CLR;
	ACT_DT = LD_DFL_LST_ATT_CNC;
	DESCRIPTION = CATX(',',DF_SPE_ACC_ID,TOTBL);

	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT USER $10. ;
	FORMAT ACT_DT MMDDYY10. ;
	FORMAT DESCRIPTION $600. ;
	IF _N_ = 1 THEN DO;
		PUT "USER,ACT_DT,DESCRIPTION";
	END;
	DO;
	   PUT USER $ @;
	   PUT ACT_DT @;
	   PUT DESCRIPTION $ ;
	END;
RUN;

