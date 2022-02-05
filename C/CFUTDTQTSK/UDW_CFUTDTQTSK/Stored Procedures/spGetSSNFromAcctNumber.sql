



CREATE PROCEDURE [dbo].[spGetSSNFromAcctNumber] 
	@AccountNumber varchar(10)
AS

BEGIN
	SET NOCOUNT ON;
	
	DECLARE @RowCount INT = 0
	
	SELECT DF_PRS_ID FROM dbo.PD10_PRS_NME WHERE DF_SPE_ACC_ID = @AccountNumber
	SET @RowCount = @@ROWCOUNT
	
	IF @RowCount != 1
	BEGIN
	
		SELECT
			DF_PRS_ID
		FROM
			ODW..PD01_PDM_INF
		WHERE
			DF_SPE_ACC_ID = @AccountNumber
		  
		  SET @RowCount = @@ROWCOUNT
	END
	 
	IF @RowCount = 0
	BEGIN
		RAISERROR('spGetSSNFromAcctNumber returned %i record(s) for AccountNumber: %s', 16, 1, @RowCount, @AccountNumber) 
	END
END