CREATE PROCEDURE [dbo].[spMD_GetOneLINKBorrowerLevelData]
	@AccountIdentifier				VARCHAR(10) ,
	@IdentifierType					VARCHAR(50) 
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @SSN					VARCHAR(9)
	DECLARE @AccountNumber			CHAR(10)


	SELECT
		@SSN = PD10.DF_PRS_ID,
		@AccountNumber = PD10.DF_SPE_ACC_ID
	FROM	
		PD10_PRS_NME PD10
	WHERE
		PD10.DF_SPE_ACC_ID = @AccountIdentifier OR PD10.DF_PRS_ID = @AccountIdentifier

	IF @SSN IS NULL OR @AccountNumber IS NULL
	BEGIN
		SELECT
			@SSN = PD01.DF_PRS_ID,
			@AccountNumber = PD01.DF_SPE_ACC_ID
		FROM	
			ODW..PD01_PDM_INF PD01
		WHERE
			PD01.DF_SPE_ACC_ID = @AccountIdentifier OR PD01.DF_PRS_ID = @AccountIdentifier
	END

	IF @SSN IS NOT NULL OR @AccountNumber IS NOT NULL
	BEGIN
		SELECT
			@SSN AS SSN,
			@AccountNumber AS AccountNumber
	END
	

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetOneLINKBorrowerLevelData] TO [UHEAA\Imaging Users]
    AS [dbo];

