/*----------------------------*
* UTLWP01 DAILY CASH RECOVERY *
*-----------------------------*/

/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWP01.LWP01R2"; 


/*To run for a specific date: for default use today() - 1*/
/*For a month summary, use the last day of the month*/
data _null_;
/*call symput('run_for_dt',"31jul2011"d);*/
call symput('run_for_dt',today()-1);
run;

DATA _NULL_;
	CALL SYMPUT('CROC_DT',"'"||PUT(&run_for_dt, MMDDYYD10.)||"'");
	CALL SYMPUT('YDT',PUT(&run_for_dt,MMDDYY10.));       
	CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',&run_for_dt,0,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('END',"'"||PUT(intnx('month',&run_for_dt,0,'END'), MMDDYYD10.)||"'");
	CALL SYMPUT('X',PUT(DAY(&run_for_dt),2.));
	CALL SYMPUT('Y',PUT(DAY(INTNX('MONTH',&run_for_dt,0,'END')),2.));
	CALL SYMPUT('Q',PUT(INTNX('MONTH',&run_for_dt,0,'BEGINNING'),5.));
	CALL SYMPUT('R',PUT(INTNX('MONTH',&run_for_dt,0,'END'),5.));

RUN;
/*%put &run_for_dt &croc_dt &iroc_dt &ydt &begin &end &x &y &q &r ;*/

PROC FORMAT;
	VALUE $DESCRIP
		'AC' = 'ADVICE DIRECT CONSOLIDATION'
		'BR' = 'BORROWER PAYMENT'
		'CP' = 'CONSOLIDATION PAYMENT'
		'CR' = 'CLAIM REFUND'
		'CS' = 'COSIGNER PAYMENT'
		'DC' = 'DIRECT CONSOLIDATION PAYMENT'
		'EP' = 'EMPLOYER PAYMENT'
		'FO' = 'FEDERAL TREASURY OFFSET PAYMENT'
		'GP' = 'GARNISHMENT PAYMENT'
		'LR' = 'LENDER REFUND'
		'OC' = 'OUTSIDE COLLECTOR PAYMENT'
		'OG' = 'OUTSIDE COLLECTOR GARNISHMENT'
		'OR' = 'TREASURY OFFSET OVERPAYMENT'
		'RH' = 'REHABILITATION PAYMENT'
		'RP' = 'REPURCHASE PAYMENT'
		'SO' = 'STATE OFFSET'
		'SR' = 'SCHOOL REFUND'
		'TP' = 'TRUSTEE PAYMENT'
		'OV' = 'OVERPAYMENT';
RUN;


/***********************************************************************************************
* HOLIDAY PROCESSING - THIS INFO GATHERED FROM http://aa.usno.navy.mil/faq/docs/holidays.html
************************************************************************************************
* CREATE MACRO VARIALBES FOR DAY SPECIFIC HOLIDAYS
***********************************************************************************************
* New Year's Day                                    January 1 FOR CURRENT AND NEXT YEAR
* Independence Day                                  July 4
* Pioneer Day										July 24
* Christmas Day                                     December 25
***********************************************************************************************/
DATA _NULL_;
	CALL SYMPUT('THIS_YR',YEAR(INTNX('YEAR',&run_for_dt,0,'BEGINNING')));
	CALL SYMPUT('NEXT_YR',YEAR(INTNX('YEAR',&run_for_dt,+1,'BEGINNING')));
RUN;

DATA HOLIDAYS (KEEP=HOL OBHOL) ;
FORMAT HOL OBHOL MMDDYY10.;
DO HOL=&q TO &r;
	MNTH = MONTH(HOL);
	WKDY = WEEKDAY(HOL);
	C = CEIL(DAY(HOL)/7);
	IF WKDY = 1 THEN OBHOL = HOL+1;
		ELSE IF WKDY = 7 THEN OBHOL = HOL-1;
		ELSE OBHOL = HOL;
	IF HOL IN ("01JAN&NEXT_YR"d,"01JAN&THIS_YR"d,"04JUL&THIS_YR"d,"24JUL&THIS_YR"d,
	"25DEC&THIS_YR"d) THEN OUTPUT;
	ELSE IF MNTH = 1 AND WKDY = 2 AND C=3 THEN OUTPUT ; *Martin Luther King's Birthday;
	ELSE IF MNTH = 2 AND WKDY = 2 AND C=3 THEN OUTPUT; *Washington's Birthday;
	ELSE IF MNTH = 11 AND WKDY = 5 AND C=4 THEN OUTPUT; *Thanksgiving;
	ELSE IF MNTH = 5 AND WKDY = 2 AND DAY(HOL) > 24 THEN OUTPUT ; *Memorial Day;
	ELSE IF MNTH = 9 AND WKDY = 2 AND DAY(HOL) <= 7 THEN OUTPUT ; *Labor Day;
	ELSE IF MNTH = 11 AND WKDY = 6 AND 22 < DAY(HOL) < 30 THEN OUTPUT ; *Black Friday;
END;
RUN;

/********************************************************************************************
* CREATE DENOMINATOR FOR AVERAGE CALCULATIONS
*********************************************************************************************/
DATA DNOM (DROP=Q);
FORMAT Q MMDDYY10. R MMDDYY10. K MMDDYY10.;
X = &X;
Y = &Y;
Q = &Q;
R = &R;
	DO K = &Q TO &R;
		QUES=WEEKDAY(K);
		OUTPUT;
	END;
RUN;

PROC SQL;
CREATE TABLE TDNOM AS 
SELECT A.K
	,QUES 
	,CASE 
		WHEN QUES IN (2 3 4 5 6) AND B.OBHOL = . THEN 1
		ELSE 0
	 END AS D
	,B.OBHOL
FROM DNOM A
LEFT OUTER JOIN HOLIDAYS B
	ON A.K = B.OBHOL
;
QUIT;

DATA DNOM (KEEP=K DENOM );
SET TDNOM;
DENOM+D;
RUN;
/*END CALCULATION FOR DENOMINATOR*/

/*CREATE MULTIPLIER FOR USE IN CALCULATING PROJECTED MONTHLY TOTALS*/
PROC SQL;
CREATE TABLE PMT AS 
SELECT MAX(DENOM) AS PMT_DN
FROM DNOM;
QUIT;

/*OUTPUT DENOMINATOR TO SEPARATE DATA SET FOR CURRENT DATE*/
DATA DNOM1;
SET DNOM;
WHERE K = &run_for_dt;
/*WHERE K = '31MAY2004'D ;*/
RUN;

/*CREATE MACRO VARIABLES FOR CALCULATIONS*/
DATA _NULL_;
SET DNOM1;
	CALL SYMPUT('DNOM',DENOM);
RUN;

DATA _NULL_;
SET PMT;
	CALL SYMPUT('PDEN',PMT_DN);
RUN;

/*VARIABLE FOR MULTIPLYER IN PERCENT CALCULATION*/
DATA PMV;
DENOM = &DNOM;
PDEN = &PDEN;
PMV = &PDEN - &DNOM ;
RUN;

DATA _NULL_;
SET PMV;
	CALL SYMPUT('PMV',PMV);
RUN;

/*CREATE STATIC LIST OF PAYMENT TRX TYPES AND ORD VARIABLE FOR SORTING*/


%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;
LIBNAME  WORKLOCL  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT;
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
LIBNAME SAS_TAB V8 '/sas/whse/progrevw';
DATA WORK.XREF;
SET SAS_TAB.XREF;
RUN;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DCAR AS
SELECT A.TXTYP
	,A.LA_TRX
	,A.BD_TRX_PST_HST
	,B.RANGEID
FROM CONNECTION TO DB2 (
SELECT A.LC_TRX_TYP AS TXTYP
	,A.LA_TRX
	,A.BD_TRX_PST_HST
	,SUBSTR(A.BF_SSN,8,2) AS SS
FROM OLWHRM1.DC11_LON_FAT A
LEFT OUTER JOIN 
	(SELECT BD_TRX_PST
		,BN_DLY_RCI_SEQ
		,BA_TRX
		,BC_TRX_TYP
	 FROM OLWHRM1.DR01_DLY_RCI 
	 WHERE BC_TRX_BCH = 'M1'
	 ) B
	ON A.BD_TRX_PST_HST = B.BD_TRX_PST
	AND A.LA_TRX = B.BA_TRX
	AND A.BN_DLY_RCI_SEQ_HST = B.BN_DLY_RCI_SEQ
	AND A.LC_TRX_TYP = B.BC_TRX_TYP
WHERE (A.LC_TRX_TYP IN ('OV','OR')
	   OR 
	  (A.LC_TRX_TYP IN 
			('AC','BR','CP','CR','CS','DC',
			 'EP','FO','GP','LR','OC','OG',
			 'RH',/*'RP',*/'SO','SR'/*,'TP'*/)
	   AND A.LD_TRX_ADJ IS NULL)
	   )
AND B.BD_TRX_PST IS NULL 
AND A.BD_TRX_PST_HST BETWEEN &BEGIN AND &END
) A, XREF B
WHERE INPUT(A.SS,2.) BETWEEN B.SSNRANGEBEGIN AND B.SSNRANGEEND
;
DISCONNECT FROM DB2;
QUIT;

DATA TRXLST;
INPUT TXTYP $ 1-2 ORD;
DATALINES;
AC 1
BR 2
CP 3
CR 4
CS 5
DC 6
EP 7
FO 8
GP 9
LR 10
OC 11
OG 12
OR 17
OV 18
RH 13
RT 19
SO 14
SR 15
ST 16
TT 20
;
RUN;

PROC SQL;
CREATE TABLE TRX AS 
SELECT COALESCE(A.TXTYP,B.TXTYP) AS TXTYP
	,SUM(A.LA_TRX) AS TRX_TOT
	,A.BD_TRX_PST_HST
	,B.ORD
	,A.RANGEID 
FROM DCAR A
FULL OUTER JOIN TRXLST B
	ON A.TXTYP = B.TXTYP 
GROUP BY A.TXTYP
	,B.TXTYP
	,A.BD_TRX_PST_HST
	,B.ORD
	,A.RANGEID;
QUIT;

PROC SORT DATA=TRX; BY TXTYP BD_TRX_PST_HST; RUN;

ENDRSUBMIT;

DATA TRX; SET WORKLOCL.TRX; RUN;
DATA TRXLST; SET WORKLOCL.TRXLST; RUN;

PROC SORT DATA=TRX; BY TXTYP BD_TRX_PST_HST; RUN;
PROC SORT DATA=TRXLST; BY TXTYP; RUN;
PROC SQL NOPRINT;
SELECT MAX(RANGEID) INTO :MRG
FROM TRX;
QUIT;
/*A BACKUP COPY OF THE DATA IS NEEDED FOR */
/*PROCESSING THE INDIVIDUAL USERS REPORTS */
/*AFTER THE COMBINED REPORT*/
DATA TRX2;
SET TRX;
RUN;

%MACRO PROCESS(RT);
DATA TRX;
	SET TRX2;
	IF &RT = 0 THEN OUTPUT;
	ELSE IF RANGEID IN (&RT,.) THEN OUTPUT;
RUN;

DATA TRX;
	MERGE TRX TRXLST;
	BY TXTYP;
RUN;

DATA TRX (KEEP = TXTYP MTD DTOT ORD);
SET TRX;
BY TXTYP BD_TRX_PST_HST;
RETAIN DTOT MTD;
IF FIRST.TXTYP THEN MTD = 0;
	IF FIRST.BD_TRX_PST_HST THEN DTOT = 0;
IF BD_TRX_PST_HST = &run_for_dt THEN DO;
	DTOT + TRX_TOT ;
END;
MTD + TRX_TOT ;
IF DTOT = . THEN DTOT = 0;
IF LAST.TXTYP THEN OUTPUT ;
RUN;

PROC SORT DATA=TRX;BY ORD;RUN;

DATA TRX(KEEP = TXTYP MTD ORD DTOT MTTCAL);
SET TRX;
IF TXTYP IN ('OV','OR') THEN DO;
	MREDUCE = MTD * -1;
	DREDUCE = DTOT * -1;
END;
	MACRED+MREDUCE;
	MSUB + MTD;
	DACRED+DREDUCE;
	DSUB + DTOT;
IF TXTYP = 'ST' THEN DO;
		MTD = MSUB;
		DTOT = DSUB;
END;
	ELSE IF TXTYP = 'RT' THEN DO;
		MTD = MACRED;
		DTOT = DACRED;
	END;
IF TXTYP IN ('ST','RT') THEN DO;
	MTTCAL = MTD;
	DTTCAL = DTOT;
END;
	ELSE DO;
		MTTCAL = .;
		DTTCAL = .;
	END;
GTOT+DTTCAL;
IF TXTYP = 'TT' THEN DTOT = GTOT;
IF TXTYP = 'ST' THEN DO;
	CALL SYMPUT('DAYTOT',DTOT);
	CALL SYMPUT('SBTOT',MTTCAL);
END;

RUN;

PROC SORT DATA = TRX; BY ORD;RUN;

DATA TRX;
SET TRX;
BY ORD;
FORMAT DAV 10.2;
DAV = MTD/&DNOM;
PROTOT = ROUND((MTD + (DAV*&PMV)), .01);
PCENT = ROUND(MTD/&SBTOT * 100, .001);
IF TXTYP IN ('OR','OV','RT','ST','TT') THEN PCENT = 0;
IF PCENT = . THEN PCENT = 0;
IF TXTYP IN ('ST','RT','TT') THEN DELETE;
IF TXTYP IN ('OV' 'OR') THEN DO;
	DTOT = DTOT*-1;
	MTD = MTD*-1;
	DAV = DAV*-1;
	PROTOT = PROTOT*-1;
END;
DESCRIPTION = PUT(TXTYP,DESCRIP.);
RUN;
%if &rt = 0 %then %do;
PROC PRINTTO PRINT=REPORT2 new;
%END;
%else %do;
PROC PRINTTO PRINT=REPORT2 ;
%end;
RUN;
OPTIONS ORIENTATION=LANDSCAPE;
OPTIONS PAGENO=1 PS=39 LS=127 NODATE CENTER;
PROC PRINT NOOBS SPLIT='/' DATA=TRX;
FORMAT DTOT DOLLAR14.2 MTD DOLLAR14.2 DAV DOLLAR14.2 PROTOT DOLLAR14.2 PCENT 10.2;
SUM DTOT MTD PCENT DAV PROTOT;
VAR TXTYP DESCRIPTION DTOT MTD PCENT DAV PROTOT;
LABEL TXTYP = "PAYMENT/TYPE"
	DTOT = 'DAILY TOTAL'
	MTD = 'MTD TOTAL'
	PCENT = '%'
	DAV = "DAILY/AVERAGE"
	PROTOT = "PROJECTED/MONTHLY/TOTAL";
TITLE 'DAILY CASH RECOVERY REPORT';
TITLE2 "FOR &YDT";
TITLE3 "RANGE: &RT" ;
%if &rt = 0 %then %do;
title3 "RANGE: ALL";
%END;
FOOTNOTE 'JOB = UTLWP01     REPORT = ULWP01.LWP01R2';
RUN;
PROC PRINTTO;
RUN;
%MEND;
%MACRO REPORTS;
%PROCESS(0);
%DO I = 1 %TO &MRG;
%PROCESS(&I);
%END;
%MEND;
%REPORTS;
