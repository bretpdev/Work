CREATE PROCEDURE [tcpapns].[GetAccountNumberFromSsn] 
	@Ssn VARCHAR(9)
AS
BEGIN

	DECLARE @RowCount INT = 0

	SELECT 
		DF_SPE_ACC_ID 
	FROM 
		UDW..PD10_PRS_NME 
	WHERE 
		DF_PRS_ID = @Ssn
	
	SET @RowCount = @RowCount + @@ROWCOUNT

	IF(@@ROWCOUNT = 0)
		BEGIN
			RAISERROR('GetAccountNumberFromSsn returned 0 records for ssn:%s', 16, 1, @Ssn) 
		END
END