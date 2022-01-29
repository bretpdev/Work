/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*FILENAME REPORTZ "&RPTLIB/ULWG82.LWG82RZ";*/
/*FILENAME REPORT2 "&RPTLIB/ULWG82.LWG82R2";*/

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
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	DISTINCT
		A.DF_LCO_PRS_SSN_BR
		,B.DM_LCO_PRS_LST
		,A.AN_LCO_APL_SEQ
		,A.AD_LCO_APL_SND
		,A.AD_LCO_APL_RCV
FROM	OLWHRM1.AP1A_LCO_APL A
		INNER JOIN OLWHRM1.PD6A_LCO_PRS_DMO B ON
			A.DF_LCO_PRS_SSN_BR = B.DF_LCO_PRS_SSN
			AND DAYS(A.AD_LCO_APL_SND) > DAYS(A.AD_LCO_APL_RCV)
			AND A.AC_LCO_ACC_STA NOT IN('D1', 'D3', 'D4', 'D6', 'D7', 'D8')
ORDER BY A.DF_LCO_PRS_SSN_BR

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA DEMO;
SET WORKLOCL.DEMO;
RUN;

/*PROC PRINTTO PRINT=REPORT2;*/
/*RUN;*/
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO WIDTH=UNIFORM WIDTH=MIN;
VAR 	DF_LCO_PRS_SSN_BR
		DM_LCO_PRS_LST
		AN_LCO_APL_SEQ;
LABEL	DF_LCO_PRS_SSN_BR = 'SSN'
		DM_LCO_PRS_LST = 'LAST NAME'
		AN_LCO_APL_SEQ = 'APP SEQUENCE';
TITLE	'LCO Application Date Errors';
FOOTNOTE  'JOB = UTLWG82     REPORT = ULWG82.LWG82R2';
RUN;
