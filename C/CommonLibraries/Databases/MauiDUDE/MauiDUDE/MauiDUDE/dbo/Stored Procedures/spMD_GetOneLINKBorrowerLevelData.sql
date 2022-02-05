
CREATE PROCEDURE [dbo].[spMD_GetOneLINKBorrowerLevelData]
	@AccountIdentifier				VARCHAR(10),
	@IdentifierType					VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SSN					VARCHAR(9)

	--get SSN if account number is passed in
    IF @IdentifierType = 'SSN'
	BEGIN
		SET @SSN = @AccountIdentifier
		PRINT 'SSN'
	END 
	ELSE
	BEGIN
		SET @SSN = (SELECT BF_SSN FROM dbo.ONELINK_HANDLING WHERE DF_SPE_ACC_ID = @AccountIdentifier)
		PRINT 'ACCT#'
	END
	
	SELECT DISTINCT BF_SSN as SSN,
		DF_SPE_ACC_ID as AccountNumber
	FROM dbo.ONELINK_HANDLING
	WHERE BF_SSN = @SSN

END
