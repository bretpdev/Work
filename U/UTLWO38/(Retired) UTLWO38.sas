/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*FILENAME REPORT2 "&RPTLIB/ULWO38.LWO38R2";*/
/*FILENAME REPORTZ "&RPTLIB/ULWO38.LWO38RZ";*/
FILENAME REPORT2 "T:\SAS\ULWO38.LWO38R2";

options symbolgen;
DATA _NULL_;		
	EFFDT = TODAY() - 1;
	CALL SYMPUT('EFFDATE',put(EFFDT,MMDDYY10.));
	CALL SYMPUT('RUNDATE',"'"||put(EFFDT,MMDDYY10.)||"'");
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
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (

SELECT 
A.BF_SSN AS SSN
,C.LA_RPS_ISL AS AMT
,B.BA_EFT_ADD_WDR
,D.LD_RPS_1_PAY_DU
,C.LN_RPS_TRM
,A.LC_STA_LON65
,DAYS(A.LD_CRT_LON65) CRTDATE_DAYZ
,DAYS(&RUNDATE) CDATE_DAYZ
,DAYS(&RUNDATE) CDATE
,LC_TYP_SCH_DIS


FROM OLWHRM1.LN65_LON_RPS A
INNER JOIN OLWHRM1.BR30_BR_EFT B
ON B.BF_SSN = A.BF_SSN
AND B.BC_EFT_STA = 'A'
AND B.BA_EFT_ADD_WDR > 0
INNER JOIN OLWHRM1.LN66_LON_RPS_SPF C
ON C.BF_SSN = A.BF_SSN
AND C.LN_SEQ = A.LN_SEQ
AND C.LN_RPS_SEQ = A.LN_RPS_SEQ
INNER JOIN OLWHRM1.RS10_BR_RPD D
ON A.BF_SSN = D.BF_SSN
AND A.LN_RPS_SEQ = D.LN_RPS_SEQ
AND D.LC_STA_RPST10 = 'A'
FOR READ ONLY WITH UR

);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA DEMO; SET WORKLOCL.DEMO; RUN;

DATA DEMO; SET DEMO;
FORMAT PAY_DU_MIN_1_MONTH MMDDYY10.;
PAY_DU_MIN_1_MONTH = INTNX('MONTH', LD_RPS_1_PAY_DU ,+ LN_RPS_TRM - 1 , 'S');
RUN;

/*GET RECORD WITH THE GREATEST LN_RPS_TRM*/
PROC SQL;
CREATE TABLE DEMOA AS
SELECT A.*
FROM DEMO A
INNER JOIN (SELECT DISTINCT SSN, BA_EFT_ADD_WDR, MAX(LN_RPS_TRM) AS LN_RPS_TRM
			FROM DEMO 
			GROUP BY SSN, BA_EFT_ADD_WDR
			) B
	ON A.SSN = B.SSN
	AND A.BA_EFT_ADD_WDR = B.BA_EFT_ADD_WDR
	AND A.LN_RPS_TRM = B.LN_RPS_TRM
;
QUIT;


PROC SQL;
CREATE TABLE DEMO2 AS
SELECT DISTINCT SSN, SUM(AMT) AS AMT, BA_EFT_ADD_WDR
FROM DEMOA
WHERE (LC_STA_LON65 = 'A' AND CRTDATE_DAYZ = CDATE_DAYZ) 
OR ((LC_TYP_SCH_DIS = 'S2' OR LC_TYP_SCH_DIS = 'S5')
	AND PAY_DU_MIN_1_MONTH = &EFFDATE
	)
GROUP BY SSN, BA_EFT_ADD_WDR
;
QUIT;

PROC SORT DATA=DEMO2;
BY SSN;
RUN;

data _null_;
set  Demo2                                    end=EFIEOD;
file REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;
   format SSN $9. ;
   format AMT 9.2 ;
   format BA_EFT_ADD_WDR 9.2 ;
 do;
   put SSN $ @;
   put AMT @;
   put BA_EFT_ADD_WDR ;
   ;
 end;
run;

