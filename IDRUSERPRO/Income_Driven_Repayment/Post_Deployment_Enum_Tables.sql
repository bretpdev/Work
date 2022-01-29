/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

MERGE dbo.Income_Source AS t USING
(
	SELECT 1 [income_source_id], 'TAXES' [income_source_friendly_description], 'TAX' [income_source_description]
	UNION
	SELECT 2, 'ALT - ADOI', 'ALT'
) AS s
ON t.income_source_id = s.income_source_id
WHEN NOT MATCHED BY target THEN
	INSERT (income_source_id, income_source_description, income_source_friendly_description)
	VALUES (income_source_id, income_source_description, income_source_friendly_description)
WHEN NOT MATCHED BY source THEN
	DELETE
WHEN MATCHED THEN
	UPDATE SET income_source_description = s.income_source_description, income_source_friendly_description = s.income_source_friendly_description
;


MERGE dbo.Adoi_Paystub_Frequencies AS t USING
(
	SELECT 1 [adoi_paystub_frequency_id], 'Yearly' [adoi_paystub_frequency_description]
	UNION
	SELECT 12, 'Monthly'
	UNION
	SELECT 24, 'Semi-Monthly'
	UNION
	SELECT 26, 'Bi-Weekly'
	UNION
	SELECT 52, 'Weekly'
) AS s
ON t.adoi_paystub_frequency_id = s.adoi_paystub_frequency_id
WHEN NOT MATCHED BY target THEN
	INSERT ([adoi_paystub_frequency_id], [adoi_paystub_frequency_description])
	VALUES ([adoi_paystub_frequency_id], [adoi_paystub_frequency_description])
WHEN NOT MATCHED BY source THEN
	DELETE
WHEN MATCHED THEN
	UPDATE SET [adoi_paystub_frequency_description] = s.[adoi_paystub_frequency_description]
;

DECLARE @Single BIT = 0
DECLARE @Married BIT = 1

MERGE dbo.Filing_Statuses AS t USING
(
	SELECT 1 [filing_status_id], 1 [filing_status], 'Single' [filing_status_description], @Single [status_for_married]
	UNION
	SELECT 2, 2, 'Married Filing Jointly', @Married
	UNION
	SELECT 3, 3, 'Married Filing Separately', @Married	
	UNION
	SELECT 4, 4, 'Head of Household', @Single
	UNION
	SELECT 5, 5, 'Qualifying Widow(er) with Dependent Child', @Single
	UNION
	SELECT 6, 6, 'Not Applicable/No Tax Documents', NULL
) AS s
ON t.[filing_status_id] = s.[filing_status_id]
WHEN NOT MATCHED BY target THEN
	INSERT ([filing_status_id], [filing_status], [filing_status_description], [status_for_married])
	VALUES ([filing_status_id], [filing_status], [filing_status_description], [status_for_married])
WHEN NOT MATCHED BY source THEN
	DELETE
WHEN MATCHED THEN
	UPDATE SET [filing_status] = s.[filing_status], [filing_status_description] = s.[filing_status_description], [status_for_married] = s.[status_for_married]
;

MERGE dbo.Repayment_Plan_Type_Requested AS t USING
(
	SELECT 1 [repayment_plan_type_requested_id], 'IBR' [repayment_plan_type_requested_description]
	UNION
	SELECT 2, 'ICR'
	UNION
	SELECT 3, 'PAYE'
	UNION
	SELECT 4, 'IBR,ICR'
	UNION
	SELECT 5, 'IBR,PAYE'
	UNION
	SELECT 6, 'ICR,PAYE'
	UNION
	SELECT 7, 'IBR,ICR,PAYE'
	UNION
	SELECT 8, 'REPAYE'
	UNION
	SELECT 9, 'REPAYE, IBR'
	UNION
	SELECT 10, 'REPAYE, ICR'
	UNION
	SELECT 11, 'REPAYE, PAYE'
	UNION
	SELECT 12, 'REPAYE, IBR, ICR, PAYE'
	UNION
	SELECT 13, 'REPAYE, IBR, ICR'
	UNION
	SELECT 14, 'REPAYE, IBR, PAYE'
	UNION
	SELECT 15, 'REPAYE, ICR, PAYE'
) AS s
ON t.[repayment_plan_type_requested_id] = s.[repayment_plan_type_requested_id]
WHEN NOT MATCHED BY target THEN
	INSERT ([repayment_plan_type_requested_id], [repayment_plan_type_requested_description])
	VALUES ([repayment_plan_type_requested_id], [repayment_plan_type_requested_description])
WHEN NOT MATCHED BY source THEN
	DELETE
WHEN MATCHED THEN
	UPDATE SET [repayment_plan_type_requested_description] = s.[repayment_plan_type_requested_description]
;

MERGE dbo.Repayment_Plan_Type AS t USING
(
	SELECT 1 [repayment_plan_type_id], 'IBR' [repayment_plan]
	UNION
	SELECT 2, 'ICR'
	UNION
	SELECT 3, 'PAYE'
	UNION
	SELECT 4, 'REPAYE'
) AS s
ON t.[repayment_plan_type_id] = s.[repayment_plan_type_id]
WHEN NOT MATCHED BY target THEN
	INSERT ([repayment_plan_type_id], [repayment_plan])
	VALUES ([repayment_plan_type_id], [repayment_plan])
WHEN NOT MATCHED BY source THEN
	DELETE
WHEN MATCHED THEN
	UPDATE SET [repayment_plan] = s.[repayment_plan]
;


IF DB_NAME() = 'Income_Driven_Repayment'
BEGIN
	MERGE dbo.Borrower_Eligibility AS t USING
	(
		SELECT 1 [eligibility_id], 'P' [eligibility_code], 'ICR, IBR, PAYE, REPAYE' [eligibility_description]
		UNION
		SELECT 2, 'R', 'ICR, IBR, REPAYE'
		UNION
		SELECT 3, '4', 'IBR 2014, REPAYE, PAYE'
		UNION
		SELECT 4, 'C', 'ICR Only'
		UNION
		SELECT 5, 'B', 'IBR Only'
	) AS s
	ON t.[eligibility_id] = s.[eligibility_id]
	WHEN NOT MATCHED BY target THEN
		INSERT ([eligibility_id], [eligibility_code], [eligibility_description])
		VALUES ([eligibility_id], [eligibility_code], [eligibility_description])
	WHEN NOT MATCHED BY source THEN
		DELETE
	WHEN MATCHED THEN
		UPDATE SET [eligibility_code] = s.[eligibility_code], [eligibility_description] = s.[eligibility_description]
	;
END
IF DB_NAME() = 'IncomeBasedRepaymentUheaa'
BEGIN
	MERGE dbo.Borrower_Eligibility AS t USING
	(
		SELECT 3 [eligibility_id], 'B' [eligibility_code], 'IBR' [eligibility_description]
	) AS s
	ON t.[eligibility_id] = s.[eligibility_id]
	WHEN NOT MATCHED BY target THEN
		INSERT ([eligibility_id], [eligibility_code], [eligibility_description])
		VALUES ([eligibility_id], [eligibility_code], [eligibility_description])
	WHEN NOT MATCHED BY source THEN
		DELETE
	WHEN MATCHED THEN
		UPDATE SET [eligibility_code] = s.[eligibility_code], [eligibility_description] = s.[eligibility_description]
	;
END
