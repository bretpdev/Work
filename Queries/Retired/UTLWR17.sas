/*UTLWR17 - OneLINK Unresolved Anticipated PUT Dates*/

/*-----Production settings-----*/
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);

/*-----Development settings-----*/
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*%LET RPTLIB = T:\SAS;*/

FILENAME REPORTZ "&RPTLIB/ULWR17.LWR17RZ";
FILENAME REPORT2 "&RPTLIB/ULWR17.LWR17R2";

/*RSUBMIT;*/

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
	GA01.DF_PRS_ID_BR
	,GA10.AF_APL_ID || GA10.AF_APL_ID_SFX	AS CLUID
	,GA10.AD_ECA_PUT_ANT
	,CURRENT_DATE
FROM	OLWHRM1.GA01_APP GA01
INNER JOIN OLWHRM1.GA10_LON_APP GA10
		ON GA01.AF_APL_ID = GA10.AF_APL_ID
WHERE	DAYS(GA10.AD_ECA_PUT_ANT) < DAYS(CURRENT DATE) - 15
AND GA10.AF_APL_ID || GA10.AF_APL_ID_SFX NOT IN (
	SELECT LN10.LF_LON_ALT || '0' || CHAR(LN10.LN_LON_ALT_SEQ)	AS CLUID
	FROM OLWHRM1.LN10_LON LN10
	INNER JOIN OLWHRM1.LN90_FIN_ATY LN90
		ON LN10.BF_SSN = LN90.BF_SSN
		AND LN10.LN_SEQ = LN90.LN_SEQ
	WHERE LN90.PC_FAT_TYP = '04'
	AND LN90.PC_FAT_SUB_TYP = '95'
	AND LN90.LC_STA_LON90 = 'A'
	AND (LN90.LC_FAT_REV_REA IS NULL OR LN90.LC_FAT_REV_REA = '')
)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*-----Production settings-----*/
%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;
%sqlcheck;
quit;

/*-----Development settings-----*/
/*ENDRSUBMIT;*/
/*DATA QUERY; SET WORKLOCL.QUERY; RUN;*/

PROC SORT DATA=QUERY;
BY DF_PRS_ID_BR CLUID;
RUN;

DATA QUERY (KEEP=TARGET_ID QUEUE_NAME INSTITUTION_ID INSTITUTION_TYPE DATE_DUE TIME_DUE COMMENTS);
SET QUERY;
LENGTH COMMENTS $ 600.;
TARGET_ID = DF_PRS_ID_BR;
QUEUE_NAME = 'ANTICPUT';
INSTITUTION_ID = '';
INSTITUTION_TYPE = '';
DATE_DUE = CURRENT_DATE;
TIME_DUE = '';
COMMENTS = CLUID||';'||AD_ECA_PUT_ANT;
RUN;

DATA _NULL_;
SET QUERY;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT COMMENTS $600. ;
FORMAT TARGET_ID $9. ;
FORMAT QUEUE_NAME $8. ;
FORMAT INSTITUTION_ID $1. ;
FORMAT INSTITUTION_TYPE $1. ;
FORMAT DATE_DUE MMDDYY10. ;
FORMAT TIME_DUE $1. ;
DO;
	PUT TARGET_ID $ @;
	PUT QUEUE_NAME $ @;
	PUT INSTITUTION_ID $ @;
	PUT INSTITUTION_TYPE $ @;
	PUT DATE_DUE $ @;
	PUT TIME_DUE$  @;
	PUT COMMENTS $ ;
END;
RUN;
