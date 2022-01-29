SELECT
	Data.DF_SPE_ACC_ID,
	CASE WHEN Data.PaymentAmount < XX AND Data.Repay = X THEN 'REPAYE, IBR, PAYE'
		 WHEN Data.PaymentAmount < X AND Data.ICR = X THEN 'ICR'
		 ELSE '' 
	END,
	Data.PaymentAmount
FROM
(
	SELECT DISTINCT
		PDXX.DF_SPE_ACC_ID,
		SUM(RS.LA_RPS_ISL) AS PaymentAmount,
		CASE WHEN RS.LC_TYP_SCH_DIS IN ('CA','CP','IB','IL','IA','IX','IP','IX') THEN X ELSE X END AS Repay,
		CASE WHEN RS.LC_TYP_SCH_DIS IN ('CQ','CX','CX','CX') THEN X ELSE X END AS ICR
	FROM
		CDW..PDXX_PRS_NME PDXX
		INNER JOIN CDW..LNXX_LON LNXX
			ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		INNER JOIN CDW.calc.RepaymentSchedules RS
			ON RS.BF_SSN = LNXX.BF_SSN
			AND RS.LN_SEQ = LNXX.LN_SEQ
			AND RS.CurrentGradation = X
			AND RS.LC_TYP_SCH_DIS IN('CA', 'CP', 'CX', 'CX', 'CX', 'CQ', 'IB', 'IL', 'IX', 'IP', 'IX', 'IA')
	WHERE
		LNXX.LA_CUR_PRI > X
		AND LNXX.LC_STA_LONXX = 'R'
	GROUP BY
		PDXX.DF_SPE_ACC_ID,
		CASE WHEN RS.LC_TYP_SCH_DIS IN ('CA','CP','IB','IL','IA','IX','IP','IX') THEN X ELSE X END,
		CASE WHEN RS.LC_TYP_SCH_DIS IN ('CQ','CX','CX','CX') THEN X ELSE X END
) Data
WHERE
	CASE WHEN Data.PaymentAmount < XX AND Data.Repay = X THEN 'REPAYE, IBR, PAYE'
		 WHEN Data.PaymentAmount < X AND Data.ICR = X THEN 'ICR'
		 ELSE '' 
	END != ''
