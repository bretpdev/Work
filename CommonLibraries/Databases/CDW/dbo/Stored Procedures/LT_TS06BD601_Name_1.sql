CREATE PROCEDURE [dbo].[LT_TS06BD601_Name]
	@AccountNumber		char(10)
AS
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT DISTINCT
		PD10.DM_PRS_1 AS FirstName
	FROM 
		PD10_Borrower PD10
	WHERE 
		PD10.DF_SPE_ACC_ID = @AccountNumber
END

IF @@ROWCOUNT = 0 
	BEGIN
		RAISERROR('No data returned. ([LT_TS06BD601_Name])',11,2)
	END