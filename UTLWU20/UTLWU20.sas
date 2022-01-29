/*UTLWU20 - QC Rpt - 1038/1039 Finan Tran - Not Owned by UHEAA*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWU20.LWU20RZ";
FILENAME REPORT2 "&RPTLIB/ULWU20.LWU20R2";

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
CREATE TABLE QUERY AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.DF_SPE_ACC_ID,
	B.LN_SEQ,
	A.DM_PRS_LST,
	C.LD_FAT_EFF,
	C.LA_FAT_CUR_PRI
FROM	OLWHRM1.PD10_PRS_NME A
	INNER JOIN OLWHRM1.LN10_LON B
		ON A.DF_PRS_ID = B.BF_SSN
	INNER JOIN OLWHRM1.LN90_FIN_ATY C
		ON B.BF_SSN = C.BF_SSN
		AND B.LN_SEQ = C.LN_SEQ
WHERE	B.LA_CUR_PRI > 0
	AND B.LC_STA_LON10 = 'R'
	AND B.LF_LON_CUR_OWN <> '828476'
	AND C.PC_FAT_TYP = '10'
	AND C.PC_FAT_SUB_TYP IN ('38', '39')
	AND C.LC_STA_LON90 = 'A'
	AND (C.LC_FAT_REV_REA = '' OR C.LC_FAT_REV_REA IS NULL)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;
DATA QUERY;
SET WORKLOCL.QUERY;
RUN;
PROC SORT DATA=QUERY;
BY DF_SPE_ACC_ID;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
TITLE	'QC Rpt - 1038/1039 Finan Tran - Not Owned by UHEAA';

PROC CONTENTS DATA=QUERY OUT=EMPTYSET NOPRINT;
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
		@32 "JOB = UTLWU20     REPORT = ULWU20.LWU20R2";
	END;
RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=QUERY WIDTH=UNIFORM WIDTH=MIN;
FORMAT LD_FAT_EFF MMDDYY10.;
VAR DF_SPE_ACC_ID
	LN_SEQ
	DM_PRS_LST
	LD_FAT_EFF
	LA_FAT_CUR_PRI;
LABEL	DF_SPE_ACC_ID = 'ACCOUNT #'
		LN_SEQ = 'LOAN SEQ #'
		DM_PRS_LST = 'LAST NAME'
		LD_FAT_EFF = 'EFFECTIVE DATE'
		LA_FAT_CUR_PRI = 'AMOUNT';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWU20     REPORT = ULWU20.LWU20R2';
RUN;

PROC PRINTTO;
RUN;
