/*-----------------------------------------*
| UTLWK18 SKIPS WITH VALID EMAIL - ONELINK |
*-----------------------------------------*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWK18.LWK18R2";
FILENAME REPORTZ "&RPTLIB/ULWK18.LWK18RZ";

DATA _NULL_;
     CALL SYMPUT('DTE',"'"||PUT(INTNX('DAY',TODAY(),-45,'BEGINNING'), MMDDYYD10.)||"'");
RUN;
%SYSLPUT DTE = &DTE;

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
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
/*----------- ADDRESS SKIP -----------*/
CREATE TABLE EML1 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.DF_PRS_ID
	,A.DX_EML_ADR
	,'A' AS SKP
FROM OLWHRM1.PD03_PRS_ADR_PHN A
	JOIN OLWHRM1.GA01_APP B
		ON A.DF_PRS_ID = B.DF_PRS_ID_BR
	JOIN OLWHRM1.GA14_LON_STA C
		ON B.AF_APL_ID = C.AF_APL_ID
WHERE A.DC_ADR = 'L'
AND A.DI_VLD_ADR = 'N'
AND A.DI_EML_ADR_VAL = 'Y'
AND A.DI_PHN_VLD = 'Y'
AND C.AC_STA_GA14 = 'A'
AND C.AC_LON_STA_TYP IN ('CR', 'DA', 'FB', 'IA', 'ID', 'IG', 'IM', 'RF', 'RP', 'UA', 'UB')
FOR READ ONLY WITH UR
);

/*----------- PHONE SKIP -----------*/
CREATE TABLE EML2 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.DF_PRS_ID
	,A.DX_EML_ADR
	,'P' AS SKP
FROM OLWHRM1.PD03_PRS_ADR_PHN A
	JOIN OLWHRM1.GA01_APP B
		ON A.DF_PRS_ID = B.DF_PRS_ID_BR
	JOIN OLWHRM1.GA14_LON_STA C
		ON B.AF_APL_ID = C.AF_APL_ID
WHERE A.DI_PHN_VLD = 'N'
AND A.DI_FGN_PHN = 'N' 
AND A.DC_ADR = 'L'
AND A.DI_VLD_ADR = 'Y'
AND A.DI_EML_ADR_VAL = 'Y'
AND C.AC_STA_GA14 = 'A'
AND C.AC_LON_STA_TYP IN ('CR', 'DA', 'FB', 'IA', 'ID', 'IG', 'IM', 'RF', 'RP', 'UA', 'UB')
FOR READ ONLY WITH UR
);

/*------------ BOTH SKIP ------------*/
CREATE TABLE EML3 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.DF_PRS_ID
	,A.DX_EML_ADR
	,'B' AS SKP
FROM OLWHRM1.PD03_PRS_ADR_PHN A
	JOIN OLWHRM1.GA01_APP B
		ON A.DF_PRS_ID = B.DF_PRS_ID_BR
	JOIN OLWHRM1.GA14_LON_STA C
		ON B.AF_APL_ID = C.AF_APL_ID
WHERE A.DC_ADR = 'L'
AND A.DI_VLD_ADR = 'N'
AND A.DI_EML_ADR_VAL = 'Y'
AND A.DI_PHN_VLD = 'N'
AND A.DI_FGN_PHN = 'N' 
AND A.DC_ADR = 'L'
AND C.AC_STA_GA14 = 'A'
AND C.AC_LON_STA_TYP IN ('CR', 'DA', 'FB', 'IA', 'ID', 'IG', 'IM', 'RF', 'RP', 'UA', 'UB')
FOR READ ONLY WITH UR
);

/*------- EXCLUSION DATA SETS -------*/
CREATE TABLE X1 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DF_PRS_ID AS SSN
FROM OLWHRM1.AY01_BR_ATY 
WHERE PF_ACT = 'KEMAL'
AND BD_ATY_PRF >= &DTE
FOR READ ONLY WITH UR
);

CREATE TABLE X2 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DF_PRS_ID_BR AS SSN
FROM OLWHRM1.CT30_CALL_QUE 
WHERE IF_WRK_GRP = 'KSKEMAIL'
FOR READ ONLY WITH UR
);

DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWK18.LWK18RZ);*/
/*QUIT;*/
DATA EML;
SET EML1 EML2 EML3;
RUN;

DATA EX;
SET X1 X2;
RUN;

PROC SQL;
CREATE TABLE EMAIL AS 
SELECT DISTINCT A.DF_PRS_ID
	,A.DX_EML_ADR
	,A.SKP
FROM EML A
WHERE A.DF_PRS_ID NOT IN (
	SELECT DISTINCT SSN
	FROM EX 
	)
;
QUIT;
ENDRSUBMIT  ;

DATA EMAIL;
SET WORKLOCL.EMAIL;
RUN;

PROC SORT DATA=EMAIL;
BY DF_PRS_ID;
RUN;

DATA EMAIL (KEEP=TARGET_ID QUEUE_NAME INSTITUTION_ID INSTITUTION_TYPE DATE_DUE TIME_DUE COMMENTS);
SET EMAIL ;
LENGTH COMMENTS $ 600.;
TARGET_ID = DF_PRS_ID;
QUEUE_NAME = 'KSKEMAIL';
INSTITUTION_ID = '';
INSTITUTION_TYPE = '';
DATE_DUE = '';
TIME_DUE = '';
COMMENTS = 'OneLINK Email: '||TRIM(LEFT(DX_EML_ADR))||','||'Skip type: '||TRIM(LEFT(SKP));
RUN;

DATA _NULL_;
SET  WORK.EMAIL; 
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT COMMENTS $600. ;
   FORMAT TARGET_ID $9. ;
   FORMAT QUEUE_NAME $8. ;
   FORMAT INSTITUTION_ID $1. ;
   FORMAT INSTITUTION_TYPE $1. ;
   FORMAT DATE_DUE $1. ;
   FORMAT TIME_DUE $1. ;
DO;
	PUT TARGET_ID $ @;
	PUT QUEUE_NAME $ @;
	PUT INSTITUTION_ID $ @;
	PUT INSTITUTION_TYPE $ @;
	PUT DATE_DUE $ @;
	PUT TIME_DUE $ @;
	PUT COMMENTS $ ;
END;
RUN;