USE [CentralData]
GO
/****** Object:  StoredProcedure [adoifedrp].[GetWeeklyIdrAdoiFed]    Script Date: 7/17/2020 8:04:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [adoifedrp].[GetWeeklyIdrAdoiFed]
	@Begin DATE,
	@End DATE
AS

DECLARE @IncomeSum TABLE (ApplicationId INT, TotalAgi MONEY)
INSERT INTO @IncomeSum (ApplicationId, TotalAgi)

SELECT DISTINCT
	APP.application_id [ApplicationId],
	SUM(COALESCE(APP.manually_submitted_income, CAST(APP.total_income AS DECIMAL(14,2)), 0.00) + COALESCE(SPS.spouse_alt_submitted_income, 0.00) + ISNULL(APP.adjusted_grose_income,0.00) + ISNULL(SPS.spouse_AGI,0.00)) [TotalAgi]
FROM
	[Income_Driven_Repayment]..Applications APP
	LEFT JOIN [Income_Driven_Repayment]..Spouses SPS
		ON SPS.spouse_id = APP.spouse_id
WHERE
	CAST(APP.updated_at AS DATE) BETWEEN @Begin AND @End
GROUP BY
	APP.application_id

SELECT DISTINCT
	BOR.account_number [AccountNumber],
	APP.application_id [ApplicationId],
	CONVERT(VARCHAR(19), APP.updated_at, 110) [DateOfSubmission],
	REPLACE(REPLACE(LTRIM(RIGHT(CONVERT(VARCHAR(19),APP.updated_at,0),7)),'P',' P'),'A',' A') [TimeOfSubmission], --Adding a space before PM / AM for readability
	ISNULL(APP.UserId, APP.updated_by) [AgentId],
	INCOME.TotalAgi,
	CASE
		WHEN ADOI.is_spouse = 0 THEN 'Borrower'
		WHEN ADOI.is_spouse = 1 THEN 'Spouse'
	END AS IncomeEarner,
	ADOI.employer_name [EmployerName],
	FREQ.adoi_paystub_frequency_description [PayFrequency],
	ADOI.ftw [FTW],
	ADOI.gross [GrossIncome],
	ADOI.pre_tax_deductions [PreTaxDeductions],
	ADOI.bonus [BonusAmount],
	ADOI.overtime [OvertimeAmount],
	ADOI.adoi_paystub_id --Required to get distinct paystubs without losing data.  Wont be displayed in output
FROM
	@IncomeSum INCOME
	INNER JOIN [Income_Driven_Repayment]..Applications APP
		ON APP.application_id = INCOME.ApplicationId
	INNER JOIN [Income_Driven_Repayment]..Loans LON
		ON LON.application_id = APP.application_id
	INNER JOIN [Income_Driven_Repayment]..Borrowers BOR
		ON BOR.borrower_id = LON.borrower_id
	INNER JOIN [Income_Driven_Repayment]..Repayment_Plan_Selected RPS
		ON RPS.application_id = APP.application_id
	INNER JOIN [Income_Driven_Repayment]..Repayment_Plan_Type_Status_History HIS
		ON HIS.repayment_plan_type_id = RPS.repayment_plan_type_id
	INNER JOIN
	(
		SELECT
			repayment_plan_type_id,
			MAX(created_at) AS created_at
		FROM
			[Income_Driven_Repayment]..Repayment_Plan_Type_Status_History
		GROUP BY
			repayment_plan_type_id
	) MaxHIS
		ON MaxHIS.repayment_plan_type_id = HIS.repayment_plan_type_id
		AND MaxHIS.created_at = HIS.created_at
	INNER JOIN [Income_Driven_Repayment]..Repayment_Plan_Type_Substatus SUBSTA
		ON SUBSTA.repayment_plan_type_substatus_id = HIS.repayment_plan_type_status_mapping_id
	INNER JOIN [Income_Driven_Repayment]..Adoi_Paystubs ADOI  --USING INNER JOIN SINCE WE ONLY WANT APPS WITH PAYSTUBS
		ON ADOI.application_id = APP.application_id
	INNER JOIN [Income_Driven_Repayment]..Adoi_Paystub_Frequencies FREQ
		ON FREQ.adoi_paystub_frequency_id = ADOI.adoi_paystub_frequency_id
	LEFT JOIN [Income_Driven_Repayment]..Spouses SPS
		ON SPS.spouse_id = APP.spouse_id
WHERE
	CAST(APP.updated_at AS DATE) BETWEEN @Begin AND @End 
	AND 
	(
		ISNULL(APP.income_source_id,0) = 2 --ONLY WANT APPS THAT USED ADOI (borrower)
		OR ISNULL(SPS.spouse_income_source_id,0) = 2 --(spouse adoi)
	)
	AND 
	(
		APP.taxable_income = 1 --DO NOT WANT SELF-CERTIFICATION
		OR SPS.spouse_taxable_income = 1
	)
	AND SUBSTA.repayment_plan_type_substatus NOT LIKE '%Self-Cert%' --EXCLUDES Self Certification substatus
	AND 
	(
		APP.repayment_plan_status_id = 1 
		OR 
		(
			APP.repayment_plan_status_id = 3 
			AND SUBSTA.repayment_plan_type_substatus_id = 11
		)
	) --EITHER APPROVED OR DENIED BASED ON INCOME TOO HIGH