/*UTLWO24 DELINQUENT AUTO PAY AND DEFERMENT/FORBEARANCE*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
/*%LET RPTLIB = %SYSGET(reportdir);*/

%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWO24.LWO24RZ";
FILENAME REPORT2 "&RPTLIB/ULWO24.LWO24R2";
FILENAME REPORT3 "&RPTLIB/ULWO24.LWO24R3";

DATA _NULL_;
	CALL SYMPUT('RUNDT',PUT(INTNX('DAY',TODAY(),0,'BEGINNING'),MMDDYY10.));
RUN;

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
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
CREATE TABLE DAPADF1 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,D.DM_PRS_LST
	,D.DM_PRS_1
	,D.DF_SPE_ACC_ID
	,F.LN_DLQ_MAX
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.PD10_PRS_NME D
	ON A.BF_SSN = D.DF_PRS_ID
INNER JOIN OLWHRM1.LN16_LON_DLQ_HST F
	ON A.BF_SSN = F.BF_SSN
	AND A.LN_SEQ = F.LN_SEQ
WHERE B.WC_DW_LON_STA IN ('04','05')
AND F.LC_STA_LON16 = '1' 
AND F.LN_DLQ_MAX >= 1
AND A.LC_STA_LON10 = 'R'
AND A.LA_CUR_PRI > 0
AND B.WC_DW_LON_STA NOT IN ('20','21')
AND F.LC_DLQ_TYP = 'P'

);

CREATE TABLE DAPADF2 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,D.DM_PRS_LST
	,D.DM_PRS_1
	,D.DF_SPE_ACC_ID
	,F.LN_DLQ_MAX
	,C.BD_EFT_STA
FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.BR30_BR_EFT C
	ON A.BF_SSN = C.BF_SSN
INNER JOIN OLWHRM1.PD10_PRS_NME D
	ON A.BF_SSN = D.DF_PRS_ID
INNER JOIN OLWHRM1.LN16_LON_DLQ_HST F
	ON A.BF_SSN = F.BF_SSN
	AND A.LN_SEQ = F.LN_SEQ
WHERE C.BC_EFT_STA = 'A'
AND DAYS(C.BD_EFT_STA) < DAYS(CURRENT DATE) - 30
AND F.LC_STA_LON16 = '1' 
AND F.LN_DLQ_MAX >= 7
AND A.LA_CUR_PRI > 0
AND A.LC_STA_LON10 = 'R'
AND B.WC_DW_LON_STA NOT IN ('20','21')
AND F.LC_DLQ_TYP = 'P'

);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWO24.LWO24RZ);*/
/*QUIT;*/
ENDRSUBMIT;

DATA DAPADF1;SET WORKLOCL.DAPADF1;RUN;
DATA DAPADF2;SET WORKLOCL.DAPADF2;RUN;

PROC SORT DATA=DAPADF1;
BY LN_DLQ_MAX DF_SPE_ACC_ID;
RUN;

PROC SORT DATA=DAPADF2;
BY LN_DLQ_MAX DF_SPE_ACC_ID;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1 NODATE CENTER;
TITLE 'DELINQUENT AUTO PAY AND DEFERMENT/FORBEARANCE REPORT';
TITLE2 'DEFERMENT/FORBEARANCE STATUS AND DELINQUENT';
TITLE3	"FOR &RUNDT";
FOOTNOTE 'JOB = UTLWO24  	 REPORT = ULWO24.LWO24R2' ;
PROC CONTENTS DATA=DAPADF1 OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 127*'-';
	PUT      ////////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT ////////
		@57 '-- END OF REPORT --';
	PUT ////////////
		@46 "JOB = UTLWO24  	 REPORT = ULWO24.LWO24R2";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=DAPADF1 WIDTH=UNIFORM ;
VAR DF_SPE_ACC_ID DM_PRS_LST DM_PRS_1 LN_DLQ_MAX;
LABEL DF_SPE_ACC_ID = 'ACCT #' DM_PRS_LST = 'LAST NAME' DM_PRS_1 = 'FIRST NAME' 
LN_DLQ_MAX = 'DAYS DELINQUENT';
RUN;

PROC PRINTTO PRINT=REPORT3 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1 NODATE CENTER;
TITLE 'DELINQUENT AUTO PAY AND DEFERMENT/FORBEARANCE REPORT';
TITLE2 'AUTOPAY AND SEVEN PLUS DAYS DELINQUENT';
TITLE3	"FOR &RUNDT";
FOOTNOTE 'JOB = UTLWO24  	 REPORT = ULWO24.LWO24R3' ;
PROC CONTENTS DATA=DAPADF2 OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 127*'-';
	PUT      ////////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT ////////
		@57 '-- END OF REPORT --';
	PUT ////////////
		@46 "JOB = UTLWO24  	 REPORT = ULWO24.LWO24R3";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=DAPADF2 WIDTH=UNIFORM ;
FORMAT BD_EFT_STA MMDDYY10.;
VAR DF_SPE_ACC_ID DM_PRS_LST DM_PRS_1 LN_DLQ_MAX BD_EFT_STA;
LABEL DF_SPE_ACC_ID = 'ACCT #' DM_PRS_LST = 'LAST NAME' DM_PRS_1 = 'FIRST NAME' 
LN_DLQ_MAX = 'DAYS DELINQUENT' BD_EFT_STA='APPROVED DATE';
RUN;

PROC PRINTTO;
RUN;
