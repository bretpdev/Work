/*UTLWA13 - TILP Borrower Benefit Data File*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWA13.LWA13RZ";
FILENAME REPORT2 "&RPTLIB/ULWA13.LWA13R2";

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
CONNECT TO DB2 (DATABASE=DLGSUTST);
CREATE TABLE BORROWER AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	A.BF_SSN,
	A.LN_SEQ,
	B.LD_EFT_EFF_BEG,
	B.LD_EFT_EFF_END,
	B.LR_EFT_RDC
FROM
	OLWHRM1.LN10_LON A
	INNER JOIN OLWHRM1.LN83_EFT_TO_LON B
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.LN_SEQ
	INNER JOIN OLWHRM1.BR30_BR_EFT C
		ON B.BF_SSN = C.BF_SSN
		AND B.BN_EFT_SEQ = C.BN_EFT_SEQ
WHERE
	A.LC_STA_LON10 = 'R'
	AND A.LA_CUR_PRI > 0
/*	AND A.IC_LON_PGM = 'TILP'*/
	AND C.BC_EFT_STA = 'A'
FOR READ ONLY WITH UR
);

CREATE TABLE ARC AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	BF_SSN,
	AN_SEQ,
	COUNT(PF_REQ_ACT) AS NUM_UNSFA
FROM
	OLWHRM1.AY10_BR_LON_ATY
WHERE PF_REQ_ACT = 'UNSFA'
GROUP BY BF_SSN, AN_SEQ
FOR READ ONLY WITH UR
);

DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

PROC SQL;
CREATE TABLE FINAL AS
SELECT
	A.BF_SSN,
	A.LN_SEQ,
	A.LD_EFT_EFF_BEG,
	A.LD_EFT_EFF_END,
	A.LR_EFT_RDC,
	B.NUM_UNSFA
FROM
	BORROWER A
	INNER JOIN ARC B
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.AN_SEQ
;
QUIT;
ENDRSUBMIT;

DATA FINAL;
SET WORKLOCL.FINAL;
RUN;

PROC SORT DATA=FINAL;
BY BF_SSN LN_SEQ;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
TITLE	'TILP Borrower Benefit Data File';

PROC CONTENTS DATA=FINAL OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 95*'-';
	PUT      //////
		@35 '**** NO OBSERVATIONS FOUND ****';
	PUT ////////
		@41 '-- END OF REPORT --';
	PUT /////////////
		@32 "JOB = UTLWA13     REPORT = ULWA13.LWA13R2";
	END;
RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=FINAL WIDTH=UNIFORM WIDTH=MIN;
VAR BF_SSN
	LN_SEQ
	LD_EFT_EFF_BEG
	LD_EFT_EFF_END
	LR_EFT_RDC
	NUM_UNSFA;
LABEL	BF_SSN = 'SSN'
		LN_SEQ = 'LOAN SEQ #'
		LD_EFT_EFF_BEG = 'ACH EFFECTIVE BEGIN DATE'
		LD_EFT_EFF_END = 'ACH EFFECTIVE END DATE'
		LR_EFT_RDC = 'ACH REDUCED RATE'
		NUM_UNSFA = '# ACH NSF';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWA13     REPORT = ULWA13.LWA13R2';
RUN;

PROC PRINTTO;
RUN;
