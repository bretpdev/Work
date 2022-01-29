*----------------------------------------*
| UTLWO40 120 DAY PAYMENT CHANGES NEEDED |
*----------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATAOTDPCN=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = C:\WINDOWS\TEMP;
FILENAME REPORT2 "&RPTLIB/ULWO40.LWO40R2";
FILENAME REPORT3 "&RPTLIB/ULWO40.LWO40R3";
FILENAME REPORT4 "&RPTLIB/ULWO40.LWO40R4";
FILENAME REPORTZ "&RPTLIB/ULWO40.LWO40RZ";

DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
CALL SYMPUT('RUNTIME',PUT(TIME(), TIME.));
RUN;

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
CREATE TABLE OTDPCN AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT LN90.BF_SSN
		,LN90.LN_SEQ
		,LN90.LD_FAT_EFF
		,ABS( 
			SUM(
				COALESCE(LN90.LA_FAT_NSI,0) +
				COALESCE(LN90.LA_FAT_LTE_FEE,0) + 
				COALESCE(LN90.LA_FAT_ILG_PRI,0) + 
				COALESCE(LN90.LA_FAT_CUR_PRI,0) 
				) 
			) AS PAY_AMT
	,PD10.DM_PRS_LST
	,LN10.IC_LON_PGM
	,RM10.LC_RMT_BCH
	,LN10.LC_STA_LON10
	,LN90.LD_FAT_PST
	,CASE
	WHEN LN10.IF_TIR_PCE IS NULL THEN 'Zero OFEE loan'
	WHEN LN10.IF_TIR_PCE IS NOT NULL THEN 'OFEE loan'
	END AS TIER
	,DAYS(LN90.LD_FAT_PST) AS LD_FAT_PST_DAYS
	,DAYS(CURRENT DATE) AS TODAY_DAYS
	,ABS(COALESCE(LN90.LA_FAT_CUR_PRI,0)) AS LA_FAT_CUR_PRI
	,CASE
	WHEN LF_RGL_CAT_LP20 <= '1999030' THEN .03
	WHEN LF_RGL_CAT_LP20 = '2006020' AND LN10.IC_LON_PGM IN ('PLUS','PLUSGB') AND LN10.LD_LON_1_DSB BETWEEN '07/01/2006' AND '08/07/2006' THEN .02
	WHEN LF_RGL_CAT_LP20 = '2006020' THEN .03
	END AS FEE_RATE 
	,LN10.LD_END_GRC_PRD

FROM OLWHRM1.LN10_LON LN10
INNER JOIN OLWHRM1.PD10_PRS_NME PD10
	ON LN10.BF_SSN = PD10.DF_PRS_ID
INNER JOIN OLWHRM1.LN90_FIN_ATY LN90
	ON LN10.BF_SSN = LN90.BF_SSN
	AND LN10.LN_SEQ = LN90.LN_SEQ
	AND DAYS(LN90.LD_FAT_EFF) < DAYS(LN10.LD_END_GRC_PRD) + 1 
LEFT OUTER JOIN (
	SELECT A.BF_SSN
		,A.LN_SEQ
		,A.LN_FAT_SEQ
		,B.LC_RMT_BCH
	FROM OLWHRM1.LN94_LON_PAY_FAT A
	INNER JOIN OLWHRM1.RM10_RMT_BCH B
		ON A.LD_RMT_BCH_INI = B.LD_RMT_BCH_INI
		AND A.LC_RMT_BCH_SRC_IPT = B.LC_RMT_BCH_SRC_IPT
		AND A.LN_RMT_BCH_SEQ = B.LN_RMT_BCH_SEQ
	 ) RM10
	ON LN90.BF_SSN = RM10.BF_SSN
	AND LN90.LN_SEQ = RM10.LN_SEQ
	AND LN90.LN_FAT_SEQ = RM10.LN_FAT_SEQ
WHERE LN10.IC_LON_PGM IN ('STFFRD','UNSTFD','PLUS','PLUSGB')
AND LN90.PC_FAT_TYP = '10'
AND LN90.PC_FAT_SUB_TYP = '10'
AND LN90.LC_FAT_REV_REA = ''
AND LN90.LC_STA_LON90 = 'A'
AND ABS(LN90.LA_FAT_CUR_PRI) > 0
AND NOT EXISTS (
	SELECT *
	FROM OLWHRM1.LN90_FIN_ATY X
	WHERE X.BF_SSN = LN90.BF_SSN
	AND X.LN_SEQ = LN90.LN_SEQ
	AND X.LD_FAT_EFF = LN90.LD_FAT_EFF
	AND PC_FAT_TYP = '10'
	AND PC_FAT_SUB_TYP = '41'
	AND LC_FAT_REV_REA = ''
	AND LC_STA_LON90 = 'A'
	)
AND EXISTS (
	SELECT *
	FROM OLWHRM1.LN15_DSB Y
	WHERE Y.BF_SSN = LN90.BF_SSN
	AND Y.LN_SEQ = LN90.LN_SEQ
	AND DAYS(LN90.LD_FAT_EFF) <= DAYS(Y.LD_DSB) + 120
	AND Y.LC_DSB_TYP = '2' 
	AND LN90.LD_FAT_EFF > Y.LD_DSB
	AND Y.LC_STA_LON15 IN ('1','3')
	AND COALESCE(Y.LA_DSB,0) <> COALESCE(Y.LA_DSB_CAN,0)
	)
AND NOT EXISTS (
	SELECT * 
	FROM OLWHRM1.WQ20_TSK_QUE Q
	WHERE Q.WF_QUE IN ('7C','7A')
	AND Q.BF_SSN = LN90.BF_SSN
	AND INT(SUBSTR(Q.WN_CTL_TSK,10,4)) = LN90.LN_SEQ
	)
/*and LN90.BF_SSN = ''*/

GROUP BY LN90.BF_SSN
	,LN90.LN_SEQ
	,LN90.LD_FAT_EFF
	,PD10.DM_PRS_LST
	,LN10.IC_LON_PGM
	,RM10.LC_RMT_BCH
	,LN90.LA_FAT_CUR_PRI 
	,LF_RGL_CAT_LP20
	,LN10.LD_LON_1_DSB
	,LN90.LD_FAT_PST
	,LN10.LC_STA_LON10
	,LN10.LD_END_GRC_PRD
	,LN10.IF_TIR_PCE
FOR READ ONLY WITH UR
);

CREATE TABLE LN15 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT LN10.BF_SSN
	,LN10.LN_SEQ
	,LN10.IC_LON_PGM
	,LN15.LA_DSB_ORG_CHK_EFT
	,LN15.LD_DSB
	,AP03.AF_CNL || AP03.AF_CNL_SFX AS AF_CNL
	,LN15.LN_BR_DSB_SEQ
FROM OLWHRM1.LN10_LON LN10
INNER JOIN OLWHRM1.LN15_DSB LN15
	ON LN10.BF_SSN = LN15.BF_SSN
	AND LN10.LN_SEQ = LN15.LN_SEQ
INNER JOIN OLWHRM1.LN90_FIN_ATY LN90
	ON LN10.BF_SSN = LN90.BF_SSN
	AND LN10.LN_SEQ = LN90.LN_SEQ
INNER JOIN OLWHRM1.AP03_MASTER_APL AP03
	ON AP03.AF_APL_ID = LN15.AF_APL_ID
	AND AP03.BF_SSN = LN15.BF_SSN
	AND AP03.AN_SEQ = LN15.AN_SEQ
WHERE LN10.IC_LON_PGM IN ('PLUS','PLUSGB','STFFRD','UNSTFD')
AND LN90.PC_FAT_TYP = '10'
AND LN90.PC_FAT_SUB_TYP = '10'
AND LN90.LC_FAT_REV_REA = ''
AND LN90.LC_STA_LON90 = 'A'
AND ABS(LN90.LA_FAT_CUR_PRI) > 0
AND LN15.LC_STA_LON15 IN ('1','3')
AND COALESCE(LN15.LA_DSB,0) <> COALESCE(LN15.LA_DSB_CAN,0)
/*and LN10.BF_SSN = ''*/

FOR READ ONLY WITH UR
);

CREATE TABLE EXCL AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT LN10.BF_SSN
FROM OLWHRM1.LN10_LON LN10
INNER JOIN OLWHRM1.LN90_FIN_ATY LN90
	ON LN10.BF_SSN = LN90.BF_SSN
	AND LN10.LN_SEQ = LN90.LN_SEQ
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
	ON LN10.BF_SSN = DW01.BF_SSN
	AND LN10.LN_SEQ = DW01.LN_SEQ
WHERE LN10.IC_LON_PGM <> 'TILP'
AND LN90.LD_FAT_EFF >= DW01.WD_LON_RPD_SR
AND LN10.LA_CUR_PRI <> 0
/*and LN10.BF_SSN = ''*/
);


DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWO40.LWO40RZ);*/
/*QUIT;*/

PROC SORT DATA=OTDPCN;BY BF_SSN LN_SEQ;RUN;
PROC SORT DATA=EXCL;BY BF_SSN;RUN;

DATA OTDPCNA;
MERGE OTDPCN (IN=A) EXCL (IN=B); 
BY BF_SSN;
IF A AND NOT B THEN OUTPUT OTDPCNA;
ELSE DELETE;
RUN;

ENDRSUBMIT;

DATA OTDPCNA;SET WORKLOCL.OTDPCNA;RUN;
DATA LN15;SET WORKLOCL.LN15;RUN;



PROC SQL;
CREATE TABLE OTDPCNB AS
SELECT DISTINCT 
	BF_SSN
	,LN_SEQ
	,LD_FAT_EFF
	,PAY_AMT
	,DM_PRS_LST
	,IC_LON_PGM
	,LC_RMT_BCH
	,LC_STA_LON10
	,LD_FAT_PST
	,LD_FAT_PST_DAYS
	,TODAY_DAYS
	,LA_FAT_CUR_PRI
	,FEE_RATE 
	,TIER
FROM OTDPCNA
order BY BF_SSN
	,LN_SEQ
	,LD_FAT_EFF
	,DM_PRS_LST
	,IC_LON_PGM
	,LC_RMT_BCH
	,LC_STA_LON10
	,LD_FAT_PST
	,LD_FAT_PST_DAYS
	,TODAY_DAYS
	,FEE_RATE 
	,TIER
;
QUIT;

DATA OTDPCN;
SET OTDPCNB;
FORMAT FEE_CREDIT;
FEE_CREDIT = LA_FAT_CUR_PRI * FEE_RATE;
RUN;

PROC SORT DATA=OTDPCN;BY BF_SSN LN_SEQ;RUN;

PROC SQL;
CREATE TABLE OTDPCN2 AS 
SELECT DISTINCT A.*
	,B.LD_DSB
	,B.LA_DSB_ORG_CHK_EFT
	,C.MXDT AS MXDT FORMAT=DATE9.
	,B.AF_CNL
FROM OTDPCN A
LEFT OUTER JOIN LN15 B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,MAX(LD_DSB) AS MXDT
	FROM LN15
	GROUP BY BF_SSN
		,LN_SEQ
	 ) C
	ON A.BF_SSN = C.BF_SSN
	AND A.LN_SEQ = C.LN_SEQ 
;
QUIT;

DATA STAF (DROP=LA_DSB_ORG_CHK_EFT) PLUS DECONVERTED (DROP=LD_DSB) OTHER;
SET OTDPCN2;
IF IC_LON_PGM IN ('STFFRD','UNSTFD') AND LC_STA_LON10 = 'R' THEN OUTPUT STAF; 
ELSE IF (IC_LON_PGM IN ('PLUS','PLUSGB')
	AND LC_STA_LON10 = 'R'
	AND PAY_AMT >= LA_DSB_ORG_CHK_EFT 
	AND LD_DSB < LD_FAT_EFF
	AND LD_FAT_EFF < LD_DSB + 120
	AND LD_FAT_EFF <= MXDT)
	THEN OUTPUT PLUS;
ELSE IF ((IC_LON_PGM IN ('STFFRD','UNSTFD') AND LC_STA_LON10 = 'D' ) OR 
	(IC_LON_PGM IN ('PLUS','PLUSGB') AND LC_STA_LON10 = 'D'
	AND PAY_AMT >= LA_DSB_ORG_CHK_EFT 
	AND LD_DSB < LD_FAT_EFF
	AND LD_FAT_EFF <= MXDT))
	AND LD_FAT_PST_DAYS >= TODAY_DAYS - 7
	THEN OUTPUT DECONVERTED;
ELSE OUTPUT OTHER;
RUN;

PROC SORT DATA=STAF NODUPKEY;BY BF_SSN LN_SEQ PAY_AMT;RUN;
PROC SORT DATA=PLUS NODUPKEY;BY BF_SSN LN_SEQ PAY_AMT;RUN;
PROC SORT DATA=DECONVERTED NODUPKEY;BY BF_SSN LN_SEQ PAY_AMT;RUN;
*----------------------------------------------------------------*
|						STAFFORD REPORT                          |
*----------------------------------------------------------------*;
data _null_;
set  WORK.Staf;
file REPORT2 delimiter=',' DSD DROPOVER lrecl=32767;
   format BF_SSN $9. ;
   format DM_PRS_LST $23. ;
   format LD_FAT_EFF MMDDYY10. ;
   format PAY_AMT best12. ;
   format LC_RMT_BCH $1. ;
   format LN_SEQ 6. ;
   format LA_FAT_CUR_PRI best12. ;
if _n_ = 1 then        /* write column names */
 do;
   put
   'BF_SSN'
   ','
   'LN_SEQ'
   ','
   'DM_PRS_LST'
   ','
   'LC_RMT_BCH'
   ','
   'LD_FAT_EFF'
   ','
   'PAY_AMT'
   ','
   'LA_FAT_CUR_PRI'
   ;
 end;
 do;
   EFIOUT + 1;
   put BF_SSN $ @;
   put LN_SEQ $ @;
   put DM_PRS_LST $ @;
   put LC_RMT_BCH $ @;
   put LD_FAT_EFF @;
   put PAY_AMT @;
   put LA_FAT_CUR_PRI $ ;
   ;
 end;
RUN;

*----------------------------------------------------------------*
|						PLUS REPORT                              |
*----------------------------------------------------------------*;
data _null_;
set  WORK.PLUS;
file REPORT3 delimiter=',' DSD DROPOVER lrecl=32767;
   format BF_SSN $9. ;
   format DM_PRS_LST $23. ;
   format LD_FAT_EFF MMDDYY10. ;
   format PAY_AMT best12. ;
   format LC_RMT_BCH $1. ;
   format LN_SEQ 6. ;
   format LA_FAT_CUR_PRI best12. ;
if _n_ = 1 then        /* write column names */
 do;
   put
   'BF_SSN'
   ','
   'LN_SEQ'
   ','
   'DM_PRS_LST'
   ','
   'LC_RMT_BCH'
   ','
   'LD_FAT_EFF'
   ','
   'PAY_AMT'
   ','
   'LA_FAT_CUR_PRI'
   ;
 end;
 do;
   EFIOUT + 1;
   put BF_SSN $ @;
   put LN_SEQ $ @;
   put DM_PRS_LST $ @;
   put LC_RMT_BCH $ @;
   put LD_FAT_EFF @;
   put PAY_AMT @;
   put LA_FAT_CUR_PRI $ ;
   ;
 end;
RUN;
*----------------------------------------------------------------*
|						DECONVERTED REPORT                       |
*----------------------------------------------------------------*;
PROC PRINTTO PRINT=REPORT4 NEW;
RUN;
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS PS=52 LS=96 PAGENO=1 NODATE CENTER;
TITLE '120 DAY PAYMENT CHANGES NEEDED - DECONVERTED';
TITLE2	"&RUNDATE - &RUNTIME";
FOOTNOTE 'JOB = UTLWO40  	 REPORT = ULWO40.LWO40R4';
FOOTNOTE2 "This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE3 "Please take appropriate precautions to safeguard this information.";
PROC CONTENTS DATA=DECONVERTED OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 96*'-';
	PUT      ///////////
		@35 '**** NO OBSERVATIONS FOUND ****';
	PUT ///////////
		@41 '-- END OF REPORT --';
	PUT ////////////////////
		@30 "JOB = UTLWO40  	 REPORT = ULWO40.LWO40R3";
	END;
RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=DECONVERTED WIDTH=UNIFORM WIDTH=MIN;
FORMAT LD_FAT_EFF MMDDYY10. PAY_AMT DOLLAR10.2 FEE_CREDIT DOLLAR10.2 LA_FAT_CUR_PRI DOLLAR10.2 MXDT MMDDYY10.; 
BY TIER;
VAR BF_SSN DM_PRS_LST AF_CNL LD_FAT_EFF PAY_AMT MXDT LA_FAT_CUR_PRI FEE_CREDIT;
LABEL BF_SSN = 'SSN'
	DM_PRS_LST = 'BORROWER LAST NAME' 
	AF_CNL = 'CLID'	
	LD_FAT_EFF = 'PAYMENT EFFECTIVE DATE'	
	PAY_AMT = 'PAYMENT AMOUNT'
	MXDT = 'DISBURSEMENT DATE'
	LA_FAT_CUR_PRI = 'PRINCIPAL AMOUNT'
	FEE_CREDIT = 'FEE CREDIT';
RUN;
PROC PRINTTO;
RUN;
