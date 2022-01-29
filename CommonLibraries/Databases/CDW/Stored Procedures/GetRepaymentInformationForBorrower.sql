﻿CREATE PROCEDURE [dbo].[GetRepaymentInformationForBorrower]
	@AccountNumber varchar(10)
AS

SELECT DISTINCT
	LN65.DF_SPE_ACC_ID AS AccountNumber,
    CAST(LN65.[LN_SEQ] AS INT) AS LoanSeq,
	LN65Sched.LC_TYP_SCH_DIS AS ScheduleType,
    LN65.[LA_RPS_ISL] AS PaymentAmount
FROM 
	[LN65_RepaymentSched] LN65
	INNER JOIN 
	(
		SELECT 
			DF_SPE_ACC_ID,
			LN_SEQ,
			MAX(CAST(LD_CRT_LON65 AS DATE)) AS MAXDATE 
		FROM 
			LN65_RepaymentSched
		GROUP BY 
			DF_SPE_ACC_ID,
			LN_SEQ
	)MaxVal
		ON MaxVal.DF_SPE_ACC_ID = LN65.DF_SPE_ACC_ID
		AND MaxVal.LN_SEQ = LN65.LN_SEQ
		AND MaxVal.maxDate = CAST(LN65.LD_CRT_LON65 AS DATE)
	INNER JOIN PD10_PRS_NME PD10
		ON PD10.DF_SPE_ACC_ID = LN65.DF_SPE_ACC_ID
	INNER JOIN LN65_LON_RPS LN65Sched
		ON LN65Sched.BF_SSN = PD10.DF_PRS_ID
		AND LN65Sched.LN_SEQ = LN65.LN_SEQ
		AND LN65Sched.LC_STA_LON65 = 'A'
	WHERE
		LN65.DF_SPE_ACC_ID = @AccountNumber
		AND LN65Sched.LC_TYP_SCH_DIS NOT IN ('IC','IB','IL', 'C1', 'C2', 'C3', 'CA', 'CL', 'CP', 'CQ','I3','IP')

RETURN 0
