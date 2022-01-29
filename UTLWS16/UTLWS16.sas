/*UTLWS16 BORROWERS WITH MORE THAN ONE DELINQUENT CALLING QUEUE TASK*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWS16.LWS16R2";

DATA _NULL_;
     CALL SYMPUT('RUNDT',PUT(INTNX('DAY',TODAY(),0,'BEGINNING'), MMDDYYS10.));
RUN;

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE BWMTODCQ AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT B.DF_SPE_ACC_ID
	,A.PF_REQ_ACT
	,A.WD_ACT_REQ
	,A.WF_QUE
	,A.WF_USR_ASN_TSK
FROM OLWHRM1.WQ20_TSK_QUE A
INNER JOIN OLWHRM1.PD10_PRS_NME B
	ON A.BF_SSN = B.DF_PRS_ID
WHERE WF_QUE IN 
		('C0','C1','C2','C3',
		 'C4','C5','C6','C7',
		 'C8')
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA BWMTODCQ;
SET WORKLOCL.BWMTODCQ;
RUN;

PROC SQL;
CREATE TABLE QDUP AS 
SELECT DF_SPE_ACC_ID
	,COUNT(*) AS COUNT
FROM BWMTODCQ
GROUP BY DF_SPE_ACC_ID
HAVING COUNT(*) > 1;
QUIT;
RUN;

PROC SORT DATA=BWMTODCQ;BY DF_SPE_ACC_ID;RUN;
PROC SORT DATA=QDUP; BY DF_SPE_ACC_ID; RUN;

PROC SQL;
CREATE TABLE BWMTODCQ2 AS 
SELECT A.*
FROM BWMTODCQ A
INNER JOIN QDUP B 
	ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
;
QUIT;

DATA _NULL_;
SET BWMTODCQ2 ;
LENGTH DESCRIPTION $600.;
USER = WF_USR_ASN_TSK;
ACT_DT = WD_ACT_REQ;
DESCRIPTION = CATX(',',
		DF_SPE_ACC_ID,
		WF_QUE,
		PF_REQ_ACT,
		PUT(WD_ACT_REQ,MMDDYY10.)
);
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