*-----------------------------------------------*
| UTLWO48 - U36 MANUAL DISQUALIFICATION         |
*-----------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWO48.LWO48R2";
FILENAME REPORTZ "&RPTLIB/ULWO48.LWO48RZ";

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
CREATE TABLE BASE AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT LN10.BF_SSN
	,LN10.LN_SEQ
FROM OLWHRM1.LN10_LON LN10
/***********************************
* NEW CODE
***********************************/
INNER JOIN OLWHRM1.LN54_LON_BBS B
	ON LN10.BF_SSN = B.BF_SSN
	AND LN10.LN_SEQ = B.LN_SEQ
/**********************************/
WHERE LN10.LF_LON_CUR_OWN = '828476'
AND LN10.LA_CUR_PRI > 0
AND LN10.LC_STA_LON10 = 'R'
AND LN10.IC_LON_PGM IN ('SUBCNS','UNCNS','SUBSPC','UNSPC')
AND LN10.LD_LON_1_DSB  >= '05/01/2006'
/**********************************
* LEGACY CODE
***********************************
AND LN10.LI_RTE_RDC_ELG = 'Y'
AND LN10.LC_CUR_RDC_PGM_NME = 'U36'
***********************************
* NEW CODE
***********************************/
AND B.LC_BBS_ELG = 'Y' 
AND B.PM_BBS_PGM = 'U36'
/***********************************/

AND NOT EXISTS (
	SELECT *
	FROM OLWHRM1.AY10_BR_LON_ATY X
	INNER JOIN OLWHRM1.LN85_LON_ATY Y
		ON X.BF_SSN = Y.BF_SSN
		AND X.LN_ATY_SEQ = Y.LN_ATY_SEQ
	WHERE Y.BF_SSN = LN10.BF_SSN
	AND Y.LN_SEQ = LN10.LN_SEQ
	AND X.PF_REQ_ACT = 'U36TP'
	AND X.LC_STA_ACTY10 = 'A'
	)
AND NOT EXISTS (
	SELECT *
	FROM OLWHRM1.PD24_PRS_BKR Z
	WHERE Z.DF_PRS_ID = LN10.BF_SSN
	AND Z.DC_BKR_TYP = '07'
	AND Z.DC_BKR_STA IN ('04','06')
	)
FOR READ ONLY WITH UR
);

CREATE TABLE BILL_A AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT BL10.BF_SSN
	,LN10B.LN_SEQ
	,BL10.LD_BIL_CRT	
	,BL10.LN_SEQ_BIL_WI_DTE
	,BL10.LD_BIL_DU
	,'A' AS SCN_STA
FROM OLWHRM1.LN80_LON_BIL_CRF LN80
INNER JOIN OLWHRM1.BL10_BR_BIL BL10
	ON BL10.BF_SSN = LN80.BF_SSN
	AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
	AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
INNER JOIN OLWHRM1.LN10_LON LN10B
 	ON LN80.BF_SSN = LN10B.BF_SSN
	AND LN80.LN_SEQ = LN10B.LN_SEQ
WHERE BL10.LC_BIL_TYP = 'P'
AND BL10.LC_STA_BIL10 = 'A'
AND DAYS(LN80.LD_BIL_STS_RIR_TOL) > DAYS(LN80.LD_BIL_DU_LON) + 15
AND LN10B.LA_CUR_PRI > 0
AND LN80.LI_BIL_DLQ_OVR_RIR <> 'Y'

FOR READ ONLY WITH UR
);

CREATE TABLE BILL_I AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT BL10.BF_SSN
	,LN10B.LN_SEQ
	,BL10.LD_BIL_CRT	
	,BL10.LN_SEQ_BIL_WI_DTE
	,BL10.LD_BIL_DU
	,'I' AS SCN_STA
FROM OLWHRM1.LN80_LON_BIL_CRF LN80
INNER JOIN OLWHRM1.BL10_BR_BIL BL10
	ON BL10.BF_SSN = LN80.BF_SSN
	AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
	AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
INNER JOIN OLWHRM1.LN10_LON LN10B
 	ON LN80.BF_SSN = LN10B.BF_SSN
	AND LN80.LN_SEQ = LN10B.LN_SEQ
INNER JOIN OLWHRM1.LN50_BR_DFR_APV LN50
	ON LN10B.BF_SSN = LN50.BF_SSN
	AND LN10B.LN_SEQ = LN50.LN_SEQ
INNER JOIN OLWHRM1.DF10_BR_DFR_REQ DF10
	ON DF10.BF_SSN = LN50.BF_SSN
	AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
WHERE BL10.LC_BIL_TYP = 'P'
AND BL10.LC_STA_BIL10 = 'I'
AND DAYS(LN50.LD_DFR_APL) > DAYS(LN80.LD_BIL_DU_LON) + 15
AND LN10B.LA_CUR_PRI > 0
AND LN80.LI_BIL_DLQ_OVR_RIR <> 'Y'
AND LN50.LC_STA_LON50 = 'A'
AND DF10.LC_DFR_TYP NOT IN ('15','18')
AND LN50.LD_DFR_APL >= '01/20/2006'

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWO48.LWO48RZ);*/
/*QUIT;*/
DATA BILL;
SET BILL_A BILL_I;
RUN;

PROC SQL;
CREATE TABLE U3MDQL AS 
SELECT DISTINCT B.BF_SSN
	,B.LN_SEQ
	,B.LD_BIL_CRT	
	,B.LN_SEQ_BIL_WI_DTE
	,B.LD_BIL_DU
	,B.SCN_STA
FROM BASE A
INNER JOIN BILL B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
;
QUIT;
ENDRSUBMIT;

DATA U3MDQL;
SET WORKLOCL.U3MDQL;
RUN;

PROC SORT DATA=U3MDQL NODUPKEY;
BY BF_SSN LN_SEQ LD_BIL_CRT	LN_SEQ_BIL_WI_DTE LD_BIL_DU SCN_STA;
RUN;

PROC SQL;
CREATE TABLE U3MDQLR AS 
SELECT A.BF_SSN 
	,A.LN_SEQ 
	,A.LD_BIL_DU
FROM U3MDQL A
WHERE A.LD_BIL_DU = (
	SELECT MIN(LD_BIL_DU)
	FROM U3MDQL X
	WHERE X.BF_SSN = A.BF_SSN
	AND X.LN_SEQ = A.LN_SEQ
	)
;
QUIT;

DATA _NULL_;
SET  WORK.U3MDQLR; 
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT BF_SSN $9. ;
   FORMAT LN_SEQ 6. ;
   FORMAT LD_BIL_DU MMDDYYn8. ;
DO;
   PUT BF_SSN $ @;
   PUT LN_SEQ @;
   PUT LD_BIL_DU ;
END;
RUN;
