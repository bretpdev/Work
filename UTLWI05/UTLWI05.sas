*------------------------------------*
| UTLWI05 PNote Recon Monthly Update |
*------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWI05.LWI05R2";
FILENAME REPORTZ "&RPTLIB/ULWI05.LWI05RZ";
DATA _NULL_;
	CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'END'), MMDDYYD10.)||"'");
	CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	DATA _NULL_;
    FILE REPORTZ NOTITLES;
    PUT @01 " ********************************************************************* "
      / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
      / @01 " ****  THE SAS LOG IN &SQLRPT SHOULD BE REVIEWED.          **** "       
      / @01 " ********************************************************************* "
      / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
      / @01 " ****  &SQLXMSG   **** "
      / @01 " ********************************************************************* ";
	RUN;
%END;
%MEND;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE PRMU AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.PF_ACT
	,A.BD_ATY_PRF
FROM OLWHRM1.AY01_BR_ATY A
WHERE A.PF_aCT = 'MPROM'
	AND A.BD_ATY_PRF BETWEEN &BEGIN AND &END
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWI05.LWI05RZ);*/
/*QUIT;*/
ENDRSUBMIT;
DATA PRMU ;
SET WORKLOCL.PRMU;
RUN;
PROC SORT DATA=PRMU;
	BY BD_ATY_PRF;
RUN;
PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS PS=52 LS=96 PAGENO=1 CENTER NODATE;
TITLE 'PNOTE RECON MONTHLY UPDATE';
TITLE2	"&RUNDATE";
FOOTNOTE   'JOB = UTLWI05  	 REPORT = ULWI05.LWI05R2';
PROC PRINT NOOBS SPLIT='/' DATA=PRMU WIDTH=UNIFORM WIDTH=MIN LABEL;
	FORMAT BD_ATY_PRF MMDDYY10.;
	VAR PF_ACT BD_ATY_PRF;
	LABEL PF_ACT = 'ACTION CODE'
	BD_ATY_PRF = 'ACTIVITY DATE';
RUN;
PROC PRINTTO;
RUN;