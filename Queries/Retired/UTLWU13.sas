LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORT2 "&RPTLIB/ULWU13.LWU13R2";
FILENAME REPORTZ "&RPTLIB/ULWU13.LWU13RZ";

OPTIONS SYMBOLGEN NOCENTER NODATE NONUMBER LS=132;
DATA _NULL_;
     CALL SYMPUT('YESTERDAY',PUT(INTNX('DAY',TODAY(),-1,'BEGINNING'), MMDDYYD10.));
RUN;
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
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
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (

SELECT B.DF_SPE_ACC_ID
	,B.DM_PRS_LST
	,A.AI_LLR
FROM	OLWHRM1.GA01_APP A
	INNER JOIN OLWHRM1.PD01_PDM_INF B
	ON A.DF_PRS_ID_BR = B.DF_PRS_ID
WHERE A.AI_LLR = 'Y'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;
%sqlcheck;
quit;

/*ENDRSUBMIT;*/
/**/
/*DATA DEMO; SET WORKLOCL.DEMO; RUN;*/

PROC SORT DATA = DEMO;
BY DF_SPE_ACC_ID;
RUN;

DATA _NULL_;
SET DEMO ;
LENGTH DESCRIPTION $600.;
USER = ' ';
ACT_DT = &YESTERDAY ;
DESCRIPTION = CATX(',',
			DF_SPE_ACC_ID
			,DM_PRS_LST
			,AI_LLR
);
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
