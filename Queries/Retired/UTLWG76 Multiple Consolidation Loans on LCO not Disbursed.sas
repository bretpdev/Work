/*=========================================================*/
/*UTLWG76 MULTIPLE CONSOLIDATION LOANS ON LCO NOT DISBURSED*/
/*=========================================================*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWG76.LWG76R2";
FILENAME REPORTZ "&RPTLIB/ULWG76.LWG76RZ";
DATA _NULL_;
     CALL SYMPUT('RUNDT',PUT(INTNX('DAY',TODAY(),0,'BEGINNING'), MMDDYYS10.));
RUN;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	FILENAME REPORTZ "&RPTLIB/&SQLRPT";
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
CREATE TABLE MCLOLND AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.DF_LCO_PRS_SSN_BR
	,B.DM_LCO_PRS_LST
	,A.AN_LCO_APL_SEQ
	,A.AF_CRT_USR_AP1A
	,A.AD_CRT_AP1A
	,A.AD_LCO_APL_DSB
	,B.DF_SPE_ACC_ID
FROM OLWHRM1.AP1A_LCO_APL A
INNER JOIN OLWHRM1.PD6A_LCO_PRS_DMO B
	ON A.DF_LCO_PRS_SSN_BR = B.DF_LCO_PRS_SSN
WHERE EXISTS
	(SELECT X.DF_LCO_PRS_SSN_BR
		,COUNT(*) AS COUNT
	 FROM OLWHRM1.AP1A_LCO_APL X
	 WHERE X.DF_LCO_PRS_SSN_BR = A.DF_LCO_PRS_SSN_BR
	 AND X.AD_LCO_APL_DSB IS NULL
	 GROUP BY X.DF_LCO_PRS_SSN_BR
	 HAVING COUNT (*) >= 2)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWG76.LWG76RZ);*/
/*QUIT;*/

ENDRSUBMIT;

DATA MCLOLND;
SET WORKLOCL.MCLOLND;
RUN;

PROC SORT DATA=MCLOLND;
BY DF_SPE_ACC_ID AN_LCO_APL_SEQ;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1 CENTER NODATE;
TITLE 'BORROWERS WITH MULTIPLE NON-DISBURSED LOANS ON LCO';
TITLE2	"RUNDATE &RUNDT";
FOOTNOTE 'JOB = UTLWG76  	 REPORT = ULWG76.LWG76R2';

PROC CONTENTS DATA=MCLOLND OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 127*'-';
	PUT      //////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT ////////
		@57 '-- END OF REPORT --';
	PUT /////////////
		@46 "JOB = UTLWG76  	 REPORT = ULWG76.LWG76R2";
	END;
RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=MCLOLND WIDTH=UNIFORM WIDTH=MIN;
WHERE AD_LCO_APL_DSB EQ .;
FORMAT AD_CRT_AP1A MMDDYY10.;
VAR DF_SPE_ACC_ID
	DM_LCO_PRS_LST
	AN_LCO_APL_SEQ
	AF_CRT_USR_AP1A
	AD_CRT_AP1A;
LABEL DF_SPE_ACC_ID = 'ACCT #'
	DM_LCO_PRS_LST = 'LAST NAME'
	AN_LCO_APL_SEQ = 'LCO LOAN SEQ #'
	AF_CRT_USR_AP1A = 'USER ID'
	AD_CRT_AP1A = 'DATE CREATED' ;
RUN;

PROC PRINTTO;
RUN;
