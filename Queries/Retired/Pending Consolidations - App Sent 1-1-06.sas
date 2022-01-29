*--------------------------------------------*
| PENDING CONSOLIDATIONS - APP SENT 01/01/06 |
*--------------------------------------------*;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE PCAPS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.DF_LCO_PRS_SSN_BR 
	,C.DM_LCO_PRS_1
	,C.DM_LCO_PRS_MI
	,C.DM_LCO_PRS_LST
	,C.DX_LCO_PRS_STR_1 
	,C.DX_LCO_PRS_STR_2 
	,C.DX_LCO_PRS_STR_3
	,C.DM_LCO_PRS_CT
	,C.DC_LCO_PRS_ST
	,C.DF_LCO_PRS_ZIP
	,C.DM_LCO_PRS_FGN_ST
	,C.DM_LCO_PRS_FGN_CNY
	,'MA2324' AS COST_CENTER_CODE
	,'L' as DC_ADR
	,CASE 
		WHEN DC_LCO_PRS_ST = '' THEN 1
		ELSE 2
	 END AS SVAR
	,A.AD_CRT_AP1A 
	,SUBSTR(A.AF_CRT_USR_AP1A,1,2) AS USER_ID

FROM OLWHRM1.AP1A_LCO_APL A
INNER JOIN OLWHRM1.LC10_UND_LN_INF B
	ON A.DF_LCO_PRS_SSN_BR = B.DF_LCO_PRS_SSN_BR
	AND A.AN_LCO_APL_SEQ = B.AN_LCO_APL_SEQ
INNER JOIN OLWHRM1.PD6A_LCO_PRS_DMO C
	ON A.DF_LCO_PRS_SSN_BR = C.DF_LCO_PRS_SSN
WHERE B.LI_UND_LN_CON = 'Y'
AND B.LD_UND_LN_ACL_PAY IS NULL
AND A.AD_LCO_APL_SND >= '01/01/2006'

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA PCAPS;
SET WORKLOCL.PCAPS;
RUN;

DATA PCAPS;
SET PCAPS;
IF AD_CRT_AP1A >= '12JUN2006'D AND USER_ID ^= 'UT' THEN DELETE;
ELSE OUTPUT ;
RUN;

%MACRO KEY_CLC(TBL);
*CALCULATE KEYLINE;
DATA &TBL (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
SET &TBL;
KEYSSN = TRANSLATE(DF_LCO_PRS_SSN_BR,'MYLAUGHTER','0987654321');
MODAY = PUT(DATE(),MMDDYYN4.);
KEYLINE = "P"||KEYSSN||MODAY||DC_ADR;
CHKDIG = 0;
LENGTH DIG $2.;
DO I = 1 TO LENGTH(KEYLINE);
	IF I/2 NE ROUND(I/2,1) 
		THEN DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4) * 2, 2.);
	ELSE DIG = PUT(INPUT(SUBSTR(KEYLINE,I,1),BITS4.4), 2.);
	IF SUBSTR(DIG,1,1) = " " 
		THEN CHKDIG = CHKDIG + INPUT(SUBSTR(DIG,2,1),1.);
		ELSE DO;
			CHK1 = INPUT(SUBSTR(DIG,1,1),1.);
			CHK2 = INPUT(SUBSTR(DIG,2,1),1.);
			IF CHK1 + CHK2 >= 10
				THEN DO;
					CHK3 = PUT(CHK1 + CHK2,2.);
					CHK1 = INPUT(SUBSTR(CHK3,1,1),1.);
					CHK2 = INPUT(SUBSTR(CHK3,2,1),1.);
				END;
			CHKDIG = CHKDIG + CHK1 + CHK2;
		END;
END;
CHKDIGIT = 10 - INPUT(SUBSTR((RIGHT(PUT(CHKDIG,3.))),3,1),3.);
IF CHKDIGIT = 10 THEN CHKDIGIT = 0;
CHECK = PUT(CHKDIGIT,1.);
ACSKEY = "#"||KEYLINE||CHECK||"#";
RUN;
%MEND KEY_CLC;
%KEY_CLC(PCAPS);

PROC SORT DATA=PCAPS;
BY SVAR DC_LCO_PRS_ST;
RUN;

DATA _NULL_;
SET  WORK.PCAPS ;
FILE 'T:\SAS\PEND.CONSOL.AP010106.R2' DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT ACSKEY $18.;
	FORMAT DF_LCO_PRS_SSN_BR $9. ;
	FORMAT DM_LCO_PRS_1 $13. ;
	FORMAT DM_LCO_PRS_MI $1. ;
	FORMAT DM_LCO_PRS_LST $23. ;
	FORMAT DX_LCO_PRS_STR_1 $40. ;
	FORMAT DX_LCO_PRS_STR_2 $40. ;
	FORMAT DX_LCO_PRS_STR_3 $40. ;
	FORMAT DM_LCO_PRS_CT $30. ;
	FORMAT DC_LCO_PRS_ST $3. ;
	FORMAT DF_LCO_PRS_ZIP $17. ;
	FORMAT DM_LCO_PRS_FGN_ST $30. ;
	FORMAT DM_LCO_PRS_FGN_CNY $25. ;
	FORMAT COST_CENTER_CODE $6. ;
	FORMAT SVAR 11. ;
IF _N_ = 1 THEN
DO;
PUT
'ACSKEY'
','
'DF_LCO_PRS_SSN_BR'
','
'DM_LCO_PRS_1'
','
'DM_LCO_PRS_MI'
','
'DM_LCO_PRS_LST'
','
'DX_LCO_PRS_STR_1'
','
'DX_LCO_PRS_STR_2'
','
'DX_LCO_PRS_STR_3'
','
'DM_LCO_PRS_CT'
','
'DC_LCO_PRS_ST'
','
'DF_LCO_PRS_ZIP'
','
'DM_LCO_PRS_FGN_ST'
','
'DM_LCO_PRS_FGN_CNY'
','
'STATE_IND'
','
'COST_CENTER_CODE'
;
END;
DO;
PUT ACSKEY $ @;
PUT DF_LCO_PRS_SSN_BR $ @;
PUT DM_LCO_PRS_1 $ @;
PUT DM_LCO_PRS_MI $ @;
PUT DM_LCO_PRS_LST $ @;
PUT DX_LCO_PRS_STR_1 $ @;
PUT DX_LCO_PRS_STR_2 $ @;
PUT DX_LCO_PRS_STR_3 $ @;
PUT DM_LCO_PRS_CT $ @;
PUT DC_LCO_PRS_ST $ @;
PUT DF_LCO_PRS_ZIP $ @;
PUT DM_LCO_PRS_FGN_ST $ @;
PUT DM_LCO_PRS_FGN_CNY $ @;
PUT DC_LCO_PRS_ST $ @;
PUT COST_CENTER_CODE $ ;
END;
RUN;

/*USED FOR TESTING*/
/*DATA PCAPS2 NONU;*/
/*SET PCAPS;*/
/*IF AD_CRT_AP1A >= '12JUN2006'D AND USER_ID ^= 'UT' THEN OUTPUT NONU;*/
/*ELSE OUTPUT PCAPS2;*/
/*RUN;*/
/*PROC SQL;*/
/*CREATE TABLE X AS */
/*SELECT A.**/
/*FROM PCAPS A*/
/*INNER JOIN NONU B*/
/*	ON A.DF_LCO_PRS_SSN_BR = B.DF_LCO_PRS_SSN_BR */
/*INNER JOIN PCAPS2 C*/
/*	ON C.DF_LCO_PRS_SSN_BR = B.DF_LCO_PRS_SSN_BR */
/*ORDER BY DF_LCO_PRS_SSN_BR */
/*;*/
/*QUIT;*/