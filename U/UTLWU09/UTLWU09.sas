/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*%LET FILEDIR = /sas/whse/progrevw;*/
%LET RPTLIB = T:\SAS;
%LET FILEDIR = Q:\Process Automation\TabSAS;

FILENAME REPORT2 "&RPTLIB/ULWU09.LWU09R2";
FILENAME REPORTZ "&RPTLIB/ULWU09.LWU09RZ";


OPTIONS SYMBOLGEN NOCENTER NODATE NONUMBER LS=132;
DATA _NULL_;		
	EFFDT = TODAY() - 1; /*GETS THE PREVIOUS DAY*/
    CALL SYMPUT('EFFDATE',put(EFFDT,MMDDYY10.)); /*GETS THE PREVIOUS DAY*/
	CALL SYMPUT('RUNDATE',"'"||put(EFFDT,MMDDYY10.)||"'"); /*GETS THE PREVIOUS DAY*/
RUN;
%SYSLPUT RUNDATE = &RUNDATE;

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
CREATE TABLE OW10 AS
SELECT *
FROM CONNECTION TO DB2 (

SELECT 
	A.IF_LST_USR_OW10 AS USERID
	,DATE(A.IF_LST_DTS_OW10) AS ADATE
	,A.IF_OWN AS ID
	,A.IM_OWN_SHO AS NAME

FROM	OLWHRM1.OW10_OWN A
WHERE DAYS(A.IF_LST_DTS_OW10) = DAYS(&RUNDATE)
FOR READ ONLY WITH UR

);

CREATE TABLE OW20 AS
SELECT *
FROM CONNECTION TO DB2 (

SELECT DISTINCT 
	B.IF_LST_USR_OW20 AS USERID
	,DATE(B.IF_LST_DTS_OW20) AS ADATE
	,B.IF_OWN AS ID
	,B.IF_OWN_POR AS NAME

FROM OLWHRM1.OW20_OWN_POR B
WHERE DAYS(B.IF_LST_DTS_OW20) = DAYS(&RUNDATE)
FOR READ ONLY WITH UR

);

DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA OW10; SET WORKLOCL.OW10; RUN;
DATA OW20; SET WORKLOCL.OW20; RUN;

DATA DEMO;
SET OW10 OW20;
RUN;
/*************************************************************************************
* NOTE: THE UTLWU09.txt FILE THAT THIS DATA STEP READS NEEDS TO FOLLOW THIS CONVENTION
* VAL1 = USER ID
* WHERE VAL1 IS CHARACTER DATA 
**************************************************************************************/
DATA AUTHORIZED;
INFILE "&FILEDIR/UTLWU09.txt" DLM=',' MISSOVER DSD;
INPUT VAL1 $ ;
RUN;

PROC SORT DATA=DEMO;
BY USERID;
RUN;

PROC SQL;
CREATE TABLE DEMO2 AS
SELECT DISTINCT *
FROM DEMO
WHERE USERID NOT IN (SELECT VAL1 FROM AUTHORIZED);
QUIT;

DATA _NULL_;
	SET DEMO2 ;
	LENGTH DESCRIPTION $600.;
	USER = USERID;
	ACT_DT = ADATE;
	DESCRIPTION = CATX(',',ID,NAME);
	FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT USER $10. ;
	FORMAT ACT_DT MMDDYY10. ;
	FORMAT DESCRIPTION $600. ;
	IF _N_ = 1 THEN DO;
		PUT "USER,ACT_DT,DESCRIPTION";
	END;
	DO;
	   PUT USER $ @;
	   PUT ACT_DT @;
	   PUT DESCRIPTION $ ;
	END;
RUN;
