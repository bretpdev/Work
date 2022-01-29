/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWU31.LWU31RZ";
FILENAME REPORT2 "&RPTLIB/ULWU31.LWU31R2";
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
DATA _NULL_;
     CALL SYMPUT('PRV_DAY',"'"||PUT(INTNX('DAY',TODAY(),-1,'beginning'), MMDDYYD10.)||"'");
RUN;
%SYSLPUT PRV_DAY = &PRV_DAY;
RSUBMIT;
%MACRO SQLCHECK ;
  %IF  &SQLXRC NE 0  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @01 " ********************************************************************* "
              / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @01 " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @01 " ****  &SQLXMSG   **** "
              / @01 " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.DF_SPE_ACC_ID
FROM OLWHRM1.PD01_PDM_INF A
	INNER JOIN
	OLWHRM1.GA01_APP B
		ON A.DF_PRS_ID = B.DF_PRS_ID_BR 
WHERE B.AC_CRD_CHK_PRF = 'D'
	AND B.AD_CRD_CHK_PRF = &PRV_DAY
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK;*/
/*QUIT;*/

ENDRSUBMIT;
DATA DEMO;
	SET WORKLOCL.DEMO;
RUN;
PROC SORT DATA=DEMO;
	BY DF_SPE_ACC_ID;
RUN;
PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
/*FOR PORTRAIT REPORTS;*/
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS PS=52 LS=96;
TITLE 'QC - PLUS Credit Denials';
FOOTNOTE1  	"THIS DOCUMENT MAY CONTAIN BORROWERS' SENSITIVE INFORMATION THAT UHEAA HAS PLEDGED TO PROTECT.";
FOOTNOTE2	'PLEASE TAKE APPROPRIATE PRECAUTIONS TO SAFEGUARD THIS INFORMATION.';
FOOTNOTE3	;
FOOTNOTE4   'JOB = UTLWU31  	 REPORT = ULWU31.LWU31R2';

PROC CONTENTS DATA=DEMO OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 132*'-';
	PUT      //////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT //////
		@57 '-- END OF REPORT --';
	PUT //////////////
		@46 "JOB = UTLWU31  	 REPORT = ULWU31.LWU31R2";
	END;
RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO WIDTH=UNIFORM WIDTH=MIN LABEL;
VAR DF_SPE_ACC_ID;
LABEL DF_SPE_ACC_ID = 'Account Number';
RUN;

PROC PRINTTO;
RUN;
