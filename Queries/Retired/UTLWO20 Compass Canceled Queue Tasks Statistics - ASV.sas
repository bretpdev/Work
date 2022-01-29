/*UTLWO20 ASV CANCELLED QUEUE STATISTICS*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWO20.LWO20R2";*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE ACCTQS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT D.BF_SSN
	,D.LN_ATY_SEQ
	,C.PF_REQ_ACT
	,D.LF_PRF_BY
	,A.WF_QUE
	,A.WM_QUE_FUL
	,B.WF_SUB_QUE
	,B.WM_SUB_QUE_FUL
	,A.WF_USR_OWN_QUE
	,C.PX_ACT_DSC_REQ
FROM OLWHRM1.AY10_BR_LON_ATY D
INNER JOIN OLWHRM1.AC10_ACT_REQ C
	ON C.PF_REQ_ACT = D.PF_REQ_ACT
INNER JOIN OLWHRM1.WQ10_TSK_QUE_DFN A
	ON C.WF_QUE = A.WF_QUE
LEFT OUTER JOIN OLWHRM1.WQ15_TSK_SUB_QUE B
	ON A.WF_QUE = B.WF_QUE
WHERE DAYS(D.LD_ATY_RSP) = DAYS(CURRENT DATE) - 1
AND D.PF_RSP_ACT = 'CANCL'
AND D.LF_PRF_BY LIKE 'UT%'
AND A.WF_USR_OWN_QUE = 'UT00018'
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA ACCTQS;
SET WORKLOCL.ACCTQS;
RUN;

PROC SORT DATA=ACCTQS;
BY PF_REQ_ACT WF_QUE WF_SUB_QUE BF_SSN;
RUN;

DATA _NULL_;                                      
CURRENT=TODAY();                                  
CALL SYMPUT('CURDATE',PUT(CURRENT,MMDDYY10.));       
RUN;  

/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS CENTER NODATE NUMBER PAGENO=1 LS=132;
TITLE 'ASV CANCELLED TASKS QUEUE STATISTICS';
TITLE2 "Run Date: &CURDATE";
PROC CONTENTS DATA=ACCTQS OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 132*'-';
	PUT      ////////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT ////////
		@57 '-- END OF REPORT --';
	PUT ////////////////
		@46 "JOB = JOB = UTLWO20  	 REPORT = ULWO20.LWO20R2";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=ACCTQS WIDTH=UNIFORM N="TOTAL: ";
BY LF_PRF_BY ;
VAR WF_QUE
	WM_QUE_FUL
	WF_SUB_QUE
	WM_SUB_QUE_FUL
	BF_SSN
	LN_ATY_SEQ
	PF_REQ_ACT
	PX_ACT_DSC_REQ;
LABEL BF_SSN = 'SSN'
	LN_ATY_SEQ = 'ACTIVITY SEQ'
	PF_REQ_ACT = 'ARC'
	LF_PRF_BY = 'USER ID'
	WF_QUE = 'QUEUE'
	WM_QUE_FUL = 'QUEUE NAME'
	WF_SUB_QUE = 'SUB QUEUE'
	WM_SUB_QUE_FUL = 'SUB QUEUE NAME'
	PX_ACT_DSC_REQ = 'ARC DESCRIPTION';
FOOTNOTE  'JOB = UTLWO20  	 REPORT = ULWO20.LWO20R2';
RUN;


