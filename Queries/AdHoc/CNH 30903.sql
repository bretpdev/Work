--CORE DATA:
SELECT DISTINCT
	LEGD.NAME
	,LEGD.SSN
	,LEGD.REMAINING_PRINCIPAL
	,LEGD.REMAINING_INTEREST
	,(REMAINING_PRINCIPAL + REMAINING_INTEREST) AS TOTAL_PAYOFF
	,CASE
		WHEN LEGD.TOTAL_TERM BETWEEN X AND XXX
		THEN XXX
		WHEN LEGD.TOTAL_TERM BETWEEN XXX AND XXX
		THEN XXX
		WHEN LEGD.TOTAL_TERM BETWEEN XXX AND XXX
		THEN XXX
		WHEN LEGD.TOTAL_TERM BETWEEN XXX AND XXX
		THEN XXX
		WHEN LEGD.TOTAL_TERM BETWEEN XXX AND XXX
		THEN XXX
		WHEN LEGD.TOTAL_TERM BETWEEN XXX AND XXXX
		THEN XXXX
		ELSE LEGD.TOTAL_TERM
	END AS TOTAL_TERM
	,LEGD.DATE_LAST_DISCLOSED
	,LEGD.REPAYMENT_PLAN
INTO 
	#LEGD
FROM 
	OPENQUERY 
	(
		LEGEND,
		'
			SELECT DISTINCT
				TRIM(PDXX.DM_PRS_X) || '' '' ||TRIM(PDXX.DM_PRS_LST) AS NAME
				,WQXX.BF_SSN										 AS SSN
				,LNXX_RP.LA_CUR_PRI									 AS REMAINING_PRINCIPAL
				,DWXX_RI.WA_TOT_BRI_OTS								 AS REMAINING_INTEREST
				,LNXX_TT.TOTAL_TERM									 
				,LNXX_DLD.LD_CRT_LONXX								 AS DATE_LAST_DISCLOSED
				,LNXX_RP.LC_TYP_SCH_DIS								 AS REPAYMENT_PLAN
			FROM
				PKUB.WQXX_TSK_QUE WQXX
				LEFT JOIN PKUB.PDXX_PRS_NME PDXX
					ON WQXX.BF_SSN = PDXX.DF_PRS_ID
									
				LEFT JOIN 
				(
				--remaining principal:

					SELECT DISTINCT
						LNXX.BF_SSN
						,SUM(COALESCE(LNXX.LA_CUR_PRI,X)) AS LA_CUR_PRI
					FROM
						PKUB.LNXX_LON LNXX
					GROUP BY
						LNXX.BF_SSN
				) LNXX_RP
					ON WQXX.BF_SSN = LNXX_RP.BF_SSN

				LEFT JOIN
				(
				--remaining interest:

					SELECT DISTINCT
						DWXX.BF_SSN
						,SUM(COALESCE(DWXX.WA_TOT_BRI_OTS,X)) AS WA_TOT_BRI_OTS
					FROM
						PKUB.DWXX_DW_CLC_CLU DWXX
					GROUP BY
						DWXX.BF_SSN
				) DWXX_RI
					ON WQXX.BF_SSN = DWXX_RI.BF_SSN 

				LEFT JOIN
				(
				--total term:

					SELECT
						LNXX.BF_SSN
						,SUM(LNXX.LN_RPS_TRM) AS TOTAL_TERM
					FROM
						PKUB.LNXX_LON_RPS_SPF LNXX
						INNER JOIN
						(
							SELECT
								LNXX.BF_SSN
								,LNXX.LD_CRT_LONXX
								,LNXX.LN_RPS_SEQ
								,MAX(LNXX.LN_SEQ) AS LN_SEQ
							FROM
								PKUB.LNXX_LON_RPS_SPF LNXX
								INNER JOIN PKUB.RSXX_BR_RPD RSXX
									ON LNXX.BF_SSN = RSXX.BF_SSN
									AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
									AND	RSXX.LC_STA_RPSTXX = ''A''
								INNER JOIN
								(
									SELECT
										LNXX.BF_SSN
										,LNXX.LD_CRT_LONXX
										,MAX(LNXX.LN_RPS_SEQ) AS LN_RPS_SEQ										
									FROM
										PKUB.LNXX_LON_RPS_SPF LNXX
										INNER JOIN PKUB.RSXX_BR_RPD RSXX
											ON LNXX.BF_SSN = RSXX.BF_SSN
											AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
											AND	RSXX.LC_STA_RPSTXX = ''A''
										INNER JOIN
										(
											SELECT 
												LNXX.BF_SSN
												,MAX(LNXX.LD_CRT_LONXX) AS LD_CRT_LONXX
											FROM
												PKUB.LNXX_LON_RPS_SPF LNXX
												INNER JOIN PKUB.RSXX_BR_RPD RSXX
													ON LNXX.BF_SSN = RSXX.BF_SSN
													AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
													AND	RSXX.LC_STA_RPSTXX = ''A''
											GROUP BY
												LNXX.BF_SSN
										) LNXX_MAX_CRT
											ON LNXX.BF_SSN = LNXX_MAX_CRT.BF_SSN
											AND LNXX.LD_CRT_LONXX = LNXX_MAX_CRT.LD_CRT_LONXX		
										GROUP BY
											LNXX.BF_SSN
											,LNXX.LD_CRT_LONXX
								) LNXX_MAX_RPS
									ON LNXX_MAX_RPS.BF_SSN = LNXX.BF_SSN
									AND LNXX_MAX_RPS.LD_CRT_LONXX = LNXX.LD_CRT_LONXX
									AND LNXX_MAX_RPS.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
							GROUP BY
								LNXX.BF_SSN
								,LNXX.LD_CRT_LONXX
								,LNXX.LN_RPS_SEQ
						) LNXX_MAX_LN
							ON LNXX_MAX_LN.BF_SSN = LNXX.BF_SSN
							AND LNXX_MAX_LN.LD_CRT_LONXX = LNXX.LD_CRT_LONXX
							AND LNXX_MAX_LN.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
							AND LNXX_MAX_LN.LN_SEQ = LNXX.LN_SEQ
						GROUP BY
							LNXX.BF_SSN
					) LNXX_TT
						ON WQXX.BF_SSN = LNXX_TT.BF_SSN

				LEFT JOIN
				(
				--date last disclosed:

					SELECT DISTINCT
						LNXX.BF_SSN
						,MAX(LNXX.LD_CRT_LONXX) AS LD_CRT_LONXX
					FROM
						PKUB.LNXX_LON_RPS LNXX
					GROUP BY
						LNXX.BF_SSN
				) LNXX_DLD
					ON WQXX.BF_SSN = LNXX_DLD.BF_SSN

				LEFT JOIN
				(
				--repayment plan:

					SELECT DISTINCT
						LNXX.BF_SSN
						,LNXX.LC_TYP_SCH_DIS
					FROM
						PKUB.LNXX_LON_RPS LNXX
						INNER JOIN
						(
							SELECT DISTINCT
								LNXX.BF_SSN
								,MAX(LNXX.LD_CRT_LONXX) AS LD_CRT_LONXX
							FROM
								PKUB.LNXX_LON_RPS LNXX
							GROUP BY
								LNXX.BF_SSN
						) LNXX_MAX
							ON LNXX.BF_SSN = LNXX_MAX.BF_SSN
							AND LNXX.LD_CRT_LONXX = LNXX_MAX.LD_CRT_LONXX
				) LNXX_RP
					ON WQXX.BF_SSN = LNXX_RP.BF_SSN
			WHERE
				WQXX.WX_MSG_X_TSK IN (''MONITOR-LOANS WILL NOT PAYOFF WITHIN CURRENT TERMS'', ''NO TERM REMAINS. LOAN IS NOT A CURE'')
				AND WQXX.WF_QUE = ''RX''
		'
	) LEGD;

--SELECT * FROM #LEGD

--CURRENT PAYMENT:
SELECT
	BF_SSN
	,SUM(LA_BIL_CUR_DU) AS CURRENT_PAYMENT_AMOUNT
INTO
	#CURPAY
FROM
	OPENQUERY
	(
		LEGEND,
		'
		--current payment amount:

			SELECT distinct 
				LNXX.BF_SSN
				,LNXX.LN_SEQ
				,LNXX.LA_BIL_CUR_DU
				,LNXX.LD_BIL_DU_LON
				,LNXX.LN_SEQ_BIL_WI_DTE
			FROM
				PKUB.LNXX_LON_BIL_CRF LNXX
					INNER JOIN PKUB.LNXX_LON LNXX
						ON LNXX.BF_SSN = LNXX.BF_SSN
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
					INNER JOIN PKUB.WQXX_TSK_QUE WQXX
						ON LNXX.BF_SSN = WQXX.BF_SSN
					INNER JOIN
					(
						SELECT
							LNXX.BF_SSN
							,LNXX.LD_BIL_DU_LON
							,MAX(LNXX.LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE
						FROM
							PKUB.LNXX_LON_BIL_CRF LNXX
								INNER JOIN PKUB.LNXX_LON LNXX
									ON LNXX.BF_SSN = LNXX.BF_SSN
									AND LNXX.LN_SEQ = LNXX.LN_SEQ
								INNER JOIN PKUB.WQXX_TSK_QUE WQXX
									ON LNXX.BF_SSN = WQXX.BF_SSN		
								INNER JOIN
								(
									SELECT
										LNXX.BF_SSN
										,MAX(LNXX.LD_BIL_DU_LON) AS LD_BIL_DU_LON	
									FROM
										PKUB.LNXX_LON_BIL_CRF LNXX
										INNER JOIN PKUB.LNXX_LON LNXX
											ON LNXX.BF_SSN = LNXX.BF_SSN
											AND LNXX.LN_SEQ = LNXX.LN_SEQ
										INNER JOIN PKUB.WQXX_TSK_QUE WQXX
											ON LNXX.BF_SSN = WQXX.BF_SSN									
									WHERE
										LNXX.LA_CUR_PRI > X
										AND LNXX.LC_STA_LONXX = ''A''
										AND LNXX.LC_BIL_TYP_LON = ''P''
										AND DAYS(LNXX.LD_BIL_CRT) < DAYS(CURRENT DATE)
										AND	WQXX.WX_MSG_X_TSK IN (''MONITOR-LOANS WILL NOT PAYOFF WITHIN CURRENT TERMS'', ''NO TERM REMAINS. LOAN IS NOT A CURE'')
										AND WQXX.WF_QUE = ''RX''
									GROUP BY
										LNXX.BF_SSN
								) LNXX_MAX
									ON LNXX_MAX.BF_SSN = LNXX.BF_SSN
									AND LNXX_MAX.LD_BIL_DU_LON = LNXX.LD_BIL_DU_LON
						WHERE
							LNXX.LA_CUR_PRI > X
							AND LNXX.LC_STA_LONXX = ''A''
							AND LNXX.LC_BIL_TYP_LON = ''P''
							AND DAYS(LNXX.LD_BIL_CRT) < DAYS(CURRENT DATE)
							AND	WQXX.WX_MSG_X_TSK IN (''MONITOR-LOANS WILL NOT PAYOFF WITHIN CURRENT TERMS'', ''NO TERM REMAINS. LOAN IS NOT A CURE'')
							AND WQXX.WF_QUE = ''RX''
						GROUP BY
							LNXX.BF_SSN
							,LNXX.LD_BIL_DU_LON
					) LNXX_MAX
						ON LNXX_MAX.BF_SSN = LNXX.BF_SSN
						AND LNXX_MAX.LD_BIL_DU_LON = LNXX.LD_BIL_DU_LON
						AND LNXX_MAX.LN_SEQ_BIL_WI_DTE = LNXX.LN_SEQ_BIL_WI_DTE
			WHERE
				LNXX.LA_CUR_PRI > X
				AND LNXX.LC_STA_LONXX = ''A''
				AND LNXX.LC_BIL_TYP_LON = ''P''
				AND DAYS(LNXX.LD_BIL_CRT) < DAYS(CURRENT DATE)
				AND	WQXX.WX_MSG_X_TSK IN (''MONITOR-LOANS WILL NOT PAYOFF WITHIN CURRENT TERMS'', ''NO TERM REMAINS. LOAN IS NOT A CURE'')
				AND WQXX.WF_QUE = ''RX''
			'
	) CURPAY
GROUP BY 
	BF_SSN;

SELECT * FROM #CURPAY

--BALLOON PAYMENTS:
SELECT DISTINCT
	BALLOON.BF_SSN
	,SUM(BALLOON.NORMAL_AMT) AS NORMAL_AMOUNT
	,SUM(BALLOON.BALLOON_AMT) AS BALLOON_AMOUNT
INTO 
	#BALLOON
FROM 
	OPENQUERY 
	(
		LEGEND,
		'
		--query from NH XXXX - Balloon RS Level -FED

			SELECT DISTINCT
				LNXX.BF_SSN
				--,LNXX.LN_RPS_SEQ
				--,LNXX.LC_TYP_SCH_DIS
--/*			,GRDS.ALM_LN_GRD_RPS_SEQ*/
--/*			,GRDS.MAX_LN_GRD_RPS_SEQ*/
				,LNXX_ALM.LA_RPS_ISL AS NORMAL_AMT
				,LNXX_MAX.LA_RPS_ISL AS BALLOON_AMT
			FROM
				PKUB.LNXX_LON_RPS LNXX
				INNER JOIN PKUB.LNXX_LON LNXX
					ON LNXX.BF_SSN = LNXX.BF_SSN
					AND LNXX.LN_SEQ = LNXX.LN_SEQ
				INNER JOIN 
				(
					SELECT
						LNXX.BF_SSN
						,LNXX.LN_RPS_SEQ
						,LNXX.LN_GRD_RPS_SEQ AS ALM_LN_GRD_RPS_SEQ
						,MAX_GRD.LN_GRD_RPS_SEQ AS MAX_LN_GRD_RPS_SEQ
					FROM 
						PKUB.LNXX_LON_RPS LNXX
						INNER JOIN PKUB.LNXX_LON_RPS_SPF LNXX
							ON LNXX.BF_SSN = LNXX.BF_SSN
							AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
						INNER JOIN 
						(
							SELECT
								LNXX.BF_SSN
								,LNXX.LN_RPS_SEQ
								,MAX(LNXX.LN_GRD_RPS_SEQ) AS LN_GRD_RPS_SEQ
							FROM 
								PKUB.LNXX_LON_RPS LNXX	
								INNER JOIN PKUB.LNXX_LON_RPS_SPF LNXX	
									ON LNXX.BF_SSN = LNXX.BF_SSN
									AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
							WHERE 
								LNXX.LC_STA_LONXX = ''A''
								AND LNXX.LN_GRD_RPS_SEQ > X
								AND LNXX.LN_RPS_TRM = X
--/*								AND LNXX.BF_SSN LIKE ''XXXX%''*/
							GROUP BY 
								LNXX.BF_SSN
								,LNXX.LN_RPS_SEQ
						) MAX_GRD
							ON LNXX.BF_SSN = MAX_GRD.BF_SSN
							AND LNXX.LN_RPS_SEQ = MAX_GRD.LN_RPS_SEQ
					WHERE 
						LNXX.LN_GRD_RPS_SEQ = MAX_GRD.LN_GRD_RPS_SEQ - X
				) GRDS
					ON LNXX.BF_SSN = GRDS.BF_SSN
					AND LNXX.LN_RPS_SEQ = GRDS.LN_RPS_SEQ
				INNER JOIN 
				(
					SELECT
						LNXX.BF_SSN
						,LNXX.LN_RPS_SEQ
						,LNXX.LN_GRD_RPS_SEQ
						,SUM(LNXX.LA_RPS_ISL) AS LA_RPS_ISL
					FROM 
						PKUB.LNXX_LON_RPS_SPF LNXX
					GROUP BY 
						LNXX.BF_SSN
						,LNXX.LN_RPS_SEQ
						,LNXX.LN_GRD_RPS_SEQ
				) LNXX_MAX
					ON LNXX.BF_SSN = LNXX_MAX.BF_SSN
					AND LNXX.LN_RPS_SEQ = LNXX_MAX.LN_RPS_SEQ
					AND GRDS.MAX_LN_GRD_RPS_SEQ = LNXX_MAX.LN_GRD_RPS_SEQ
				INNER JOIN 
				(
					SELECT
						LNXX.BF_SSN
						,LNXX.LN_RPS_SEQ
						,LNXX.LN_GRD_RPS_SEQ
						,SUM(LNXX.LA_RPS_ISL) AS LA_RPS_ISL
					FROM 
						PKUB.LNXX_LON_RPS_SPF LNXX
					GROUP BY
						LNXX.BF_SSN
						,LNXX.LN_RPS_SEQ
						,LNXX.LN_GRD_RPS_SEQ
				) LNXX_ALM
					ON LNXX.BF_SSN = LNXX_ALM.BF_SSN
					AND LNXX.LN_RPS_SEQ = LNXX_ALM.LN_RPS_SEQ
					AND GRDS.ALM_LN_GRD_RPS_SEQ = LNXX_ALM.LN_GRD_RPS_SEQ
			WHERE
				LNXX_MAX.LA_RPS_ISL >= (LNXX_ALM.LA_RPS_ISL * X)
		'
	) BALLOON
GROUP BY
	BALLOON.BF_SSN;

SELECT * FROM #LEGD 
SELECT * FROM #CURPAY
SELECT * FROM #BALLOON 

--DROP TABLE #LEGD
--DROP TABLE #CURPAY
--DROP TABLE #BALLOON

--combining those with several plan rows into one row field
;WITH CTE AS
(
	SELECT DISTINCT
		LX.SSN
		,(LX.REPAYMENT_PLAN + ', ' + LX.REPAYMENT_PLAN) AS REPAYMENT_PLAN
		,ROW_NUMBER() OVER (PARTITION BY LX.SSN ORDER BY LX.SSN) AS ROWNUM
	FROM 
		#LEGD LX
		INNER JOIN #LEGD LX
			ON LX.SSN = LX.SSN
			AND LX.REPAYMENT_PLAN != LX.REPAYMENT_PLAN
)
SELECT 
	* 
INTO
	#MULTIPLANS
FROM 
	CTE
WHERE
	ROWNUM = X

SELECT * FROM #MULTIPLANS



--REMAINING TERM CALC:
SELECT DISTINCT 
	LNXX_RPS.BF_SSN
	,-(DATEDIFF(DAY,GETDATE(),LNXX_RPS.RPS_START)) AS DAY_DIFF
INTO 
	#DAY_DIFF
FROM 
	OPENQUERY 
	(
		LEGEND,
		'
			SELECT DISTINCT
				LNXX.BF_SSN
				,LNXX.LD_CRT_LONXX
				,LNXX.LN_RPS_SEQ
				,MAX(RSXX.LD_RPS_X_PAY_DU) AS RPS_START
			FROM
				PKUB.LNXX_LON_RPS_SPF LNXX
				INNER JOIN PKUB.RSXX_BR_RPD RSXX
					ON LNXX.BF_SSN = RSXX.BF_SSN
				INNER JOIN
				(
					SELECT DISTINCT 
						LNXX.BF_SSN
						,LNXX.LD_CRT_LONXX
						,MAX(LNXX.LN_RPS_SEQ) AS LN_RPS_SEQ
					FROM
						PKUB.LNXX_LON_RPS_SPF LNXX
						INNER JOIN
						(
							SELECT DISTINCT
								LNXX.BF_SSN
								,MAX(LNXX.LD_CRT_LONXX) AS LD_CRT_LONXX
							FROM
								PKUB.LNXX_LON_RPS_SPF LNXX
							GROUP BY
								LNXX.BF_SSN
						) LNXX_MAX_CRT
							ON LNXX.BF_SSN = LNXX_MAX_CRT.BF_SSN
							AND LNXX.LD_CRT_LONXX = LNXX_MAX_CRT.LD_CRT_LONXX		
					GROUP BY
						LNXX.BF_SSN
						,LNXX.LD_CRT_LONXX
				) LNXX_MAX_SEQ
					ON LNXX_MAX_SEQ.BF_SSN = LNXX.BF_SSN
					AND LNXX_MAX_SEQ.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
					AND LNXX_MAX_SEQ.LD_CRT_LONXX = LNXX.LD_CRT_LONXX
			GROUP BY
				LNXX.BF_SSN
				,LNXX.LD_CRT_LONXX
				,LNXX.LN_RPS_SEQ
		'
	) LNXX_RPS;


IF OBJECT_ID('tempdb..#D_DAYS') IS NOT NULL
BEGIN
	PRINT 'dropping table #D_DAYS'
	DROP TABLE #D_DAYS
END

SELECT
	*
INTO
	#D_DAYS
FROM
	OPENQUERY
	(
		LEGEND,
		'	--deferment days:
			SELECT
				SEQ.BF_SSN,
				SUM(DAYS(CASE WHEN LNXX.LD_DFR_END > CURRENT DATE THEN CURRENT DATE ELSE LNXX.LD_DFR_END END ) - DAYS(LNXX.LD_DFR_BEG) + X) AS DEF_DAYS_USED
			FROM
				(
					SELECT
						RSXX.BF_SSN,
						LNXX.LF_DFR_CTL_NUM,
						RSXX.LN_RPS_SEQ,
						RSXX.LD_RPS_X_PAY_DU,
						LNXX.LN_SEQ,
						ROW_NUMBER() OVER (PARTITION BY RSXX.BF_SSN ORDER BY RSXX.LN_RPS_SEQ DESC, LNXX.LN_SEQ DESC) AS SEQ
					FROM
						(
							SELECT
								RSXX.BF_SSN,
								RSXX.LN_RPS_SEQ,
								RSXX.LD_RPS_X_PAY_DU,
								ROW_NUMBER() OVER (PARTITION BY RSXX.BF_SSN ORDER BY RSXX.LD_RPS_X_PAY_DU DESC, RSXX.LN_RPS_SEQ DESC) AS RSXX_SEQ
							FROM
								PKUB.RSXX_BR_RPD RSXX
						) RSXX
						INNER JOIN PKUB.DFXX_BR_DFR_REQ DFXX ON DFXX.BF_SSN = RSXX.BF_SSN AND DFXX.LC_DFR_STA = ''A'' AND DFXX.LC_STA_DFRXX = ''A''
						INNER JOIN PKUB.LNXX_BR_DFR_APV LNXX ON LNXX.BF_SSN = RSXX.BF_SSN AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
					WHERE
						LNXX.LC_STA_LONXX = ''A''
						AND 
						LNXX.LD_DFR_BEG >= RSXX.LD_RPS_X_PAY_DU
						AND
						LNXX.LD_DFR_BEG <= CURRENT DATE
						AND
						RSXX.RSXX_SEQ = X
				) SEQ
				INNER JOIN PKUB.LNXX_BR_DFR_APV LNXX ON LNXX.BF_SSN = SEQ.BF_SSN AND LNXX.LN_SEQ = SEQ.LN_SEQ AND SEQ.SEQ = X AND LNXX.LD_DFR_BEG >= SEQ.LD_RPS_X_PAY_DU AND LNXX.LD_DFR_BEG <= CURRENT DATE AND LNXX.LC_STA_LONXX = ''A''
			WHERE
				LNXX.LC_STA_LONXX = ''A''
			GROUP BY
				SEQ.BF_SSN							
		'
	) D

SELECT
	*
FROM
	#D_DAYS

IF OBJECT_ID('tempdb..#F_DAYS') IS NOT NULL
BEGIN
	PRINT 'dropping table #F_DAYS'
	DROP TABLE #F_DAYS
END
SELECT
	*
INTO
	#F_DAYS
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT
				SEQ.BF_SSN,
				SUM(DAYS(CASE WHEN LNXX.LD_FOR_END > CURRENT DATE THEN CURRENT DATE ELSE LNXX.LD_FOR_END END) - DAYS(LNXX.LD_FOR_BEG) + X) AS FRB_DAYS_USED
			FROM
				(
					SELECT
						RSXX.BF_SSN,
						LNXX.LF_FOR_CTL_NUM,
						RSXX.LN_RPS_SEQ,
						RSXX.LD_RPS_X_PAY_DU,
						LNXX.LN_SEQ,
						ROW_NUMBER() OVER (PARTITION BY RSXX.BF_SSN ORDER BY RSXX.LN_RPS_SEQ DESC, LNXX.LN_SEQ DESC) AS SEQ
					FROM
						(
							SELECT
								RSXX.BF_SSN,
								RSXX.LN_RPS_SEQ,
								RSXX.LD_RPS_X_PAY_DU,
								ROW_NUMBER() OVER (PARTITION BY RSXX.BF_SSN ORDER BY RSXX.LD_RPS_X_PAY_DU DESC, RSXX.LN_RPS_SEQ DESC) AS RSXX_SEQ
							FROM
								PKUB.RSXX_BR_RPD RSXX
						) RSXX
						INNER JOIN PKUB.FBXX_BR_FOR_REQ FBXX ON FBXX.BF_SSN = RSXX.BF_SSN AND FBXX.LC_FOR_STA = ''A'' AND FBXX.LC_STA_FORXX = ''A''
						INNER JOIN PKUB.LNXX_BR_FOR_APV LNXX ON LNXX.BF_SSN = RSXX.BF_SSN AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
					WHERE
						LNXX.LC_STA_LONXX = ''A''
						AND 
						LNXX.LD_FOR_BEG >= RSXX.LD_RPS_X_PAY_DU
						AND
						LNXX.LD_FOR_BEG <= CURRENT DATE
						AND
						RSXX.RSXX_SEQ = X
				) SEQ
				INNER JOIN PKUB.LNXX_BR_FOR_APV LNXX ON LNXX.BF_SSN = SEQ.BF_SSN AND LNXX.LN_SEQ = SEQ.LN_SEQ AND SEQ.SEQ = X AND LNXX.LD_FOR_BEG >= SEQ.LD_RPS_X_PAY_DU AND LNXX.LD_FOR_BEG <= CURRENT DATE AND LNXX.LC_STA_LONXX = ''A''
			GROUP BY
				SEQ.BF_SSN		
		'
	) F

SELECT
	*
FROM
	#F_DAYS

IF OBJECT_ID('tempdb..#GRADATIONS') IS NOT NULL
BEGIN
	PRINT 'dropping table #GRADATIONS'
	DROP TABLE #GRADATIONS
END
SELECT 
	*
INTO
	#GRADATIONS
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT
				LNXX.BF_SSN
				,SUM(LNXX.LN_RPS_TRM) AS TOTAL_TERM
			FROM
				PKUB.LNXX_LON_RPS_SPF LNXX
				INNER JOIN
				(
					SELECT
						LNXX.BF_SSN
						,LNXX.LD_CRT_LONXX
						,LNXX.LN_RPS_SEQ
						,MAX(LNXX.LN_SEQ) AS LN_SEQ
					FROM
						PKUB.LNXX_LON_RPS_SPF LNXX
						INNER JOIN PKUB.RSXX_BR_RPD RSXX
							ON LNXX.BF_SSN = RSXX.BF_SSN
							AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
							AND	RSXX.LC_STA_RPSTXX = ''A''
						INNER JOIN
						(
							SELECT
								LNXX.BF_SSN
								,LNXX.LD_CRT_LONXX
								,MAX(LNXX.LN_RPS_SEQ) AS LN_RPS_SEQ										
							FROM
								PKUB.LNXX_LON_RPS_SPF LNXX
								INNER JOIN PKUB.RSXX_BR_RPD RSXX
									ON LNXX.BF_SSN = RSXX.BF_SSN
									AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
									AND	RSXX.LC_STA_RPSTXX = ''A''
								INNER JOIN
								(
									SELECT 
										LNXX.BF_SSN
										,MAX(LNXX.LD_CRT_LONXX) AS LD_CRT_LONXX
									FROM
										PKUB.LNXX_LON_RPS_SPF LNXX
										INNER JOIN PKUB.RSXX_BR_RPD RSXX
											ON LNXX.BF_SSN = RSXX.BF_SSN
											AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
											AND	RSXX.LC_STA_RPSTXX = ''A''
									GROUP BY
										LNXX.BF_SSN
								) LNXX_MAX_CRT
									ON LNXX.BF_SSN = LNXX_MAX_CRT.BF_SSN
									AND LNXX.LD_CRT_LONXX = LNXX_MAX_CRT.LD_CRT_LONXX		
								GROUP BY
									LNXX.BF_SSN
									,LNXX.LD_CRT_LONXX
						) LNXX_MAX_RPS
							ON LNXX_MAX_RPS.BF_SSN = LNXX.BF_SSN
							AND LNXX_MAX_RPS.LD_CRT_LONXX = LNXX.LD_CRT_LONXX
							AND LNXX_MAX_RPS.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
					GROUP BY
						LNXX.BF_SSN
						,LNXX.LD_CRT_LONXX
						,LNXX.LN_RPS_SEQ
				) LNXX_MAX_LN
					ON LNXX_MAX_LN.BF_SSN = LNXX.BF_SSN
					AND LNXX_MAX_LN.LD_CRT_LONXX = LNXX.LD_CRT_LONXX
					AND LNXX_MAX_LN.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
					AND LNXX_MAX_LN.LN_SEQ = LNXX.LN_SEQ
				GROUP BY
					LNXX.BF_SSN
	')


DROP TABLE #TERMS_REMAIN
SELECT 
	G.BF_SSN,
	TMS.sum_LN_RPS_TRM,
	G.TOTAL_TERM,
	TMS.LD_RPS_X_PAY_DU,
	ISNULL(D.DEF_DAYS_USED, X.XX) [DEF_DAYS_USED],
	ISNULL(F.FRB_DAYS_USED, X.XX) [FRB_DAYS_USED],
	(ISNULL(D.DEF_DAYS_USED, X.XX) + ISNULL(F.FRB_DAYS_USED, X.XX)) [TotalDaysUsed],
	((ISNULL(D.DEF_DAYS_USED, X.XX) + ISNULL(F.FRB_DAYS_USED, X.XX)) /XXX.XX) [YEARS],
	((ISNULL(D.DEF_DAYS_USED, X.XX) + ISNULL(F.FRB_DAYS_USED, X.XX)) / XXX.XX*XX) [TERMS_USED],
	DATEDIFF(DAY, TMS.LD_RPS_X_PAY_DU, GETDATE()) [DaysSinceFirstPayment],
	(ISNULL(TMS.sum_LN_RPS_TRM, X.XX) - 
		(
			(
				DATEDIFF(DAY, TMS.LD_RPS_X_PAY_DU, GETDATE()) 
				- 
				(
					ISNULL(D.DEF_DAYS_USED, X.XX) 
					+ 
					ISNULL(F.FRB_DAYS_USED, X.XX)
				)
			)
			/ XXX.XX*XX
		)
	) AS TERMS_REMAIN
INTO
	#TERMS_REMAIN
FROM 
	#GRADATIONS G
	LEFT JOIN #D_DAYS D on D.BF_SSN = G.BF_SSN
	LEFT JOIN #F_DAYS F on F.BF_SSN = G.BF_SSN
	LEFT JOIN 
	(
		SELECT
			LNXX.BF_SSN,
			MAX(LNXXm.LD_RPS_X_PAY_DU) LD_RPS_X_PAY_DU,
			SUM(LN_RPS_TRM) AS sum_LN_RPS_TRM
		FROM
			CDW..LNXX_LON_RPS_SPF LNXX
			INNER JOIN 
			(
				SELECT
					LNXX.BF_SSN,
					LNXX.LN_SEQ,
					RSXX.LD_RPS_X_PAY_DU,
					LNXX.LN_RPS_SEQ,
					ROW_NUMBER() OVER (PARTITION BY LNXX.BF_SSN ORDER BY LNXX.LN_RPS_SEQ DESC, LNXX.LN_SEQ DESC) SEQ
				FROM
					CDW..LNXX_LON_RPS_SPF LNXX
					INNER JOIN CDW..RSXX_BR_RPD RSXX ON LNXX.BF_SSN = RSXX.BF_SSN AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ AND	RSXX.LC_STA_RPSTXX = 'A'
				) LNXXm ON LNXXm.BF_SSN = LNXX.BF_SSN AND LNXXm.LN_SEQ = LNXX.LN_SEQ AND LNXXm.LN_RPS_SEQ = LNXX.LN_RPS_SEQ AND LNXXm.SEQ = X
		GROUP BY
			LNXX.BF_SSN,
			LNXXm.LD_RPS_X_PAY_DU
	) TMS ON TMS.BF_SSN = G.BF_SSN
ORDER BY
	G.BF_SSN


--FINAL OUTPUT TO FSA:
SELECT DISTINCT
	L.NAME
	,L.SSN
	,C.CURRENT_PAYMENT_AMOUNT
	,L.REMAINING_PRINCIPAL
	,L.REMAINING_INTEREST
	,L.TOTAL_PAYOFF
	,ISNULL(L.TOTAL_TERM, X.XX) TOTAL_TERM
	,T.TERMS_REMAIN
	,L.DATE_LAST_DISCLOSED
	,CASE 
		WHEN M.REPAYMENT_PLAN IS NOT NULL 
		THEN M.REPAYMENT_PLAN
		ELSE L.REPAYMENT_PLAN
	END AS REPAYMENT_PLAN
	,B.NORMAL_AMOUNT
	,B.BALLOON_AMOUNT
	,CASE
		WHEN B.BALLOON_AMOUNT >= (B.NORMAL_AMOUNT)*X 
		THEN 'Y'
		ELSE 'N'
	END AS 'XX_BALLOON'
FROM
	#LEGD L
	LEFT JOIN #CURPAY C
		ON L.SSN = C.BF_SSN
	LEFT JOIN #BALLOON B
		ON L.SSN = B.BF_SSN
	LEFT JOIN #MULTIPLANS M
		ON L.SSN = M.SSN
	LEFT JOIN #TERMS_REMAIN T ON L.SSN = T.BF_SSN
WHERE
	T.DaysSinceFirstPayment >= X
