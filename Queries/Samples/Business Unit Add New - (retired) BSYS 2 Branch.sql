--Business Unit Add New - BSYS 2 Branch.sql

-- add new business unit to GENR_REF_BU_Branch (under the old organizational strucuture, this table stored which branch the unit belonged to)
-- see notes below to update values


GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	INSERT INTO [BSYS].[dbo].[GENR_REF_BU_Branch]
		(
			[Branch]
		   ,[BusinessUnit]
		)
	VALUES
		(
			'BO' -- since the old organizational structure no longer applies, 'BO' is as good as anything to put here
			,'Training & Communications' -- update this with the name of the new business unit
		)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 1 AND @ERROR = 0 -- only one row should be affected
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END