

-- =============================================
-- Author:		Jarom Ryan
-- Create date: 11/27/2012
-- Description:	Gets the borrowers account number for the given SSN
-- =============================================
CREATE PROCEDURE [dbo].[spGetAccountNumberFromSSN] 
	
	@Ssn AS Varchar(9)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	
	DECLARE @RowCount INT = 0
	
	SELECT DF_SPE_ACC_ID FROM dbo.PD10_PRS_NME WHERE DF_PRS_ID = @Ssn
	SET @RowCount = @@ROWCOUNT
	
	IF @RowCount != 1
	BEGIN
	
		DECLARE @SQLStatement VARCHAR(MAX)
		
		SELECT @SQLStatement = 
			'
				SELECT 
					* 
				FROM 
					OPENQUERY
					(
						DUSTER,
						''
							SELECT 
								DF_SPE_ACC_ID 
							FROM 
								OLWHRM1.PD10_PRS_NME 
							WHERE 
								DF_PRS_ID = ''''' + @Ssn + '''''
						  ''
					)
			'
		  
		  EXEC(@SQLStatement)
		  SET @RowCount = @@ROWCOUNT
	END
	 
	IF @RowCount = 0
	BEGIN
		RAISERROR('spGetSSNFromAcctNumber returned %i record(s) for AccountNumber: %s', 16, 1, @RowCount, @Ssn) 
	END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetAccountNumberFromSSN] TO [db_executor]
    AS [dbo];

