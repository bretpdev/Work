/*UTLWQ20 - Invalid LP50 or TD2A Activity Characters*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWQ20.LWQ20R2";
FILENAME REPORTZ "&RPTLIB/ULWQ20.LWQ20RZ";
options symbolgen;

DATA _NULL_;	
	FORMAT YESTERDAY MMDDYY10.;
	YESTERDAY = TODAY() -1; /*START DATE IS SET TO YESTERDAY*/
	IF WEEKDAY(YESTERDAY) = 1 THEN YESTERDAY = TODAY() - 2;	/*IF ITS A MONDAY THEN START DAY IS SET TO THE PREVIOUS SATURDAY*/
	CALL SYMPUT('YESTERDAY',"'"||put(YESTERDAY,MMDDYY10.)||"'");
RUN;
%SYSLPUT YESTERDAY = &YESTERDAY;		
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
CREATE TABLE ONE AS
SELECT *
FROM CONNECTION TO DB2 (
	SELECT DISTINCT A.DF_PRS_ID AS SSN
	,A.BF_LST_USR_AY01 AS USER
	,A.PF_ACT AS ACTION
	,A.BD_ATY_PRF AS DATE
	,A.BX_CMT AS COMMENT
	,B.DF_SPE_ACC_ID
	FROM OLWHRM1.AY01_BR_ATY A
	INNER JOIN OLWHRM1.PD01_PDM_INF B	
		ON A.DF_PRS_ID = B.DF_PRS_ID
	WHERE A.BX_CMT  LIKE '%"%'
	AND A.BF_LST_USR_AY01 LIKE 'UT%'
	AND DAYS(A.BD_ATY_PRF) = DAYS(&YESTERDAY)
	FOR READ ONLY WITH UR
);

CREATE TABLE COMP AS
SELECT *
FROM CONNECTION TO DB2 (
	SELECT DISTINCT A.BF_SSN AS SSN
	,B.LF_USR_REQ_ATY AS USER
	,B.PF_REQ_ACT AS ACTION
	,B.LD_ATY_REQ_RCV AS DATE
	,A.LX_ATY AS COMMENT
	,C.DF_SPE_ACC_ID
	FROM	OLWHRM1.AY20_ATY_TXT A
	INNER JOIN OLWHRM1.AY10_BR_LON_ATY B
	ON A.BF_SSN = B.BF_SSN 
	AND A.LN_ATY_SEQ = B.LN_ATY_SEQ
	INNER JOIN OLWHRM1.PD10_PRS_NME C
	ON A.BF_SSN = C.DF_PRS_ID
	WHERE A.LX_ATY  LIKE '%"%'
	AND B.LF_USR_REQ_ATY LIKE 'UT%'
	AND DAYS(B.LD_ATY_REQ_RCV) = DAYS(&YESTERDAY)
	FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;
DATA ONE; SET WORKLOCL.ONE; RUN;
DATA COMP; SET WORKLOCL.COMP; RUN;

PROC SORT DATA=ONE;
	BY SSN;
RUN;
PROC SORT DATA=COMP;
	BY SSN;
RUN;

DATA COMMENTS;
	SET ONE COMP;
	DESCRIPTION = DF_SPE_ACC_ID||','||ACTION||','||COMMENT;
RUN;

DATA _NULL_;
SET COMMENTS;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT DATE MMDDYY10.;
IF _N_ = 1 THEN PUT 'USER,ACT_DT,DESCRIPTION';
DO;
	PUT USER $ @;
	PUT DATE @;
	PUT DESCRIPTION $;
END;
RUN;
