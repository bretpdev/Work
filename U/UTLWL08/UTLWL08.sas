/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWL08.LWL08R2";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	F.BF_SSN,
	RTRIM(F.DM_PRS_LST)||', '||RTRIM(F.DM_PRS_1)||' '||RTRIM(F.DM_PRS_MID)	AS DM_PRS_NAME,
	A.LN_SEQ,
	A.IC_LON_PGM,
	A.LA_CUR_PRI,
	B.WA_TOT_BRI_OTS,
	H.LF_LON_CUR_OWN,
	DAYS(CURRENT DATE) - DAYS(D.LD_DLQ_OCC) AS DAYS_DEL,
	CASE 
		WHEN D.LC_DLQ_TYP = 'P' THEN 'Installment'
		WHEN D.LC_DLQ_TYP = 'I' THEN 'Interest'
	END										AS DEL_TYP,
	I.LA_BIL_PAS_DU

FROM	OLWHRM1.LN10_LON A 
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
ON A.BF_SSN = B.BF_SSN
AND A.LN_SEQ = B.LN_SEQ
AND A.LC_STA_LON10 = 'R'
AND B.WC_DW_LON_STA IN ('22','23')
AND (A.LA_CUR_PRI > 0 OR B.WA_TOT_BRI_OTS > 0)
INNER JOIN OLWHRM1.RS10_BR_RPD C
ON A.BF_SSN = C.BF_SSN
INNER JOIN OLWHRM1.LN16_LON_DLQ_HST D
ON A.BF_SSN = D.BF_SSN
AND A.LN_SEQ = D.LN_SEQ
INNER JOIN OLWHRM1.MR01_MGT_RPT_LON F
ON A.BF_SSN = F.BF_SSN
AND A.LN_SEQ = F.LN_SEQ
INNER JOIN OLWHRM1.GR10_RPT_LON_APL H
ON A.BF_SSN = H.BF_SSN
AND A.LN_SEQ = H.LN_SEQ
INNER JOIN OLWHRM1.LN80_LON_BIL_CRF I
ON A.BF_SSN = I.BF_SSN
AND A.LN_SEQ = I.LN_SEQ

WHERE C.LC_STA_RPST10 = 'A'
OR (C.LC_STA_RPST10 <> 'A' AND D.LC_STA_LON16 = '1')

ORDER BY BF_SSN
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA DEMO;
SET WORKLOCL.DEMO;
RUN;

DATA _NULL_;
     CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;

/*PROC PRINTTO PRINT=REPORT2;
RUN;*/

OPTIONS CENTER NODATE NUMBER PAGENO=1;
OPTIONS PS=39 LS=127;
TITLE1 'PIF OR NON-FULLY ORIGINATED BORROWERS';
TITLE2 'WITH ACTIVE RS OR DELINQUENCY STATUS';
TITLE3 "&RUNDATE";

PROC CONTENTS DATA=DEMO OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 127 *'-';
	PUT      ////////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT ////////
		@57 '-- END OF REPORT --';
	PUT /////////////
		@46 "JOB = UTLWL08     REPORT = ULWL08.LWL08R2";
	;
	END;
RETURN;
run;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO WIDTH=UNIFORM WIDTH=MIN;
VAR BF_SSN
	DM_PRS_NAME
	LN_SEQ
	IC_LON_PGM
	LA_CUR_PRI
	WA_TOT_BRI_OTS
	LF_LON_CUR_OWN
	DAYS_DEL
	DEL_TYP
	LA_BIL_PAS_DU;

LABEL 	BF_SSN='BORROWER SSN' 
		DM_PRS_NAME='BORROWER NAME'
		LN_SEQ = 'LOAN SEQ'
		IC_LON_PGM = 'LOAN PROG'
		LA_CUR_PRI = 'CURR PRINC'
		WA_TOT_BRI_OTS = 'INTEREST'
		LF_LON_CUR_OWN = 'OWNER'
		DAYS_DEL = 'DAYS DEL'
		DEL_TYP = 'DEL TYPE'
		LA_BIL_PAS_DU = 'AMT PAST DUE';
FORMAT  LA_CUR_PRI DOLLAR12.
		WA_TOT_BRI_OTS DOLLAR12.
		LA_BIL_PAS_DU DOLLAR12.;
FOOTNOTE  'JOB = UTLWL08     REPORT = ULWL08.LWL08R2';
RUN;