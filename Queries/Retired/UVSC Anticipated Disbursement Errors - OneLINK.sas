/*DSASG03 - UVSC ONELINK ANTIC DISB IN ERROR*/

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE UVDB AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT INTEGER(A.DF_PRS_ID_BR) AS DF_PRS_ID_BR
	,B.AF_APL_ID||B.AF_APL_ID_SFX	as CLUID
	,A.AD_IST_TRM_BEG
	,B.AC_LON_TYP
	,C.AN_DSB_SEQ
	,A.AD_APL_DSB_1
	,A.AX_APL_DSB_1_IAD
FROM  OLWHRM1.GA01_APP A 
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.GA11_LON_DSB_ATY C
	ON B.AF_APL_ID = C.AF_APL_ID
	AND B.AF_APL_ID_SFX = C.AF_APL_ID_SFX

WHERE A.AF_APL_OPS_SCL = '00402700'
AND A.AD_IST_TRM_BEG = '01/07/2004'
AND (A.AD_APL_DSB_1 = '02/06/2003'
	OR A.AX_APL_DSB_1_IAD = '02062003')
AND C.AN_DSB_SEQ = 1
AND C.AC_DSB_ADJ = 'E'			/*ESTIMATED DISBURSEMENT*/
AND C.AC_DSB_ADJ_STA = 'A' 		/*ACTIVE ROW*/
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA UVDB; 
SET WORKLOCL.UVDB; 
RUN;

PROC SORT DATA=UVDB;
BY DF_PRS_ID_BR CLUID;
RUN;

DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;

OPTIONS CENTER NODATE NUMBER PAGENO=1 LS=126;
TITLE 'UVSC ANTICIPATED DISB SCHEDULED IN ERROR';
TITLE2 'ONELINK';
TITLE3 "RUN DATE:  &RUNDATE";

PROC CONTENTS DATA=UVDB OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 132 *'-';
	PUT      ////////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT ////////
		@57 '-- END OF REPORT --';
	PUT ////////////////
		@46 'JOB = DSASG03     REPORT = DSASG03.R2';
	END;
RETURN;
run;
PROC PRINT NOOBS SPLIT='/' DATA=UVDB WIDTH=UNIFORM WIDTH=MIN;
VAR DF_PRS_ID_BR CLUID AD_IST_TRM_BEG AC_LON_TYP AN_DSB_SEQ AD_APL_DSB_1
AX_APL_DSB_1_IAD;
LABEL DF_PRS_ID_BR='SSN' CLUID='CL UNIQUE ID' AD_IST_TRM_BEG='LOAN PERIOD BEGIN'
AC_LON_TYP='LOAN TYPE' AN_DSB_SEQ='DISB SEQ #' AD_APL_DSB_1='DISB DATE AFTER VALIDATION'
AX_APL_DSB_1_IAD='DISB DATE PRIOR TO VALIDATION';
FORMAT DF_PRS_ID_BR SSN11. AD_IST_TRM_BEG AD_APL_DSB_1 MMDDYY10.;
FOOTNOTE  'JOB = DSASG03     REPORT = DSASG03.R2';
RUN;