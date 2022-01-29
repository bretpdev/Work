CREATE PROCEDURE [covidforb].[InsertProcessingRecord]
	@AccountNumber VARCHAR(10),
	@StartDate DATE,
	@EndDate DATE,
	@ForbToClearDelq CHAR(1),
	@ComakerEligibility CHAR(1)
AS
BEGIN TRANSACTION
BEGIN TRY

	DECLARE @BusinessUnitId INT = (SELECT BusinessUnitId FROM covidforb.BusinessUnits WHERE BusinessUnit = 'Loan Services')
	DECLARE @Today DATE = GETDATE()
	DECLARE @ExistingRecord BIT = 
	CASE WHEN EXISTS (
		SELECT 
			FP.ForbearanceProcessingId
		FROM 
			covidforb.ForbearanceProcessing FP
		WHERE
			FP.DeletedAt IS NULL
			AND FP.DeletedBy IS NULL
			AND FP.AccountNumber = @AccountNumber	
			AND FP.StartDate = @StartDate
			AND FP.EndDate = @EndDate
	) THEN 1 ELSE 0 END

	IF @ExistingRecord = 0
	BEGIN
		INSERT INTO covidforb.ForbearanceProcessing
		(
			AccountNumber, 
			ForbCode, 
			DateRequested, 
			ForbearanceType, 
			StartDate,
			EndDate,
			DateCertified,
			SubType,
			SelectAllLoans,
			BusinessUnitId,
			ProcessOn,
			ForbToClearDelq,
			CoMakerEligibility
		)
		VALUES 
		(
			@AccountNumber, --AccountNumer
			'F', --Forb Code
			@Today, --Date Requested
			'40', --Forbearance Type
			@StartDate, --Start Date
			@EndDate, --End Date
			@Today, --Date Certified
			NULL, --Sub Type
			1, --Select All Loans
			@BusinessUnitId, --Business Unit Id
			@Today, --Process On
			@ForbToClearDelq, --Forbearance To Clear Delinquency
			@ComakerEligibility --Comaker Eligibility
		)
	END
	
	COMMIT TRANSACTION
	SELECT CAST(1 AS BIT)
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION
	SELECT CAST(0 AS BIT)
	THROW
END CATCH
RETURN 0
