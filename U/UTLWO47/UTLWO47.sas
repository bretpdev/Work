*-----------------------------------------------*
| UTLWO47 36 Manual Disqual - Reinstate Benefit |
*-----------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWO47.LWO47R2";
FILENAME REPORTZ "&RPTLIB/ULWO47.LWO47RZ";

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
CREATE TABLE MDRB AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.BF_SSN
	,A.LN_SEQ
	,B.LN_DLQ_MAX
	,B.LD_DLQ_ITL
	,B.LD_DLQ_OCC
	,C.LD_DFR_BEG
	,C.LD_DFR_END
	,C.LF_DFR_CTL_NUM
	,D.LD_BIL_CRT
	,D.LN_SEQ_BIL_WI_DTE
	,D.LD_BIL_DU
	,D.LC_STA_LON80
	,D.LC_STA_BIL10
	,D.LI_BIL_DLQ_OVR_RIR

FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.LN54_LON_BBS LN54
	ON A.BF_SSN = LN54.BF_SSN
	AND A.LN_SEQ = LN54.LN_SEQ
INNER JOIN OLWHRM1.LN16_LON_DLQ_HST B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.LN60_BR_FOR_APV LN60
	ON A.BF_SSN = LN60.BF_SSN
	AND A.LN_SEQ = LN60.LN_SEQ
INNER JOIN OLWHRM1.FB10_BR_FOR_REQ FB10
	ON LN60.BF_SSN = FB10.BF_SSN
	AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
INNER JOIN (
	SELECT DF10.BF_SSN
		,LN50.LD_DFR_BEG
		,LN50.LD_DFR_END
		,LN50.LF_DFR_CTL_NUM
	FROM OLWHRM1.DF10_BR_DFR_REQ DF10
	INNER JOIN OLWHRM1.LN50_BR_DFR_APV LN50
		ON DF10.BF_SSN = LN50.BF_SSN
		AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
	WHERE DF10.LC_STA_DFR10 = 'A'
	AND LN50.LC_STA_LON50 = 'A'
	AND DF10.LD_DFR_INF_CER >= '05/01/2006'
	 ) C
	ON A.BF_SSN = C.BF_SSN
LEFT OUTER JOIN (
	SELECT BL10.BF_SSN
		,LN80.LN_SEQ
		,BL10.LD_BIL_CRT
		,BL10.LN_SEQ_BIL_WI_DTE
		,BL10.LD_BIL_DU
		,LN80.LC_STA_LON80
		,BL10.LC_STA_BIL10
		,LN80.LI_BIL_DLQ_OVR_RIR
	FROM OLWHRM1.BL10_BR_BIL BL10
	INNER JOIN OLWHRM1.LN80_LON_BIL_CRF LN80
		ON BL10.BF_SSN = LN80.BF_SSN
		AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
		AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
	WHERE BL10.LC_BIL_TYP = 'P'
	AND LN80.LI_BIL_DLQ_OVR_RIR = 'N'
	 ) D
	ON A.BF_SSN = D.BF_SSN
	AND A.LN_SEQ = D.LN_SEQ

WHERE A.IC_LON_PGM NOT IN ('SUBCNS', 'UNCNS', 'SUBSPC', 'UNSPC')
AND A.LF_LON_CUR_OWN = '828476'
AND A.LA_CUR_PRI > 0
AND A.LC_STA_LON10 = 'R'
AND LN54.LC_BBS_ELG IN ('X','N')
AND LN54.PM_BBS_PGM = 'U36'
AND FB10.LC_FOR_TYP IN ('01', '04', '10', '12', '16', '18', '20', '21', '25', '26', '39', '40', '41', '43')
AND B.LN_DLQ_MAX >= 15
AND B.LD_DLQ_OCC BETWEEN C.LD_DFR_BEG AND C.LD_DFR_END
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWO42.LWO42RZ);*/
/*QUIT;*/
ENDRSUBMIT;

DATA MDRB;
SET WORKLOCL.MDRB;
RUN;

PROC SORT DATA=MDRB;BY BF_SSN LN_SEQ;RUN;

PROC SQL;
CREATE TABLE MDRB2 AS 
SELECT DISTINCT A.BF_SSN
	,A.LF_DFR_CTL_NUM
	,A.LN_SEQ
	,A.LD_BIL_DU
	,A.LI_BIL_DLQ_OVR_RIR
FROM MDRB A
WHERE MONTH(A.LD_DLQ_OCC) = MONTH(A.LD_BIL_DU)
AND YEAR(A.LD_DLQ_OCC) = YEAR(A.LD_BIL_DU)
AND A.LD_DLQ_OCC = (
	SELECT MIN(LD_DLQ_OCC)
	FROM MDRB X
	WHERE X.BF_SSN = A.BF_SSN
	AND X.LN_SEQ = X.LN_SEQ
	)
;
QUIT;

DATA _NULL_;
SET  WORK.MDRB2;
FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT BF_SSN $9. ;
   FORMAT LN_SEQ 6. ;
   FORMAT LD_BIL_DU MMDDYY10. ;
IF _N_ = 1 THEN     
DO;
   PUT
   'BF_SSN'
   ','
   'LN_SEQ'
   ','
   'LD_BIL_DU'
   ;
 END;
 DO;
   PUT BF_SSN $ @;
   PUT LN_SEQ @;
   PUT LD_BIL_DU ;
   ;
 END;
RUN;