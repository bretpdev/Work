/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWM30.LWM30RZ";
FILENAME REPORT2 "&RPTLIB/ULWM30.LWM30R2";
FILENAME REPORT3 "&RPTLIB/ULWM30.LWM30R3";
FILENAME REPORT4 "&RPTLIB/ULWM30.LWM30R4";
FILENAME REPORT5 "&RPTLIB/ULWM30.LWM30R5";
FILENAME REPORT6 "&RPTLIB/ULWM30.LWM30R6";
FILENAME REPORT7 "&RPTLIB/ULWM30.LWM30R7";
FILENAME REPORT9 "&RPTLIB/ULWM30.LWM30R9";
FILENAME REPORT10 "&RPTLIB/ULWM30.LWM30R10";
FILENAME REPORT11 "&RPTLIB/ULWM30.LWM30R11";
FILENAME REPORT12 "&RPTLIB/ULWM30.LWM30R12";

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;
CREATE TABLE LOANS AS
SELECT DISTINCT A.DF_PRS_ID_BR
	,B.AF_APL_ID||B.AF_APL_ID_SFX AS CLUID
	,C.LD_LDR_POF
	,C.LA_CLM_PRI 
	,C.LA_CLM_INT 
	,C.LA_PRI_COL 
	,C.LA_INT_ACR 
	,C.LA_INT_COL 
	,C.LA_LEG_CST_ACR 
	,C.LA_LEG_CST_COL 
	,C.LA_OTH_CHR_ACR 
	,C.LA_OTH_CHR_COL 
	,C.LA_COL_CST_ACR 
	,C.LA_COL_CST_COL
		,C.LI_DIR_PAY
		,C.LD_LST_BR_PAY 
		,C.LD_NXT_PAY_DUE
	,COALESCE(D.LA_CLM_INT_ACR,0) AS LA_CLM_INT_ACR
	,COALESCE(D.LA_CLM_PRJ_COL_CST,0) AS LA_CLM_PRJ_COL_CST
	,C.LC_GRN
	,E.DF_SPE_ACC_ID
	,E.DM_PRS_1
	,E.DM_PRS_LST
	,E.DI_PHN_VLD
	,F.DX_STR_ADR_1
	,F.DX_STR_ADR_2
	,F.DM_CT
	,F.DC_DOM_ST
	,F.DF_ZIP
	,F.DM_FGN_CNY
	,F.DI_VLD_ADR 
	,F.DC_ADR
	,G.BD_ATY_PRF
	,G.PF_ACT
	,(C.LA_CLM_PRI 
		+C.LA_CLM_INT
		-C.LA_PRI_COL
		+C.LA_INT_ACR
		+D.LA_CLM_INT_ACR
		-C.LA_INT_COL)					
		 + (C.LA_LEG_CST_ACR
		-C.LA_LEG_CST_COL
		+C.LA_OTH_CHR_ACR
		-C.LA_OTH_CHR_COL
		+C.LA_COL_CST_ACR
		-C.LA_COL_CST_COL)
		+(D.LA_CLM_PRJ_COL_CST) AS TOT_PAYOFF
	,D.LR_CUR_INT / 1200 as RATE
	,111 as MONTHS
	,BR01.BN_RHB_PAY_CTR
	,C.LD_PCL_SUP_LST_ATT 
	,C.LD_PCL_SUP_LST_CNC
	,C.LC_RHB_CON
	,LA11.BC_LEG_ACT_ATY_TYP 
FROM DLGSUTWH.GA01_APP A
INNER JOIN DLGSUTWH.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN DLGSUTWH.DC01_LON_CLM_INF C
	ON B.AF_APL_ID = C.AF_APL_ID
	AND B.AF_APL_ID_SFX = C.AF_APL_ID_SFX
INNER JOIN DLGSUTWH.DC02_BAL_INT D
	ON C.AF_APL_ID = D.AF_APL_ID
	AND C.AF_APL_ID_SFX = D.AF_APL_ID_SFX
INNER JOIN DLGSUTWH.PD01_PDM_INF E
	ON C.BF_SSN = E.DF_PRS_ID
LEFT OUTER JOIN DLGSUTWH.LA11_LEG_ACT_ATY LA11
	ON A.DF_PRS_ID_BR = LA11.DF_PRS_ID_BR
	AND BC_LEG_ACT_ATY_TYP = 'JD' 
LEFT OUTER JOIN (
	SELECT DF_PRS_ID
		,DX_STR_ADR_1
		,DX_STR_ADR_2
		,DM_CT
		,DC_DOM_ST
		,DF_ZIP
		,DM_FGN_CNY
		,DI_VLD_ADR 
		,DC_ADR
	FROM DLGSUTWH.PD03_PRS_ADR_PHN 
	WHERE DC_ADR = 'L'
		AND DI_VLD_ADR = 'Y'
	) F
	ON E.DF_PRS_ID = F.DF_PRS_ID
LEFT OUTER JOIN (SELECT DF_PRS_ID
				,MAX(BD_ATY_PRF) AS BD_ATY_PRF
				,PF_ACT
			FROM DLGSUTWH.AY01_BR_ATY
			WHERE PF_ACT IN ('DLHB1','DLHB2','DLHB3','DLHB4','DLBH1','DRTRE','DRBWR')
			GROUP BY DF_PRS_ID, PF_ACT
	) G 
	ON E.DF_PRS_ID = G.DF_PRS_ID
INNER JOIN DLGSUTWH.BR01_BR_CRF BR01
	ON C.BF_SSN = BR01.BF_SSN
WHERE C.LC_STA_DC10 = '03'
	AND C.LC_AUX_STA = ''
	AND C.LD_CLM_ASN_DOE = .
	AND C.LC_PCL_REA IN ('DF','DQ','DB')
	AND C.LC_RHB_CON ^= '96'
	AND BR01.BC_RHB_NOT_ELG NOT IN ('IN','PR')
	AND CALCULATED TOT_PAYOFF >= 500
;
QUIT;
ENDRSUBMIT;
DATA LOANS;
	SET WORKLOCL.LOANS;
RUN;

*CALCULATE KEYLINE;
DATA LOANS (DROP = KEYSSN MODAY KEYLINE CHKDIG DIG I 
	CHKDIG CHK1 CHK2 CHK3 CHKDIGIT CHECK);
SET LOANS;
KEYSSN = TRANSLATE(DF_PRS_ID_BR,'MYLAUGHTER','0987654321');
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

/*NOW BREAK THIS TABLE INTO THE SEPARATE REPORTS*/
DATA R2(KEEP=DF_SPE_ACC_ID BN_RHB_PAY_CTR LI_DIR_PAY REHAB LD_NXT_PAY_DUE)
	R11(KEEP=DF_PRS_ID_BR BN_RHB_PAY_CTR)
	R3(KEEP=DF_PRS_ID_BR)
	R5(KEEP=DF_PRS_ID_BR)
	R6(KEEP=DF_PRS_ID_BR)
	R7(KEEP=DF_PRS_ID_BR LD_LST_BR_PAY)
	R12(KEEP=DF_PRS_ID_BR);
SET LOANS;
WHERE BN_RHB_PAY_CTR >= 3;
if pf_act in ('DRTRE','DRBWR') then rehab = 'Y';
if LC_RHB_CON EQ '' then rhb_con = 'Y';
IF BC_LEG_ACT_ATY_TYP = 'JD' THEN OUTPUT R11;
ELSE OUTPUT R2;
IF DI_PHN_VLD = 'Y'
	AND LD_LST_BR_PAY < TODAY() - 20
	AND TODAY()-5 <= LD_NXT_PAY_DUE <= TODAY()
	THEN DO;
		IF BN_RHB_PAY_CTR = 5 THEN OUTPUT R3;
		ELSE IF BN_RHB_PAY_CTR = 6 THEN OUTPUT R5;
		ELSE IF BN_RHB_PAY_CTR = 7 THEN DO;
					OUTPUT R6;
					IF BC_LEG_ACT_ATY_TYP = 'JD' THEN OUTPUT R12;
				END;
		ELSE IF BN_RHB_PAY_CTR = 8 THEN OUTPUT R7;
	END;
RUN;
PROC SORT DATA=R2 NODUPKEY; BY BN_RHB_PAY_CTR DF_SPE_ACC_ID;RUN;
PROC SORT DATA=R11 NODUPKEY; BY BN_RHB_PAY_CTR DF_PRS_ID_BR;RUN;

/*DATA FOR R4 AND R9 AND R10*/
%MACRO GET_SUMMARY(NEW_DS,REHB_CRT);
PROC SQL;
CREATE TABLE &NEW_DS AS
SELECT BN_RHB_PAY_CTR
	,SUM(TOT_PAYOFF) AS TOT_PAYOFF
	,COUNT(DISTINCT DF_PRS_ID_BR) AS BOR
FROM LOANS A
WHERE BN_RHB_PAY_CTR >= 3
	AND BC_LEG_ACT_ATY_TYP ^= 'JD'
	AND &REHB_CRT
GROUP BY BN_RHB_PAY_CTR;
QUIT;
%MEND GET_SUMMARY;
%GET_SUMMARY(R4,LC_RHB_CON ^= '96' and pf_act not in ('DRTRE','DRBWR'));
%GET_SUMMARY(R9, pf_act in ('DRTRE','DRBWR')); 
%GET_SUMMARY(R10,LC_RHB_CON EQ '' AND BC_LEG_ACT_ATY_TYP = 'JD');

/*--------------------------------------------------------------*/
PROC PRINTTO PRINT=REPORT2 NEW;
RUN;

/*For portrait reports;*/
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS CENTER NODATE NUMBER PAGENO=1 PS=52 LS=96;
%MACRO CONTENTS(REP,SET,TITLE);
PROC CONTENTS DATA=&SET OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 96*'-';
	PUT      ////////
		@35 '**** NO RECORDS FOUND ****';
	PUT ////////
		@38 '-- END OF REPORT --';
	PUT ////////////////
		@27 "JOB = UTLWM30     REPORT = UTLWM30.LWM30&REP";
	END;
RETURN;
TITLE &TITLE;
RUN;
%MEND;
%CONTENTS(R2,R2,'BORROWERS IN REHABILITATION');
PROC PRINT NOOBS SPLIT='/' DATA=R2;
by 	BN_RHB_PAY_CTR;
PAGEBY BN_RHB_PAY_CTR;
FORMAT LD_NXT_PAY_DUE MMDDYY10.;
VAR DF_SPE_ACC_ID BN_RHB_PAY_CTR LI_DIR_PAY LD_NXT_PAY_DUE REHAB ;
LABEL	DF_SPE_ACC_ID = 'ACCOUNT NUMBER'
		BN_RHB_PAY_CTR = 'REHAB COUNTER'
		LD_NXT_PAY_DUE = 'NEXT PAYMENT DUE'
		REHAB = 'REQUESTED TO REHAB'
;
TITLE 'BORROWERS IN REHABILITATION';
FOOTNOTE  'JOB = UTLWM30     REPORT = UTLWM30.LWM30R2';
RUN;

%macro que(r,q);
PROC SORT DATA=R&R NODUPKEY; BY DF_PRS_ID_BR;RUN;

DATA _NULL_;
SET  R&R;
FILE REPORT&R DELIMITER=',' DSD DROPOVER LRECL=32767;
IF _N_ = 1 THEN PUT 'SSN,QNAME,INST_ID,INT_TYPE,DUE_DATE,DUE_TIME,TEXT';
QNAME = "&Q";
CALL MISSING(INST_ID,INT_TYPE,DUE_DATE,DUE_TIME,TEXT);
DO;
   PUT DF_PRS_ID_BR $ @;
   PUT QNAME $ @;
   PUT INST_ID $ @;
   PUT INT_TYPE $ @;
   PUT DUE_DATE $ @;
   PUT DUE_TIME $ @;
   PUT TEXT $ ;
END;
RUN;
%mend;
%que(3,REHABCAN);
%que(5,RHBCAN6);
%que(6,RHBCAN7);
%que(7,RHBCAN8);
%que(12,RHBCANJD);


PROC PRINTTO PRINT=REPORT4 NEW;
RUN;
%CONTENTS(R4,R4,'Borrowers in Rehabilitation');

PROC REPORT DATA = R4 NOWD HEADLINE HEADSKIP SPLIT = '/' SPACING=2;
TITLE1	'Rehabilitation Totals';
FOOTNOTE  'JOB = UTLWM30     REPORT = UTLWM30.LWM30R4';
COLUMN BN_RHB_PAY_CTR BOR TOT_PAYOFF;
DEFINE 	BOR / ANALYSIS 'BORROWERS' WIDTH = 10;
DEFINE	BN_RHB_PAY_CTR / DISPLAY 'REHAB COUNT' WIDTH = 15;
DEFINE	TOT_PAYOFF / ANALYSIS FORMAT = DOLLAR15.2 'PAYOFF' WIDTH = 22;
COMPUTE AFTER;
	LINE ' ';
	LINE ' ';
	LINE 'TOTAL BORROWERS       - ' @55 BOR.SUM COMMA6.;
	LINE 'TOTAL PAYOFF AMOUNT   - ' @47 TOT_PAYOFF.SUM DOLLAR14.2;
ENDCOMP;
RUN;

PROC PRINTTO PRINT=REPORT9 NEW;
RUN;

%CONTENTS(R9,R9,"BORROWER'S WHO HAVE REQUESTED VERBAL REHAB");

PROC REPORT DATA = R9 NOWD HEADLINE HEADSKIP SPLIT = '/' SPACING=2;
TITLE1	"BORROWER'S WHO HAVE REQUESTED VERBAL REHAB";
FOOTNOTE  'JOB = UTLWM30     REPORT = UTLWM30.LWM30R9';
COLUMN BN_RHB_PAY_CTR BOR TOT_PAYOFF;
DEFINE	BN_RHB_PAY_CTR / DISPLAY 'REHAB COUNT' WIDTH = 15;
DEFINE 	BOR / ANALYSIS 'BORROWERS' WIDTH = 10;
DEFINE	TOT_PAYOFF / ANALYSIS FORMAT = DOLLAR15.2 'OUTSTANDING/PRINCIPAL/BALANCE' WIDTH = 22;
COMPUTE AFTER;
	LINE ' ';
	LINE ' ';
	LINE 'TOTAL BORROWERS       - ' @55 BOR.SUM COMMA6.;
	LINE 'TOTAL PAYOFF AMOUNT   - ' @47 TOT_PAYOFF.SUM DOLLAR14.2;
ENDCOMP;
RUN;
PROC PRINTTO PRINT=REPORT10 NEW;
RUN;

%CONTENTS(R10,R10,'Borrowers in Rehabilitation');

PROC REPORT DATA = R10 NOWD HEADLINE HEADSKIP SPLIT = '/' SPACING=2;
TITLE1	'Rehabilitation Totals';
FOOTNOTE  'JOB = UTLWM30     REPORT = UTLWM30.LWM30R10';
COLUMN BN_RHB_PAY_CTR BOR TOT_PAYOFF;
	DEFINE 	BOR / ANALYSIS 'BORROWERS' WIDTH = 10;
	DEFINE	BN_RHB_PAY_CTR / DISPLAY 'REHAB COUNT' WIDTH = 15;
	DEFINE	TOT_PAYOFF / ANALYSIS FORMAT = DOLLAR15.2 'PAYOFF' WIDTH = 22;
COMPUTE AFTER;
	LINE ' ';
	LINE ' ';
	LINE 'TOTAL BORROWERS       - ' @55 BOR.SUM COMMA6.;
	LINE 'TOTAL PAYOFF AMOUNT   - ' @47 TOT_PAYOFF.SUM DOLLAR14.2;
ENDCOMP;
RUN;

PROC PRINTTO PRINT=REPORT11 NEW;
RUN;

%CONTENTS(R11,R11,'Borrowers in Rehabilitation');

PROC PRINT NOOBS SPLIT='/' DATA=R11;
VAR DF_PRS_ID_BR BN_RHB_PAY_CTR;
LABEL	DF_PRS_ID_BR = 'SSN'
		BN_RHB_PAY_CTR = 'REHAB COUNTER';
TITLE 'Borrowers in Rehabilitation';
FOOTNOTE  'JOB = UTLWM30     REPORT = UTLWM30.LWM30R11';
RUN;
PROC PRINTTO;
RUN;