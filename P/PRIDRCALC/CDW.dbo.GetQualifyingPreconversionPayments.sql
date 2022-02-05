USE [CDW]
GO
/****** Object:  StoredProcedure [dbo].[GetQualifyingPreconversionPayments]    Script Date: 8/2/2019 1:58:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetQualifyingPreconversionPayments]
    @PlanType VARCHAR(10),
	@BF_SSN VARCHAR(9),
	@PlanStartDate DATE = '2099-01-01'
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

--For ICR plans update the payment amount to reflect a 10 year level payment
EXEC [CLS].[pridrcalc].[Update10YearLevelPayments] @PlanType, @BF_SSN

--Determine inclusion date for bills and deferments
IF @PlanType = 'ICR' 
	BEGIN
		SET @PlanStartDate = '1994-07-01'
	END
ELSE IF @PlanType = 'IBR'
	BEGIN
		SET @PlanStartDate = '2009-07-01'
	END
ELSE IF @PlanType = 'IBR 2014'
	BEGIN
		SET @PlanStartDate = '2014-07-01' 
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
	@PlanType AS ScheduleCode,
	ISNULL(Final.PaymentsQualifyingLevelPrevious, 0) AS PaymentsQualifyingLevelPrevious,
	ISNULL(Final.PaymentsQualifyingIDRPrevious, 0) AS PaymentsQualifyingIDRPrevious,
	ISNULL(Final.PaymentsQualifyingPermanentStandardPrevious, 0) AS PaymentsQualifyingStandardPrevious,
	Final.PaymentsCoveredByEHDPre,
	ISNULL(ISNULL(Final.PaymentsQualifyingLevelPrevious, 0) 
		+ ISNULL(Final.PaymentsQualifyingIDRPrevious, 0) 
		+ ISNULL(Final.PaymentsQualifyingPermanentStandardPrevious, 0) 
		+ ISNULL(Final.PaymentsCoveredByEHDPre,0), 0) 
	AS Total
FROM
	(
	SELECT 
		LN10.BF_SSN,
		LN10.LN_SEQ,
		--Flatten the records from the individual calculations so that we have the max for each calculation
		MAX(CASE WHEN Schedules.BF_SSN IS NOT NULL THEN Schedules.SatisfiesLevel ELSE 0 END) AS PaymentsQualifyingLevelPrevious, 
		MAX(CASE WHEN Schedules.BF_SSN IS NOT NULL THEN Schedules.SatisfiesCurrentIDR ELSE 0 END) AS PaymentsQualifyingIDRPrevious,
		MAX(CASE WHEN Schedules.BF_SSN IS NOT NULL THEN Schedules.SatisfiesPermanentStandard ELSE 0 END) AS PaymentsQualifyingPermanentStandardPrevious,
		MAX(CASE WHEN EHD.BF_SSN IS NOT NULL THEN EHD.MonthsOnEHD ELSE 0 END) AS PaymentsCoveredByEHDPre
	FROM
		CDW..LN10_LON LN10
		--Population totaling the qualifying bills for the Level, IDR, and Permanent Standard calculations
		INNER JOIN 
		(
			SELECT DISTINCT
				LN10.BF_SSN,
				LN10.LN_SEQ,
				MonthsInRepayment.QualifyingLevel AS SatisfiesLevel,
				MonthsInRepayment.QualifyingIDR AS SatisfiesCurrentIDR,
				MonthsInRepayment.QualifyingPermStandard AS SatisfiesPermanentStandard
			FROM
				CDW..LN10_LON LN10
				LEFT JOIN 
				( --fully satisfied bills applying after the start date for plan forgiveness
					SELECT DISTINCT
						Months.BF_SSN,
						Months.LN_SEQ,
						Months.Months,
						Months.QualifyingIDR,
						Months.QualifyingLevel,
						Months.QualifyingPermStandard
					FROM
						CDW..LN10_LON LN10
						INNER JOIN 
						(
							SELECT	
								FS10.BF_SSN AS BF_SSN,
								FS10.LN_SEQ AS LN_SEQ,
								SUM(CASE WHEN REPLACE(RPT.RepaymentPlanType, ' ', '') IN ('FORCEDICR','INCOMECONTINGENT','INCOMEBASED') THEN 1
									 ELSE 0
								END) AS QualifyingIDR,
								SUM(CASE WHEN 
										REPLACE(RPT.RepaymentPlanType, ' ', '') NOT IN ('FORCEDICR','INCOMECONTINGENT','INCOMEBASED')
										AND
										(
											(MIR.AmountDueAdjusted IS NOT NULL AND (MIR.AmountDueAdjusted - 5.00) <= MIR.AmountDue)
											OR REPLACE(RPT.RepaymentPlanType, ' ', '') IN ('STANDARD','STANDARD-GRANDFATHERED')--,'EXTENDEDFIXED')
										) THEN 1
									 ELSE 0
								END) AS QualifyingLevel,
								0 AS QualifyingPermStandard,
								COUNT(MIR.MonthsInRepaymentId) AS Months
							FROM
								[CLS].[pridrcrp].[MonthsInRepayment] MIR
								INNER JOIN [CLS].[pridrcrp].[RepaymentPlanTypes] RPT
									ON RPT.RepaymentPlanTypeId = MIR.RepaymentPlanTypeId
								--First Repayment Plan to Loan Sequence Mapping
								INNER JOIN 
								(
									SELECT DISTINCT
										BI.BorrowerInformationId,
										FS10.BF_SSN,
										FS10.LN_SEQ
									FROM
										CLS.pridrcrp.BorrowerInformation BI 
										INNER JOIN CLS.pridrcrp.Disbursements D
											ON D.BorrowerInformationId = BI.BorrowerInformationId
										INNER JOIN CDW..FS10_DL_LON FS10
											ON FS10.BF_SSN = BI.Ssn
											AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)), 3) = BI.Ssn + 
												CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
														WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
														WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
												END + D.LoanId
									WHERE
										BI.DeletedAt IS NULL
								) FS10
									ON MIR.BorrowerInformationId = FS10.BorrowerInformationId
								--Determines if a borrower has been on an IBR plan
								LEFT JOIN
								(
									SELECT DISTINCT
										BI.Ssn AS BF_SSN,
										FS10.LN_SEQ,
										MIN(RPC.EffectiveDate) AS IBREffective
									FROM 
										CLS.pridrcrp.BorrowerInformation BI
										INNER JOIN CLS.pridrcrp.Disbursements D
											ON D.BorrowerInformationId = BI.BorrowerInformationId
										INNER JOIN CDW..FS10_DL_LON FS10
											ON FS10.BF_SSN = BI.Ssn
											AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)), 3) = BI.Ssn + 
												CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
														WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
														WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
												END + D.LoanId
										LEFT JOIN CLS.pridrcrp.RepaymentPlanChanges RPC
											ON RPC.BorrowerInformationId = BI.BorrowerInformationId
											AND REPLACE(RPC.PlanType, ' ', '') LIKE '%INCOMEBASED%'
									WHERE
										REPLACE(RPC.PlanType, ' ', '') LIKE '%INCOMEBASED%'
										AND BI.DeletedAt IS NULL
									GROUP BY
										BI.Ssn,
										FS10.LN_SEQ
								) IBR
									ON IBR.BF_SSN = FS10.BF_SSN
									AND IBR.LN_SEQ = FS10.LN_SEQ
								--Determines if a borrower has been on an ICR plan
								LEFT JOIN
								(
									SELECT DISTINCT
										BI.Ssn AS BF_SSN,
										FS10.LN_SEQ,
										MIN(ISNULL(RPC.EffectiveDate, LN65.EffectiveDate)) AS ICREffective
									FROM 
										CLS.pridrcrp.BorrowerInformation BI
										INNER JOIN CLS.pridrcrp.Disbursements D
											ON D.BorrowerInformationId = BI.BorrowerInformationId
										INNER JOIN CDW..FS10_DL_LON FS10
											ON FS10.BF_SSN = BI.Ssn
											AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)), 3) = BI.Ssn + 
												CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
														WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
														WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
												END + D.LoanId
										LEFT JOIN CLS.pridrcrp.RepaymentPlanChanges RPC
											ON RPC.BorrowerInformationId = BI.BorrowerInformationId
											AND REPLACE(RPC.PlanType, ' ', '') LIKE '%INCOMECONTINGENT%'
											AND RPC.InactivatedAt IS NULL
										LEFT JOIN 
										(
											SELECT
												LN65.BF_SSN,
												LN65.LN_SEQ,
												MIN(CAST(LN65.LD_RPD_MAX_TRM_SR AS DATE)) AS EffectiveDate
											FROM
												CDW..LN65_LON_RPS LN65 -- We do not want to use the active flag since they can be inactive/past ICR and qualify
											WHERE
												LC_TYP_SCH_DIS IN ('CQ', 'C1', 'C2', 'C3')
											GROUP BY
												LN65.BF_SSN,
												LN65.LN_SEQ
										) LN65
											ON FS10.BF_SSN = LN65.BF_SSN
											AND FS10.LN_SEQ = LN65.LN_SEQ
									WHERE
										(REPLACE(RPC.PlanType, ' ', '') LIKE ('%INCOMECONTINGENT%') OR LN65.BF_SSN IS NOT NULL)
										AND BI.DeletedAt IS NULL
									GROUP BY
										BI.Ssn,
										FS10.LN_SEQ
								) ICR
									ON ICR.BF_SSN = FS10.BF_SSN
									AND ICR.LN_SEQ = FS10.LN_SEQ
							WHERE
								MIR.CoveredByDefFor = 0
								AND MIR.CoveredByEHD = 0
								AND MIR.InactivatedAt IS NULL
								AND MIR.[Date] >= 
									(CASE 
										--We always want to count valid months in repayment from the beginning of the possible plan start date
										WHEN IBR.LN_SEQ IS NOT NULL AND ICR.LN_SEQ IS NOT NULL AND @PlanType = 'IBR'
											THEN '1994-07-01'							
										WHEN IBR.LN_SEQ IS NOT NULL AND ICR.LN_SEQ IS NULL AND @PlanType = 'IBR' 
											THEN '2009-07-01'
										WHEN IBR.LN_SEQ IS NULL AND ICR.LN_SEQ IS NOT NULL AND @PlanType = 'IBR' 
											THEN '1994-07-01'
										ELSE @PlanStartDate 
									END)
							GROUP BY
								FS10.BF_SSN,
								FS10.LN_SEQ
						) Months
							ON Months.BF_SSN = LN10.BF_SSN
							AND Months.LN_SEQ = LN10.LN_SEQ
				) MonthsInRepayment
					ON MonthsInRepayment.BF_SSN = LN10.BF_SSN
					AND MonthsInRepayment.LN_SEQ = LN10.LN_SEQ
			WHERE
				LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 = 'R'
		) Schedules
			ON Schedules.BF_SSN = LN10.BF_SSN
			AND Schedules.LN_SEQ = LN10.LN_SEQ
		LEFT JOIN
		(
			SELECT DISTINCT
				BI.Ssn AS BF_SSN,
				LN10.LN_SEQ,
				SUM(CAST(MIR.CoveredByEHD AS INT)) AS MonthsOnEHD
			FROM 
				CLS.pridrcrp.BorrowerInformation BI
				INNER JOIN
				(
					SELECT DISTINCT
						BI.BorrowerInformationId,
						FS10.BF_SSN,
						FS10.LN_SEQ
					FROM
						CLS.pridrcrp.BorrowerInformation BI
						INNER JOIN CLS.pridrcrp.Disbursements D
							ON D.BorrowerInformationId = BI.BorrowerInformationId
						INNER JOIN CDW..FS10_DL_LON FS10
							ON FS10.BF_SSN = BI.Ssn
							AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)), 3) = BI.Ssn + 
								CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
										WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
										WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
								END + D.LoanId
				) BorToLoan
					ON BI.BorrowerInformationId = BorToLoan.BorrowerInformationId	
				INNER JOIN CDW..LN10_LON LN10
					ON BorToLoan.BF_SSN = LN10.BF_SSN
					AND BorToLoan.LN_SEQ = LN10.LN_SEQ
					AND LN10.LA_CUR_PRI > 0.00
					AND LN10.LC_STA_LON10 = 'R'
				INNER JOIN CLS.pridrcrp.MonthsInRepayment MIR
					ON BI.BorrowerInformationId = MIR.BorrowerInformationId
					AND MIR.InactivatedAt IS NULL
				LEFT JOIN
				(
					SELECT DISTINCT
						BI.Ssn AS BF_SSN,
						FS10.LN_SEQ,
						MIN(RPC.EffectiveDate) AS IBREffective
					FROM 
						CLS.pridrcrp.BorrowerInformation BI
						INNER JOIN CLS.pridrcrp.Disbursements D
							ON D.BorrowerInformationId = BI.BorrowerInformationId
						INNER JOIN CDW..FS10_DL_LON FS10
							ON FS10.BF_SSN = BI.Ssn
							AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)), 3) = BI.Ssn + 
								CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
										WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
										WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
								END + D.LoanId
						LEFT JOIN CLS.pridrcrp.RepaymentPlanChanges RPC
							ON RPC.BorrowerInformationId = BI.BorrowerInformationId
							AND REPLACE(RPC.PlanType, ' ', '') LIKE '%INCOMEBASED%'
					WHERE
						REPLACE(RPC.PlanType, ' ', '') LIKE '%INCOMEBASED%'
						AND BI.DeletedAt IS NULL
					GROUP BY
						BI.Ssn,
						FS10.LN_SEQ
				) IBR
					ON IBR.BF_SSN = LN10.BF_SSN
					AND IBR.LN_SEQ = LN10.LN_SEQ
				LEFT JOIN
				(
					SELECT DISTINCT
						BI.Ssn AS BF_SSN,
						FS10.LN_SEQ,
						MIN(RPC.EffectiveDate) AS ICREffective
					FROM 
						CLS.pridrcrp.BorrowerInformation BI
						INNER JOIN CLS.pridrcrp.Disbursements D
							ON D.BorrowerInformationId = BI.BorrowerInformationId
						INNER JOIN CDW..FS10_DL_LON FS10
							ON FS10.BF_SSN = BI.Ssn
							AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)), 3) = BI.Ssn + 
								CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
										WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
										WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
								END + D.LoanId
						LEFT JOIN CLS.pridrcrp.RepaymentPlanChanges RPC
							ON RPC.BorrowerInformationId = BI.BorrowerInformationId
							AND REPLACE(RPC.PlanType, ' ', '') LIKE '%INCOMECONTINGENT%'
							AND RPC.InactivatedAt IS NULL
						LEFT JOIN 
						(
							SELECT
								LN65.BF_SSN,
								LN65.LN_SEQ,
								MIN(CAST(LN65.LD_RPD_MAX_TRM_SR AS DATE)) AS EffectiveDate
							FROM
								CDW..LN65_LON_RPS LN65 -- We do not want to use the active flag since they can be inactive/past ICR and qualify
							WHERE
								LC_TYP_SCH_DIS IN ('CQ', 'C1', 'C2', 'C3')
							GROUP BY
								LN65.BF_SSN,
								LN65.LN_SEQ
						) LN65
							ON FS10.BF_SSN = LN65.BF_SSN
							AND FS10.LN_SEQ = LN65.LN_SEQ
					WHERE
						(REPLACE(RPC.PlanType, ' ', '') LIKE ('%INCOMECONTINGENT%') OR LN65.BF_SSN IS NOT NULL)
						AND BI.DeletedAt IS NULL
					GROUP BY
						BI.Ssn,
						FS10.LN_SEQ
				) ICR
					ON ICR.BF_SSN = LN10.BF_SSN
					AND ICR.LN_SEQ = LN10.LN_SEQ	
			WHERE
				BI.DeletedAt IS NULL
				AND MIR.[Date] < LN10.LD_LON_ACL_ADD
				AND MIR.[Date] >= 
					(CASE 
						--We always want to count EHD from the beginning of the possible plan start date
						WHEN IBR.LN_SEQ IS NOT NULL AND ICR.LN_SEQ IS NOT NULL AND @PlanType = 'IBR'
							THEN '1994-07-01'							
						WHEN IBR.LN_SEQ IS NOT NULL AND ICR.LN_SEQ IS NULL AND @PlanType = 'IBR' 
							THEN '2009-07-01'
						WHEN IBR.LN_SEQ IS NULL AND ICR.LN_SEQ IS NOT NULL AND @PlanType = 'IBR' 
							THEN '1994-07-01'
						ELSE @PlanStartDate 
					END)
			GROUP BY
				BI.Ssn,
				LN10.LN_SEQ
		) EHD
			ON EHD.BF_SSN = LN10.BF_SSN
			AND EHD.LN_SEQ = LN10.LN_SEQ
	WHERE
		LN10.LA_CUR_PRI > 0.00
		AND LN10.LC_STA_LON10 = 'R'
	GROUP BY
		LN10.BF_SSN,
		LN10.LN_SEQ,
		EHD.BF_SSN,
		Schedules.BF_SSN
) Final
WHERE
	(
		Final.BF_SSN = @BF_SSN
		OR @BF_SSN IS NULL
	)
	AND ISNULL(Final.PaymentsQualifyingLevelPrevious, 0) 
		+ ISNULL(Final.PaymentsQualifyingIDRPrevious, 0) 
		+ ISNULL(Final.PaymentsQualifyingPermanentStandardPrevious , 0)
		+ ISNULL(Final.PaymentsCoveredByEHDPre, 0) >= 0
END
