CREATE PROCEDURE [pridrcrp].[GetAccountNumberFromSsn] 
	@SSN VARCHAR(10)
AS
BEGIN

	DECLARE @RowCount INT = 0

	SELECT 
		DF_SPE_ACC_ID 
	FROM 
		CDW..PD10_PRS_NME 
	WHERE 
		DF_PRS_ID = @Ssn
	
	SET @RowCount = @@ROWCOUNT
	
	IF @RowCount != 1
	BEGIN
	
		DECLARE @QUERY VARCHAR(MAX), @TSQL VARCHAR(MAX)
	
		SELECT @TSQL = 'SELECT * FROM OPENQUERY(LEGEND,'
	
		SELECT @QUERY = @TSQL +
			'''
			SELECT 
				DF_SPE_ACC_ID 
			FROM 
				PKUB.PD10_PRS_NME 
			WHERE 
				DF_PRS_ID = '''''+@SSN +'''''
			'')'
	  
		EXEC(@QUERY)
	 
		IF(@@ROWCOUNT = 0)
		BEGIN
			RAISERROR('spGetAcctNumberFromSSN returned 0 records for ssn:%s', 16, 1, @SSN) 
		END
	END
END