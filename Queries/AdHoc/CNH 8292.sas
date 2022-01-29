/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*LIBNAME OLWHRMX DBX DATABASE=DLGSUTWH OWNER=OLWHRMX;*/

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

DATA _null_;
StartingMonth = X;
CALL SYMPUT('StartingMonth',StartingMonth);
RUN;
%PUT &StartingMonth;

PROC SQL;
	CONNECT TO DBX (DATABASE=DNFPUTDL);
	CREATE TABLE LNXXCTE AS
		SELECT 
			*
		FROM 
			CONNECTION TO DBX 
				(
					WITH RPL (BF_SSN, LN_SEQ, LN_RPS_SEQ, LN_GRD_RPS_SEQ, LA_RPS_ISL, LN_RPS_TRM, StartingMonth, EndingMonth) AS
					(
						 SELECT 
								G.BF_SSN,
								G.LN_SEQ,
								G.LN_RPS_SEQ,
								G.LN_GRD_RPS_SEQ,
								G.LA_RPS_ISL,
								G.LN_RPS_TRM,
								&StartingMonth AS StartingMonth,
								G.LN_RPS_TRM AS EndingMonth
						FROM PKUB.LNXX_LON_RPS_SPF G WHERE G.LN_GRD_RPS_SEQ = X
					  
						UNION ALL
						
						SELECT 
								GX.BF_SSN,
								GX.LN_SEQ,
								GX.LN_RPS_SEQ,
								GX.LN_GRD_RPS_SEQ,
								GX.LA_RPS_ISL,
								GX.LN_RPS_TRM,
								RPL.EndingMonth + X AS StartingMonth,	
								RPL.EndingMonth + GX.LN_RPS_TRM	AS EndingMonth	
						FROM RPL, PKUB.LNXX_LON_RPS_SPF GX 
								WHERE GX.LN_GRD_RPS_SEQ = RPL.LN_GRD_RPS_SEQ+X
								AND GX.BF_SSN = RPL.BF_SSN
								AND GX.LN_SEQ = RPL.LN_SEQ
								AND GX.LN_RPS_SEQ = RPL.LN_RPS_SEQ
					)
					SELECT
						BF_SSN, LN_SEQ, LN_RPS_SEQ, LN_GRD_RPS_SEQ, LA_RPS_ISL, LN_RPS_TRM, StartingMonth, EndingMonth
					FROM 
						RPL
					ORDER BY 
						BF_SSN, LN_SEQ, LN_RPS_SEQ, LN_GRD_RPS_SEQ, LA_RPS_ISL, LN_RPS_TRM				

				FOR READ ONLY WITH UR
				);

		DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

PROC SQL;
	CONNECT TO DBX (DATABASE=DNFPUTDL);
	CREATE TABLE BaseRX AS
		SELECT 
			*
		FROM 
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN
						,LNXX.LN_SEQ
						,MAX(CONCAT(FSXX.LF_FED_AWD,FSXX.LN_FED_AWD_SEQ)) AS AwardId
						,MaxRPS.LN_RPS_SEQ
						,(MAX(RSXX.DaysSinceFirstPayment)
							- SUM(CASE WHEN (LNXX.LD_DFR_BEG IS NOT NULL AND RSXX.LD_RPS_X_PAY_DU >= LNXX.LD_DFR_BEG AND RSXX.LD_RPS_X_PAY_DU <= LNXX.LD_DFR_END) THEN DAYS(LNXX.LD_DFR_END) - DAYS(RSXX.LD_RPS_X_PAY_DU)
							      WHEN (LNXX.LD_DFR_BEG IS NOT NULL AND RSXX.LD_RPS_X_PAY_DU <= LNXX.LD_DFR_BEG AND RSXX.LD_RPS_X_PAY_DU <= LNXX.LD_DFR_END) THEN DAYS(LNXX.LD_DFR_END) - DAYS(LNXX.LD_DFR_BEG)
								  ELSE X END) /*Deferment days*/
							- SUM(CASE WHEN (LNXX.LD_FOR_BEG IS NOT NULL AND RSXX.LD_RPS_X_PAY_DU >= LNXX.LD_FOR_BEG AND RSXX.LD_RPS_X_PAY_DU <= LNXX.LD_FOR_END) THEN DAYS(LNXX.LD_FOR_END) - DAYS(RSXX.LD_RPS_X_PAY_DU)
							      WHEN (LNXX.LD_FOR_BEG IS NOT NULL AND RSXX.LD_RPS_X_PAY_DU <= LNXX.LD_FOR_BEG AND RSXX.LD_RPS_X_PAY_DU <= LNXX.LD_FOR_END) THEN DAYS(LNXX.LD_FOR_END) - DAYS(LNXX.LD_FOR_BEG)
								  ELSE X END) /*Forbearance days*/
						)/XX AS Months /*Days to calculate active graduation term from*/
						/*If the answer is greater than the amount of gradation X terms (LN_RPS_TRM where LN_GRD_RPS_SEQ = �X�), 
						subtract the amount of gradation X terms from the answer and repeat the check with each subsequent gradation.  
						Once the gradation term is greater, then that is the current gradation.  
						Select the LA_RPS_ISL for that gradation as the current monthly payment.*/
						,MAX((LNXX.LR_ITR * LNXX.LA_CUR_PRI)/XXXX) AS MonthlyInterestAccrual /*XX months and adjusting the interest rate decimal point*/
						,MAX(DWXX.WA_TOT_BRI_OTS) AS OutstandingInterest
					FROM	
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.FSXX_DL_LON FSXX
							ON FSXX.BF_SSN = LNXX.BF_SSN
							AND FSXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
							ON DWXX.BF_SSN = LNXX.BF_SSN
							AND DWXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.LNXX_LON_RPS LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
							AND LNXX.LC_STA_LONXX = 'A'
							AND LNXX.LC_TYP_SCH_DIS NOT IN('IB','IL','IX','IP','CX','CX','CX','CL','CQ','CA','CP','IA','IX','RE')
						INNER JOIN PKUB.LNXX_INT_RTE_HST LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
							AND LNXX.LD_ITR_EFF_BEG <= CURRENT DATE
							AND LNXX.LD_ITR_EFF_END >= CURRENT DATE
							AND LNXX.LC_STA_LONXX = 'A'
						INNER JOIN (
								SELECT
									MAX(LNXX.LN_RPS_SEQ) AS LN_RPS_SEQ,
									LNXX.BF_SSN,
									LNXX.LN_SEQ
								FROM
									PKUB.LNXX_LON_RPS_SPF LNXX
								GROUP BY
									LNXX.BF_SSN,
									LNXX.LN_SEQ
								) MaxRPS
								ON MaxRPS.BF_SSN = LNXX.BF_SSN
								AND MaxRPS.LN_SEQ = LNXX.LN_SEQ
								AND MaxRPS.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
						INNER JOIN (
								SELECT
									DAYS(Current Date) - DAYS(RSXXI.LD_RPS_X_PAY_DU) AS DaysSinceFirstPayment
									,RSXXI.LD_RPS_X_PAY_DU
									,RSXXI.BF_SSN
									,RSXXI.LN_RPS_SEQ
								FROM 
									PKUB.RSXX_BR_RPD RSXXI
								WHERE
									DAYS(RSXXI.LD_RPS_X_PAY_DU) <= DAYS(Current Date)
								) RSXX
								ON RSXX.BF_SSN = LNXX.BF_SSN
								AND RSXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
						LEFT JOIN (
								SELECT
									LNXXI.BF_SSN
									,LNXXI.LN_SEQ
									,LNXXI.LD_DFR_BEG
									,LNXXI.LD_DFR_END
								FROM
									PKUB.DFXX_BR_DFR_REQ DFXXI
									INNER JOIN PKUB.LNXX_BR_DFR_APV LNXXI
										ON LNXXI.BF_SSN = DFXXI.BF_SSN
										AND LNXXI.LF_DFR_CTL_NUM = DFXXI.LF_DFR_CTL_NUM
								WHERE
									DFXXI.LC_DFR_STA = 'A'
									AND DFXXI.LC_STA_DFRXX = 'A'
									AND LNXXI.LC_STA_LONXX = 'A'
									AND LNXXI.LC_DFR_RSP <> 'XXX'
							) LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						LEFT JOIN (
								SELECT
									LNXXI.BF_SSN
									,LNXXI.LN_SEQ
									,LNXXI.LD_FOR_BEG
									,LNXXI.LD_FOR_END
								FROM
									PKUB.FBXX_BR_FOR_REQ FBXXI
									INNER JOIN PKUB.LNXX_BR_FOR_APV LNXXI
										ON LNXXI.BF_SSN = FBXXI.BF_SSN
										AND LNXXI.LF_FOR_CTL_NUM = FBXXI.LF_FOR_CTL_NUM
								WHERE
									FBXXI.LC_FOR_STA = 'A'
									AND FBXXI.LC_STA_FORXX = 'A'
									AND LNXXI.LC_STA_LONXX = 'A'
									AND LNXXI.LC_FOR_RSP <> 'XXX'
							) LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
					WHERE	
						LNXX.LA_CUR_PRI > X 
						AND	LNXX.LC_STA_LONXX = 'R'
					GROUP BY
						LNXX.BF_SSN
						,LNXX.LN_SEQ
						,MaxRPS.LN_RPS_SEQ	
						
					FOR READ ONLY WITH UR
				);	

		DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

PROC SQL;
CREATE TABLE RX AS
(
	SELECT
		B.*,
		LNXX.*
	FROM
		BaseRX B 
		INNER JOIN LNXXCTE LNXX
			ON B.BF_SSN = LNXX.BF_SSN
			AND B.LN_SEQ = LNXX.LN_SEQ
			AND B.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
			AND B.Months BETWEEN LNXX.StartingMonth AND LNXX.EndingMonth
	WHERE
		B.MonthlyInterestAccrual >= LNXX.LA_RPS_ISL
);

QUIT;

PROC SQL;
	CONNECT TO DBX (DATABASE=DNFPUTDL);
	CREATE TABLE BaseRX AS
		SELECT 
			*
		FROM 
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN
						,LNXX.LN_SEQ
						,MAX(CONCAT(FSXX.LF_FED_AWD,FSXX.LN_FED_AWD_SEQ)) AS AwardId
						,MAX(LNXX.LN_RPS_SEQ) AS LN_RPS_SEQ
						,(MAX(RSXX.DaysSinceFirstPayment)
							- SUM(CASE WHEN (LNXX.LD_DFR_BEG IS NOT NULL AND RSXX.LD_RPS_X_PAY_DU >= LNXX.LD_DFR_BEG AND RSXX.LD_RPS_X_PAY_DU <= LNXX.LD_DFR_END) THEN DAYS(LNXX.LD_DFR_END) - DAYS(RSXX.LD_RPS_X_PAY_DU)
							      WHEN (LNXX.LD_DFR_BEG IS NOT NULL AND RSXX.LD_RPS_X_PAY_DU <= LNXX.LD_DFR_BEG AND RSXX.LD_RPS_X_PAY_DU <= LNXX.LD_DFR_END) THEN DAYS(LNXX.LD_DFR_END) - DAYS(LNXX.LD_DFR_BEG)
								  ELSE X END) /*Deferment days*/
							- SUM(CASE WHEN (LNXX.LD_FOR_BEG IS NOT NULL AND RSXX.LD_RPS_X_PAY_DU >= LNXX.LD_FOR_BEG AND RSXX.LD_RPS_X_PAY_DU <= LNXX.LD_FOR_END) THEN DAYS(LNXX.LD_FOR_END) - DAYS(RSXX.LD_RPS_X_PAY_DU)
							      WHEN (LNXX.LD_FOR_BEG IS NOT NULL AND RSXX.LD_RPS_X_PAY_DU <= LNXX.LD_FOR_BEG AND RSXX.LD_RPS_X_PAY_DU <= LNXX.LD_FOR_END) THEN DAYS(LNXX.LD_FOR_END) - DAYS(LNXX.LD_FOR_BEG)
								  ELSE X END) /*Forbearance days*/
						)/XX AS Months /*Days to calculate active graduation term from*/
					FROM	
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.FSXX_DL_LON FSXX
							ON FSXX.BF_SSN = LNXX.BF_SSN
							AND FSXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.LNXX_LON_RPS LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
							AND LNXX.LC_STA_LONXX = 'I'
							AND LNXX.LC_TYP_SCH_DIS NOT IN('IB','IL','IX','IP','CX','CX','CX','CL','CQ','CA','CP','IA','IX','RE')
						INNER JOIN PKUB.LNXX_LON_RPS_SPF LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ 
							AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
							AND LNXX.LN_GRD_RPS_SEQ = X
						INNER JOIN (
								SELECT
									DAYS(Current Date) - DAYS(RSXXI.LD_RPS_X_PAY_DU) AS DaysSinceFirstPayment
									,RSXXI.LD_RPS_X_PAY_DU
									,RSXXI.BF_SSN
									,RSXXI.LN_RPS_SEQ
								FROM 
									PKUB.RSXX_BR_RPD RSXXI
								WHERE
									DAYS(RSXXI.LD_RPS_X_PAY_DU) <= DAYS(Current Date)
								) RSXX
								ON RSXX.BF_SSN = LNXX.BF_SSN
								AND RSXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
						LEFT JOIN (
								SELECT
									LNXXI.BF_SSN
									,LNXXI.LN_SEQ
									,LNXXI.LD_DFR_BEG
									,LNXXI.LD_DFR_END
								FROM
									PKUB.DFXX_BR_DFR_REQ DFXXI
									INNER JOIN PKUB.LNXX_BR_DFR_APV LNXXI
										ON LNXXI.BF_SSN = DFXXI.BF_SSN
										AND LNXXI.LF_DFR_CTL_NUM = DFXXI.LF_DFR_CTL_NUM
								WHERE
									DFXXI.LC_DFR_STA = 'A'
									AND DFXXI.LC_STA_DFRXX = 'A'
									AND LNXXI.LC_STA_LONXX = 'A'
									AND LNXXI.LC_DFR_RSP <> 'XXX'
							) LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						LEFT JOIN (
								SELECT
									LNXXI.BF_SSN
									,LNXXI.LN_SEQ
									,LNXXI.LD_FOR_BEG
									,LNXXI.LD_FOR_END
								FROM
									PKUB.FBXX_BR_FOR_REQ FBXXI
									INNER JOIN PKUB.LNXX_BR_FOR_APV LNXXI
										ON LNXXI.BF_SSN = FBXXI.BF_SSN
										AND LNXXI.LF_FOR_CTL_NUM = FBXXI.LF_FOR_CTL_NUM
								WHERE
									FBXXI.LC_FOR_STA = 'A'
									AND FBXXI.LC_STA_FORXX = 'A'
									AND LNXXI.LC_STA_LONXX = 'A'
									AND LNXXI.LC_FOR_RSP <> 'XXX'
							) LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
					WHERE	
						LNXX.LA_CUR_PRI > X 
						AND	LNXX.LC_STA_LONXX = 'R'
					GROUP BY
						LNXX.BF_SSN
						,LNXX.LN_SEQ

					FOR READ ONLY WITH UR
				) 			
;	

		DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

PROC SQL;
CONNECT TO DBX (DATABASE=DNFPUTDL);
CREATE TABLE NextRPS AS
	SELECT
		BaseRX.*,
		NextRps.LN_RPS_SEQ AS NewRps,
		NextRps.MonthlyInterestAccrual,
		NextRps.InterestAtRedisclosure,
		NextRps.DateOfRedisclosure
	FROM 
		BaseRX INNER JOIN CONNECTION TO DBX 
				(
					SELECT
						Y.LN_RPS_SEQ AS LN_RPS_SEQ,
						X.LN_RPS_SEQ AS InactiveRPS,
						Y.BF_SSN,
						Y.LN_SEQ,
						Y.MonthlyInterestAccrual,
						Y.InterestAtRedisclosure,
						Y.DateOfRedisclosure
					FROM
						(
							SELECT
								LNXXA.LN_RPS_SEQ,
								LNXXA.BF_SSN,
								LNXXA.LN_SEQ,
								ROWNUMBER() OVER (PARTITION BY LNXXA.BF_SSN, LNXXA.LN_SEQ ORDER BY LNXXA.LN_RPS_SEQ) AS RPSRank /*rank the rps sequences so we can join on it later (This is the inactive row)*/
							FROM
								PKUB.LNXX_LON_RPS LNXXA
								INNER JOIN PKUB.LNXX_LON_RPS_SPF LNXXA
									ON LNXXA.BF_SSN = LNXXA.BF_SSN
									AND LNXXA.LN_SEQ = LNXXA.LN_SEQ
									AND LNXXA.LN_RPS_SEQ = LNXXA.LN_RPS_SEQ 
									AND LNXXA.LN_GRD_RPS_SEQ = X
						) X
						INNER JOIN 
						(
							SELECT
								LNXXA.LN_RPS_SEQ,
								LNXXA.BF_SSN,
								LNXXA.LN_SEQ,
								((LNXXA.LR_INT_RPD_DIS * LNXXA.LA_CPI_RPD_DIS)/XXXX) AS MonthlyInterestAccrual,
								LNXXA.LA_ACR_INT_RPD AS InterestAtRedisclosure,
								LNXXA.LD_CRT_LONXX AS DateOfRedisclosure,
								ROWNUMBER() OVER (PARTITION BY LNXXA.BF_SSN, LNXXA.LN_SEQ ORDER BY LNXXA.LN_RPS_SEQ) AS RPSRank /*rank the rps sequences so we cna join on it later (this one is going to be the next rps after the inactive row)*/
							FROM
								PKUB.LNXX_LON_RPS LNXXA
								INNER JOIN PKUB.LNXX_LON_RPS_SPF LNXXA
									ON LNXXA.BF_SSN = LNXXA.BF_SSN
									AND LNXXA.LN_SEQ = LNXXA.LN_SEQ
									AND LNXXA.LN_RPS_SEQ = LNXXA.LN_RPS_SEQ
									AND LNXXA.LN_GRD_RPS_SEQ = X
						) Y 
							ON X.BF_SSN = Y.BF_SSN
							AND X.LN_SEQ = Y.LN_SEQ
							AND X.RPSRank+X = Y.RPSRank
				) NextRps
					ON NextRps.BF_SSN = BaseRX.BF_SSN
					AND NextRps.LN_SEQ = BaseRX.LN_SEQ
					AND NextRps.InactiveRPS = BaseRX.LN_RPS_SEQ /*Limit result set to only pick up the first RPS after the inactive one*/
;	

		DISCONNECT FROM DBX;

QUIT;
PROC SQL;
CREATE TABLE RX AS
(
	SELECT
		B.*,
		LNXX.*
	FROM
		NextRPS B 
		INNER JOIN LNXXCTE LNXX
			ON B.BF_SSN = LNXX.BF_SSN
			AND B.LN_SEQ = LNXX.LN_SEQ
			AND B.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
			AND B.Months BETWEEN LNXX.StartingMonth AND LNXX.EndingMonth
	WHERE
		B.MonthlyInterestAccrual >= LNXX.LA_RPS_ISL
);

QUIT;

PROC SQL;
	CONNECT TO DBX (DATABASE=DNFPUTDL);
	CREATE TABLE RX AS
		SELECT 
			*
		FROM 
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN
						,LNXX.LN_SEQ
						,CONCAT(FSXX.LF_FED_AWD,FSXX.LN_FED_AWD_SEQ) AS AwardId
						,DWXX.WC_DW_LON_STA
						,DWXX.WX_OVR_DW_LON_STA
						,LNXX.Inactivated
						,LNXX.DeferMax
						,LNXX.ForbMax
						,CASE WHEN LNXX.ForbMax is NULL THEN LNXX.DeferMax
							  WHEN LNXX.DeferMax is NULL THEN LNXX.ForbMax
							  WHEN LNXX.DeferMax >= LNXX.ForbMax THEN LNXX.DeferMax 
							  ELSE LNXX.ForbMax END AS LastEndDate
					FROM	
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.FSXX_DL_LON FSXX
							ON FSXX.BF_SSN = LNXX.BF_SSN
							AND FSXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
							ON DWXX.BF_SSN = LNXX.BF_SSN
							AND DWXX.LN_SEQ = LNXX.LN_SEQ
						LEFT JOIN PKUB.LNXX_LON_RPS LNXXActive
							ON LNXX.BF_SSN = LNXXActive.BF_SSN
							AND LNXX.LN_SEQ = LNXXActive.LN_SEQ
							AND LNXXActive.LC_STA_LONXX = 'A'
						LEFT JOIN ( 
								SELECT
									MAX(LNXXI.LF_LST_DTS_LNXX) AS Inactivated
									,LNXXI.BF_SSN
									,LNXXI.LN_SEQ
								FROM
									PKUB.LNXX_LON_RPS LNXXI
								WHERE 
									LNXXI.LC_STA_LONXX <> 'A'
								GROUP BY 
									LNXXI.BF_SSN
									,LNXXI.LN_SEQ								
							) LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						LEFT JOIN (
								SELECT
									MAX(LNXXI.LD_DFR_END) AS DeferMax
									,LNXXI.BF_SSN
									,LNXXI.LN_SEQ
								FROM
									PKUB.DFXX_BR_DFR_REQ DFXXI
									INNER JOIN PKUB.LNXX_BR_DFR_APV LNXXI
										ON LNXXI.BF_SSN = DFXXI.BF_SSN
										AND LNXXI.LF_DFR_CTL_NUM = DFXXI.LF_DFR_CTL_NUM
								WHERE
									DFXXI.LC_DFR_STA = 'A'
									AND DFXXI.LC_STA_DFRXX = 'A'
									AND LNXXI.LC_STA_LONXX = 'A'
									AND LNXXI.LC_DFR_RSP <> 'XXX'
								GROUP BY
									LNXXI.BF_SSN
									,LNXXI.LN_SEQ
							) LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						LEFT JOIN (
								SELECT
									MAX(LNXXI.LD_FOR_END) AS ForbMax
									,LNXXI.BF_SSN
									,LNXXI.LN_SEQ
								FROM
									PKUB.FBXX_BR_FOR_REQ FBXXI
									INNER JOIN PKUB.LNXX_BR_FOR_APV LNXXI
										ON LNXXI.BF_SSN = FBXXI.BF_SSN
										AND LNXXI.LF_FOR_CTL_NUM = FBXXI.LF_FOR_CTL_NUM
								WHERE
									FBXXI.LC_FOR_STA = 'A'
									AND FBXXI.LC_STA_FORXX = 'A'
									AND LNXXI.LC_STA_LONXX = 'A'
									AND LNXXI.LC_FOR_RSP <> 'XXX'
								GROUP BY
									LNXXI.BF_SSN
									,LNXXI.LN_SEQ
							) LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
					WHERE	
						LNXX.LA_CUR_PRI > X 
						AND	LNXX.LC_STA_LONXX = 'R'
						AND LNXXActive.BF_SSN IS NULL
						AND DWXX.WC_DW_LON_STA NOT IN('XX','XX','XX','XX','XX','XX','XX','XX','XX','XX')
						

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA RX;
	SET LEGEND.RX;
RUN;
DATA RX;
	SET LEGEND.RX;
RUN;
DATA RX;
	SET LEGEND.RX;
RUN;

PROC EXPORT DATA = WORK.RX 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RX"; 
RUN;
PROC EXPORT DATA = WORK.RX 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RX"; 
RUN;
PROC EXPORT DATA = WORK.RX 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RX"; 
RUN;

