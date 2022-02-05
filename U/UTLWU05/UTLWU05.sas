*-----------------------------------------------------------*
|UTLWU05 - INVALID CHARACTERS IN THE DRIVERS LICENSE FIELD |
*-----------------------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWU05.LWU05R2";
FILENAME REPORTZ "&RPTLIB/ULWU05.LWU05RZ";
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
CREATE TABLE MASTER AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	A.DM_PRS_1 AS FirstName,
	A.DM_PRS_LST as LastName,
	B.DC_DOM_ST AS DLState,
	A.DF_DRV_LIC as DLNumber,
	C.DF_LST_USR_PD05,
	DATE(C.DF_LST_DTS_PD05) AS DF_LST_DTS_PD05,
	A.DF_SPE_ACC_ID
FROM OLWHRM1.PD01_PDM_INF A 
INNER JOIN OLWHRM1.PD05_PRS_NME_HST C
	ON A.DF_PRS_ID = C.DF_PRS_ID
LEFT OUTER JOIN OLWHRM1.PD03_PRS_ADR_PHN B
	ON A.DF_PRS_ID = B.DF_PRS_ID
	AND B.DC_ADR = 'L'
WHERE A.DF_DRV_LIC like('%!%') or A.DF_DRV_LIC like('%@%') or A.DF_DRV_LIC like('%#%') or 
A.DF_DRV_LIC like('%$%') or A.DF_DRV_LIC like('%\%%') ESCAPE '\' or A.DF_DRV_LIC like('%^%') or 
A.DF_DRV_LIC like('%&%') or A.DF_DRV_LIC like('%(%') or A.DF_DRV_LIC like('%)%') or 
A.DF_DRV_LIC like('%-%') or A.DF_DRV_LIC like('%+%') or A.DF_DRV_LIC like('%=%') or 
A.DF_DRV_LIC like('%<%') or A.DF_DRV_LIC like('%>%') or A.DF_DRV_LIC like('%,%') or  
A.DF_DRV_LIC like('%.%') or A.DF_DRV_LIC like('%"%') or A.DF_DRV_LIC like('%;%') or 
A.DF_DRV_LIC like('%:%') or A.DF_DRV_LIC like('%~%') or A.DF_DRV_LIC like('%`%') 
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWU05.LWU05RZ);*/
/*QUIT;*/
ENDRSUBMIT;
DATA MASTER;SET WORKLOCL.MASTER;RUN;

DATA _NULL_;
SET MASTER ;
LENGTH DESCRIPTION $600.;
USER = DF_LST_USR_PD05;
ACT_DT = DF_LST_DTS_PD05;
DESCRIPTION = CATX(',',
		DF_SPE_ACC_ID
		,FIRSTNAME
		,LASTNAME
		,DLNUMBER
		,DLSTATE
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
