/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWM28.LWM28RZ";
FILENAME REPORT2 "&RPTLIB/ULWM28.LWM28R2";

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
		IF_IST
		,IM_IST_FUL
		,IX_GEN_STR_ADR_1
		,IX_GEN_STR_ADR_2
		,IM_GEN_CT
		,IC_GEN_ST
		,IF_GEN_ZIP
FROM	OLWHRM1.IN01_LGS_IDM_MST
WHERE	IC_IST_TYP IN ('006','001') AND
			(IX_GEN_STR_ADR_1 LIKE '%:%' OR
			 IX_GEN_STR_ADR_2 LIKE '%:%' OR
			 IM_GEN_CT LIKE '%:%' OR
			 IC_GEN_ST LIKE '%:%' OR
			 IF_GEN_ZIP LIKE '%:%')
ORDER BY IF_IST
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

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS NOCENTER PAGENO=1 ORIENTATION=LANDSCAPE;
OPTIONS LS=127 PS=39;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO WIDTH=UNIFORM WIDTH=MIN;
VAR 	IF_IST
		IM_IST_FUL
		IX_GEN_STR_ADR_1
		IX_GEN_STR_ADR_2
		IM_GEN_CT
		IC_GEN_ST
		IF_GEN_ZIP;
LABEL	IF_IST = 'INSTITUTION ID'
		IM_IST_FUL = 'INSTITUTION NAME'
		IX_GEN_STR_ADR_1 = 'STREET ADDRESS - LINE 1'
		IX_GEN_STR_ADR_2 = 'STREET ADDRESS - LINE 2'
		IM_GEN_CT = 'CITY'
		IC_GEN_ST = 'STATE'
		IF_GEN_ZIP = 'ZIP';
TITLE	'Employer and School Address Hygiene Report ';
FOOTNOTE4 	'JOB = UTLWM28     REPORT = ULWM28.LWM28R2';
RUN;

PROC PRINTTO;
RUN;
