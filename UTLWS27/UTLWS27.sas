*-----------------------------------------------*
| UTLWS27 Cancelled Calling Queue Tasks Compass |
*-----------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWS27.LWS27R2";
FILENAME REPORTZ "&RPTLIB/ULWS27.LWS27RZ";
DATA _NULL_;
	CALL SYMPUT('DAYS_AGO_1',"'"||PUT(INTNX('DAY',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'");
RUN;
%SYSLPUT DAYS_AGO_1 = &DAYS_AGO_1;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
%macro sqlcheck ;
  %if  &sqlxrc ne 0  %then  %do  ;
    data _null_  ;
            file reportz notitles  ;
            put @01 " ********************************************************************* "
              / @01 " ****  The SQL code above has experienced an error.               **** "
              / @01 " ****  The SAS should be reviewed.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  The SQL error code is  &sqlxrc  and the SQL error message  **** "
              / @01 " ****  &sqlxmsg   **** "
              / @01 " ********************************************************************* "
            ;
         run  ;
  %end  ;
%mend  ;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE CCQTA AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
		A.DF_PRS_ID
		,A.DF_SPE_ACC_ID
		,B.LF_ATY_RCP
		,B.LD_ATY_REQ_RCV
		,B.LD_ATY_RSP
		,B.PF_REQ_ACT
		,B.PF_RSP_ACT
FROM	OLWHRM1.PD10_PRS_NME A
		INNER JOIN OLWHRM1.AY10_BR_LON_ATY B ON
			A.DF_PRS_ID = B.BF_SSN
		INNER JOIN OLWHRM1.LN16_LON_DLQ_HST C ON
			A.DF_PRS_ID = C.BF_SSN
WHERE	B.LD_ATY_RSP = &DAYS_AGO_1
		AND B.PF_RSP_ACT = 'CANCL'
		AND B.PF_RSP_ACT != 'COMPL'
		AND B.LF_PRF_BY = 'TDXGM'
		AND C.LC_STA_LON16 = '1'
		AND DAYS(CURRENT DATE) - DAYS(C.LD_DLQ_OCC) > 10
FOR READ ONLY WITH UR
);

CREATE TABLE XOUT AS
SELECT DISTINCT BF_SSN
FROM CONNECTION TO DB2 (
		SELECT BF_SSN
		FROM OLWHRM1.LN90_FIN_ATY 
		WHERE LD_FAT_PST = &DAYS_AGO_1
	UNION
		SELECT BF_SSN
		FROM OLWHRM1.LN50_BR_DFR_APV 
		WHERE LD_DFR_APL = &DAYS_AGO_1
	UNION
		SELECT BF_SSN
		FROM OLWHRM1.LN60_BR_FOR_APV 
		WHERE LD_FOR_APL = &DAYS_AGO_1
	UNION
		SELECT DF_PRS_ID AS BF_SSN
		FROM OLWHRM1.PD24_PRS_BKR 
		WHERE DD_BKR_NTF = &DAYS_AGO_1
	UNION			
		SELECT DF_PRS_ID AS BF_SSN
		FROM OLWHRM1.PD27_PRS_SKP_PRC U
		WHERE DD_SKP_EFF_OCC = &DAYS_AGO_1
	UNION
		SELECT DF_PRS_ID AS BF_SSN
		FROM OLWHRM1.PD30_PRS_ADR
		WHERE DD_VER_ADR = &DAYS_AGO_1
	UNION
		SELECT DF_PRS_ID AS BF_SSN
		FROM OLWHRM1.PD42_PRS_PHN
		WHERE DD_PHN_VER = &DAYS_AGO_1
	UNION 
		SELECT BF_SSN
		FROM OLWHRM1.AY10_BR_LON_ATY
		WHERE PF_REQ_ACT = 'P199A' 
			AND LD_ATY_REQ_RCV = &DAYS_AGO_1
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/
PROC SQL;
CREATE TABLE CCQT AS 
SELECT DISTINCT A.*
FROM CCQTA A
WHERE NOT EXISTS 
	(
		SELECT 1
		FROM XOUT X 
		WHERE X.BF_SSN = A.DF_PRS_ID
	);
QUIT;
ENDRSUBMIT;

DATA CCQT;
SET WORKLOCL.CCQT;
RUN;

PROC SORT DATA=CCQT;
BY DF_SPE_ACC_ID;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
PROC CONTENTS DATA=CCQT OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 85*'-';
	PUT      //////
		@31 '**** NO OBSERVATIONS FOUND ****';
	PUT //////
		@37 '-- END OF REPORT --';
	PUT //////////////
		@26 "JOB = UTLWS27  	 REPORT = ULWS27.LWS27R2";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=CCQT WIDTH=UNIFORM WIDTH=MIN;
FORMAT LD_ATY_REQ_RCV LD_ATY_RSP MMDDYY10.;
VAR 	DF_SPE_ACC_ID
		LF_ATY_RCP
		LD_ATY_REQ_RCV
		LD_ATY_RSP
		PF_REQ_ACT
		PF_RSP_ACT;
LABEL	DF_SPE_ACC_ID = 'ACCOUNT NO'
		LF_ATY_RCP = 'TARGET ID'
		LD_ATY_REQ_RCV = 'REQUEST DATE'
		LD_ATY_RSP = 'RESPONSE DATE'
		PF_REQ_ACT = 'REQUEST CODE'
		PF_RSP_ACT = 'RESPONSE CODE';
TITLE		'CANCELLED CALLING QUEUE TASKS';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB=UTLWS27     			REPORT = ULWS27.LWS27R2';
RUN;

PROC PRINTTO;
RUN;
