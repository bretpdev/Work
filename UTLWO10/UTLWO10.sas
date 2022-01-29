/*UTLWO10 - BORROWERS WITH NO REPAYMENT SCHEDULE AT GRACE END*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWO10.LWO10R2";
libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE NORS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.LF_LON_CUR_OWN
	,INTEGER(A.BF_SSN) AS BF_SSN
	,RTRIM(E.DM_PRS_LST)||', '||RTRIM(E.DM_PRS_1)||' '||DM_PRS_MID AS NAME
	,A.LN_SEQ
	,A.LA_CUR_PRI
	,D.LD_SCL_SPR
	,A.LD_END_GRC_PRD
	,E.DF_SPE_ACC_ID
FROM  OLWHRM1.LN10_LON A 
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.LN13_LON_STU_OSD C
	ON A.BF_SSN = C.BF_SSN
	AND A.LN_SEQ = C.LN_SEQ
	AND C.LC_STA_LON13 = 'A'
INNER JOIN OLWHRM1.SD10_STU_SPR D
	ON D.LF_STU_SSN = C.LF_STU_SSN
	AND D.LN_STU_SPR_SEQ = C.LN_STU_SPR_SEQ
	AND D.LC_STA_STU10 = 'A'
INNER JOIN OLWHRM1.PD10_PRS_NME E
	ON A.BF_SSN = E.DF_PRS_ID

WHERE A.LC_STA_LON10 = 'R'
AND A.LA_CUR_PRI BETWEEN 0.01 AND 49.99
AND B.WC_DW_LON_STA NOT IN ('06','07','08','11','12','16','17','18','19','20','21','22')
AND DAYS(A.LD_END_GRC_PRD) <= DAYS(CURRENT DATE) + 30
AND ((NOT EXISTS
		(SELECT *
		FROM OLWHRM1.LN65_LON_RPS X
		INNER JOIN OLWHRM1.RS10_BR_RPD Y
		ON X.BF_SSN = Y.BF_SSN
		AND X.LN_RPS_SEQ = Y.LN_RPS_SEQ
		WHERE X.LC_STA_LON65 = 'A'
		AND Y.LC_STA_RPST10 = 'A'
		AND X.BF_SSN = A.BF_SSN
		AND X.LN_SEQ = A.LN_SEQ)
	)
	OR
	(NOT EXISTS
		(SELECT *
		FROM OLWHRM1.LN66_LON_RPS_SPF X
		WHERE X.LA_RPS_ISL IS NOT NULL
		AND X.BF_SSN = A.BF_SSN
		AND X.LN_SEQ = A.LN_SEQ)
	))
);
DISCONNECT FROM DB2;
endrsubmit  ;

DATA NORS; 
SET WORKLOCL.NORS; 
RUN;

PROC SORT DATA=NORS;
BY NAME LN_SEQ;
RUN;

DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER NODATE NUMBER PAGENO=1 LS=132;
TITLE "NO REPAYMENT SCHEDULE AT GRACE END";
TITLE2 "&RUNDATE";
FOOTNOTE  'JOB = UTLWO10     REPORT = ULW010.LWO10R2';
PROC CONTENTS DATA=NORS OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 132*'-';
	PUT      ////////
		@54 '**** NO OBSERVATIONS FOUND ****';
	PUT ////////
		@60 '-- END OF REPORT --';
	PUT ////////////////
		@49 "JOB = UTLWO10     REPORT = ULW010.LWO10R2";
	END;
RETURN;
RUN;
PROC PRINT DATA=NORS NOOBS SPLIT='/' WIDTH=UNIFORM WIDTH=MIN;  
VAR LF_LON_CUR_OWN DF_SPE_ACC_ID NAME LN_SEQ LA_CUR_PRI LD_SCL_SPR LD_END_GRC_PRD;  
LABEL LF_LON_CUR_OWN="OWNER" DF_SPE_ACC_ID='ACCT #' LN_SEQ='LOAN SEQ #' 
LA_CUR_PRI='PRINCIPAL BALANCE' LD_SCL_SPR='SEPARATION DATE'
LD_END_GRC_PRD='GRACE END DATE';
FORMAT LA_CUR_PRI DOLLAR10.2 LD_SCL_SPR LD_END_GRC_PRD MMDDYY10.;
RUN;
PROC PRINTTO;
RUN;
