CREATE PROCEDURE [verforbfed].[GetBorrowerIdentifiers]
	@AccountIdentifier CHAR(10)
AS
	SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED	
	DECLARE @RowCount INT = 0
	
	SELECT
		PD10.DF_PRS_ID AS Ssn,
		PD10.DF_SPE_ACC_ID as AccountNumber
	FROM
		dbo.PD10_PRS_NME PD10
	WHERE
		PD10.DF_SPE_ACC_ID = @AccountIdentifier
		OR
		PD10.DF_PRS_ID = @AccountIdentifier

	SET @RowCount = @@ROWCOUNT
	
	IF @RowCount != 1
	BEGIN
		DECLARE @Server VARCHAR(20)
		IF @@ServerName = 'UHEAASQLDB'
			SET @Server = 'LEGEND'
		ELSE 
			SET @Server = 'LEGEND_TEST_VUK3'
		DECLARE @SQLStatement VARCHAR(MAX) = 
		'
			SELECT 
				DF_PRS_ID Ssn,
				DF_SPE_ACC_ID AccountNumber
			FROM 
				OPENQUERY
				(
					' + @Server + ',
					''
						SELECT 
							PD10.DF_PRS_ID,
							PD10.DF_SPE_ACC_ID
						FROM 
							PKUB.PD10_PRS_NME PD10
						WHERE 
							PD10.DF_SPE_ACC_ID = ''''' + @AccountIdentifier + '''''
							OR
							PD10.DF_PRS_ID = ''''' + @AccountIdentifier + '''''
						''
				)
		'
		  
		  EXEC(@SQLStatement)
		  SET @RowCount = @@ROWCOUNT
	END
	 
	IF @RowCount = 0
	BEGIN
		RAISERROR('[verforbfed].[GetBorrowerIdentifiers] returned %i record(s) for Account Identifier: %s', 16, 1, @RowCount, @AccountIdentifier) 
	END
RETURN 0
