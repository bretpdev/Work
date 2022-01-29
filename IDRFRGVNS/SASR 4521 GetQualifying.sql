USE [CDW]
GO
/****** Object:  StoredProcedure [dbo].[GetQualifyingIDRForgivenessPayments]    Script Date: 5/14/2019 8:44:05 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetQualifyingIDRForgivenessPayments]
	@PlanType VARCHAR(10),
	@BF_SSN VARCHAR(9) = NULL,
	@PlanStartDate DATE = '2099-01-01'
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

--Determine inclusion date for bills and deferments
IF @PlanType = 'ICR' 
	BEGIN
		SET @PlanStartDate = '1994-07-01'
	END
--count ICR payments for borrowers that are IBR but had ICR in the past
ELSE IF @PlanType = 'IBR'
	BEGIN
		SET @PlanStartDate = '2009-07-01'
	END
ELSE IF @PlanType = 'IBR 2014'
	BEGIN
		SET @PlanStartDate = '2014-07-01' --Future enhancement (dont let this one project if loans are disbursed prior to this date)
	END
ELSE IF @PlanType = 'PAYE'
	BEGIN
		SET @PlanStartDate = '2007-10-01'
	END
ELSE IF @PlanType = 'REPAYE'
	BEGIN
		SET @PlanStartDate = '1900-01-01'
	END

SELECT
	Final.BF_SSN,
	Final.LN_SEQ,
	CASE WHEN Final.CurrentActiveSchedule IN('IB','IL') THEN 'IBR'
	     WHEN Final.CurrentActiveSchedule IN('I3','IP') THEN 'IBR 2014'
		 WHEN Final.CurrentActiveSchedule IN('C1','C2','C3','CQ') THEN 'ICR'
		 WHEN Final.CurrentActiveSchedule IN('CA','CP') THEN 'PAYE'
		 WHEN Final.CurrentActiveSchedule IN('I5','IA') THEN 'REPAYE'
		 ELSE 'LEVEL'
	END + ' - ' + Final.CurrentActiveSchedule AS CurrentActiveSchedule,
	Final.CurrentActiveSchedule AS ScheduleCode,
	Final.PaymentsQualifyingLevel,
	Final.PaymentsQualifyingLevelPrevious,
	CASE WHEN @PlanType IN('IBR','REPAYE') THEN Final.PaymentsQualifyingPreConversionIBR ELSE 0 END AS PaymentsQualifyingPreConversionIBR,
	CASE WHEN @PlanType IN('IBR','REPAYE','ICR') THEN Final.PaymentsQualifyingPreConversionICR ELSE 0 END AS PaymentsQualifyingPreConversionICR,
	Final.PaymentsQualifyingIDR,
	Final.PaymentsQualifyingIDRPrevious,
	Final.PaymentsQualifyingPermanentStandard,
	Final.PaymentsQualifyingPermanentStandardPrevious,
	Final.PaymentsCoveredByEHD,
	Final.PaymentsQualifyingLevel 
		+ Final.PaymentsQualifyingLevelPrevious 
		+ CASE WHEN @PlanType IN('IBR','REPAYE') AND Final.PaymentsQualifyingPreConversionICR = 0 THEN Final.PaymentsQualifyingPreConversionIBR
			   WHEN @PlanType IN('ICR') THEN Final.PaymentsQualifyingPreConversionICR 
			   WHEN @PlanType IN('IBR','REPAYE') THEN Final.PaymentsQualifyingPreConversionICR 
			   ELSE 0
		END
		+ Final.PaymentsQualifyingIDR 
		+ Final.PaymentsQualifyingIDRPrevious 
		+ Final.PaymentsQualifyingPermanentStandard 
		+ Final.PaymentsQualifyingPermanentStandardPrevious 
		+ Final.PaymentsCoveredByEHD 
	AS Total
FROM
	(
	SELECT 
		LN10.BF_SSN,
		LN10.LN_SEQ,
		MAX(COALESCE(Schedules.LC_TYP_SCH_DIS,'')) AS CurrentActiveSchedule,
		SUM(CASE WHEN Schedules.LC_STA_LON65 = 'A' AND Schedules.LD_BIL_DU_LON IS NOT NULL THEN Schedules.SatisfiesLevel ELSE 0 END) AS PaymentsQualifyingLevel,
		SUM(CASE WHEN Schedules.LC_STA_LON65 = 'I' AND Schedules.LD_BIL_DU_LON IS NOT NULL THEN Schedules.SatisfiesLevel ELSE 0 END) AS PaymentsQualifyingLevelPrevious, 
		COALESCE(PreConversion.LN_IBR_QLF_PAY_PCV,0) AS PaymentsQualifyingPreConversionIBR,
		COALESCE(PreConversion.LN_ICR_ON_TME_PAY,0) AS PaymentsQualifyingPreConversionICR,
		SUM(CASE WHEN Schedules.LC_STA_LON65 = 'A' AND Schedules.LD_BIL_DU_LON IS NOT NULL THEN Schedules.SatisfiesCurrentIDR ELSE 0 END) AS PaymentsQualifyingIDR, 
		SUM(CASE WHEN Schedules.LC_STA_LON65 = 'I' AND Schedules.LD_BIL_DU_LON IS NOT NULL THEN Schedules.SatisfiesCurrentIDR ELSE 0 END) AS PaymentsQualifyingIDRPrevious,
		SUM(CASE WHEN Schedules.LC_STA_LON65 = 'A' AND Schedules.LD_BIL_DU_LON IS NOT NULL THEN Schedules.SatisfiesPermanentStandard ELSE 0 END) AS PaymentsQualifyingPermanentStandard, 
		SUM(CASE WHEN Schedules.LC_STA_LON65 = 'I' AND Schedules.LD_BIL_DU_LON IS NOT NULL THEN Schedules.SatisfiesPermanentStandard ELSE 0 END) AS PaymentsQualifyingPermanentStandardPrevious,
		CASE WHEN EconHardDefer.BF_SSN IS NOT NULL THEN EconHardDefer.Months + COALESCE(Preconversion.LN_IBR_EHD_DFR_USE,0) ELSE COALESCE(Preconversion.LN_IBR_EHD_DFR_USE,0) END AS PaymentsCoveredByEHD
	FROM
		CDW..LN10_LON LN10
		INNER JOIN
		( --gather preconversion payments
			SELECT
				LN09.BF_SSN,
				LN09.LN_SEQ,
				SUM(COALESCE(LN09.LN_IBR_QLF_PAY_PCV,0)) AS LN_IBR_QLF_PAY_PCV,
				SUM(COALESCE(LN09.LN_ICR_ON_TME_PAY,LN09.LN_IBR_QLF_PAY_PCV,0)) AS LN_ICR_ON_TME_PAY,
				SUM(FLOOR(CAST(COALESCE(LN09.LN_IBR_EHD_DFR_USE,0) AS DECIMAL(14,2))/CAST(30 AS DECIMAL(14,2))) + --number of full months difference
				CASE WHEN CAST(COALESCE(LN09.LN_IBR_EHD_DFR_USE,0) AS DECIMAL(14,2))/CAST(30 AS DECIMAL(14,2)) % 1 > .6 
						THEN 1 --Counts 1 additional month if we are at least 60% through an additional month
						ELSE 0 
				END) AS LN_IBR_EHD_DFR_USE
			FROM
				CDW..LN09_RPD_PIO_CVN LN09
			GROUP BY
				LN09.BF_SSN,
				LN09.LN_SEQ
		) PreConversion
			ON PreConversion.BF_SSN = LN10.BF_SSN
			AND PreConversion.LN_SEQ = LN10.LN_SEQ
		INNER JOIN 
		(
			SELECT DISTINCT
				LN65.BF_SSN,
				LN65.LN_SEQ,
				CASE WHEN LN65.LC_STA_LON65 = 'A' THEN LN65.LC_TYP_SCH_DIS ELSE NULL END AS LC_TYP_SCH_DIS,
				Billing.LD_BIL_DU_LON,
				CASE WHEN LN65.LC_TYP_SCH_DIS = 'L' THEN 1 ELSE 0 END AS SatisfiesLevel,
				CASE WHEN LN65.LC_TYP_SCH_DIS IN('CA','I5','C1','C2','C3','IB','I3') THEN 1 ELSE 0 END AS SatisfiesCurrentIDR,
				CASE WHEN LN65.LC_TYP_SCH_DIS IN('CP','IA','CQ','IL','IP') THEN 1 ELSE 0 END AS SatisfiesPermanentStandard,
				LN65.LC_STA_LON65,
				Billing.LA_BIL_DU_PRT,
				Billing.LA_TOT_BIL_STS
			FROM
				CDW..LN10_LON LN10
				INNER JOIN CDW..LN65_LON_RPS LN65
					ON LN65.BF_SSN = LN10.BF_SSN
					AND LN65.LN_SEQ = LN10.LN_SEQ
				INNER JOIN CDW..RS10_BR_RPD RS10
					ON RS10.BF_SSN = LN65.BF_SSN
					AND RS10.LN_RPS_SEQ = LN65.LN_RPS_SEQ
				LEFT JOIN 
				(
					SELECT
						LN65.BF_SSN,
						LN65.LN_SEQ,
						LN65.LN_RPS_SEQ,
						MIN(NextRS10.LN_RPS_SEQ) OVER(PARTITION BY LN65.BF_SSN, LN65.LN_SEQ, LN65.LN_RPS_SEQ) AS LN_RPS_SEQ_Next,
						MIN(NextRS10.LD_RPS_1_PAY_DU) OVER(PARTITION BY LN65.BF_SSN, LN65.LN_SEQ, LN65.LN_RPS_SEQ) AS LD_RPS_1_PAY_DU,
						ROW_NUMBER() OVER(PARTITION BY LN65.BF_SSN, LN65.LN_SEQ, NextRS10.LD_RPS_1_PAY_DU ORDER BY NextRS10.LD_RPS_1_PAY_DU, LN65.LN_RPS_SEQ DESC) AS Rnk
					FROM
						CDW..LN65_LON_RPS LN65
						INNER JOIN CDW..RS10_BR_RPD RS10
							ON LN65.BF_SSN = RS10.BF_SSN
							AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
						LEFT JOIN CDW..LN65_LON_RPS NextLN65
							ON NextLN65.BF_SSN = LN65.BF_SSN
							AND NextLN65.LN_SEQ = LN65.LN_SEQ
							AND NextLN65.LN_RPS_SEQ > LN65.LN_RPS_SEQ
						LEFT JOIN CDW..RS10_BR_RPD NextRS10
							ON NextRS10.BF_SSN = NextLN65.BF_SSN
							AND NextRS10.LN_RPS_SEQ = NextLN65.LN_RPS_SEQ
							AND NextRS10.LD_RPS_1_PAY_DU >= RS10.LD_RPS_1_PAY_DU
				) RS10Projections
					ON RS10Projections.BF_SSN = LN65.BF_SSN
					AND RS10Projections.LN_SEQ = LN65.LN_SEQ
					AND RS10Projections.LN_RPS_SEQ = LN65.LN_RPS_SEQ
					AND RS10Projections.Rnk = 1
					AND RS10Projections.LN_RPS_SEQ_Next IS NOT NULL
					AND RS10Projections.LD_RPS_1_PAY_DU IS NOT NULL
				LEFT JOIN 
				( --fully satisfied bills applying after the start date for plan forgiveness
					SELECT DISTINCT
						LN80.BF_SSN,
						LN80.LN_SEQ,
						LN80.LA_BIL_DU_PRT,
						LN80.LA_TOT_BIL_STS,
						CAST(LN80.LD_BIL_DU_LON AS DATE) AS LD_BIL_DU_LON
					FROM
						CDW..LN10_LON LN10
						INNER JOIN CDW..LN80_LON_BIL_CRF LN80
							ON LN80.BF_SSN = LN10.BF_SSN
							AND LN80.LN_SEQ = LN10.LN_SEQ
						LEFT JOIN 
						(
							SELECT DISTINCT
								BF_SSN,
								LN_SEQ
							FROM
								CDW..LN65_LON_RPS
							WHERE
								LC_STA_LON65 = 'A'
								AND LC_TYP_SCH_DIS IN('IB','IL')
						) LN65IBR
							ON LN65IBR.BF_SSN = LN80.BF_SSN
							AND LN65IBR.LN_SEQ = LN80.LN_SEQ
						LEFT JOIN
						(
							SELECT DISTINCT
								BF_SSN,
								LN_SEQ
							FROM
								CDW..LN65_LON_RPS
							WHERE
								LC_TYP_SCH_DIS IN('C1','C2','C3','CQ')

							UNION
							
							SELECT DISTINCT
								BF_SSN,
								LN_SEQ
							FROM
								CDW..LN09_RPD_PIO_CVN
							WHERE
								COALESCE(LN_ICR_ON_TME_PAY,0) > 0
						 ) LN65ICR
							ON LN65ICR.BF_SSN = LN80.BF_SSN
							AND LN65ICR.LN_SEQ = LN80.LN_SEQ		
					WHERE
						LN80.LC_STA_LON80 = 'A'
						AND CAST(LN80.LD_BIL_DU_LON AS DATE) >= (CASE WHEN LN65IBR.LN_SEQ IS NOT NULL AND LN65ICR.LN_SEQ IS NOT NULL THEN '1994-07-01' ELSE @PlanStartDate END)
						AND ISNULL(LN80.LA_BIL_DU_PRT,0.00) <= ISNULL(LN80.LA_TOT_BIL_STS,0.00)
						AND LN80.LC_BIL_TYP_LON = 'P'
						AND
						( 
							( --if consolidation then use ld_lon_1_dsb for start date rather than @PlanStartDate
								LN10.IC_LON_PGM IN('CNSLDN','DLCNSL','DLPCNS','DLSCCN','DLSCNS','DLSCPG','DLSCPL','DLSCSC','DLSCSL','DLSCST','DLSCUC','DLSCUN','DLSPCN','DLSSPL','DLUCNS','DLUSPL','DSCON','DUCON','SUBCNS','SUBSPC','UNSPC','UNCNS') 
								AND 
								(
									LN80.LD_BIL_DU_LON >= CAST(CASE WHEN LN10.LD_LON_1_DSB <= (CASE WHEN LN65IBR.LN_SEQ IS NOT NULL AND LN65ICR.LN_SEQ IS NOT NULL THEN '1994-07-01' ELSE @PlanStartDate END) THEN (CASE WHEN LN65IBR.LN_SEQ IS NOT NULL AND LN65ICR.LN_SEQ IS NOT NULL THEN '1994-07-01' ELSE @PlanStartDate END) ELSE LN10.LD_LON_1_DSB END AS DATE)
									OR LN80.LD_BIL_DU_LON IS NULL --Account for active schedule that hasnt had any satisfying payments
								)
							)
							OR LN10.IC_LON_PGM NOT IN('CNSLDN','DLCNSL','DLPCNS','DLSCCN','DLSCNS','DLSCPG','DLSCPL','DLSCSC','DLSCSL','DLSCST','DLSCUC','DLSCUN','DLSPCN','DLSSPL','DLUCNS','DLUSPL','DSCON','DUCON','SUBCNS','SUBSPC','UNSPC','UNCNS') 
						)
				)Billing
					ON Billing.BF_SSN = LN65.BF_SSN
					AND Billing.LN_SEQ = LN65.LN_SEQ
					AND 
					(
						(
							Billing.LD_BIL_DU_LON >= CAST(RS10.LD_RPS_1_PAY_DU AS DATE) 
							AND Billing.LD_BIL_DU_LON < CAST(CASE WHEN RS10Projections.LD_RPS_1_PAY_DU > GETDATE() THEN GETDATE() ELSE RS10Projections.LD_RPS_1_PAY_DU END AS DATE)
							AND LN65.LC_STA_LON65 = 'I'
						)
						OR
						(
							Billing.LD_BIL_DU_LON >= CAST(RS10.LD_RPS_1_PAY_DU AS DATE) 
							AND Billing.LD_BIL_DU_LON <= CAST(GETDATE() AS DATE)
							AND LN65.LC_STA_LON65 = 'A'
						)
					)
				LEFT JOIN 
				(
					SELECT DISTINCT
						BF_SSN,
						LN_SEQ
					FROM
						CDW..LN65_LON_RPS
					WHERE
						LC_TYP_SCH_DIS IN('CA','I5','C1','C2','C3','IB','I3')
				) HadIDRPrevious
					ON HadIDRPrevious.BF_SSN = LN65.BF_SSN
					AND HadIDRPrevious.LN_SEQ = LN65.LN_SEQ
			WHERE
				LN65.LC_TYP_SCH_DIS IN('L','CA','I5','C1','C2','C3','IB','I3','CP','IA','CQ','IL','IP')
				AND HadIDRPrevious.BF_SSN IS NOT NULL --Had an IDR at some point
		) Schedules
			ON Schedules.BF_SSN = LN10.BF_SSN
			AND Schedules.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT
				LN50.BF_SSN,
				LN50.LN_SEQ,
				SUM(FLOOR(CAST(DATEDIFF(DAY,CASE WHEN (CASE WHEN LN65IBR.LN_SEQ IS NOT NULL AND LN65ICR.LN_SEQ IS NOT NULL AND @PlanType IN('IBR') THEN '1994-07-01' ELSE @PlanStartDate END) <= LN50.LD_DFR_BEG THEN LN50.LD_DFR_BEG ELSE (CASE WHEN LN65IBR.LN_SEQ IS NOT NULL AND LN65ICR.LN_SEQ IS NOT NULL AND @PlanType IN('IBR') THEN '1994-07-01' ELSE @PlanStartDate END) END,LN50.LD_DFR_END) AS DECIMAL(14,2))/CAST(30 AS DECIMAL(14,2))) + --number of full months difference
				CASE WHEN CAST(DATEDIFF(DAY,CASE WHEN (CASE WHEN LN65IBR.LN_SEQ IS NOT NULL AND LN65ICR.LN_SEQ IS NOT NULL AND @PlanType IN('IBR') THEN '1994-07-01' ELSE @PlanStartDate END) <= LN50.LD_DFR_BEG THEN LN50.LD_DFR_BEG ELSE (CASE WHEN LN65IBR.LN_SEQ IS NOT NULL AND LN65ICR.LN_SEQ IS NOT NULL AND @PlanType IN('IBR') THEN '1994-07-01' ELSE @PlanStartDate END) END,LN50.LD_DFR_END) AS DECIMAL(14,2))/CAST(30 AS DECIMAL(14,2)) % 1 > .6 
						THEN 1 --Counts 1 additional month if we are at least 60% through an additional month
						ELSE 0 
				END) AS Months
			FROM
				CDW..DF10_BR_DFR_REQ DF10
				INNER JOIN CDW..LN50_BR_DFR_APV LN50
					ON LN50.BF_SSN = DF10.BF_SSN
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
					AND LN50.LC_STA_LON50 = 'A'
					AND LN50.LC_DFR_RSP != '003' --exclude denied deferments
				LEFT JOIN 
				(
					SELECT DISTINCT
						BF_SSN,
						LN_SEQ
					FROM
						CDW..LN65_LON_RPS
					WHERE
						LC_STA_LON65 = 'A'
						AND LC_TYP_SCH_DIS IN('IB','IL')
				) LN65IBR
					ON LN65IBR.BF_SSN = LN50.BF_SSN
					AND LN65IBR.LN_SEQ = LN50.LN_SEQ
				LEFT JOIN
				(
					SELECT DISTINCT
						BF_SSN,
						LN_SEQ
					FROM
						CDW..LN65_LON_RPS
					WHERE
						LC_TYP_SCH_DIS IN('C1','C2','C3','CQ')

					UNION
							
					SELECT DISTINCT
						BF_SSN,
						LN_SEQ
					FROM
						CDW..LN09_RPD_PIO_CVN
					WHERE
						COALESCE(LN_ICR_ON_TME_PAY,0) > 0
				) LN65ICR
					ON LN65ICR.BF_SSN = LN50.BF_SSN
					AND LN65ICR.LN_SEQ = LN50.LN_SEQ	
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND DF10.LC_DFR_TYP = '29'
				AND	(CASE WHEN LN65IBR.LN_SEQ IS NOT NULL AND LN65ICR.LN_SEQ IS NOT NULL AND @PlanType IN('IBR') THEN '1994-07-01' ELSE @PlanStartDate END) <= CAST(LN50.LD_DFR_END AS DATE) --make sure at least part of the deferment is covered in our timeframe
				AND CAST(LN50.LD_DFR_BEG AS DATE) <= CAST(GETDATE() AS DATE)
			GROUP BY
				LN50.BF_SSN,
				LN50.LN_SEQ
		) EconHardDefer
			ON EconHardDefer.BF_SSN = LN10.BF_SSN
			AND EconHardDefer.LN_SEQ = LN10.LN_SEQ		
	WHERE
		LN10.LA_CUR_PRI > 0.00
		AND LN10.LC_STA_LON10 = 'R'
	GROUP BY
		LN10.BF_SSN,
		LN10.LN_SEQ,
		EconHardDefer.BF_SSN,
		EconHardDefer.Months,
		COALESCE(PreConversion.LN_IBR_QLF_PAY_PCV,0),
		COALESCE(PreConversion.LN_ICR_ON_TME_PAY,0),
		COALESCE(Preconversion.LN_IBR_EHD_DFR_USE,0)
) Final
WHERE
	(
		Final.BF_SSN = @BF_SSN
		OR @BF_SSN IS NULL
	)
	AND Final.CurrentActiveSchedule != ''
	AND Final.PaymentsQualifyingLevel --had to have at least 1 qualifying payment made
		+ Final.PaymentsQualifyingLevelPrevious 
		+ CASE WHEN @PlanType IN('IBR','REPAYE') AND Final.PaymentsQualifyingPreConversionICR = 0 THEN Final.PaymentsQualifyingPreConversionIBR
			   WHEN @PlanType IN('ICR') THEN Final.PaymentsQualifyingPreConversionICR 
			   WHEN @PlanType IN('IBR','REPAYE') THEN Final.PaymentsQualifyingPreConversionICR 
			   ELSE 0
		END
		+ Final.PaymentsQualifyingIDR 
		+ Final.PaymentsQualifyingIDRPrevious 
		+ Final.PaymentsQualifyingPermanentStandard 
		+ Final.PaymentsQualifyingPermanentStandardPrevious 
		+ Final.PaymentsCoveredByEHD > 0
END
