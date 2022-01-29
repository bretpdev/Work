CREATE PROCEDURE [monitor].[GetRedisclosureExemptions]
	@Ssn CHAR(9),
	@R0CreateDate DATETIME
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT
	CASE WHEN ES.IsNotInExemptStatusType = 0 THEN 1 ELSE 0 END [HasAllLoansInExemptStatus],
	CASE WHEN SUM(CASE WHEN LN10.LD_PIF_RPT IS NULL THEN 1 ELSE 0 END) > 0 THEN 0 ELSE 1 END [HasAllLoansPaidInFull],
	CASE WHEN SUM(CASE WHEN LN10.LC_STA_LON10 != 'L' THEN 1 ELSE 0 END) > 0 THEN 0 ELSE 1 END [HasAllLoansInLitigation],
	CASE WHEN FRB.BF_SSN IS NULL THEN 0 ELSE 1 END [IsInExemptForbType],
	CASE WHEN EST.IsInExemptScheduleType > 0 THEN 1 ELSE 0 END [IsInExemptScheduleType],
	CASE WHEN EST.IsInNonExemptScheduleType > 0 THEN 1 ELSE 0 END [IsInNonExemptScheduleType],
	CASE WHEN EST.IsInExemptScheduleType = COUNT(*) THEN 1 ELSE 0 END [AllExemptScheduleTypes],
	CASE WHEN EST.IsInFixedAlternativeScheduleType > 0 THEN 1 ELSE 0 END [IsInFixedAlternativeScheduleType],
	CASE WHEN UDA.HasUndisclosedSetupArc > 0 THEN 1 ELSE 0 END [HasUndisclosedSetupArc],
	CASE WHEN UDA.HasUndisclosedRepayOptionsArc > 0 THEN 1 ELSE 0 END [HasUndisclosedRepayOptionsArc],
	CD10.CreateDate10,
	CASE WHEN LN65.BF_SSN IS NULL THEN 0 ELSE 1 END [RedisclosedAfterR0Date],
	CASE WHEN SUM(CASE WHEN LN10.LC_STA_LON10 != 'D' THEN 1 ELSE 0 END) > 0 THEN 0 ELSE 1 END [HasAllLoansDeconverted]
FROM
	LN10_LON LN10
	LEFT JOIN
	(	-- exempt statuses
		SELECT
			DW01.BF_SSN,
			SUM(CASE WHEN ELS.ExemptLoanStatusId IS NULL THEN 1 ELSE 0 END) [IsNotInExemptStatusType]
		FROM
			DW01_DW_CLC_CLU DW01
			LEFT JOIN monitor.ExemptLoanStatuses ELS ON ELS.LoanStatusCode = DW01.WC_DW_LON_STA
		GROUP BY
			DW01.BF_SSN
	) ES ON ES.BF_SSN = LN10.BF_SSN
	LEFT JOIN LN65_LON_RPS LN65 ON LN65.BF_SSN = LN10.BF_SSN AND LN65.LD_CRT_LON65 > @R0CreateDate
	LEFT JOIN 
	(	-- undisclosed ARC's
		SELECT 
			WQ20.BF_SSN,
			SUM(CASE WHEN ESA.ExemptSetupArcId IS NOT NULL THEN 1 ELSE 0 END) [HasUndisclosedSetupArc],
			SUM(CASE WHEN WQ20.WF_QUE = '10' THEN 1 ELSE 0 END) [HasUndisclosedRepayOptionsArc]
		FROM
			WQ20_TSK_QUE WQ20
			LEFT JOIN monitor.ExemptSetupArcs ESA ON ESA.[Queue] = WQ20.WF_QUE
		WHERE
			WQ20.WC_STA_WQUE20 IN ('A', 'H', 'P', 'U', 'W')
		GROUP BY
			WQ20.BF_SSN
	) UDA ON UDA.BF_SSN = LN10.BF_SSN
	LEFT JOIN
	(	-- minimum qualifying activity request received date
		SELECT
			AY10.BF_SSN,
			MIN(AY10.LD_ATY_REQ_RCV) [CreateDate10]
		FROM
			AY10_BR_LON_ATY AY10
		WHERE
			AY10.PF_REQ_ACT = 'OVRPS'
			AND
			AY10.LD_ATY_REQ_RCV > @R0CreateDate
			AND
			AY10.LC_STA_ACTY10 = 'A'
		GROUP BY
			AY10.BF_SSN
	) CD10 ON CD10.BF_SSN = LN10.BF_SSN
	LEFT JOIN
	( -- is in a forbearance
		SELECT
			FB10.BF_SSN
		FROM
			dbo.FB10_BR_FOR_REQ FB10
			INNER JOIN LN60_BR_FOR_APV LN60 ON LN60.BF_SSN = FB10.BF_SSN AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			INNER JOIN monitor.ExemptForbearanceTypes EFT ON FB10.LC_FOR_TYP = EFT.ForbearanceTypeCode
		WHERE
			GETDATE() BETWEEN LN60.LD_FOR_BEG AND LN60.LD_FOR_END
			AND
			LN60.LC_STA_LON60 = 'A'
			AND 
			FB10.LC_FOR_STA = 'A'
			AND 
			FB10.LC_STA_FOR10 = 'A'
			AND
			LN60.LC_FOR_RSP <> '003' --rejected request
	) FRB ON FRB.BF_SSN = LN10.BF_SSN
	LEFT JOIN
	(	-- exempt schedule types
		SELECT
			LN65.BF_SSN, 
			SUM(CASE WHEN EST.ExemptScheduleTypeId IS NOT NULL THEN 1 ELSE 0 END) [IsInExemptScheduleType],
			SUM(CASE WHEN EST.ExemptScheduleTypeId IS NULL THEN 1 ELSE 0 END) [IsInNonExemptScheduleType],
			SUM(CASE WHEN LN65.LC_TYP_SCH_DIS IN ('FS', 'FG') THEN 1 ELSE 0 END) [IsInFixedAlternativeScheduleType]
		FROM 
			dbo.LN65_LON_RPS LN65
			LEFT JOIN monitor.ExemptScheduleTypes EST ON LN65.LC_TYP_SCH_DIS = EST.ScheduleCode
		WHERE
			LN65.LC_STA_LON65 = 'A'
		GROUP BY
			LN65.BF_SSN
	) EST ON EST.BF_SSN = LN10.BF_SSN
WHERE
	LN10.BF_SSN = @SSN
GROUP BY
	LN65.BF_SSN,
	FRB.BF_SSN,
	ES.IsNotInExemptStatusType,
	CD10.CreateDate10,
	EST.IsInExemptScheduleType,
	EST.IsInNonExemptScheduleType,
	EST.IsInFixedAlternativeScheduleType,
	UDA.HasUndisclosedSetupArc,
	UDA.HasUndisclosedRepayOptionsArc

RETURN 0
