*---------------------------------------*
| INCORRECT CONSOLIDATION INTEREST RATE |
*---------------------------------------*;
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/IncorrectConsolidationIntRate.R2";
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE ICIR AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT C.DF_SPE_ACC_ID
	,C.DM_LCO_PRS_1
	,C.DM_LCO_PRS_LST
	,B.LC_UND_LN_PGM
	,B.LF_UND_LN_ACC_NUM
	,A.AD_LCO_APL_DSB
FROM OLWHRM1.AP1A_LCO_APL A
INNER JOIN OLWHRM1.LC10_UND_LN_INF B
	ON A.DF_LCO_PRS_SSN_BR = B.DF_LCO_PRS_SSN_BR
	AND A.AN_LCO_APL_SEQ = B.AN_LCO_APL_SEQ
INNER JOIN OLWHRM1.PD6A_LCO_PRS_DMO C
	ON A.DF_LCO_PRS_SSN_BR = C.DF_LCO_PRS_SSN
WHERE A.AD_LCO_APL_RCV BETWEEN '07/01/2005' AND '06/30/2006' 
AND A.IC_LCO_DSB_APV_STA = 'A'
AND A.AD_LCO_DSB_APV_STA >= '07/01/2006'
AND (
		(
			B.LC_UND_LN_PGM IN ('STFFRD','UNSTFD') AND 
			B.LR_UND_LN_INT IN (6.54,7.14)
		)
	OR
		(
			B.LC_UND_LN_PGM = 'PLUS' AND 
			B.LR_UND_LN_INT = 7.94
		)
	)
ORDER BY A.AD_LCO_APL_DSB
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA ICIR;
SET WORKLOCL.ICIR;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 DATE;
TITLE 'INCORRECT CONSOLIDATION INTEREST RATE';
FOOTNOTE 'JOB = INCORRECTCONSOLIDATIONINTRATE  	 REPORT = R2';
PROC PRINT NOOBS SPLIT='/' DATA=ICIR WIDTH=UNIFORM WIDTH=MIN;
FORMAT AD_LCO_APL_DSB MMDDYY10.;
VAR DF_SPE_ACC_ID DM_LCO_PRS_1 DM_LCO_PRS_LST LC_UND_LN_PGM LF_UND_LN_ACC_NUM AD_LCO_APL_DSB;
LABEL DF_SPE_ACC_ID = 'ACCT #'
DM_LCO_PRS_1 = 'FIRST NAME'
DM_LCO_PRS_LST = 'LAST NAME'
LC_UND_LN_PGM = 'UNDERLYING LOAN PROGRAM'
LF_UND_LN_ACC_NUM = 'UNDERLYING LOAN ACCT #'
AD_LCO_APL_DSB = 'DISBURSEMENT DATE'
;
RUN;
PROC PRINTTO;
RUN;
