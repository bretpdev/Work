CREATE PROCEDURE [scra].[GetUnprocessedRecords]
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

UPDATE
	scra.ScriptProcessing
SET
	TS06Indicator = 0,
	TS06Updated = GETDATE(),
	TS0NIndicator = 0,
	TS0NUpdated = GETDATE()
WHERE
	ScriptAction = 'B'
	AND DeletedAt IS NULL
	AND DeletedBy IS NULL
	AND ErroredAt IS NULL
	AND 
	(
		TS06Indicator IS NULL
		OR TS06Updated IS NULL
		OR TS0NIndicator IS NULL
		OR TS0NUpdated IS NULL
	)


UPDATE
	scra.ScriptProcessing
SET
	TXCXIndicator = 0,
	TXCXUpdated = GETDATE()
WHERE
	TXCXUpdated IS NULL

UPDATE
	scra.ScriptProcessing
SET
	TS0NIndicator = 0,
	TS0NUpdated = GETDATE()
WHERE
	LN10CurPri <= 0
	AND TS0NUpdated IS NULL


SELECT DISTINCT
	SP.ScriptProcessingId,
	SP.BorrSSN,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	SP.DataComparisonId,
	SP.Loan,
	SP.LN72Begin,
	SP.LN72End,
	SP.LN72RegRate,
	SP.LN72SCRA,
	SP.LN10LnAdd,
	SP.LN10Disb,
	SP.LN10CurPri,
	SP.LN10Sta,
	SP.LN10Sub,
	LTRIM(RTRIM(LN10.IC_LON_PGM)) AS LoanProgram,
	SP.DW01Sta,
	SP.DODBegin,
	SP.DODEnd,
	SP.TXCXBegin,  
	SP.TXCXEnd,  
	SP.TXCXType, 
	SP.BenefitSourceId,
	SP.ScriptAction,
	SP.TS06Updated,
	SP.TXCXUpdated,
	SP.TS0NUpdated,
	SP.AAPUpdated,
	DC.CreatedAt AS DataComparisonDate,
	CASE WHEN ExemptSchedule.LN_SEQ IS NOT NULL THEN 1 ELSE 0 END AS ExemptSchedule,
	CASE WHEN LN10.LC_STA_LON10 = 'D' THEN 1 ELSE 0 END AS Deconverted,
	CASE WHEN DW01.WC_DW_LON_STA IN ('01','02','04','08','11','12','16','17','18','19','20','21','22') THEN 1 ELSE 0 END AS ExemptLoanStatus,
	CASE WHEN ExemptForb.LN_SEQ IS NOT NULL THEN 1 ELSE 0 END AS ExemptForbType,
	CASE WHEN LN10.LC_STA_LON10 = 'L' THEN 1 ELSE 0 END AS ExemptLitigation,
	CASE WHEN ExemptDiffering.BF_SSN IS NOT NULL THEN 1 ELSE 0 END AS ExemptDifferingSchedules,
	0 AS ExemptFixedAlternateSchedule,
	CASE WHEN ExemptInactiveSchedule.BF_SSN IS NOT NULL THEN 1 ELSE 0 END AS ExemptInactiveSchedule,
	SUM(LN10.LA_CUR_PRI) OVER(PARTITION BY LN10.BF_SSN) AS BorrBalance,
	SUM(LN15.LA_DSB - COALESCE(LN15.LA_DSB_CAN,0)) OVER(PARTITION BY LN10.BF_SSN) AS BalAtRepay,
	CASE WHEN COALESCE(DW01.WD_LON_RPD_SR,'9999-01-01') = '9999-01-01' THEN LN10.LD_LON_1_DSB ELSE DW01.WD_LON_RPD_SR END AS RepayStart,
	CASE 
		WHEN (LN72Beg.BF_SSN IS NOT NULL AND LN72End.BF_SSN IS NOT NULL AND SP.ScriptAction = 'U')
			OR (LN72Beg.BF_SSN IS NOT NULL AND MilitaryDefer.BF_SSN IS NOT NULL AND MilitaryDefer.LN50_SSN IS NOT NULL AND SP.ScriptAction = 'E')
		THEN 1
		ELSE 0
	END AS SpecialBypass,
	LN10.LF_LON_CUR_OWN AS LoanOwner,
	SP.TSX0TUpdated
FROM
	ULS.scra.ScriptProcessing SP
	INNER JOIN ULS.scra.DataComparison DC
		ON DC.DataComparisonId = SP.DataComparisonId
		AND DC.ActiveRow = 1
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = SP.BorrSSN
		AND LN10.LN_SEQ = SP.Loan
		--AND LN10.LA_CUR_PRI > 0
		AND LN10.LC_STA_LON10 IN('R','D','L')
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
	INNER JOIN UDW..LN15_DSB LN15
		ON LN15.BF_SSN = LN10.BF_SSN
		AND LN15.LN_SEQ = LN10.LN_SEQ
		AND LN15.LC_DSB_TYP = '2' --1's are anticipated and we dont care about those until they become 2 (actual)
		AND LN15.LC_DSB_RLS_STA = 'R' --released disbursment
		AND LN15.LC_STA_LON15 IN ('1','3') --active or reissued
	LEFT JOIN
	(
		SELECT DISTINCT
			LN65.BF_SSN,
			LN65.LN_SEQ
		FROM
			UDW..LN65_LON_RPS LN65
			LEFT JOIN UDW..LN65_LON_RPS NonExempt --finds any loan in a non exempt status
				ON NonExempt.BF_SSN = LN65.BF_SSN
				AND NonExempt.LC_TYP_SCH_DIS NOT IN('IL','IS','RP','IB','I3','IP')
				AND NonExempt.LC_STA_LON65 = 'A'
		WHERE
			LN65.LC_STA_LON65 = 'A'
			AND LN65.LC_TYP_SCH_DIS IN('IL','IS','RP','IB','I3','IP')
			AND NonExempt.BF_SSN IS NULL --Forces only borrowers with ALL loans exempt to show up
	) ExemptSchedule
		ON ExemptSchedule.BF_SSN = LN10.BF_SSN
		AND ExemptSchedule.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT DISTINCT
			LN65.BF_SSN
		FROM
			UDW..LN65_LON_RPS LN65
			INNER JOIN UDW..LN65_LON_RPS NonExempt --finds any loan in a non exempt status
				ON NonExempt.BF_SSN = LN65.BF_SSN
				AND NonExempt.LC_TYP_SCH_DIS NOT IN('IL','IS','RP','IB','I3','IP')
				AND NonExempt.LC_STA_LON65 = 'A'
		WHERE
			LN65.LC_STA_LON65 = 'A'
			AND LN65.LC_TYP_SCH_DIS IN('IL','IS','RP','IB','I3','IP')
	) ExemptDiffering
		ON ExemptDiffering.BF_SSN = LN10.BF_SSN
	LEFT JOIN
	(
		SELECT DISTINCT
			LN60.BF_SSN,
			LN60.LN_SEQ
		FROM
			UDW..LN60_BR_FOR_APV LN60
			INNER JOIN UDW..FB10_BR_FOR_REQ FB10
				ON FB10.BF_SSN = LN60.BF_SSN
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
		WHERE
			CAST(GETDATE() AS DATE) BETWEEN LN60.LD_FOR_BEG AND LN60.LD_FOR_END
			AND LN60.LC_STA_LON60 = 'A'
			AND FB10.LC_STA_FOR10 = 'A'
			AND FB10.LC_FOR_STA = 'A'
			AND LN60.LC_FOR_RSP != '003'
			AND FB10.LC_FOR_TYP IN('10','14') --bky and disability forbs
	) ExemptForb
		ON ExemptForb.BF_SSN = LN10.BF_SSN
		AND ExemptForb.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT DISTINCT	
			LN10.BF_SSN
		FROM
			UDW..LN10_LON LN10
			LEFT JOIN UDW..LN65_LON_RPS LN65
				ON LN65.BF_SSN = LN10.BF_SSN
				AND LN65.LN_SEQ = LN10.LN_SEQ
				AND LN65.LC_STA_LON65 = 'A'
		WHERE
			LN65.BF_SSN IS NULL
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0
	) ExemptInactiveSchedule
		ON ExemptInactiveSchedule.BF_SSN = LN10.BF_SSN
	LEFT JOIN
	(
		SELECT DISTINCT
			LN72.BF_SSN,
			LN72.LN_SEQ,
			LN72.LD_ITR_EFF_BEG
		FROM
			UDW..LN72_INT_RTE_HST LN72
		WHERE
			LN72.LC_STA_LON72 = 'A'
			AND LN72.LC_INT_RDC_PGM IN ('M','P')
	) LN72Beg
		ON LN72Beg.BF_SSN = LN10.BF_SSN
		AND LN72Beg.LN_SEQ = LN10.LN_SEQ
		AND LN72Beg.LD_ITR_EFF_BEG = SP.DODBegin
	LEFT JOIN
	(
		SELECT DISTINCT
			LN72.BF_SSN,
			LN72.LN_SEQ,
			LN72.LD_ITR_EFF_END
		FROM
			UDW..LN72_INT_RTE_HST LN72
		WHERE
			LN72.LC_STA_LON72 = 'A'
			AND	LN72.LC_INT_RDC_PGM IN ('M','P')
	) LN72End
		ON LN72End.BF_SSN = LN10.BF_SSN
		AND LN72End.LN_SEQ = LN10.LN_SEQ
		AND LN72End.LD_ITR_EFF_END = SP.DODEnd
	LEFT JOIN
	(
		SELECT DISTINCT
			LN72.BF_SSN,
			LN72.LN_SEQ,
			LN72.LD_ITR_EFF_END,
			LN50.BF_SSN LN50_SSN
		FROM
			UDW..LN72_INT_RTE_HST LN72
			LEFT JOIN
			(
				SELECT
					LN50.BF_SSN
				FROM
					UDW..LN50_BR_DFR_APV LN50
					INNER JOIN UDW..DF10_BR_DFR_REQ DF10
						ON DF10.BF_SSN = LN50.BF_SSN
						AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
				WHERE
					DF10.LC_DFR_TYP IN ('04', '38', '40')
					AND DF10.LC_STA_DFR10 = 'A'
					AND DF10.LC_DFR_STA = 'A'
					AND LN50.LC_STA_LON50 = 'A'
			) LN50
				ON LN50.BF_SSN = LN72.BF_SSN
		WHERE
			LN72.LC_STA_LON72 = 'A'
			AND	LN72.LC_INT_RDC_PGM IN ('M','P')
	) MilitaryDefer
		ON MilitaryDefer.BF_SSN = LN10.BF_SSN
		AND MilitaryDefer.LN_SEQ = LN10.LN_SEQ
		AND MilitaryDefer.LD_ITR_EFF_END > SP.DODEnd
WHERE
	SP.ErroredAt IS NULL
	AND SP.DeletedAt IS NULL
	AND SP.ScriptAction != 'B'
	AND
	(
		(SP.TS06Indicator IS NULL AND SP.TS06Updated IS NULL) --Interest people
		OR (SP.TS0NIndicator IS NULL AND SP.TS0NUpdated IS NULL AND SP.TS06Indicator = 1 AND SP.TS06Updated IS NOT NULL) --Redisclose people
	)
	AND SP.CreatedBy = SUSER_SNAME()

RETURN 0