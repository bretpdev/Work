%LET RPTLIB = T:\SAS;
OPTIONS COMPRESS=BINARY;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
PROC SQL;
CONNECT TO DB2 (DATABASE=DNFPUTDL);
/****************************************************************************
QUEUE LEVEL DATA
*****************************************************************************/
CREATE TABLE WQ20 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT C.DF_SPE_ACC_ID 
	,A.BF_SSN
	,A.WX_MSG_1_TSK 
	,A.WX_MSG_2_TSK
	,A.WF_QUE
	,A.WF_SUB_QUE
	,A.WN_CTL_TSK
	,A.PF_REQ_ACT
	,A.WD_ACT_REQ
FROM PKUB.WQ20_TSK_QUE A
INNER JOIN PKUB.PD10_PRS_NME C
	ON A.BF_SSN = C.DF_PRS_ID
WHERE A.WF_QUE = 'R0'
	AND A.WF_SUB_QUE = '01'
FOR READ ONLY WITH UR
);
/****************************************************************************
LOAN LEVEL DATA
*****************************************************************************/
CREATE TABLE LN10 AS /*THIS TABLE IS MODIFIED INTO LN102 AT THE END OF THE REMOTE SUBMIT*/
SELECT DISTINCT A.DF_SPE_ACC_ID
	,B.*
FROM WQ20 A
LEFT OUTER JOIN CONNECTION TO DB2 (
	SELECT B.BF_SSN
		,B.LN_SEQ
		,B.IC_LON_PGM 
		,B.LC_STA_LON10
		,B.LA_CUR_PRI /*+ D.WA_TOT_BRI_OTS */ AS P_AND_I
		,B.LD_END_GRC_PRD 
		,F.LD_DSB_MAX 
		,B.LD_END_GRC_PRD + 30 DAYS AS LD_END_GRC_PRD_PLUS_1
		,F.LD_DSB_MAX + 60 DAYS AS LD_DSB_MAX_PLUS_60
/*		,COALESCE(B.LD_END_GRC_PRD + 30 DAYS,F.LD_DSB_MAX + 60 DAYS) AS RPS_START_DT*/
		,COALESCE(B.LD_END_GRC_PRD + 30 DAYS,B.LD_LON_1_DSB) AS RPS_START_DT

		,B.BF_SSN||CHAR(B.LN_SEQ) AS LID
		,D.WC_DW_LON_STA 
		,D.WD_LON_RPD_SR
		,C.LR_ITR
		,B.LC_STA_LON10 
		,CASE
			WHEN B.IC_LON_PGM IN 
				(
					'DLPCNS','DLSCNS','DLUCNS','DLSSPL','DLUSPL',
					'DLSCSL','DLSCSC','DLSCUC','DLSCCN'
				) THEN 1
			ELSE 0
		 END AS IS_CONSOL
		,G.LN_PRE_06_RPD_TRM
		,B.LD_LON_1_DSB
		,H.ORIG_LN_AMT
		,B.LD_LON_ACL_ADD
	FROM PKUB.LN10_LON B
	INNER JOIN PKUB.DW01_DW_CLC_CLU D
		ON B.BF_SSN = D.BF_SSN	
		AND B.LN_SEQ = D.LN_SEQ
	LEFT OUTER JOIN PKUB.LN72_INT_RTE_HST C
		ON B.BF_SSN = C.BF_SSN
		AND B.LN_SEQ = C.LN_SEQ
		AND C.LC_STA_LON72 = 'A'
		AND DAYS(CURRENT DATE) BETWEEN DAYS(C.LD_ITR_EFF_BEG) AND DAYS(C.LD_ITR_EFF_END)
	LEFT OUTER JOIN (
		SELECT BF_SSN
			,LN_SEQ
			,MAX(LD_DSB) AS LD_DSB_MAX
		FROM PKUB.LN15_DSB
		GROUP BY BF_SSN
			,LN_SEQ
		) F
		ON B.BF_SSN = F.BF_SSN
		AND B.LN_SEQ = F.LN_SEQ
	LEFT OUTER JOIN (
		SELECT BF_SSN
			,LN_SEQ
			,LN_PRE_06_RPD_TRM
		FROM PKUB.FS10_DL_LON
		) G
		ON B.BF_SSN = G.BF_SSN
		AND B.LN_SEQ = G.LN_SEQ
	LEFT OUTER JOIN (
		SELECT BF_SSN
			,LN_SEQ
			,SUM(LA_DSB - COALESCE(LA_DSB_CAN,0)) AS ORIG_LN_AMT
		FROM PKUB.LN15_DSB
		GROUP BY BF_SSN
			,LN_SEQ
		) H
		ON B.BF_SSN = H.BF_SSN
		AND B.LN_SEQ = H.LN_SEQ
	FOR READ ONLY WITH UR
	) B
	ON A.BF_SSN = B.BF_SSN;
/****************************************************************************
PAID AHEAD INDICATOR AT A BORROWER LEVEL
*****************************************************************************/
CREATE TABLE PAID_AHEAD AS
SELECT *
FROM CONNECTION TO DB2 (
	SELECT DISTINCT A.BF_SSN
		,'Y' AS IND
	FROM PKUB.BL10_BR_BIL A
	LEFT OUTER JOIN (
		SELECT AX.BF_SSN
			,AX.LD_BIL_DU AS MAX_CRT_DUE
		FROM PKUB.BL10_BR_BIL AX
		INNER JOIN (
			SELECT BF_SSN
				,MAX(LD_BIL_CRT) AS LD_BIL_CRT
			FROM PKUB.BL10_BR_BIL 
			WHERE LC_BIL_TYP = 'P'
			GROUP BY BF_SSN
			) B1
			ON AX.BF_SSN = B1.BF_SSN
			AND AX.LD_BIL_CRT = B1.LD_BIL_CRT
	) MAX_BIL_CRT
	ON A.BF_SSN = MAX_BIL_CRT.BF_SSN 
	LEFT OUTER JOIN (
		SELECT BF_SSN
			,MAX(LD_BIL_DU) AS LD_BIL_DU
		FROM PKUB.BL10_BR_BIL 
		WHERE LC_BIL_TYP = 'P'
		GROUP BY BF_SSN
		) MAX_BIL_DUE
		ON A.BF_SSN = MAX_BIL_DUE.BF_SSN
	WHERE MAX_BIL_DUE.LD_BIL_DU > MAX_BIL_CRT.MAX_CRT_DUE 
FOR READ ONLY WITH UR
);
/****************************************************************************
DEFERMENT/FORBEARANCE DATA AT A LOAN LEVEL 
*****************************************************************************/
CREATE TABLE DEF_FOR AS
SELECT DISTINCT A.DF_SPE_ACC_ID
	,B.*
FROM WQ20 A
INNER JOIN CONNECTION TO DB2 (
	SELECT DISTINCT LN50.BF_SSN
		,LN50.LN_SEQ
		,LN50.LF_DFR_CTL_NUM 	AS CTL_NUM
		,LN50.LN_DFR_OCC_SEQ 	AS OCC_SEQ
		,DF10.LC_DFR_TYP 		AS TYP
		,'D' 					AS DEF_FOR
		,LN50.LD_DFR_BEG 		AS LD_BEG
		,LN50.LD_DFR_END	    AS LD_END
	FROM PKUB.DF10_BR_DFR_REQ DF10
	INNER JOIN PKUB.LN50_BR_DFR_APV LN50  
		ON DF10.BF_SSN = LN50.BF_SSN
		AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM 
	WHERE DF10.LC_DFR_STA = 'A'
		AND DF10.LC_STA_DFR10 = 'A'
		AND LN50.LC_STA_LON50 = 'A'
UNION 
	SELECT DISTINCT LN60.BF_SSN
		,LN60.LN_SEQ
		,LN60.LF_FOR_CTL_NUM 	AS CTL_NUM
		,LN60.LN_FOR_OCC_SEQ 	AS OCC_SEQ
		,FB10.LC_FOR_TYP 		AS TYP
		,'F' 					AS DEF_FOR
		,LN60.LD_FOR_BEG 		AS LD_BEG
		,LN60.LD_FOR_END		AS LD_END
	FROM PKUB.FB10_BR_FOR_REQ FB10
	INNER JOIN PKUB.LN60_BR_FOR_APV LN60  
		ON FB10.BF_SSN = LN60.BF_SSN
		AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM 
	WHERE FB10.LC_FOR_STA = 'A'
		AND FB10.LC_STA_FOR10 = 'A'
		AND LN60.LC_STA_LON60 = 'A'
FOR READ ONLY WITH UR
) B
	ON A.BF_SSN = B.BF_SSN;
/****************************************************************************
REPAYMENT LEVEL DATA 
*****************************************************************************/
CREATE TABLE Active_RPS AS
SELECT DISTINCT A.DF_SPE_ACC_ID
	,B.*
FROM LN10 A
LEFT OUTER JOIN CONNECTION TO DB2 (
	SELECT B.BF_SSN
		,B.LN_SEQ
		,B.LN_RPS_SEQ
		,B.LN_GRD_RPS_SEQ
		,B.LN_RPS_TRM
		,B.LA_RPS_ISL
		,C.LD_RPS_1_PAY_DU
		,A.LC_TYP_SCH_DIS
		,A.LC_STA_LON65
	FROM PKUB.LN65_LON_RPS A
	INNER JOIN PKUB.LN66_LON_RPS_SPF B
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.LN_SEQ
		AND A.LN_RPS_SEQ = B.LN_RPS_SEQ
	INNER JOIN PKUB.RS10_BR_RPD C
		ON B.BF_SSN = C.BF_SSN
		AND B.LN_RPS_SEQ = C.LN_RPS_SEQ
	WHERE A.LC_STA_LON65 = 'A'
	FOR READ ONLY WITH UR
	) B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
ORDER BY BF_SSN, LN_SEQ, LN_RPS_SEQ;

CREATE TABLE Inactive_RPS AS
SELECT DISTINCT A.DF_SPE_ACC_ID
	,B.*
FROM LN10 A
LEFT OUTER JOIN CONNECTION TO DB2 (
	SELECT B.BF_SSN
		,B.LN_SEQ
		,B.LN_RPS_SEQ
		,B.LN_GRD_RPS_SEQ
		,B.LN_RPS_TRM
		,B.LA_RPS_ISL
		,C.LD_RPS_1_PAY_DU
		,A.LC_TYP_SCH_DIS
		,A.LC_STA_LON65
	FROM PKUB.LN65_LON_RPS A
	INNER JOIN PKUB.LN66_LON_RPS_SPF B
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.LN_SEQ
		AND A.LN_RPS_SEQ = B.LN_RPS_SEQ
	INNER JOIN PKUB.RS10_BR_RPD C
		ON B.BF_SSN = C.BF_SSN
		AND B.LN_RPS_SEQ = C.LN_RPS_SEQ
	INNER JOIN (
		SELECT BF_SSN
			,LN_SEQ
			,MAX(LN_RPS_SEQ) AS LN_RPS_SEQ
		FROM PKUB.LN65_LON_RPS
		WHERE LC_STA_LON65 = 'I'
		GROUP BY BF_SSN
			,LN_SEQ
		) D
		ON A.BF_SSN = D.BF_SSN
		AND A.LN_SEQ = D.LN_SEQ
		AND A.LN_RPS_SEQ = D.LN_RPS_SEQ
	WHERE A.LC_STA_LON65 = 'I'
	FOR READ ONLY WITH UR
	) B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
LEFT OUTER JOIN Active_RPS C
	ON A.BF_SSN = C.BF_SSN
	AND A.LN_SEQ = C.LN_SEQ 
WHERE C.BF_SSN IS NULL
	AND C.LN_SEQ IS NULL
ORDER BY BF_SSN, LN_SEQ, LN_RPS_SEQ;

DISCONNECT FROM DB2;
QUIT;

DATA RPS;
	SET Active_RPS Inactive_RPS;
	WHERE BF_SSN ^= '';
RUN;

PROC SORT DATA=RPS NODUPKEY;
	BY BF_SSN LN_SEQ LN_RPS_SEQ LN_GRD_RPS_SEQ;
RUN;
DATA RPS;
	FORMAT GRD_BEG_DT GRD_END_DT MMDDYY10.;
	SET RPS;
	BY BF_SSN LN_SEQ LN_RPS_SEQ;
	RETAIN GRD_END_DT;
	IF FIRST.LN_RPS_SEQ THEN DO;
		GRD_BEG_DT = LD_RPS_1_PAY_DU;
		GRD_END_DT = INTNX('MONTH',LD_RPS_1_PAY_DU,+LN_RPS_TRM,'S');
	END;
	ELSE DO;
		GRD_BEG_DT = GRD_END_DT;
		GRD_END_DT = INTNX('MONTH',GRD_BEG_DT,+LN_RPS_TRM,'S');
	END;
	IF GRD_BEG_DT <= DATE() <= GRD_END_DT THEN USE=1; 
RUN;
/*CREATE FINAL REPAYMNET AND LOAN TABLES*/
PROC SQL;
	CREATE TABLE CUR_RPS_GRD AS 
	SELECT DISTINCT A.*
	FROM RPS A
	WHERE USE = 1;

	CREATE TABLE NON_CUR_RPS_GRD AS 
	SELECT DISTINCT A.*
	FROM RPS A
	LEFT OUTER JOIN CUR_RPS_GRD B
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.LN_SEQ
	WHERE B.BF_SSN IS NULL
		AND B.LN_SEQ IS NULL
		AND A.LN_GRD_RPS_SEQ = 1;

	CREATE TABLE RPS(KEEP=DF_SPE_ACC_ID BF_SSN LN_SEQ LA_RPS_ISL LC_TYP_SCH_DIS) AS 
			SELECT * FROM CUR_RPS_GRD
		UNION 
			SELECT * FROM NON_CUR_RPS_GRD;

	 CREATE TABLE LN102 AS
		SELECT DISTINCT A.*,B.ORIG_LN_AMT 	
		FROM LN10(DROP=ORIG_LN_AMT) A
		INNER JOIN ( SELECT DF_SPE_ACC_ID,SUM(ORIG_LN_AMT) AS ORIG_LN_AMT FROM LN10 GROUP BY DF_SPE_ACC_ID ) B
			ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID;
/*CLEAN UP THE WORK LIB*/
	DROP TABLE LN10;
	DROP TABLE CUR_RPS_GRD ;
	DROP TABLE NON_CUR_RPS_GRD ;
QUIT;
ENDRSUBMIT;

DATA WQ20;SET LEGEND.WQ20;RUN;
DATA LN10;SET LEGEND.LN102;RUN; /*NOTICE THAT THE REMOTE DATA SET IS LN102 AND IT IS BROUGHT DOWN AS LN10*/
DATA DEF_FOR;SET LEGEND.DEF_FOR;RUN;
DATA RPS; SET LEGEND.RPS;RUN;
DATA PAID_AHEAD; SET LEGEND.PAID_AHEAD;RUN;

/*CALCULATE A NEW PAYMENT AT A LOAN LEVEL*/
PROC SORT DATA=DEF_FOR NODUPKEY;
	BY BF_SSN LN_SEQ CTL_NUM OCC_SEQ DEF_FOR;
RUN;

PROC SORT DATA=DEF_FOR OUT=IN_DEF_FOR(KEEP=BF_SSN DF_SPE_ACC_ID) NODUPKEY;
	BY BF_SSN;
	WHERE LD_BEG <= TODAY() <= LD_END;
RUN;

PROC SQL;
	CREATE TABLE DEF_FOR_BX AS 
	SELECT DISTINCT C.BF_SSN
		,C.LN_SEQ
		,C.DF_SPE_ACC_ID
		,C.LID
		,C.RPS_START_DT
		,C.P_AND_I	
		,INTCK('MONTH',C.RPS_START_DT,TODAY()) 								AS GORSS_RPS_USED
		,COALESCE(D.TOT_DEF_FOR,0) 											AS TOT_DEF_FOR
		,INTCK('MONTH',C.RPS_START_DT,TODAY()) - COALESCE(D.TOT_DEF_FOR,0)	AS NET_RPS_USED 
		,INTCK('MONTH',C.LD_LON_1_DSB,TODAY()) - COALESCE(D.TOT_DEF_FOR,0)	AS NET_RPS_USED_CONSOL 
		,360 - CALCULATED NET_RPS_USED 										AS PAYMENT_TERM
		,(LR_ITR/100)/12 													AS ADJ_INT_RATE
		,(LR_ITR/100)	 													AS INT_RATE
		,COALESCE(E.LA_RPS_ISL,0)											AS EXISTING_PAYMENT
		,E.LC_TYP_SCH_DIS													AS RPMT_TYP
		,C.IC_LON_PGM
		,C.IS_CONSOL
		,C.LN_PRE_06_RPD_TRM
		,C.LD_LON_1_DSB
		,C.ORIG_LN_AMT
	FROM LN10 C
	LEFT OUTER JOIN DEF_FOR A
		ON C.BF_SSN = A.BF_SSN
		AND C.LN_SEQ = A.LN_SEQ
	LEFT OUTER JOIN IN_DEF_FOR B
		ON A.BF_SSN = B.BF_SSN		
	LEFT OUTER JOIN (
		SELECT BF_SSN
			,LN_SEQ
			,SUM(INTCK('MONTH',LD_BEG,LD_END)) AS TOT_DEF_FOR
		FROM DEF_FOR
		GROUP BY BF_SSN
			,LN_SEQ
		) D
		ON A.BF_SSN = D.BF_SSN
		AND A.LN_SEQ = D.LN_SEQ
	LEFT OUTER JOIN RPS E
		ON C.BF_SSN = E.BF_SSN
		AND C.LN_SEQ = E.LN_SEQ
	WHERE B.BF_SSN IS NULL
		AND C.P_AND_I > 0
	ORDER BY C.BF_SSN, C.LID;
QUIT;

DATA ACT_RPS_SCHD;
	SET DEF_FOR_BX(DROP=PAYMENT_TERM);
	BASE_DATE = MDY(07,01,2006);

	IF RPMT_TYP IN ('PL','PG') THEN DO;
		IF ORIG_LN_AMT < 10000 THEN BASE_TERM = 144;
			ELSE IF 10000 <= ORIG_LN_AMT < 20000 THEN BASE_TERM = 180;
			ELSE IF 20000 <= ORIG_LN_AMT < 40000 THEN BASE_TERM = 240;
			ELSE IF 40000 <= ORIG_LN_AMT < 60000 THEN BASE_TERM = 300;
			ELSE BASE_TERM = 360;
/*		BASE_TERM = LN_PRE_06_RPD_TRM;*/
	END;

	ELSE IF RPS_START_DT < BASE_DATE & (RPMT_TYP = 'L' & IS_CONSOL | RPMT_TYP IN ('EL','G'))THEN DO;
		IF RPMT_TYP = 'L' THEN BASE_TERM = 120;
		ELSE DO;
			IF ORIG_LN_AMT < 10000 THEN BASE_TERM = 156;
			ELSE IF 10000 <= ORIG_LN_AMT < 20000 THEN BASE_TERM = 180;
			ELSE IF 20000 <= ORIG_LN_AMT < 40000 THEN BASE_TERM = 240;
			ELSE IF 40000 <= ORIG_LN_AMT < 60000 THEN BASE_TERM = 300;
			ELSE BASE_TERM = 360;
		END;
	END;

	ELSE IF RPS_START_DT >= BASE_DATE AND (RPMT_TYP = 'G' | RPMT_TYP = 'L' & IS_CONSOL) THEN DO;
		IF IS_CONSOL THEN DO;
			IF ORIG_LN_AMT < 7500 THEN BASE_TERM = 120;
			ELSE IF 7500 <= ORIG_LN_AMT < 10000 THEN BASE_TERM = 144;
			ELSE IF 10000 <= ORIG_LN_AMT < 20000 THEN BASE_TERM = 180;
			ELSE IF 20000 <= ORIG_LN_AMT < 40000 THEN BASE_TERM = 240;
			ELSE IF 40000 <= ORIG_LN_AMT < 60000 THEN BASE_TERM = 300;
			ELSE BASE_TERM = 360;
		END;
		ELSE IF RPMT_TYP = 'G' THEN BASE_TERM = 120;
	END;

	ELSE IF RPMT_TYP = 'EG' 
		AND LD_LON_1_DSB > = MDY(10,7,1998) 
		AND RPS_START_DT >= BASE_DATE  
		AND ORIG_LN_AMT > 30000 
	THEN BASE_TERM = 300;
	
	ELSE IF RPMT_TYP = 'L' AND ^IS_CONSOL THEN BASE_TERM = 120;
	ELSE IF RPMT_TYP = 'FS' THEN BASE_TERM = 360;
	ELSE BASE_TERM = 0;

	IF IS_CONSOL THEN 
		PAYMENT_TERM = BASE_TERM - NET_RPS_USED_CONSOL;
	ELSE 
		PAYMENT_TERM = BASE_TERM - NET_RPS_USED;
RUN;
%MACRO RPS_PAYMENT_CALCS(NDS,IDS);
	PROC SORT DATA=&IDS ;
		BY BF_SSN;
	RUN;
	DATA &NDS(KEEP=DF_SPE_ACC_ID BF_SSN INT_RATE PAYMENT_TERM BOR_PAYMENT XPAY BOR_TOT DIF);
		SET &IDS;
		BY BF_SSN; 
		RETAIN BOR_PAYMENT XPAY BOR_TOT;
		LN_PMT = PMT(ADJ_INT_RATE,PAYMENT_TERM,P_AND_I);
		IF FIRST.BF_SSN THEN DO;
			BOR_PAYMENT = LN_PMT;
			XPAY = EXISTING_PAYMENT;
			BOR_TOT = P_AND_I;
		END;
		ELSE DO;
			BOR_PAYMENT = BOR_PAYMENT + LN_PMT;
			XPAY = XPAY + EXISTING_PAYMENT;
			BOR_TOT = BOR_TOT + P_AND_I ;
		END;
		DIF = ROUND(XPAY - BOR_PAYMENT,.01);
		IF LAST.BF_SSN;
	RUN;
%MEND RPS_PAYMENT_CALCS;
%RPS_PAYMENT_CALCS(DEF_FOR_BX2,DEF_FOR_BX);/*FS SCHEDULE*/
%RPS_PAYMENT_CALCS(ACT_RPS_SCHD2,ACT_RPS_SCHD);/*ACTUAL REPAYMENT TYPE*/
PROC SORT DATA=RPS ;
	BY BF_SSN;
RUN;
DATA RPS_BOR(KEEP=DF_SPE_ACC_ID BF_SSN BOR_PAYMENT LC_TYP_SCH_DIS);
	SET RPS;
	BY BF_SSN;
	RETAIN BOR_PAYMENT;
	LN_PMT = LA_RPS_ISL;
	IF FIRST.BF_SSN THEN BOR_PAYMENT = LN_PMT;
	ELSE BOR_PAYMENT = BOR_PAYMENT + LN_PMT;
	IF LAST.BF_SSN;
RUN;
PROC FORMAT ;
	VALUE $LNSTA '01' = 'In Grace'
		'02' = 'In School'
		'03' = 'Repayment'
		'04' = 'Deferment'
		'05' = 'Forbearance'
		'06' = 'Cure'
		'07' = 'Claim Pending'
		'08' = 'Claim Submitted'
		'09' = 'Claim Cncl'
		'10' = 'Claim Reject'
		'11' = 'Claim Rtn'
		'12' = 'Claim Paid'
		'13' = 'PreClaim Pending'
		'14' = 'PreClaim Submitted'
		'15' = 'PreClaim Cncl'
		'16' = 'Alleged Death'
		'17' = 'Verified Death'
		'18' = 'Alleged Disability'
		'19' = 'Verified Disability'
		'20' = 'Alleged Bankruptcy'
		'21' = 'Verified Bankruptcy'
		'22' = 'PIF'
		'23' = 'Not Fully Originated'
		'88' = 'Processing Error'
		'98' = 'Unknown';
RUN;
PROC SORT DATA=LN10 OUT=LN_TYP(KEEP=DF_SPE_ACC_ID LN_SEQ IC_LON_PGM) NODUPKEY;
	BY DF_SPE_ACC_ID IC_LON_PGM;
RUN;
PROC SORT DATA=LN10 OUT=LN_STA(KEEP=DF_SPE_ACC_ID LN_SEQ WC_DW_LON_STA) NODUPKEY;
	BY DF_SPE_ACC_ID WC_DW_LON_STA;
RUN;
DATA LN_STA;
	SET LN_STA;
	LENGTH STA_DESC $25.;
	STA_DESC = PUT(WC_DW_LON_STA,$LNSTA.);
RUN;
%MACRO ONE_LINER(DS,LVAR,IVAR); 
DATA &DS(KEEP=DF_SPE_ACC_ID &LVAR);
	SET &DS;
	LENGTH &LVAR $5000. ;
	RETAIN &LVAR ;
	BY DF_SPE_ACC_ID;
	IF FIRST.DF_SPE_ACC_ID THEN &LVAR = &IVAR;
	ELSE &LVAR = CATX(' ',&LVAR,&IVAR);
	IF LAST.DF_SPE_ACC_ID;
RUN;
%MEND; 
%ONE_LINER(LN_STA,STA_LIST,STA_DESC);
%ONE_LINER(LN_TYP,LN_LIST,IC_LON_PGM);

/*EXCLUSION DATA SETS*/
PROC SQL;
CREATE TABLE MULT_RPS_OR_START AS
	SELECT DISTINCT C1.DF_SPE_ACC_ID
		,'MULTIPLE' AS EXCLUSION_REASON
		,1 AS EXCLD
	FROM LN10 C1
	INNER JOIN LN10 C2
		ON C1.DF_SPE_ACC_ID = C2.DF_SPE_ACC_ID
	WHERE C1.RPS_START_DT ^= C2.RPS_START_DT 
UNION 
	SELECT DISTINCT D1.DF_SPE_ACC_ID
		,'MULTIPLE' AS EXCLUSION_REASON
		,2 AS EXCLD
	FROM RPS D1
	INNER JOIN RPS D2
		ON D1.DF_SPE_ACC_ID = D2.DF_SPE_ACC_ID
	WHERE D1.LC_TYP_SCH_DIS ^= D2.LC_TYP_SCH_DIS 
UNION 
	SELECT DISTINCT DF_SPE_ACC_ID
		,'RPS TYPE' AS EXCLUSION_REASON
		,3 AS EXCLD
	FROM RPS 
	WHERE LC_TYP_SCH_DIS IN ('C1','C2','C3','CL','IB','IL')
UNION 
	SELECT DISTINCT DF_SPE_ACC_ID
		,'PIF' AS EXCLUSION_REASON
		,4 AS EXCLD
	FROM (
		SELECT DF_SPE_ACC_ID
			,SUM(P_AND_I) AS BOR_BAL
		FROM LN10
		GROUP BY DF_SPE_ACC_ID
		HAVING SUM(P_AND_I) = 0
		)
UNION 
	SELECT DISTINCT DF_SPE_ACC_ID
		,'DEF OR FORB' AS EXCLUSION_REASON
		,5 AS EXCLD
	FROM IN_DEF_FOR
;
QUIT;
PROC SORT DATA=MULT_RPS_OR_START OUT=A NODUPKEY;
	BY DF_SPE_ACC_ID;
RUN;
/***********************************************
PUT EVERYTHING TOGETHER
************************************************/
PROC SQL;
	CREATE TABLE DETAIL AS 
	SELECT DISTINCT A.DF_SPE_ACC_ID AS AccountNumber
		,A.BF_SSN
		,A.WF_QUE 
		,A.WF_SUB_QUE 
		,A.PF_REQ_ACT AS ErrorARCId	
		,CATX(' ',A.WX_MSG_1_TSK,A.WX_MSG_2_TSK) AS ErrorARCDescription 
		,A.WN_CTL_TSK 
		,A.WD_ACT_REQ AS ARCAddDate 
		,B2.LN_LIST AS LoanType
		,B.STA_LIST AS LoanStatus 
		,CASE 
			WHEN C.DF_SPE_ACC_ID IS NULL THEN PUT(A2.RPS_START_DT,MMDDYY10.)
			ELSE C.EXCLUSION_REASON
		 END AS RepaymentStartDate LENGTH=10
		,CASE 
			WHEN C.DF_SPE_ACC_ID IS NULL THEN H.LC_TYP_SCH_DIS
			ELSE C.EXCLUSION_REASON
		 END AS RepaymentPlan LENGTH=10
		,K.BBAL 
		,F.REP_TERM
		,E.DIF AS RS_PAYMENT_DIF
		,E.XPAY AS CUR_PAYMENT
		,COALESCE(C.EXCLD,0) AS EXCLD
		,L.DIF AS PAYMENT_DIF
		,L.BOR_PAYMENT AS ACT_CALC_REPAY_AMT
		,L.PAYMENT_TERM AS REM_PAYMENT_TERMS
		,COALESCE(M.IND,'N') AS PAI
		,A2.LD_LON_ACL_ADD
		,coalesce(n.ln_pre_06_rpd_trm,0) as ln_pre_06_rpd_trm
	FROM WQ20 A
	LEFT OUTER JOIN LN_STA B
		ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
	LEFT OUTER JOIN LN_TYP B2
		ON A.DF_SPE_ACC_ID = B2.DF_SPE_ACC_ID
	LEFT OUTER JOIN LN10 A2
		ON A.BF_SSN = A2.BF_SSN
	LEFT OUTER JOIN MULT_RPS_OR_START C
		ON A.DF_SPE_ACC_ID = C.DF_SPE_ACC_ID
	LEFT OUTER JOIN DEF_FOR_BX2 E
		ON A.BF_SSN = E.BF_SSN
	LEFT OUTER JOIN RPS_BOR H
		ON A.BF_SSN = H.BF_SSN
	LEFT OUTER JOIN (
		SELECT BF_SSN
			,MAX(NET_RPS_USED) AS REP_TERM
		FROM DEF_FOR_BX
		GROUP BY BF_SSN
		) F
		ON A.BF_SSN = F.BF_SSN
	LEFT OUTER JOIN IN_DEF_FOR G
		ON A.BF_SSN = G.BF_SSN
	LEFT OUTER JOIN (
		SELECT BF_SSN
			,SUM(P_AND_I) AS BBAL
		FROM LN10
		GROUP BY BF_SSN
		) K
		ON A.BF_SSN = K.BF_SSN
	LEFT OUTER JOIN ACT_RPS_SCHD2 L
		ON A.BF_SSN = L.BF_SSN
	LEFT OUTER JOIN PAID_AHEAD M
		ON A.BF_SSN = M.BF_SSN
	left outer join (
		select bf_ssn
			,max(ln_pre_06_rpd_trm) as ln_pre_06_rpd_trm
		from ln10
		group by bf_ssn
		) n
		on a.bf_ssn = n.bf_ssn


	WHERE G.BF_SSN IS NULL
	;
QUIT;

PROC SQL;
CREATE TABLE ALL_DETAIL AS 
SELECT DISTINCT CATT(A.WF_QUE,A.WF_SUB_QUE,A.PF_REQ_ACT,A.WN_CTL_TSK,A.WX_MSG_1_TSK,A.WX_MSG_2_TSK) AS natl_key 
	,A.PF_REQ_ACT
	,CATX(' ',A.WX_MSG_1_TSK,A.WX_MSG_2_TSK) AS WX_MSG_TSK 
	,A.BF_SSN
	,B.LID
FROM WQ20 A
INNER JOIN LN10 B
	ON A.BF_SSN = B.BF_SSN
INNER JOIN DETAIL C
	ON A.BF_SSN = C.BF_SSN
	AND	A.WF_QUE = C.WF_QUE 
	AND	A.WF_SUB_QUE = C.WF_SUB_QUE 
	AND	A.PF_REQ_ACT = C.ErrorARCId   	
	AND	A.WN_CTL_TSK = C.WN_CTL_TSK
;
QUIT;

PROC SORT DATA=ALL_DETAIL OUT=QLEV(KEEP=natl_key) NODUPKEY; BY natl_key; RUN;
PROC SORT DATA=ALL_DETAIL OUT=BORS(KEEP=natl_key BF_SSN) NODUPKEY; BY BF_SSN natl_key;RUN;
PROC SORT DATA=ALL_DETAIL OUT=LONS(KEEP=LID natl_key) NODUPKEY;BY LID natl_key;RUN;

PROC SQL;
	CREATE TABLE SUMRY AS 
	SELECT DISTINCT A.natl_key
		,A.PF_REQ_ACT
		,A.WX_MSG_TSK 
		,D.Q_CT
		,B.BOR_CT
		,C.LON_CT
	FROM ALL_DETAIL A
	INNER JOIN (SELECT natl_key ,COUNT(*) AS Q_CT   FROM QLEV GROUP BY natl_key) D ON A.natl_key = D.natl_key
	INNER JOIN (SELECT natl_key ,COUNT(*) AS BOR_CT FROM BORS GROUP BY natl_key) B ON A.natl_key = B.natl_key
	INNER JOIN (SELECT natl_key ,COUNT(*) AS LON_CT FROM LONS GROUP BY natl_key) C ON A.natl_key = C.natl_key
;
QUIT;

PROC SQL;
CREATE TABLE R4 AS 
SELECT DISTINCT B.DF_SPE_ACC_ID
	,B.PF_REQ_ACT
	,CATX(' ',B.WX_MSG_1_TSK,B.WX_MSG_2_TSK) AS WX_MSG_TSK 
	,A.LoanType
	,A.LoanStatus
FROM DETAIL A
INNER JOIN WQ20 B
	ON A.BF_SSN = B.BF_SSN
WHERE A.EXCLD IN (4,5)
;
QUIT;

PROC REPORT DATA=DETAIL OUT=DETAIL_OUT(DROP=_BREAK_) NOWD SPLIT='~' HEADSKIP;
	WHERE EXCLD = 0;
/*	WHERE EXCLD IN (0,1);*/
	COLUMN AccountNumber BF_SSN ErrorARCId ErrorARCDescription ARCAddDate LoanType LoanStatus RepaymentStartDate 
		RepaymentPlan BBAL REP_TERM RS_PAYMENT_DIF CUR_PAYMENT ACT_CALC_REPAY_AMT PAYMENT_DIF PAI REM_PAYMENT_TERMS
		LD_LON_ACL_ADD ln_pre_06_rpd_trm;
	DEFINE AccountNumber / DISPLAY 'Account number' NOPRINT;
	DEFINE BF_SSN / DISPLAY 'BF_SSN' NOPRINT;
	DEFINE ErrorARCId / DISPLAY 'Error ARC ID' NOPRINT;
	DEFINE ErrorARCDescription / DISPLAY 'Error ARC Description' NOPRINT;
	DEFINE ARCAddDate / DISPLAY 'ARC Add Date' FORMAT=MMDDYY10. NOPRINT ;
	DEFINE LoanType / DISPLAY 'Loan Type' width=102 flow NOPRINT;
	DEFINE LoanStatus/ DISPLAY 'Loan Status' NOPRINT;
	DEFINE RepaymentStartDate / 'Repayment Start Date' NOPRINT;
	DEFINE RepaymentPlan / 'Repayment Plan' NOPRINT;
	DEFINE BBAL / 'Borrower Balance' FORMAT=Dollar10.2 NOPRINT;
	DEFINE REP_TERM / 'Repayment Term Used' NOPRINT;
	DEFINE RS_PAYMENT_DIF / 'Current Payment - Calculated Payment(FS)' FORMAT=Dollar10.2 NOPRINT;
	DEFINE CUR_PAYMENT / 'Current Payment' FORMAT=Dollar10.2 NOPRINT;
	DEFINE ACT_CALC_REPAY_AMT / 'New Calculated Pay Amount' FORMAT=Dollar10.2 NOPRINT;
	DEFINE PAYMENT_DIF / 'Current Payment - Calculated Payment(Existing Repayment Type)' FORMAT=Dollar10.2 NOPRINT;
	DEFINE PAI / 'Paid Ahead Indicator' noprint;
	DEFINE REM_PAYMENT_TERMS / 'Remaining Payment Terms(Actual)' NOPRINT;
	DEFINE LD_LON_ACL_ADD / 'Loan Add Date' NOPRINT;
	define ln_pre_06_rpd_trm / 'ln_pre_06_rpd_trm' noprint;
RUN;

DATA R5 R6;
	SET DETAIL;
/*	WHERE EXCLD = 2;*/
	WHERE EXCLD IN (1,2);
	IF EXCLD = 2 THEN OUTPUT R5;
	ELSE OUTPUT R6;
RUN;

PROC SQL;
CREATE TABLE R5_ALL AS
SELECT DISTINCT A.AccountNumber
	,B.*
FROM R5 A
INNER JOIN RPS B
	ON A.BF_SSN = B.BF_SSN
;
QUIT;

PROC SQL;
CREATE TABLE R6_ALL AS
SELECT DISTINCT A.AccountNumber
	,B.BF_SSN
	,B.LN_SEQ
	,A.ERRORARCID
	,A.ERRORARCDESCRIPTION
	,A.ARCADDDATE
	,B.IC_LON_PGM AS LOANTYPE
	,A.LOANSTATUS
	,B.RPS_START_DT AS REPAYMENTSTARTDATE
	,R.LC_TYP_SCH_DIS AS REPAYMENTPLAN
	,A.BBAL
	,A.REP_TERM
	,A.RS_PAYMENT_DIF
	,A.CUR_PAYMENT
	,A.ACT_CALC_REPAY_AMT
	,A.PAYMENT_DIF
	,A.PAI
	,A.REM_PAYMENT_TERMS
	,A.LD_LON_ACL_ADD
	,A.LN_PRE_06_RPD_TRM
FROM R6 A
INNER JOIN LN10 B
	ON A.BF_SSN = B.BF_SSN
LEFT JOIN RPS R
	ON A.BF_SSN = R.BF_SSN
	AND B.LN_SEQ = R.LN_SEQ
INNER JOIN (
			SELECT
				A.BF_SSN
				,MAX(A.ARCADDDATE) AS ARCADDDATE
			FROM
				R6 A
			GROUP BY 
				A.BF_SSN
			) MAX
	ON A.BF_SSN = MAX.BF_SSN
	AND A.ARCADDDATE = MAX.ARCADDDATE	
left join ln10 z
	on a.bf_ssn = z.bf_ssn
	and b.ln_seq = z.ln_seq
	and z.wc_dw_lon_sta = '22'
left join r4
	on a.AccountNumber = r4.df_spe_acc_id
where (z.bf_ssn is null and z.ln_seq is null)
	and r4.df_spe_acc_id is null
;
QUIT;

proc sql;
create table r6_mid as
select
	a.*
	,count(*) as dup_lns
from
	r6_all a
group by 
	a.accountnumber
	,a.bf_ssn
	,a.ln_seq
;
quit;

proc sql;
create table r6_fin as
select
	a.*
from
	r6_mid a
	left join (
				select
					b.bf_ssn
				from 
					r6_mid b
				where 
					b.dup_lns ^= 1
					) dup
		on a.bf_ssn = dup.bf_ssn
where dup.bf_ssn is null
;
quit;

data r6_fin;
	set r6_fin;
	drop dup_lns;
run;

proc sql;
create table r7_fin as
select
	a.*
from
	r6_mid a
	left join r6_fin b
		on a.bf_ssn = b.bf_ssn
where b.bf_ssn is null
; 
quit;

data r7_fin;
	set r7_fin;
	drop dup_lns;
run;

PROC REPORT DATA=SUMRY OUT=SUMRY_OUT(DROP=_BREAK_) NOWD SPLIT='~' HEADSKIP;
	COLUMN PF_REQ_ACT WX_MSG_TSK Q_CT BOR_CT LON_CT ;
	DEFINE PF_REQ_ACT / GROUP 'Error ARC ID' NOPRINT ;
	DEFINE WX_MSG_TSK / GROUP 'Error ARC Description' NOPRINT ;
	DEFINE Q_CT / SUM NOPRINT;
	DEFINE BOR_CT  / SUM 'Total Borrowers' NOPRINT ;
	DEFINE LON_CT / SUM 'Total Loans' NOPRINT;
RUN;

data _null_;
file "T:\SAS\R001_R2.&SYSDATE..csv" delimiter=',' DSD DROPOVER lrecl=32767;
if _n_ = 1 then do;
   put
      "AccountNumber"
   ','
   	  "BF_SSN"
   ','
      "ErrorARCId"
   ','
      "ErrorARCDescription"
   ','
      "ARCAddDate"
   ','
      "LoanType"
   ','
      "LoanStatus"
   ','
      "RepaymentStartDate"
   ','
      "RepaymentPlan"
   ','
      "BBAL"
   ','
      "REP_TERM"
   ','
      "RS_PAYMENT_DIF"
   ','
      "CUR_PAYMENT"
   ','
   	  "New Calculated Pay Amount"
   ','	
      "PAYMENT_DIF"
   ','
      "PAI"
   ','
   	  'Remaining Payment Terms(Actual)'
   ','
      'Loan Add Date'
   ','
   	   'ln_pre_06_rpd_trm'
   ;
 end;
Set  WORK.Detail_out;
   format AccountNumber $10. ;
   format BF_SSN $9. ;
   format ErrorARCId $5. ;
   format ErrorARCDescription $200. ;
   format ARCAddDate date9. ;
   format LoanType $5000. ;
   format LoanStatus $5000. ;
   format RepaymentStartDate $10. ;
   format RepaymentPlan $10. ;
   format BBAL best12. ;
   format REP_TERM best12. ;
   format RS_PAYMENT_DIF best12. ;
   format CUR_PAYMENT best12. ;
   format PAYMENT_DIF best12. ;
   format PAI $1. ;
   format ACT_CALC_REPAY_AMT best12.;
   format LD_LON_ACL_ADD date9.;
 do;
   put AccountNumber $ @;
   put BF_SSN $ @;
   put ErrorARCId $ @;
   put ErrorARCDescription $ @;
   put ARCAddDate @;
   put LoanType $ @;
   put LoanStatus $ @;
   put RepaymentStartDate $ @;
   put RepaymentPlan $ @;
   put BBAL @;
   put REP_TERM @;
   put RS_PAYMENT_DIF @;
   put CUR_PAYMENT @;
   put ACT_CALC_REPAY_AMT @;
   put PAYMENT_DIF @;
   put PAI $ @;
   PUT REM_PAYMENT_TERMS @;
   put LD_LON_ACL_ADD @;
   put ln_pre_06_rpd_trm;
   ;
 end;
run;


data _null_;
%let _EFIERR_ = 0; /* set the ERROR detection macro variable */
%let _EFIREC_ = 0;     /* clear export record count macro variable */
file "T:\SAS\R001_R3.&SYSDATE..csv" delimiter=',' DSD DROPOVER lrecl=32767;
if _n_ = 1 then        /* write column names or labels */
 do;
   put
      "PF_REQ_ACT"
   ','
      "WX_MSG_TSK"
   ','
      "Q_CT"
   ','
      "BOR_CT"
   ','
      "LON_CT"
   ;
 end;
Set  WORK.Sumry_out   end=EFIEOD;
   format PF_REQ_ACT $5. ;
   format WX_MSG_TSK $200. ;
   format Q_CT best12. ;
   format BOR_CT best12. ;
   format LON_CT best12. ;
 do;
   EFIOUT + 1;
   put PF_REQ_ACT $ @;
   put WX_MSG_TSK $ @;
   put Q_CT @;
   put BOR_CT @;
   put LON_CT ;
   ;
  end;
 if _ERROR_ then call symputx('_EFIERR_',1);  /* set ERROR detection macro variable */
 if EFIEOD then call symputx('_EFIREC_',EFIOUT);
 run;

 data _null_;
%let _EFIERR_ = 0; /* set the ERROR detection macro variable */
%let _EFIREC_ = 0;     /* clear export record count macro variable */
file "T:\SAS\R001_R4.&SYSDATE..csv" delimiter=',' DSD DROPOVER lrecl=32767;
if _n_ = 1 then        /* write column names or labels */
 do;
   put
      "DF_SPE_ACC_ID"
   ','
      "PF_REQ_ACT"
   ','
      "WX_MSG_TSK"
   ','
	  "LoanStatus"	
   ;
 end;
Set  WORK.R4   end=EFIEOD;
   format DF_SPE_ACC_ID $10. ;
   format PF_REQ_ACT $5. ;
   format WX_MSG_TSK $200. ;
   format LoanStatus $200. ;
 do;
   EFIOUT + 1;
   put DF_SPE_ACC_ID $ @;
   put PF_REQ_ACT $ @;
   put WX_MSG_TSK $ @;
   put LoanStatus $ ;
   ;
 end;
if _ERROR_ then call symputx('_EFIERR_',1);  /* set ERROR detection macro variable */
if EFIEOD then call symputx('_EFIREC_',EFIOUT);
run;



data _null_;
%let _EFIERR_ = 0; /* set the ERROR detection macro variable */
%let _EFIREC_ = 0;     /* clear export record count macro variable */
file "T:\SAS\R001_R5.&SYSDATE..csv" delimiter=',' DSD DROPOVER lrecl=32767;
if _n_ = 1 then        /* write column names or labels */
 do;
   put
      "AccountNumber"
   ','
      "LN_SEQ"
  ','
      "LC_TYP_SCH_DIS"
   ;
 end;
Set  WORK.R5_all   end=EFIEOD;
   format AccountNumber $10. ;
   format DF_SPE_ACC_ID $10. ;
   format BF_SSN $9. ;
   format LN_SEQ 6. ;
   format LA_RPS_ISL 9.2 ;
   format LC_TYP_SCH_DIS $2. ;
 do;
   EFIOUT + 1;
   put AccountNumber $ @;
   put LN_SEQ @;
   put LC_TYP_SCH_DIS $ ;
   ;
 end;
if _ERROR_ then call symputx('_EFIERR_',1);  /* set ERROR detection macro variable */
if EFIEOD then call symputx('_EFIREC_',EFIOUT);
run;


/*data _null_;*/
/*%let _EFIERR_ = 0; /* set the ERROR detection macro variable */*/
/*%let _EFIREC_ = 0;     /* clear export record count macro variable */*/
/*file "T:\SAS\R001_R6.&SYSDATE..csv" delimiter=',' DSD DROPOVER lrecl=32767;*/
/*if _n_ = 1 then        /* write column names or labels */*/
/* do;*/
/*   put*/
/*      "AccountNumber"*/
/*   ','*/
/*      "IC_LON_PGM"*/
/*   ','*/
/*      "RPS_START_DT"*/
/*   ;*/
/* end;*/
/*Set  WORK.R6_all   end=EFIEOD;*/
/*   format AccountNumber $10. ;*/
/*   format IC_LON_PGM $6. ;*/
/*   format RPS_START_DT date9. ;*/
/* do;*/
/*   EFIOUT + 1;*/
/*   put AccountNumber $ @;*/
/*   put IC_LON_PGM $ @;*/
/*   put RPS_START_DT ;*/
/*   ;*/
/* end;*/
/*if _ERROR_ then call symputx('_EFIERR_',1);  /* set ERROR detection macro variable */*/
/*if EFIEOD then call symputx('_EFIREC_',EFIOUT);*/
/*run;*/
;
/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.r6_fin
            OUTFILE = "T:\SAS\monitor r6&sysdate..xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.r7_fin 
            OUTFILE = "T:\SAS\monitor r7&sysdate..xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
