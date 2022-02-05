/********************************************
 **   UTLWO90 - Unapproved School Report   **
 ********************************************/

/*-----Production settings-----*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/

/*-----Development settings-----*/
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/ULWO90.LWO90RZ";
FILENAME REPORT2 "&RPTLIB/ULWO90.LWO90R2";

/*-----Development settings-----*/
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
CREATE TABLE SCHOOLS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	SC01.IF_IST
	,SC01.IM_IST_FUL
FROM	OLWHRM1.SC01_LGS_SCL_INF SC01
WHERE 	SC01.IC_DOE_SCL_STA = 'A'
	AND SC01.IC_SCL_SUB_STA <> 'A'
	AND SC01.IC_SCL_USB_STA <> 'Y'
	AND SC01.IC_SCL_PLS_STA <> 'A'
	AND SC01.IC_SCL_SLS_STA <> 'A'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*-----Production settings-----*/
/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

/*-----Development settings-----*/
ENDRSUBMIT;
DATA SCHOOLS; SET WORKLOCL.SCHOOLS; RUN;

PROC SORT DATA=SCHOOLS;
	BY IF_IST;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
PROC PRINT NOOBS SPLIT='/' DATA=SCHOOLS WIDTH=UNIFORM WIDTH=MIN;
VAR IF_IST
	IM_IST_FUL;
LABEL	IF_IST = 'School Code'
		IM_IST_FUL = 'School Name';
TITLE	'Approved Schools but Unapproved Loan Types';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWO90     REPORT = ULWO90.LWO90R2';
RUN;
PROC PRINTTO;
RUN;
