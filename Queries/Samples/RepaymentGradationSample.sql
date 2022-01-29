



--NEW GRADATION LOGIC
USE CDW
GO

IF OBJECT_ID('tempdb..#RD') IS NOT NULL
	DROP TABLE #RD
GO

SELECT
	TS.BF_SSN,
	TS.TermStartDate,
	TS.LN_RPS_TRM,
	TS.TermTotalPayment,
	TS.GradationtotalPayment,
	0.00 [PaymentAmount],
	NULL [NumPayments],
	NULL [FirstPayDue],
	TS.TermSequenceASC
INTO
	#RD
FROM
	( -- term sequenced population
		SELECT
			TP.BF_SSN,
			TP.TermStartDate,
			TP.LN_RPS_TRM,
			TP.TermTotalPayment,
			TP.GradationtotalPayment,
			ROW_NUMBER() OVER (PARTITION BY TP.BF_SSN, TP.TermStartDate ORDER BY TP.LN_RPS_TRM) [TermSequenceASC]
		FROM
			( -- total payments by term and gradation
				SELECT DISTINCT
					RS.BF_SSN,
					RS.TermStartDate,
					RS.LN_RPS_TRM,
					SUM(RS.LA_RPS_ISL) OVER (PARTITION BY RS.BF_SSN, RS.TermStartDate) [GradationTotalPayment],
					SUM(RS.LA_RPS_ISL) OVER (PARTITION BY RS.BF_SSN, RS.TermStartDate, RS.LN_RPS_TRM) [TermTotalPayment]
				FROM 
					[CDW].[calc].[RepaymentSchedules] RS
			) TP
		) TS


;WITH SUMS
AS
(
	SELECT
		RD.BF_SSN,
		RD.TermStartDate,
		RD.LN_RPS_TRM,
		RD.TermTotalPayment,
		RD.GradationtotalPayment,
		RD.TermSequenceASC,
		PaymentAmount = RD.GradationTotalPayment, -- start with first term
		NumPayments = RD.LN_RPS_TRM,
		FirstPayDue = RD.TermStartDate
	FROM
		#RD RD
	WHERE
		RD.TermSequenceASC = 1 -- start with sequence 1
	
	UNION ALL
	
	SELECT
		RD.BF_SSN,
		RD.TermStartDate,
		RD.LN_RPS_TRM,
		RD.TermTotalPayment,
		RD.GradationtotalPayment,
		RD.TermSequenceASC,
		S.PaymentAmount - S.TermTotalPayment,
		RD.LN_RPS_TRM - S.LN_RPS_TRM,
		DATEADD(MONTH, (S.LN_RPS_TRM), RD.TermStartDate)
	FROM
		SUMS S
		INNER JOIN #RD RD ON RD.TermSequenceASC = S.TermSequenceASC + 1 AND RD.BF_SSN = S.BF_SSN AND RD.TermStartDate = S.TermStartDate
)


SELECT
	S.BF_SSN,
	S.TermStartDate,
	S.NumPayments,
	S.PaymentAmount,
	S.FirstPayDue
FROM
	SUMS S
