
drop table #data
SELECT DISTINCT Data.DF_SPE_ACC_ID AS AccountNumber,
	CASE WHEN Data.PaymentAmount = 0 THEN 1 ELSE 0 END AS HasZeroDollarPayment
into #data
FROM
(
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID,
		SUM(RS.LA_RPS_ISL) AS PaymentAmount
	FROM
		CDW..PD10_PRS_NME PD10
		INNER JOIN CDW..LN10_LON LN10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN CDW.calc.RepaymentSchedules RS
			ON RS.BF_SSN = LN10.BF_SSN
			AND RS.LN_SEQ = LN10.LN_SEQ
			AND RS.CurrentGradation = 1
			AND RS.LC_TYP_SCH_DIS IN('CA', 'C1', 'C2', 'C3', 'IB', 'I3', 'I5', 'IS', 'IL', 'CP', 'CQ', 'IP')
		WHERE
			LN10.LA_CUR_PRI > 0
			AND LN10.LC_STA_LON10 = 'R'
	GROUP BY
		PD10.DF_SPE_ACC_ID
) Data


select count(distinct AccountNumber) as [Zero Payment Count] from #data where HasZeroDollarPayment = 1


SELECT
POP.LC_TYP_SCH_DIS AS [Plan Type],
COUNT(*) as [Plan Count]
FROM
(
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID,
		RS.LC_TYP_SCH_DIS
	FROM
		CDW..PD10_PRS_NME PD10
		INNER JOIN CDW..LN10_LON LN10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN CDW.calc.RepaymentSchedules RS
			ON RS.BF_SSN = LN10.BF_SSN
			AND RS.LN_SEQ = LN10.LN_SEQ
			AND RS.CurrentGradation = 1
			AND RS.LC_TYP_SCH_DIS IN('CA', 'C1', 'C2', 'C3', 'IB', 'I3', 'I5', 'IS', 'IL', 'CP', 'CQ', 'IP')
	
	WHERE
		LN10.LA_CUR_PRI > 0
		AND LN10.LC_STA_LON10 = 'R'
) POP
GROUP BY
	POP.LC_TYP_SCH_DIS