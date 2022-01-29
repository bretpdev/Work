/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWK34.LWK34R2";
FILENAME REPORTZ "&RPTLIB/ULWK34.LWK34RZ";


OPTIONS SYMBOLGEN NOCENTER NODATE NONUMBER LS=132;

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
SELECT distinct A.BF_SSN
FROM	OLWHRM1.LN10_LON A
LEFT OUTER JOIN OLWHRM1.PD01_PDM_INF b
	ON A.BF_SSN = B.DF_PRS_ID
LEFT OUTER JOIN OLWHRM1.AY10_BR_LON_ATY C
	ON A.BF_SSN = C.BF_SSN
	AND C.PF_REQ_ACT = 'TPDEM'
	AND C.LD_REQ_RSP_ATY_PRF > CURRENT DATE - 30 DAYS
LEFT OUTER JOIN OLWHRM1.WQ20_TSK_QUE D
	ON A.BF_SSN = D.BF_SSN
	AND D.WF_QUE = 'TD'
	AND D.WF_SUB_QUE = 'PD'
WHERE a.IC_LON_PGM = 'TILP'
	AND a.LC_STA_LON10 = 'R'
	AND B.DF_PRS_ID IS NULL
	AND C.BF_SSN IS NULL
	AND D.BF_SSN IS NULL
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
quit;

ENDRSUBMIT;

DATA DEMO; SET WORKLOCL.DEMO; RUN;

PROC SORT DATA=DEMO;
BY BF_SSN;
RUN;

data _null_;
set  Demo;
file REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;
   format BF_SSN $9. ;
 do;
   EFIOUT + 1;
   put BF_SSN $ @;
   put 'TPDEM'  @;
   put ','  @;
   put ','  @;
   put ','  @;
   put ','  @;
   put ','  @;
   put ','  @;
   put ',ALL'  @;
   put ',Add TILP borrower demographic record to OneLINK'  ;
   ;
 end;

run;

