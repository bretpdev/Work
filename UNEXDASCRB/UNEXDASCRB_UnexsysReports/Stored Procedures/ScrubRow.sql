CREATE PROCEDURE [unexdascrb].[ScrubRow]
	@CallId VARCHAR(50),
	@ColumnName VARCHAR(50)
AS
	IF unexdascrb.VerifyColumnName(@ColumnName) = 0 
	BEGIN
		RAISERROR('Could not verify column %s', 16, 1, @ColumnName)
		RETURN
	END
	IF unexdascrb.VerifyCallId(@CallId) = 0 
	BEGIN
		RAISERROR('Could not verify call id %s', 16, 1, @CallId)
		RETURN
	END

	DECLARE @Query VARCHAR(MAX) = 
'
	UPDATE
		dbo.RPT_CALLACTIVITY
	SET
		' + @ColumnName + ' = ''''
	WHERE
		CallId = ''' + @CallId + '''
';

	EXEC(@Query)

RETURN 0
