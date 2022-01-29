USE CLS
GO

CREATE PROCEDURE [pridrcalc].[Update10YearLevelPayments]
	@PlanType VARCHAR(10),
	@BF_SSN VARCHAR(9) = NULL
AS
BEGIN

UPDATE MIR
SET
	AmountDueAdjusted = null
FROM
	pridrcrp.MonthsInRepayment MIR
	INNER JOIN pridrcrp.BorrowerInformation BI
		ON BI.BorrowerInformationId = MIR.BorrowerInformationId
WHERE
	BI.Ssn = @BF_SSN
	AND MIR.InactivatedAt IS NULL
	AND BI.DeletedAt IS NULL

DECLARE @TenYearBills TABLE(BorrowerInformationId INT, MonthsInRepaymentId INT, AmountDue DECIMAL(14,2))

INSERT INTO @TenYearBills
SELECT
	BorrowerInformationId,
	MonthsInRepaymentId,
	Numerator / Denominator AS AmountDue
FROM
(
	SELECT
		BI.BorrowerInformationId,
		MIR.MonthsInRepaymentId,
		--This is the payment amount calculation for the equivalent 10 year level plan
		--It breaks down as follows
		--10 Year Level Payment Amount = (P * R / 12) / (1 - (1 + R / 12)^-N)
		--P = Point in time principal balance
		--R = Point in time interest rate
		--N = 120, If they are on ICR and their repayment plan is EXTENDED FIXED or STANDARD-GRANDFATHERED then 144
		--CONVERT(DECIMAL(14,2), (OutstandingPrincipalPeriod.OutstandingPrincipal * (InterestRatePeriod.InterestRate / 12)) / (1 - (1 / POWER((1 + (InterestRatePeriod.InterestRate / 12)), 120)))) AS AmountDue,
		CASE WHEN @PlanType = 'ICR' AND BI.FirstPayDue < '2007-10-01' AND REPLACE(RepaymentPlanPeriod.PlanType, ' ', '') IN ('EXTENDEDFIXED', 'STANDARD-GRANDFATHERED','CONSOLSTANDARD','EXTENDED-GRANDFATHERED')
			THEN
				CASE WHEN CONVERT(DECIMAL(14,2), (1 - (1 / POWER((1 + (InterestRate.InterestRate / 1200)), 144)))) = 0
					THEN 0.00
				ELSE CONVERT(DECIMAL(14,2), (1 - (1 / POWER((1 + (InterestRate.InterestRate / 1200)), 144)))) END
		ELSE
			CASE WHEN CONVERT(DECIMAL(14,2), (1 - (1 / POWER((1 + (InterestRate.InterestRate / 1200)), 120)))) = 0
				THEN 0.00
			ELSE CONVERT(DECIMAL(14,2), (1 - (1 / POWER((1 + (InterestRate.InterestRate / 1200)), 120)))) END
		END AS Denominator,
		CONVERT(DECIMAL(14,2), (OutstandingPrincipal.OutstandingPrincipal * (InterestRate.InterestRate / 1200))) AS Numerator
	FROM
		--Join the first repayment plan on ssn, project off LN_SEQ to prevent duplication
		(
			SELECT DISTINCT
				BI.BorrowerInformationId,
				BI.FirstPayDue,
				BI.InterestRate,
				FS10.BF_SSN,
				BI.DeletedAt
			FROM
				[pridrcrp].[BorrowerInformation] BI
				INNER JOIN pridrcrp.Disbursements D
					ON D.BorrowerInformationId = BI.BorrowerInformationId
				INNER JOIN CDW..FS10_DL_LON FS10
					ON FS10.BF_SSN = BI.Ssn
					AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)), 3) = BI.Ssn + 
						CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
								WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
								WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
						END + D.LoanId
		) BI
		INNER JOIN [pridrcrp].[MonthsInRepayment] MIR
			ON BI.BorrowerInformationId = MIR.BorrowerInformationId
			AND MIR.InactivatedAt IS NULL
		--Join the first repayment plans on the plan periods
		INNER JOIN 
		(
			SELECT
				RPC.BorrowerInformationId,
				RPC.EffectiveDate AS StartDate,
				RPC.PlanType,
				CASE 
					WHEN NEXTRPC.RepaymentPlanChangeId IS NULL THEN '2099-01-01'
					WHEN NEXTRPC.EffectiveDate = RPC.EffectiveDate THEN NEXTRPC.EffectiveDate
					ELSE DATEADD(DAY, -1, NEXTRPC.EffectiveDate)
				END AS EndDate
			FROM
				[pridrcrp].[RepaymentPlanChanges] RPC
				LEFT JOIN
				(	
					SELECT	
						RPC.RepaymentPlanChangeId,
						MIN(NEXTRPC.EffectiveDate) AS EffectiveDate
					FROM
						[pridrcrp].[RepaymentPlanChanges] RPC
						INNER JOIN [pridrcrp].[RepaymentPlanChanges] NEXTRPC
							ON NEXTRPC.BorrowerInformationId = RPC.BorrowerInformationId
							AND NEXTRPC.RepaymentPlanChangeId > RPC.RepaymentPlanChangeId
							AND NEXTRPC.EffectiveDate >=RPC.EffectiveDate 
					WHERE
						RPC.InactivatedAt IS NULL
						AND NEXTRPC.InactivatedAt IS NULL
					GROUP BY
						RPC.RepaymentPlanChangeId
				) NEXTRPC
					ON NEXTRPC.RepaymentPlanChangeId = RPC.RepaymentPlanChangeId
			WHERE 
				RPC.InactivatedAt IS NULL
		) RepaymentPlanPeriod
			ON BI.BorrowerInformationId = RepaymentPlanPeriod.BorrowerInformationId
		--Find interest rate min by effective date
		INNER JOIN 
		(
			SELECT
				IRC.BorrowerInformationId,
				IRC.EffectiveDate,
				IRC.InterestRate
			FROM
			(
				SELECT
					BorrowerInformationId,
					MIN(IRC.EffectiveDate) AS EffectiveDate
				FROM
					[pridrcrp].[InterestRateChanges] IRC
				WHERE
					IRC.InactivatedAt IS NULL
				GROUP BY
					IRC.BorrowerInformationId
			) MIN_IRC
			INNER JOIN [pridrcrp].[InterestRateChanges] IRC
				ON MIN_IRC.BorrowerInformationId = IRC.BorrowerInformationId
				AND MIN_IRC.EffectiveDate = IRC.EffectiveDate
			WHERE
				IRC.InactivatedAt IS NULL
		) InterestRate
			ON BI.BorrowerInformationId = InterestRate.BorrowerInformationId
		--Find outstanding principal changes min change by effective date
		--INNER JOIN
		--(
		--	SELECT
		--		OPC.BorrowerInformationId,
		--		OPC.EffectiveDate,
		--		OPC.OutstandingPrincipal
		--	FROM
		--	(
		--		SELECT		
		--			BorrowerInformationId,
		--			MIN(OPC.EffectiveDate) AS EffectiveDate
		--		FROM
		--			[pridrcrp].[OutstandingPrincipalChanges] OPC
		--		GROUP BY
		--			OPC.BorrowerInformationId
		--	) MIN_OPC
		--	INNER JOIN [pridrcrp].[OutstandingPrincipalChanges] OPC
		--		ON MIN_OPC.BorrowerInformationId = OPC.BorrowerInformationId
		--		AND MIN_OPC.EffectiveDate = OPC.EffectiveDate

		--) OutstandingPrincipal 
		--	ON BI.BorrowerInformationId = OutstandingPrincipal.BorrowerInformationId		
		--Version with MAX Effective date before
		--INNER JOIN
		--(
		--	SELECT
		--		PH.BorrowerInformationId,
		--		PH.EffectiveDate,
		--		COALESCE(PH.PrincipalBalance, 0) AS OutstandingPrincipal,
		--		ROW_NUMBER() OVER (PARTITION BY PH.BorrowerInformationId ORDER BY CASE WHEN COALESCE(PH.PrincipalBalance, 0) > 0 THEN 0 ELSE 1 END, PH.TransactionId DESC) AS [Priority]
		--	FROM
		--	(
		--		--According to compliance they want to use the principal amount in the last
		--		--payment history record before the first pay due date for the given repayment plan
		--		SELECT		
		--			PH.BorrowerInformationId,
		--			MAX(PH.EffectiveDate) AS EffectiveDate
		--		FROM
		--			[pridrcrp].[PaymentHistory] PH
		--			INNER JOIN [pridrcrp].[BorrowerInformation] BI
		--				ON PH.BorrowerInformationId = BI.BorrowerInformationId
		--		WHERE
		--			PH.EffectiveDate <= BI.FirstPayDue 
		--			AND BI.DeletedAt IS NULL
		--		GROUP BY
		--			PH.BorrowerInformationId
		--	) PH_OPC
		--	INNER JOIN [pridrcrp].[PaymentHistory] PH
		--		ON PH_OPC.BorrowerInformationId = PH.BorrowerInformationId
		--		AND PH_OPC.EffectiveDate = PH.EffectiveDate
		--) OutstandingPrincipal 
		--	ON BI.BorrowerInformationId = OutstandingPrincipal.BorrowerInformationId
		--	AND OutstandingPrincipal.[Priority] = 1
		INNER JOIN
		(
			--According to compliance they want to use the principal amount in the last
			--payment history record before the first pay due date for the given repayment plan
			SELECT
				PH.BorrowerInformationId,
				PH.EffectiveDate,
				PH.TransactionId,
				COALESCE(PH.PrincipalBalance, 0) AS OutstandingPrincipal,
				ROW_NUMBER() OVER (PARTITION BY PH.BorrowerInformationId ORDER BY CASE WHEN COALESCE(PH.PrincipalBalance, 0) > 0 THEN 0 ELSE 1 END, PH.EffectiveDate DESC, PH.TransactionId DESC) AS [Priority]
			FROM
				[pridrcrp].[PaymentHistory] PH
				INNER JOIN [pridrcrp].[BorrowerInformation] BI
					ON PH.BorrowerInformationId = BI.BorrowerInformationId
			WHERE
				(PH.EffectiveDate <= BI.FirstPayDue OR REPLACE(PH.[Description], ' ', '')  = 'STARTINGBAL')
				AND BI.DeletedAt IS NULL
		) OutstandingPrincipal 
			ON BI.BorrowerInformationId = OutstandingPrincipal.BorrowerInformationId
			AND OutstandingPrincipal.[Priority] = 1
	WHERE
		BI.DeletedAt IS NULL
		AND BI.BF_SSN = @BF_SSN
		--They are in a plan that denotes the 10 year plan adjustment needs to be done
		AND MIR.[Date] BETWEEN RepaymentPlanPeriod.StartDate AND RepaymentPlanPeriod.EndDate
		AND 
		(
			REPLACE(RepaymentPlanPeriod.PlanType, ' ', '') IN ('EXTENDEDGRADUATED', 'EXTENDEDGRAD', 'ALTFIXEDPAYMENT', 'GRADUATED-GRANDFATHERED', 'GRADUATED10YEAR', 'ALTFIXEDTERM', 'CONSOLGRADUATED', 'EXTENDED-GRANDFATHERED', 'CONSOLSTANDARD', 'EXTENDEDFIXED')
			OR 
			(
				@PlanType = 'ICR'
				AND REPLACE(RepaymentPlanPeriod.PlanType, ' ', '') IN ('EXTENDEDFIXED')
			)
		)
		--Their point in time interest
		AND MIR.[Date] >= BI.FirstPayDue
) PTYB

--Update bills to their 10 year equivalent
UPDATE [pridrcrp].[MonthsInRepayment]
SET
	[AmountDueAdjusted] = CASE WHEN TYB.AmountDue < 50.00
		THEN 50.00
		ELSE TYB.AmountDue
	END
FROM	
	[pridrcrp].[MonthsInRepayment] MIR
	INNER JOIN @TenYearBills TYB
		ON MIR.MonthsInRepaymentId = TYB.MonthsInRepaymentId
WHERE
	MIR.InactivatedAt IS NULL

END