CREATE PROCEDURE [unexdascrb].[GetScrubbableRows]
	@ColumnName VARCHAR(50)
AS
	IF unexdascrb.VerifyColumnName(@ColumnName) = 0 
	BEGIN
		RAISERROR('Could not verify column %s', 16, 1, @ColumnName)
		RETURN
	END

	DECLARE @Query VARCHAR(MAX) = 
'
	SELECT
		CallId
	FROM
		[dbo].[RPT_CALLACTIVITY]
	WHERE
		NULLIF(RTRIM(' + @ColumnName + '), '''') IS NOT NULL
';		

	EXEC(@Query)

RETURN 0
