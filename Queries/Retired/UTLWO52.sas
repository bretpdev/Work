*--------------------------------*
| UTLWO52 LOANS TO BE SERIALIZED |
*--------------------------------*;
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORT2 "&RPTLIB/ULWO52.LWO52R2";
FILENAME REPORTZ "&RPTLIB/ULWO52.LWO52RZ";
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/
%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	DATA _NULL_;
    FILE REPORTZ NOTITLES;
    PUT @01 " ********************************************************************* "
      / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
      / @01 " ****  THE SAS LOG IN &SQLRPT SHOULD BE REVIEWED.          **** "       
      / @01 " ********************************************************************* "
      / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
      / @01 " ****  &SQLXMSG   **** "
      / @01 " ********************************************************************* ";
	RUN;
%END;
%MEND;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE L2BCER AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.DF_SPE_ACC_ID
	,PNDG.AC_APL_TYP
	,PNDG.AF_APL_OPS_SCL
	,PNDG.AF_CUR_APL_OPS_LDR
	,PNDG.AD_APL_CRT
/*	,APRVD.AF_CUR_APL_OPS_LDR AS AF_CUR*/
/*	,APRVD.LON_TYP*/
/*	,APRVD.DF_PRS_ID_STU AS ASTU*/
/*	,PNDG.DF_PRS_ID_STU AS PSTU*/
FROM OLWHRM1.PD01_PDM_INF A
INNER JOIN (
	SELECT B.DF_PRS_ID_BR
		,B.DF_PRS_ID_STU
		,B.AC_APL_TYP
		,B.AF_APL_OPS_SCL
		,B.AF_CUR_APL_OPS_LDR
		,B.AD_APL_CRT 
		,CASE
			WHEN C.AC_LON_TYP IN ('SF','SU') THEN 'STAF'
			ELSE C.AC_LON_TYP
		 END AS LON_TYP
	FROM OLWHRM1.GA01_APP B
	INNER JOIN OLWHRM1.GA10_LON_APP C
		ON B.AF_APL_ID = C.AF_APL_ID
		AND C.AC_PRC_STA = 'P'
		AND B.AF_CUR_APL_OPS_LDR <> '829505'
	) PNDG
	ON A.DF_PRS_ID = PNDG.DF_PRS_ID_BR
INNER JOIN (
	SELECT X.DF_PRS_ID_BR
		,X.AF_CUR_APL_OPS_LDR
		,X.DF_PRS_ID_STU
		,CASE
			WHEN Y.AC_LON_TYP IN ('SF','SU') THEN 'STAF'
			ELSE Y.AC_LON_TYP
		 END AS LON_TYP
	FROM OLWHRM1.GA01_APP X
	INNER JOIN OLWHRM1.GA10_LON_APP Y
		ON X.AF_APL_ID = Y.AF_APL_ID
	INNER JOIN OLWHRM1.GA40_BS_MPN_CTL E
		ON Y.AF_APL_ID = E.AF_BS_MPN_APL_ID
	WHERE Y.AC_PRC_STA = 'A'
	AND X.AF_CUR_APL_OPS_LDR <> '829505'
	AND E.AC_MPN_STA = 'A'
	) APRVD
	ON PNDG.DF_PRS_ID_BR = APRVD.DF_PRS_ID_BR
	AND PNDG.LON_TYP = APRVD.LON_TYP
WHERE 
(
	PNDG.AF_CUR_APL_OPS_LDR = APRVD.AF_CUR_APL_OPS_LDR OR
	PNDG.AF_CUR_APL_OPS_LDR = ''
)
	AND (PNDG.LON_TYP <> 'PL' OR PNDG.DF_PRS_ID_STU = APRVD.DF_PRS_ID_STU)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;
%SQLCHECK (SQLRPT=ULWO52.LWO52RZ);
QUIT;
/*ENDRSUBMIT;*/
/**/
/*DATA L2BCER;*/
/*SET WORKLOCL.L2BCER;*/
/*RUN;*/

PROC FORMAT;
VALUE $LNTYP	'S' = 'STAFFORD'
				'P' = 'PLUS/PLUSGB'
				'C' = 'CONSOLIDATION'
				'A' = 'SLS';
QUIT;

PROC SORT DATA=L2BCER;
BY AF_APL_OPS_SCL AD_APL_CRT ;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1;
TITLE 'LOANS POTENTIALLY NEEDING SERIALIZATION';
FOOTNOTE 'JOB = UTLWO52  	 REPORT = ULWO52.LWO52R2';
PROC CONTENTS DATA=L2BCER OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 126*'-';
	PUT      ////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT //////
		@57 '-- END OF REPORT --';
	PUT ////////////
		@46 "JOB = UTLWO52  	 REPORT = ULWO52.LWO52R2";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=L2BCER WIDTH=UNIFORM WIDTH=MIN;
FORMAT AC_APL_TYP $LNTYP. AD_APL_CRT MMDDYY10.;
VAR DF_SPE_ACC_ID
	AC_APL_TYP
	AF_APL_OPS_SCL
	AF_CUR_APL_OPS_LDR
	AD_APL_CRT 
	;
LABEL DF_SPE_ACC_ID = 'ACCOUNT #'
	AC_APL_TYP = 'LOAN TYPE'
	AF_APL_OPS_SCL = 'SCHOOL CODE'
	AF_CUR_APL_OPS_LDR = 'LENDER CODE'
	AD_APL_CRT = 'DATE APP CREATED'
	;
RUN;

PROC PRINTTO;
RUN;