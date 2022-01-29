%LET RPTLIB = T:\SAS;
OPTIONS COMPRESS=BINARY;

PROC IMPORT OUT= WORK.Source
            DATAFILE= "T:\NH XXXX.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="A$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.SOURCE; *Send data to Duster;
SET SOURCE;
RUN;

RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CREATE TABLE WQXX AS
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID,
			WQXX.BF_SSN,
			WQXX.WX_MSG_X_TSK, 
			WQXX.WX_MSG_X_TSK,
			WQXX.WF_QUE,
			WQXX.WF_SUB_QUE,
			WQXX.WN_CTL_TSK,
			WQXX.PF_REQ_ACT,
			WQXX.WD_ACT_REQ
		FROM
			PKUB.WQXX_TSK_QUE_HST WQXX
			INNER JOIN PKUB.PDXX_PRS_NME PDXX
				ON WQXX.BF_SSN = PDXX.DF_PRS_ID
			INNER JOIN PKUB.LNXX_LON_RPS LNXX
				ON LNXX.BF_SSN = WQXX.BF_SSN
				AND LNXX.LC_STA_LONXX = 'A'
			INNER JOIN SOURCE S
				ON S.WN_CTL_TSK = WQXX.WN_CTL_TSK
				AND S.WF_QUE = WQXX.WF_QUE
				AND S.WF_SUB_QUE = WQXX.WF_SUB_QUE
				AND S.PF_REQ_ACT = WQXX.PF_REQ_ACT
		WHERE
			LNXX.LC_TYP_SCH_DIS NOT IN ('IB', 'IL', 'IX', 'IP', 'CL', 'CQ', 'CX', 'CX', 'CX', 'CA', 'CP')  
;


CONNECT TO DBX (DATABASE=DNFPUTDL);
/****************************************************************************
QUEUE LEVEL DATA
*****************************************************************************/
/*CREATE TABLE WQXX AS*/
/*SELECT **/
/*FROM CONNECTION TO DBX (*/
/*SELECT C.DF_SPE_ACC_ID */
/*	,A.BF_SSN*/
/*	,A.WX_MSG_X_TSK */
/*	,A.WX_MSG_X_TSK*/
/*	,A.WF_QUE*/
/*	,A.WF_SUB_QUE*/
/*	,A.WN_CTL_TSK*/
/*	,A.PF_REQ_ACT*/
/*	,A.WD_ACT_REQ*/
/*FROM PKUB.WQXX_TSK_QUE A*/
/*INNER JOIN PKUB.PDXX_PRS_NME C*/
/*	ON A.BF_SSN = C.DF_PRS_ID*/
/*WHERE A.WF_QUE = 'RX'*/
/*	AND A.WF_SUB_QUE = 'XX'*/
/*FOR READ ONLY WITH UR*/
/*);*/
/****************************************************************************
LOAN LEVEL DATA
*****************************************************************************/
CREATE TABLE LNXX AS /*THIS TABLE IS MODIFIED INTO LNXXX AT THE END OF THE REMOTE SUBMIT*/
SELECT DISTINCT A.DF_SPE_ACC_ID
	,B.*
FROM WQXX A
LEFT OUTER JOIN CONNECTION TO DBX (
	SELECT B.BF_SSN
		,B.LN_SEQ
		,B.IC_LON_PGM 
		,B.LC_STA_LONXX
		,B.LA_CUR_PRI /*+ D.WA_TOT_BRI_OTS */ AS P_AND_I
		,B.LD_END_GRC_PRD 
		,F.LD_DSB_MAX 
		,B.LD_END_GRC_PRD + XX DAYS AS LD_END_GRC_PRD_PLUS_X
		,F.LD_DSB_MAX + XX DAYS AS LD_DSB_MAX_PLUS_XX
/*		,COALESCE(B.LD_END_GRC_PRD + XX DAYS,F.LD_DSB_MAX + XX DAYS) AS RPS_START_DT*/
		,COALESCE(B.LD_END_GRC_PRD + XX DAYS,B.LD_LON_X_DSB) AS RPS_START_DT

		,B.BF_SSN||CHAR(B.LN_SEQ) AS LID
		,D.WC_DW_LON_STA 
		,D.WD_LON_RPD_SR
		,C.LR_ITR
		,B.LC_STA_LONXX 
		,CASE
			WHEN B.IC_LON_PGM IN 
				(
					'DLPCNS','DLSCNS','DLUCNS','DLSSPL','DLUSPL',
					'DLSCSL','DLSCSC','DLSCUC','DLSCCN'
				) THEN X
			ELSE X
		 END AS IS_CONSOL
		,G.LN_PRE_XX_RPD_TRM
		,B.LD_LON_X_DSB
		,H.ORIG_LN_AMT
		,B.LD_LON_ACL_ADD
	FROM PKUB.LNXX_LON B
	INNER JOIN PKUB.DWXX_DW_CLC_CLU D
		ON B.BF_SSN = D.BF_SSN	
		AND B.LN_SEQ = D.LN_SEQ
	LEFT OUTER JOIN PKUB.LNXX_INT_RTE_HST C
		ON B.BF_SSN = C.BF_SSN
		AND B.LN_SEQ = C.LN_SEQ
		AND C.LC_STA_LONXX = 'A'
		AND DAYS(CURRENT DATE) BETWEEN DAYS(C.LD_ITR_EFF_BEG) AND DAYS(C.LD_ITR_EFF_END)
	LEFT OUTER JOIN (
		SELECT BF_SSN
			,LN_SEQ
			,MAX(LD_DSB) AS LD_DSB_MAX
		FROM PKUB.LNXX_DSB
		GROUP BY BF_SSN
			,LN_SEQ
		) F
		ON B.BF_SSN = F.BF_SSN
		AND B.LN_SEQ = F.LN_SEQ
	LEFT OUTER JOIN (
		SELECT BF_SSN
			,LN_SEQ
			,LN_PRE_XX_RPD_TRM
		FROM PKUB.FSXX_DL_LON
		) G
		ON B.BF_SSN = G.BF_SSN
		AND B.LN_SEQ = G.LN_SEQ
	LEFT OUTER JOIN (
		SELECT BF_SSN
			,LN_SEQ
			,SUM(LA_DSB - COALESCE(LA_DSB_CAN,X)) AS ORIG_LN_AMT
		FROM PKUB.LNXX_DSB
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
FROM CONNECTION TO DBX (
	SELECT DISTINCT A.BF_SSN
		,'Y' AS IND
	FROM PKUB.BLXX_BR_BIL A
	LEFT OUTER JOIN (
		SELECT AX.BF_SSN
			,AX.LD_BIL_DU AS MAX_CRT_DUE
		FROM PKUB.BLXX_BR_BIL AX
		INNER JOIN (
			SELECT BF_SSN
				,MAX(LD_BIL_CRT) AS LD_BIL_CRT
			FROM PKUB.BLXX_BR_BIL 
			WHERE LC_BIL_TYP = 'P'
			GROUP BY BF_SSN
			) BX
			ON AX.BF_SSN = BX.BF_SSN
			AND AX.LD_BIL_CRT = BX.LD_BIL_CRT
	) MAX_BIL_CRT
	ON A.BF_SSN = MAX_BIL_CRT.BF_SSN 
	LEFT OUTER JOIN (
		SELECT BF_SSN
			,MAX(LD_BIL_DU) AS LD_BIL_DU
		FROM PKUB.BLXX_BR_BIL 
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
FROM WQXX A
INNER JOIN CONNECTION TO DBX (
	SELECT DISTINCT LNXX.BF_SSN
		,LNXX.LN_SEQ
		,LNXX.LF_DFR_CTL_NUM 	AS CTL_NUM
		,LNXX.LN_DFR_OCC_SEQ 	AS OCC_SEQ
		,DFXX.LC_DFR_TYP 		AS TYP
		,'D' 					AS DEF_FOR
		,LNXX.LD_DFR_BEG 		AS LD_BEG
		,LNXX.LD_DFR_END	    AS LD_END
	FROM PKUB.DFXX_BR_DFR_REQ DFXX
	INNER JOIN PKUB.LNXX_BR_DFR_APV LNXX  
		ON DFXX.BF_SSN = LNXX.BF_SSN
		AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM 
	WHERE DFXX.LC_DFR_STA = 'A'
		AND DFXX.LC_STA_DFRXX = 'A'
		AND LNXX.LC_STA_LONXX = 'A'
UNION 
	SELECT DISTINCT LNXX.BF_SSN
		,LNXX.LN_SEQ
		,LNXX.LF_FOR_CTL_NUM 	AS CTL_NUM
		,LNXX.LN_FOR_OCC_SEQ 	AS OCC_SEQ
		,FBXX.LC_FOR_TYP 		AS TYP
		,'F' 					AS DEF_FOR
		,LNXX.LD_FOR_BEG 		AS LD_BEG
		,LNXX.LD_FOR_END		AS LD_END
	FROM PKUB.FBXX_BR_FOR_REQ FBXX
	INNER JOIN PKUB.LNXX_BR_FOR_APV LNXX  
		ON FBXX.BF_SSN = LNXX.BF_SSN
		AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM 
	WHERE FBXX.LC_FOR_STA = 'A'
		AND FBXX.LC_STA_FORXX = 'A'
		AND LNXX.LC_STA_LONXX = 'A'
FOR READ ONLY WITH UR
) B
	ON A.BF_SSN = B.BF_SSN;
/****************************************************************************
REPAYMENT LEVEL DATA 
*****************************************************************************/
CREATE TABLE Active_RPS AS
SELECT DISTINCT A.DF_SPE_ACC_ID
	,B.*
FROM LNXX A
LEFT OUTER JOIN CONNECTION TO DBX (
	SELECT B.BF_SSN
		,B.LN_SEQ
		,B.LN_RPS_SEQ
		,B.LN_GRD_RPS_SEQ
		,B.LN_RPS_TRM
		,B.LA_RPS_ISL
		,C.LD_RPS_X_PAY_DU
		,A.LC_TYP_SCH_DIS
		,A.LC_STA_LONXX
	FROM PKUB.LNXX_LON_RPS A
	INNER JOIN PKUB.LNXX_LON_RPS_SPF B
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.LN_SEQ
		AND A.LN_RPS_SEQ = B.LN_RPS_SEQ
	INNER JOIN PKUB.RSXX_BR_RPD C
		ON B.BF_SSN = C.BF_SSN
		AND B.LN_RPS_SEQ = C.LN_RPS_SEQ
	WHERE A.LC_STA_LONXX = 'A'
	FOR READ ONLY WITH UR
	) B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
ORDER BY BF_SSN, LN_SEQ, LN_RPS_SEQ;

CREATE TABLE Inactive_RPS AS
SELECT DISTINCT A.DF_SPE_ACC_ID
	,B.*
FROM LNXX A
LEFT OUTER JOIN CONNECTION TO DBX (
	SELECT B.BF_SSN
		,B.LN_SEQ
		,B.LN_RPS_SEQ
		,B.LN_GRD_RPS_SEQ
		,B.LN_RPS_TRM
		,B.LA_RPS_ISL
		,C.LD_RPS_X_PAY_DU
		,A.LC_TYP_SCH_DIS
		,A.LC_STA_LONXX
	FROM PKUB.LNXX_LON_RPS A
	INNER JOIN PKUB.LNXX_LON_RPS_SPF B
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.LN_SEQ
		AND A.LN_RPS_SEQ = B.LN_RPS_SEQ
	INNER JOIN PKUB.RSXX_BR_RPD C
		ON B.BF_SSN = C.BF_SSN
		AND B.LN_RPS_SEQ = C.LN_RPS_SEQ
	INNER JOIN (
		SELECT BF_SSN
			,LN_SEQ
			,MAX(LN_RPS_SEQ) AS LN_RPS_SEQ
		FROM PKUB.LNXX_LON_RPS
		WHERE LC_STA_LONXX = 'I'
		GROUP BY BF_SSN
			,LN_SEQ
		) D
		ON A.BF_SSN = D.BF_SSN
		AND A.LN_SEQ = D.LN_SEQ
		AND A.LN_RPS_SEQ = D.LN_RPS_SEQ
	WHERE A.LC_STA_LONXX = 'I'
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

DISCONNECT FROM DBX;
QUIT;

DATA RPS;
	SET Active_RPS Inactive_RPS;
	WHERE BF_SSN ^= '';
RUN;

PROC SORT DATA=RPS NODUPKEY;
	BY BF_SSN LN_SEQ LN_RPS_SEQ LN_GRD_RPS_SEQ;
RUN;
DATA RPS;
	FORMAT GRD_BEG_DT GRD_END_DT MMDDYYXX.;
	SET RPS;
	BY BF_SSN LN_SEQ LN_RPS_SEQ;
	RETAIN GRD_END_DT;
	IF FIRST.LN_RPS_SEQ THEN DO;
		GRD_BEG_DT = LD_RPS_X_PAY_DU;
		GRD_END_DT = INTNX('MONTH',LD_RPS_X_PAY_DU,+LN_RPS_TRM,'S');
	END;
	ELSE DO;
		GRD_BEG_DT = GRD_END_DT;
		GRD_END_DT = INTNX('MONTH',GRD_BEG_DT,+LN_RPS_TRM,'S');
	END;
	IF GRD_BEG_DT <= DATE() <= GRD_END_DT THEN USE=X; 
RUN;
/*CREATE FINAL REPAYMNET AND LOAN TABLES*/
PROC SQL;
	CREATE TABLE CUR_RPS_GRD AS 
	SELECT DISTINCT A.*
	FROM RPS A
	WHERE USE = X;

	CREATE TABLE NON_CUR_RPS_GRD AS 
	SELECT DISTINCT A.*
	FROM RPS A
	LEFT OUTER JOIN CUR_RPS_GRD B
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.LN_SEQ
	WHERE B.BF_SSN IS NULL
		AND B.LN_SEQ IS NULL
		AND A.LN_GRD_RPS_SEQ = X;

	CREATE TABLE RPS(KEEP=DF_SPE_ACC_ID BF_SSN LN_SEQ LA_RPS_ISL LC_TYP_SCH_DIS) AS 
			SELECT * FROM CUR_RPS_GRD
		UNION 
			SELECT * FROM NON_CUR_RPS_GRD;

	 CREATE TABLE LNXXX AS
		SELECT DISTINCT A.*,B.ORIG_LN_AMT 	
		FROM LNXX(DROP=ORIG_LN_AMT) A
		INNER JOIN ( SELECT DF_SPE_ACC_ID,SUM(ORIG_LN_AMT) AS ORIG_LN_AMT FROM LNXX GROUP BY DF_SPE_ACC_ID ) B
			ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID;
/*CLEAN UP THE WORK LIB*/
	DROP TABLE LNXX;
	DROP TABLE CUR_RPS_GRD ;
	DROP TABLE NON_CUR_RPS_GRD ;
QUIT;
ENDRSUBMIT;

DATA WQXX;SET LEGEND.WQXX;RUN;
DATA LNXX;SET LEGEND.LNXXX;RUN; /*NOTICE THAT THE REMOTE DATA SET IS LNXXX AND IT IS BROUGHT DOWN AS LNXX*/
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
		,COALESCE(D.TOT_DEF_FOR,X) 											AS TOT_DEF_FOR
		,INTCK('MONTH',C.RPS_START_DT,TODAY()) - COALESCE(D.TOT_DEF_FOR,X)	AS NET_RPS_USED 
		,INTCK('MONTH',C.LD_LON_X_DSB,TODAY()) - COALESCE(D.TOT_DEF_FOR,X)	AS NET_RPS_USED_CONSOL 
		,XXX - CALCULATED NET_RPS_USED 										AS PAYMENT_TERM
		,(LR_ITR/XXX)/XX 													AS ADJ_INT_RATE
		,(LR_ITR/XXX)	 													AS INT_RATE
		,COALESCE(E.LA_RPS_ISL,X)											AS EXISTING_PAYMENT
		,E.LC_TYP_SCH_DIS													AS RPMT_TYP
		,C.IC_LON_PGM
		,C.IS_CONSOL
		,C.LN_PRE_XX_RPD_TRM
		,C.LD_LON_X_DSB
		,C.ORIG_LN_AMT
	FROM LNXX C
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
		AND C.P_AND_I > X
	ORDER BY C.BF_SSN, C.LID;
QUIT;

DATA ACT_RPS_SCHD;
	SET DEF_FOR_BX(DROP=PAYMENT_TERM);
	BASE_DATE = MDY(XX,XX,XXXX);

	IF RPMT_TYP IN ('PL','PG') THEN DO;
		IF ORIG_LN_AMT < XXXXX THEN BASE_TERM = XXX;
			ELSE IF XXXXX <= ORIG_LN_AMT < XXXXX THEN BASE_TERM = XXX;
			ELSE IF XXXXX <= ORIG_LN_AMT < XXXXX THEN BASE_TERM = XXX;
			ELSE IF XXXXX <= ORIG_LN_AMT < XXXXX THEN BASE_TERM = XXX;
			ELSE BASE_TERM = XXX;
/*		BASE_TERM = LN_PRE_XX_RPD_TRM;*/
	END;

	ELSE IF RPS_START_DT < BASE_DATE & (RPMT_TYP = 'L' & IS_CONSOL | RPMT_TYP IN ('EL','G'))THEN DO;
		IF RPMT_TYP = 'L' THEN BASE_TERM = XXX;
		ELSE DO;
			IF ORIG_LN_AMT < XXXXX THEN BASE_TERM = XXX;
			ELSE IF XXXXX <= ORIG_LN_AMT < XXXXX THEN BASE_TERM = XXX;
			ELSE IF XXXXX <= ORIG_LN_AMT < XXXXX THEN BASE_TERM = XXX;
			ELSE IF XXXXX <= ORIG_LN_AMT < XXXXX THEN BASE_TERM = XXX;
			ELSE BASE_TERM = XXX;
		END;
	END;

	ELSE IF RPS_START_DT >= BASE_DATE AND (RPMT_TYP = 'G' | RPMT_TYP = 'L' & IS_CONSOL) THEN DO;
		IF IS_CONSOL THEN DO;
			IF ORIG_LN_AMT < XXXX THEN BASE_TERM = XXX;
			ELSE IF XXXX <= ORIG_LN_AMT < XXXXX THEN BASE_TERM = XXX;
			ELSE IF XXXXX <= ORIG_LN_AMT < XXXXX THEN BASE_TERM = XXX;
			ELSE IF XXXXX <= ORIG_LN_AMT < XXXXX THEN BASE_TERM = XXX;
			ELSE IF XXXXX <= ORIG_LN_AMT < XXXXX THEN BASE_TERM = XXX;
			ELSE BASE_TERM = XXX;
		END;
		ELSE IF RPMT_TYP = 'G' THEN BASE_TERM = XXX;
	END;

	ELSE IF RPMT_TYP = 'EG' 
		AND LD_LON_X_DSB > = MDY(XX,X,XXXX) 
		AND RPS_START_DT >= BASE_DATE  
		AND ORIG_LN_AMT > XXXXX 
	THEN BASE_TERM = XXX;
	
	ELSE IF RPMT_TYP = 'L' AND ^IS_CONSOL THEN BASE_TERM = XXX;
	ELSE IF RPMT_TYP = 'FS' THEN BASE_TERM = XXX;
	ELSE BASE_TERM = X;

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
		DIF = ROUND(XPAY - BOR_PAYMENT,.XX);
		IF LAST.BF_SSN;
	RUN;
%MEND RPS_PAYMENT_CALCS;
%RPS_PAYMENT_CALCS(DEF_FOR_BXX,DEF_FOR_BX);/*FS SCHEDULE*/
%RPS_PAYMENT_CALCS(ACT_RPS_SCHDX,ACT_RPS_SCHD);/*ACTUAL REPAYMENT TYPE*/
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
	VALUE $LNSTA 'XX' = 'In Grace'
		'XX' = 'In School'
		'XX' = 'Repayment'
		'XX' = 'Deferment'
		'XX' = 'Forbearance'
		'XX' = 'Cure'
		'XX' = 'Claim Pending'
		'XX' = 'Claim Submitted'
		'XX' = 'Claim Cncl'
		'XX' = 'Claim Reject'
		'XX' = 'Claim Rtn'
		'XX' = 'Claim Paid'
		'XX' = 'PreClaim Pending'
		'XX' = 'PreClaim Submitted'
		'XX' = 'PreClaim Cncl'
		'XX' = 'Alleged Death'
		'XX' = 'Verified Death'
		'XX' = 'Alleged Disability'
		'XX' = 'Verified Disability'
		'XX' = 'Alleged Bankruptcy'
		'XX' = 'Verified Bankruptcy'
		'XX' = 'PIF'
		'XX' = 'Not Fully Originated'
		'XX' = 'Processing Error'
		'XX' = 'Unknown';
RUN;
PROC SORT DATA=LNXX OUT=LN_TYP(KEEP=DF_SPE_ACC_ID LN_SEQ IC_LON_PGM) NODUPKEY;
	BY DF_SPE_ACC_ID IC_LON_PGM;
RUN;
PROC SORT DATA=LNXX OUT=LN_STA(KEEP=DF_SPE_ACC_ID LN_SEQ WC_DW_LON_STA) NODUPKEY;
	BY DF_SPE_ACC_ID WC_DW_LON_STA;
RUN;
DATA LN_STA;
	SET LN_STA;
	LENGTH STA_DESC $XX.;
	STA_DESC = PUT(WC_DW_LON_STA,$LNSTA.);
RUN;
%MACRO ONE_LINER(DS,LVAR,IVAR); 
DATA &DS(KEEP=DF_SPE_ACC_ID &LVAR);
	SET &DS;
	LENGTH &LVAR $XXXX. ;
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
	SELECT DISTINCT CX.DF_SPE_ACC_ID
		,'MULTIPLE' AS EXCLUSION_REASON
		,X AS EXCLD
	FROM LNXX CX
	INNER JOIN LNXX CX
		ON CX.DF_SPE_ACC_ID = CX.DF_SPE_ACC_ID
	WHERE CX.RPS_START_DT ^= CX.RPS_START_DT 
UNION 
	SELECT DISTINCT DX.DF_SPE_ACC_ID
		,'MULTIPLE' AS EXCLUSION_REASON
		,X AS EXCLD
	FROM RPS DX
	INNER JOIN RPS DX
		ON DX.DF_SPE_ACC_ID = DX.DF_SPE_ACC_ID
	WHERE DX.LC_TYP_SCH_DIS ^= DX.LC_TYP_SCH_DIS 
UNION 
	SELECT DISTINCT DF_SPE_ACC_ID
		,'RPS TYPE' AS EXCLUSION_REASON
		,X AS EXCLD
	FROM RPS 
	WHERE LC_TYP_SCH_DIS IN ('CX','CX','CX','CL','IB','IL')
UNION 
	SELECT DISTINCT DF_SPE_ACC_ID
		,'PIF' AS EXCLUSION_REASON
		,X AS EXCLD
	FROM (
		SELECT DF_SPE_ACC_ID
			,SUM(P_AND_I) AS BOR_BAL
		FROM LNXX
		GROUP BY DF_SPE_ACC_ID
		HAVING SUM(P_AND_I) = X
		)
UNION 
	SELECT DISTINCT DF_SPE_ACC_ID
		,'DEF OR FORB' AS EXCLUSION_REASON
		,X AS EXCLD
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
		,CATX(' ',A.WX_MSG_X_TSK,A.WX_MSG_X_TSK) AS ErrorARCDescription 
		,A.WN_CTL_TSK 
		,A.WD_ACT_REQ AS ARCAddDate 
		,BX.LN_LIST AS LoanType
		,B.STA_LIST AS LoanStatus 
		,CASE 
			WHEN C.DF_SPE_ACC_ID IS NULL THEN PUT(AX.RPS_START_DT,MMDDYYXX.)
			ELSE C.EXCLUSION_REASON
		 END AS RepaymentStartDate LENGTH=XX
		,CASE 
			WHEN C.DF_SPE_ACC_ID IS NULL THEN H.LC_TYP_SCH_DIS
			ELSE C.EXCLUSION_REASON
		 END AS RepaymentPlan LENGTH=XX
		,K.BBAL 
		,F.REP_TERM
		,E.DIF AS RS_PAYMENT_DIF
		,E.XPAY AS CUR_PAYMENT
		,COALESCE(C.EXCLD,X) AS EXCLD
		,L.DIF AS PAYMENT_DIF
		,L.BOR_PAYMENT AS ACT_CALC_REPAY_AMT
		,L.PAYMENT_TERM AS REM_PAYMENT_TERMS
		,COALESCE(M.IND,'N') AS PAI
		,AX.LD_LON_ACL_ADD
		,coalesce(n.ln_pre_XX_rpd_trm,X) as ln_pre_XX_rpd_trm
	FROM WQXX A
	LEFT OUTER JOIN LN_STA B
		ON A.DF_SPE_ACC_ID = B.DF_SPE_ACC_ID
	LEFT OUTER JOIN LN_TYP BX
		ON A.DF_SPE_ACC_ID = BX.DF_SPE_ACC_ID
	LEFT OUTER JOIN LNXX AX
		ON A.BF_SSN = AX.BF_SSN
	LEFT OUTER JOIN MULT_RPS_OR_START C
		ON A.DF_SPE_ACC_ID = C.DF_SPE_ACC_ID
	LEFT OUTER JOIN DEF_FOR_BXX E
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
		FROM LNXX
		GROUP BY BF_SSN
		) K
		ON A.BF_SSN = K.BF_SSN
	LEFT OUTER JOIN ACT_RPS_SCHDX L
		ON A.BF_SSN = L.BF_SSN
	LEFT OUTER JOIN PAID_AHEAD M
		ON A.BF_SSN = M.BF_SSN
	left outer join (
		select bf_ssn
			,max(ln_pre_XX_rpd_trm) as ln_pre_XX_rpd_trm
		from lnXX
		group by bf_ssn
		) n
		on a.bf_ssn = n.bf_ssn


	WHERE G.BF_SSN IS NULL
	;
QUIT;

PROC SQL;
CREATE TABLE ALL_DETAIL AS 
SELECT DISTINCT CATT(A.WF_QUE,A.WF_SUB_QUE,A.PF_REQ_ACT,A.WN_CTL_TSK,A.WX_MSG_X_TSK,A.WX_MSG_X_TSK) AS natl_key 
	,A.PF_REQ_ACT
	,CATX(' ',A.WX_MSG_X_TSK,A.WX_MSG_X_TSK) AS WX_MSG_TSK 
	,A.BF_SSN
	,B.LID
FROM WQXX A
INNER JOIN LNXX B
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
CREATE TABLE RX AS 
SELECT DISTINCT B.DF_SPE_ACC_ID
	,B.PF_REQ_ACT
	,CATX(' ',B.WX_MSG_X_TSK,B.WX_MSG_X_TSK) AS WX_MSG_TSK 
	,A.LoanType
	,A.LoanStatus
FROM DETAIL A
INNER JOIN WQXX B
	ON A.BF_SSN = B.BF_SSN
WHERE A.EXCLD IN (X,X)
;
QUIT;

PROC REPORT DATA=DETAIL OUT=DETAIL_OUT(DROP=_BREAK_) NOWD SPLIT='~' HEADSKIP;
	WHERE EXCLD = X;
/*	WHERE EXCLD IN (X,X);*/
	COLUMN AccountNumber BF_SSN ErrorARCId ErrorARCDescription ARCAddDate LoanType LoanStatus RepaymentStartDate 
		RepaymentPlan BBAL REP_TERM RS_PAYMENT_DIF CUR_PAYMENT ACT_CALC_REPAY_AMT PAYMENT_DIF PAI REM_PAYMENT_TERMS
		LD_LON_ACL_ADD ln_pre_XX_rpd_trm;
	DEFINE AccountNumber / DISPLAY 'Account number' NOPRINT;
	DEFINE BF_SSN / DISPLAY 'BF_SSN' NOPRINT;
	DEFINE ErrorARCId / DISPLAY 'Error ARC ID' NOPRINT;
	DEFINE ErrorARCDescription / DISPLAY 'Error ARC Description' NOPRINT;
	DEFINE ARCAddDate / DISPLAY 'ARC Add Date' FORMAT=MMDDYYXX. NOPRINT ;
	DEFINE LoanType / DISPLAY 'Loan Type' width=XXX flow NOPRINT;
	DEFINE LoanStatus/ DISPLAY 'Loan Status' NOPRINT;
	DEFINE RepaymentStartDate / 'Repayment Start Date' NOPRINT;
	DEFINE RepaymentPlan / 'Repayment Plan' NOPRINT;
	DEFINE BBAL / 'Borrower Balance' FORMAT=DollarXX.X NOPRINT;
	DEFINE REP_TERM / 'Repayment Term Used' NOPRINT;
	DEFINE RS_PAYMENT_DIF / 'Current Payment - Calculated Payment(FS)' FORMAT=DollarXX.X NOPRINT;
	DEFINE CUR_PAYMENT / 'Current Payment' FORMAT=DollarXX.X NOPRINT;
	DEFINE ACT_CALC_REPAY_AMT / 'New Calculated Pay Amount' FORMAT=DollarXX.X NOPRINT;
	DEFINE PAYMENT_DIF / 'Current Payment - Calculated Payment(Existing Repayment Type)' FORMAT=DollarXX.X NOPRINT;
	DEFINE PAI / 'Paid Ahead Indicator' noprint;
	DEFINE REM_PAYMENT_TERMS / 'Remaining Payment Terms(Actual)' NOPRINT;
	DEFINE LD_LON_ACL_ADD / 'Loan Add Date' NOPRINT;
	define ln_pre_XX_rpd_trm / 'ln_pre_XX_rpd_trm' noprint;
RUN;

DATA RX RX;
	SET DETAIL;
/*	WHERE EXCLD = X;*/
	WHERE EXCLD IN (X,X);
	IF EXCLD = X THEN OUTPUT RX;
	ELSE OUTPUT RX;
RUN;

PROC SQL;
CREATE TABLE RX_ALL AS
SELECT DISTINCT A.AccountNumber
	,B.*
FROM RX A
INNER JOIN RPS B
	ON A.BF_SSN = B.BF_SSN
;
QUIT;

PROC SQL;
CREATE TABLE RX_ALL AS
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
	,A.LN_PRE_XX_RPD_TRM
FROM RX A
INNER JOIN LNXX B
	ON A.BF_SSN = B.BF_SSN
LEFT JOIN RPS R
	ON A.BF_SSN = R.BF_SSN
	AND B.LN_SEQ = R.LN_SEQ
INNER JOIN (
			SELECT
				A.BF_SSN
				,MAX(A.ARCADDDATE) AS ARCADDDATE
			FROM
				RX A
			GROUP BY 
				A.BF_SSN
			) MAX
	ON A.BF_SSN = MAX.BF_SSN
	AND A.ARCADDDATE = MAX.ARCADDDATE	
left join lnXX z
	on a.bf_ssn = z.bf_ssn
	and b.ln_seq = z.ln_seq
	and z.wc_dw_lon_sta = 'XX'
left join rX
	on a.AccountNumber = rX.df_spe_acc_id
where (z.bf_ssn is null and z.ln_seq is null)
	and rX.df_spe_acc_id is null
;
QUIT;

proc sql;
create table rX_mid as
select
	a.*
	,count(*) as dup_lns
from
	rX_all a
group by 
	a.accountnumber
	,a.bf_ssn
	,a.ln_seq
;
quit;

proc sql;
create table rX_fin as
select
	a.*
from
	rX_mid a
	left join (
				select
					b.bf_ssn
				from 
					rX_mid b
				where 
					b.dup_lns ^= X
					) dup
		on a.bf_ssn = dup.bf_ssn
where dup.bf_ssn is null
;
quit;

data rX_fin;
	set rX_fin;
	drop dup_lns;
run;

proc sql;
create table rX_fin as
select
	a.*
from
	rX_mid a
	left join rX_fin b
		on a.bf_ssn = b.bf_ssn
where b.bf_ssn is null
; 
quit;

data rX_fin;
	set rX_fin;
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
file "T:\SAS\RXXX_RX.&SYSDATE..csv" delimiter=',' DSD DROPOVER lrecl=XXXXX;
if _n_ = X then do;
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
   	   'ln_pre_XX_rpd_trm'
   ;
 end;
Set  WORK.Detail_out;
   format AccountNumber $XX. ;
   format BF_SSN $X. ;
   format ErrorARCId $X. ;
   format ErrorARCDescription $XXX. ;
   format ARCAddDate dateX. ;
   format LoanType $XXXX. ;
   format LoanStatus $XXXX. ;
   format RepaymentStartDate $XX. ;
   format RepaymentPlan $XX. ;
   format BBAL bestXX. ;
   format REP_TERM bestXX. ;
   format RS_PAYMENT_DIF bestXX. ;
   format CUR_PAYMENT bestXX. ;
   format PAYMENT_DIF bestXX. ;
   format PAI $X. ;
   format ACT_CALC_REPAY_AMT bestXX.;
   format LD_LON_ACL_ADD dateX.;
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
   put ln_pre_XX_rpd_trm;
   ;
 end;
run;


data _null_;
%let _EFIERR_ = X; /* set the ERROR detection macro variable */
%let _EFIREC_ = X;     /* clear export record count macro variable */
file "T:\SAS\RXXX_RX.&SYSDATE..csv" delimiter=',' DSD DROPOVER lrecl=XXXXX;
if _n_ = X then        /* write column names or labels */
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
   format PF_REQ_ACT $X. ;
   format WX_MSG_TSK $XXX. ;
   format Q_CT bestXX. ;
   format BOR_CT bestXX. ;
   format LON_CT bestXX. ;
 do;
   EFIOUT + X;
   put PF_REQ_ACT $ @;
   put WX_MSG_TSK $ @;
   put Q_CT @;
   put BOR_CT @;
   put LON_CT ;
   ;
  end;
 if _ERROR_ then call symputx('_EFIERR_',X);  /* set ERROR detection macro variable */
 if EFIEOD then call symputx('_EFIREC_',EFIOUT);
 run;

 data _null_;
%let _EFIERR_ = X; /* set the ERROR detection macro variable */
%let _EFIREC_ = X;     /* clear export record count macro variable */
file "T:\SAS\RXXX_RX.&SYSDATE..csv" delimiter=',' DSD DROPOVER lrecl=XXXXX;
if _n_ = X then        /* write column names or labels */
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
Set  WORK.RX   end=EFIEOD;
   format DF_SPE_ACC_ID $XX. ;
   format PF_REQ_ACT $X. ;
   format WX_MSG_TSK $XXX. ;
   format LoanStatus $XXX. ;
 do;
   EFIOUT + X;
   put DF_SPE_ACC_ID $ @;
   put PF_REQ_ACT $ @;
   put WX_MSG_TSK $ @;
   put LoanStatus $ ;
   ;
 end;
if _ERROR_ then call symputx('_EFIERR_',X);  /* set ERROR detection macro variable */
if EFIEOD then call symputx('_EFIREC_',EFIOUT);
run;



data _null_;
%let _EFIERR_ = X; /* set the ERROR detection macro variable */
%let _EFIREC_ = X;     /* clear export record count macro variable */
file "T:\SAS\RXXX_RX.&SYSDATE..csv" delimiter=',' DSD DROPOVER lrecl=XXXXX;
if _n_ = X then        /* write column names or labels */
 do;
   put
      "AccountNumber"
   ','
      "LN_SEQ"
  ','
      "LC_TYP_SCH_DIS"
   ;
 end;
Set  WORK.RX_all   end=EFIEOD;
   format AccountNumber $XX. ;
   format DF_SPE_ACC_ID $XX. ;
   format BF_SSN $X. ;
   format LN_SEQ X. ;
   format LA_RPS_ISL X.X ;
   format LC_TYP_SCH_DIS $X. ;
 do;
   EFIOUT + X;
   put AccountNumber $ @;
   put LN_SEQ @;
   put LC_TYP_SCH_DIS $ ;
   ;
 end;
if _ERROR_ then call symputx('_EFIERR_',X);  /* set ERROR detection macro variable */
if EFIEOD then call symputx('_EFIREC_',EFIOUT);
run;


/*data _null_;*/
/*%let _EFIERR_ = X; /* set the ERROR detection macro variable */*/
/*%let _EFIREC_ = X;     /* clear export record count macro variable */*/
/*file "T:\SAS\RXXX_RX.&SYSDATE..csv" delimiter=',' DSD DROPOVER lrecl=XXXXX;*/
/*if _n_ = X then        /* write column names or labels */*/
/* do;*/
/*   put*/
/*      "AccountNumber"*/
/*   ','*/
/*      "IC_LON_PGM"*/
/*   ','*/
/*      "RPS_START_DT"*/
/*   ;*/
/* end;*/
/*Set  WORK.RX_all   end=EFIEOD;*/
/*   format AccountNumber $XX. ;*/
/*   format IC_LON_PGM $X. ;*/
/*   format RPS_START_DT dateX. ;*/
/* do;*/
/*   EFIOUT + X;*/
/*   put AccountNumber $ @;*/
/*   put IC_LON_PGM $ @;*/
/*   put RPS_START_DT ;*/
/*   ;*/
/* end;*/
/*if _ERROR_ then call symputx('_EFIERR_',X);  /* set ERROR detection macro variable */*/
/*if EFIEOD then call symputx('_EFIREC_',EFIOUT);*/
/*run;*/
;
/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.rX_fin
            OUTFILE = "T:\SAS\monitor rX&sysdate..xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.rX_fin 
            OUTFILE = "T:\SAS\monitor rX&sysdate..xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
