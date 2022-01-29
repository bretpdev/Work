USE CSYS

GO

BEGIN TRANSACTION
	DECLARE 
		@ERROR INT = 0,
		@ROWCOUNT INT = 0

INSERT INTO [dbo].[SYSA_DAT_RoleKeyAssignment]
           ([RoleID]
           ,[UserKeyID]
           ,[AddedBy]
           ,[StartDate])
     VALUES
           (
		   41,
		   131,
		   0,
		   getdate()
		   )

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 1 AND @ERROR = 0
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
