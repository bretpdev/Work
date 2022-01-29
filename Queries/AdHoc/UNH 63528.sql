USE NeedHelpUheaa 
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	DECLARE @mjones INT = (SELECT SqlUserId FROM CSYS..SYSA_DAT_Users WHERE WindowsUserName = 'mjones' AND [Status] = 'Active')
	DECLARE @nnguyen INT = (SELECT SqlUserId FROM CSYS..SYSA_DAT_Users WHERE WindowsUserName = 'nnguyen' AND [Status] = 'Active')

	INSERT INTO NeedHelpUheaa..LST_CourtAssignment(SqlUserId, Rotation)
	VALUES(@mjones, 4)
	,(@nnguyen, 5)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO NeedHelpCornerStone..LST_CourtAssignment(SqlUserId, Rotation)
	VALUES(@mjones, 4)
	,(@nnguyen, 5)

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 4 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END