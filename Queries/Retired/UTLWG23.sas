/*UTLWG23 - PROVISIONAL APPS REQUIRING A PLUS CREDIT CHECK*/
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWG23.LWG23R2";
/*FILENAME REPORT2 'T:\SAS\ULWG23.LWG23R2';*/
/*libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;*/
/*RSUBMIT;*/
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE PLCHK AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.DF_PRS_ID_BR
	,B.AC_LON_TYP
FROM OLWHRM1.GA01_APP A 
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
WHERE A.AC_CRD_CHK_PRF = ' '
AND A.AD_CRD_CHK_PRF IS NULL
AND B.AC_PRC_STA IN ('P','H')
AND A.AC_APL_TYP = 'P'
AND DAYS(A.AD_APL_CRT) = DAYS(CURRENT DATE) - 1
AND A.AF_BS_MPN_APL_ID IS NOT NULL
);
DISCONNECT FROM DB2;
/*ENDRSUBMIT;*/
/*DATA PLCHK; */
/*SET WORKLOCL.PLCHK; */
/*RUN;*/
DATA _NULL_;
SET WORK.PLCHK;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT DF_PRS_ID_BR $9. ;
PUT DF_PRS_ID_BR $ @;
PUT AC_LON_TYP $ ;
RUN;
