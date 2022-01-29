CREATE PROCEDURE [verforbuh].[GetBorrowerIdentifiers]
	@AccountIdentifier CHAR(10)
AS
	SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED	
	DECLARE @RowCount INT = 0
	
	SELECT
		PD10.DF_PRS_ID AS Ssn,
		PD10.DF_SPE_ACC_ID AS AccountNumber
	FROM
		dbo.PD10_PRS_NME PD10
	WHERE
		PD10.DF_SPE_ACC_ID = @AccountIdentifier
		OR PD10.DF_PRS_ID = @AccountIdentifier

	SET @RowCount = @@ROWCOUNT
	 
	IF @RowCount = 0
	BEGIN
		RAISERROR('[verforbfed].[GetBorrowerIdentifiers] returned %i record(s) for Account Identifier: %s', 16, 1, @RowCount, @AccountIdentifier) 
	END
RETURN 0
