/****UTLWD13 DELINQUENT LONAS WITH NO REFERENCES****/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWD13.LWD13R2";
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DLWNRT AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT 	 B.BF_SSN AS SSN
		,RTRIM(D.DM_PRS_LST)||' '||RTRIM(D.DM_PRS_1)||' '||D.DM_PRS_MID AS NAME
		,B.LN_SEQ
		,A.LD_DLQ_OCC
		,C.BC_STA_REFR10
		,C.BC_RFR_REL_BR
		,RTRIM(E.LF_LON_ALT)||''|| CHAR(E.LN_LON_ALT_SEQ) AS UID
		,D.DF_SPE_ACC_ID
		
FROM	  OLWHRM1.LN16_LON_DLQ_HST A
		INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
         ON A.BF_SSN = B.BF_SSN
		 AND A.LN_SEQ = B.LN_SEQ
		LEFT OUTER JOIN OLWHRM1.RF10_RFR C
		 ON A.BF_SSN = C.BF_SSN
		INNER JOIN OLWHRM1.PD10_PRS_NME D
		 ON B.BF_SSN = D.DF_PRS_ID
		INNER JOIN OLWHRM1.LN10_LON E
		 ON B.BF_SSN = E.BF_SSN
		 AND B.LN_SEQ = E.LN_SEQ

WHERE	DAYS(A.LD_DLQ_OCC) BETWEEN DAYS(CURRENT DATE) - 55 AND DAYS(CURRENT DATE) - 45
AND 	E.LC_STA_LON10 = 'R'
AND 	A.LC_STA_LON16 = '1'
AND 	E.LA_CUR_PRI > 0
AND	(SELECT COUNT(*)
	FROM OLWHRM1.RF10_RFR X
	WHERE X.BF_SSN = A.BF_SSN
	AND (X.BC_STA_REFR10 ='A'/*ACTIVE REFERENCE*/
		 OR (X.BC_STA_REFR10 = 'H' AND X.BC_REA_RFR_HST = 'R'))/*VALID REFERENCE W/ NO CONTACT REQUEST*/
	AND X.BC_RFR_REL_BR NOT IN ('05','13','15')) < 2 /*NOT EMPLOYER, PHYSICIAN, LAWYER*/
ORDER BY A.BF_SSN
);
DISCONNECT FROM DB2;
ENDRSUBMIT;
DATA DLWNRT;
SET WORKLOCL.DLWNRT;
RUN;
DATA DLWNRT;
SET DLWNRT;
DDAYS = TODAY() - LD_DLQ_OCC;
RUN;
PROC SORT DATA=DLWNRT OUT=DLWNR NODUPKEY;
BY 	SSN LN_SEQ;
RUN;
PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS PAGENO=1 LS=126 ;
PROC PRINT NOOBS SPLIT='/' DATA=DLWNR  WIDTH=MIN;
VAR DF_SPE_ACC_ID
	NAME
	LN_SEQ
	DDAYS
	UID;
LABEL DF_SPE_ACC_ID = 'ACCT #'
	LN_SEQ = 'LOAN SEQUENCE NUMBER'
	DDAYS = 'DAYS DELINQUENT'
	UID = 'UNIQUE ID';
TITLE	"DELINQUENT LOANS WITH NO REFERENCES";
FOOTNOTE  'JOB = UTLWD13     REPORT = ULWD13.LWD13R2';
RUN;
PROC PRINTTO;
RUN;