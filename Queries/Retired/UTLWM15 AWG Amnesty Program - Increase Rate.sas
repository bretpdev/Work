*---------------------------------------------*
|UTLWM15 AWG AMNESTY PROGRAM - INCREASE RATE  |
*---------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = C:\WINDOWS\TEMP;
FILENAME REPORT2 "&RPTLIB/ULWM15.LWM15R2";
FILENAME REPORT3 "&RPTLIB/ULWM15.LWM15R3";
FILENAME REPORT4 "&RPTLIB/ULWM15.LWM15R4";
FILENAME REPORTZ "&RPTLIB/ULWM15.LWM15RZ";

DATA _NULL_;
     CALL SYMPUT('DCRT',INTNX('DAY',TODAY(),-7,'BEGINNING'));
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
CREATE TABLE AWGAP AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT DC01.BF_SSN
	,PD01.DF_SPE_ACC_ID
	,PD01.DM_PRS_1 
	,PD01.DM_PRS_LST
	,PD01.DX_STR_ADR_1
	,PD01.DX_STR_ADR_2
	,PD01.DM_CT
	,PD01.DC_DOM_ST
	,PD01.DF_ZIP
	,PD01.DC_ADR
	,'MA2329' AS COST_CENTER_CODE
	,SUM(
		COALESCE(DC01.LA_CLM_PRI,0) + 
		COALESCE(DC01.LA_CLM_INT,0) -
		COALESCE(DC01.LA_PRI_COL,0) +
		COALESCE(DC01.LA_INT_ACR,0) +
		COALESCE(DC02.LA_CLM_INT_ACR,0) -
		COALESCE(DC01.LA_INT_COL,0)
		) + SUM(
				COALESCE(DC01.LA_LEG_CST_ACR,0) -
				COALESCE(DC01.LA_LEG_CST_COL,0) +
				COALESCE(DC01.LA_OTH_CHR_ACR,0) -
				COALESCE(DC01.LA_OTH_CHR_COL,0) +
				COALESCE(DC01.LA_COL_CST_ACR,0) -
				COALESCE(DC01.LA_COL_CST_COL,0) +
				COALESCE(DC02.LA_CLM_PRJ_COL_CST,0)
				) AS ACO 
	,DL1NI.BD_ATY_PRF AS DL1NI_DT
	,DL2NI.BD_ATY_PRF AS DL2NI_DT
	,DL3NI.BD_ATY_PRF AS DL3NI_DT

FROM OLWHRM1.DC01_LON_CLM_INF DC01
INNER JOIN OLWHRM1.PD01_PDM_INF PD01
	ON DC01.BF_SSN = PD01.DF_PRS_ID 
INNER JOIN (
		SELECT BF_SSN
		FROM OLWHRM1.DC01_LON_CLM_INF X
		INNER JOIN OLWHRM1.LA10_LEG_ACT Y
			ON X.BF_SSN = Y.DF_PRS_ID_BR
		INNER JOIN OLWHRM1.LA11_LEG_ACT_ATY Z
			ON Y.DF_PRS_ID_BR = Z.DF_PRS_ID_BR
			AND Y.BF_CRT_DTS_LA10 = Z.BF_CRT_DTS_LA10		
		WHERE X.LC_GRN = '07' 
		AND Z.BC_EXE_TYP = '05'
	UNION
		SELECT BF_SSN
		FROM OLWHRM1.DC01_LON_CLM_INF X
		INNER JOIN OLWHRM1.LA10_LEG_ACT Y
			ON X.BF_SSN = Y.DF_PRS_ID_BR
		WHERE X.LC_GRN = '02' 
		AND Y.BD_WDR IS NULL		
	 ) GRN_TYP
	ON DC01.BF_SSN = GRN_TYP.BF_SSN
LEFT OUTER JOIN OLWHRM1.AY01_BR_ATY DL1NI
	ON DC01.BF_SSN = DL1NI.DF_PRS_ID 
	AND DL1NI.PF_ACT = 'DL1NI'
LEFT OUTER JOIN OLWHRM1.AY01_BR_ATY DL2NI
	ON DC01.BF_SSN = DL2NI.DF_PRS_ID 
	AND DL2NI.PF_ACT = 'DL2NI'
LEFT OUTER JOIN OLWHRM1.AY01_BR_ATY DL3NI
	ON DC01.BF_SSN = DL3NI.DF_PRS_ID 
	AND DL3NI.PF_ACT = 'DL3NI'
INNER JOIN OLWHRM1.DC02_BAL_INT DC02
	ON DC01.AF_APL_ID= DC02.AF_APL_ID
	AND DC01.AF_APL_ID_SFX = DC02.AF_APL_ID_SFX

WHERE DC01.LC_STA_DC10 = '03'
AND DC01.LC_REA_CLM_ASN_DOE = ''
AND DC01.LC_AUX_STA = ''
AND PD01.DC_ADR = 'L'

GROUP BY DC01.BF_SSN
	,PD01.DF_SPE_ACC_ID
	,PD01.DM_PRS_1 
	,PD01.DM_PRS_LST
	,PD01.DX_STR_ADR_1
	,PD01.DX_STR_ADR_2
	,PD01.DM_CT
	,PD01.DC_DOM_ST
	,PD01.DF_ZIP
	,PD01.DC_ADR
	,'MA2329' 
	,DL1NI.BD_ATY_PRF 
	,DL2NI.BD_ATY_PRF 
	,DL3NI.BD_ATY_PRF

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWM15.LWM15RZ);*/
/*QUIT;*/
ENDRSUBMIT;
DATA AWGAP;SET WORKLOCL.AWGAP;RUN;

PROC SORT DATA=AWGAP;BY BF_SSN;RUN;

*CALCULATE KEYLINE;
%MACRO KEY_CLC(TBL);
DATA &TBL (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
SET &TBL;
KEYSSN = TRANSLATE(BF_SSN,'MYLAUGHTER','0987654321');
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
%KEY_CLC(AWGAP);

%MACRO SORT_IT(DS);
DATA &DS;
SET &DS;
IF DC_DOM_ST = '' THEN SVAR = 1;
ELSE SVAR = 2;
RUN;
%MEND SORT_IT;
%SORT_IT(AWGAP);

%MACRO CREATE_IT(DS,CPROC,REPNUM);
DATA REPIT;
SET &DS ;
WHERE &CPROC ;
RUN;

PROC SORT DATA=REPIT; BY SVAR DC_DOM_ST;RUN;

DATA _NULL_;
SET  REPIT;
FILE REPORT&REPNUM DELIMITER=',' DSD DROPOVER LRECL=32767;
   FORMAT BF_SSN $9. ;
   FORMAT DF_SPE_ACC_ID $10. ;
   FORMAT DM_PRS_1 $12. ;
   FORMAT DM_PRS_LST $35. ;
   FORMAT DX_STR_ADR_1 $35. ;
   FORMAT DX_STR_ADR_2 $35. ;
   FORMAT DM_CT $30. ;
   FORMAT DC_DOM_ST $2. ;
   FORMAT DF_ZIP $14. ;
   FORMAT COST_CENTER_CODE $6. ;
   FORMAT ACSKEY $18.;
   FORMAT ACO DOLLAR12.2;
IF _N_ = 1 THEN       
 DO;
   PUT
   'BF_SSN'
   ','
   'DF_SPE_ACC_ID'
   ','
   'DM_PRS_1'
   ','
   'DM_PRS_LST'
   ','
   'DX_STR_ADR_1'
   ','
   'DX_STR_ADR_2'
   ','
   'DM_CT'
   ','
   'DC_DOM_ST'
   ','
   'DF_ZIP'
   ','
   'ACSKEY'
   ','
   'TOTAL_AMOUNT_DUE'
   ','
   'STATE_IND'
   ','
   'COST_CENTER_CODE'
   ;
END;
DO;
   PUT BF_SSN $ @;
   PUT DF_SPE_ACC_ID $ @;
   PUT DM_PRS_1 $ @;
   PUT DM_PRS_LST $ @;
   PUT DX_STR_ADR_1 $ @;
   PUT DX_STR_ADR_2 $ @;
   PUT DM_CT $ @;
   PUT DC_DOM_ST $ @;
   PUT DF_ZIP $ @;
   PUT ACSKEY $ @;
   PUT ACO @;
   PUT DC_DOM_ST $ @;
   PUT COST_CENTER_CODE $ ;
 END;
RUN;
%MEND CREATE_IT;

%CREATE_IT(AWGAP,DL1NI_DT EQ .,2);
%CREATE_IT(AWGAP,DL1NI_DT LE &DCRT AND DL1NI_DT NE . AND DL2NI_DT EQ .,3);
%CREATE_IT(AWGAP,DL2NI_DT LE &DCRT AND DL2NI_DT NE . AND DL3NI_DT EQ .,4);
