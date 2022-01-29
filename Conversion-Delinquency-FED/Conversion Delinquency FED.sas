/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=PKUB;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
LIBNAME  WORKLOCL  REMOTE  SERVER=LEGEND  SLIBREF=WORK;
RSUBMIT;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;
%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;
PROC SQL;
CONNECT TO DB2 (DATABASE=&DB);
CREATE TABLE DLQBKT AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT C.DF_SPE_ACC_ID
	,A.LN_SEQ
	,A.LN_DLQ_MAX 
	,B.LA_CUR_PRI
	,B.LA_NSI_OTS
FROM PKUB.LN16_LON_DLQ_HST A
INNER JOIN PKUB.LN10_LON B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN PKUB.PD10_PRS_NME C
	ON A.BF_SSN = C.DF_PRS_ID
WHERE	A.LC_STA_LON16 = '1'
/*	AND DAYS(LD_DLQ_MAX) = DAYS(CURRENT DATE) - 1*/
	AND B.LA_CUR_PRI > 0
	AND B.LC_STA_LON10 = 'P'
	AND A.LN_DLQ_MAX > 30
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
/*QUIT;*/

ENDRSUBMIT;
DATA DLQBKT;
	SET WORKLOCL.DLQBKT;
RUN;


/*The detail files are for testing only.  Remove before putting into promotion.*/
DATA DLQBKT;
	SET DLQBKT;
	LENGTH PERIOD $12.;
	IF 30 < LN_DLQ_MAX < 61 THEN DO;
		BUCKET = 1;
		PERIOD = PUT('31-60 Days',12.0);
	END;
	ELSE IF  61 <= LN_DLQ_MAX < 151 THEN DO;
		BUCKET = 2;
		PERIOD = PUT('61-150 Days',12.0);
	END;
	ELSE IF 	151 <= LN_DLQ_MAX < 181 THEN DO;
		BUCKET = 3;
		PERIOD = PUT('151-180 Days',12.0);
	END;
	ELSE IF  181 <= LN_DLQ_MAX < 271 THEN DO;
		BUCKET = 4;
		PERIOD = PUT('181-270 Days',12.0);
	END;
	ELSE IF  271 <= LN_DLQ_MAX < 360 THEN DO;
		BUCKET = 5;
		PERIOD = PUT('271-359 Days',12.0);
	END;
	ELSE DO;
		BUCKET = 6 ;
		PERIOD = PUT('360+ Days',12.0);
	END;
RUN;

PROC SQL;
CREATE TABLE DLQ_REPORT AS
SELECT PERIOD LABEL = 'Delinquency Bucket'
	,COUNT(DISTINCT DF_SPE_ACC_ID) AS NUM_BOR LABEL = '# Borrowers'
	,COUNT(*) AS NUM_LOANS LABEL = '# Loans'
	,SUM(LA_CUR_PRI) + SUM(LA_NSI_OTS) AS TOT_BAL LABEL = 'Total Balance'
	,BUCKET
FROM DLQBKT
GROUP BY BUCKET,
		period
ORDER BY BUCKET;
QUIT;

DATA _NULL_;
SET		WORK.DLQ_REPORT;
FILE	'T:\SAS\Conversion Delinquency.txt' delimiter=',' DSD DROPOVER lrecl=32767;
/* write column names, remove this to create a file without a header row */
IF _N_ = 1 THEN
	DO;
		PUT	'Delinquency Bucket'
			','
			'# Borrowers'
			','
			'# Loans'
			','
			'Total Balance'
			;
	END;

/* write data*/	
DO;
	PUT PERIOD $ @;
	PUT NUM_BOR @;
	PUT NUM_LOANS @;
	PUT TOT_BAL;
	;
END;
RUN;
