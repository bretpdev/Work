--Business Unit Add New - BSYS 3 Manager.sql

-- add a manger for the new business unit to GENR_REF_BU_Agent_Xref
-- see notes below to update values

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	INSERT INTO [BSYS].[dbo].[GENR_REF_BU_Agent_Xref]
		(
		     [BusinessUnit]
			,[WindowsUserID]
			,[Role]
		)
	VALUES
		(
			'Training & Communications' -- update this with the name of the new business unit
			,'bfelt' -- update this with the windows user ID of the manager.  This can be found in [BSYS].[dbo].[SYSA_LST_Users]
			,'Manager' -- leave this as 'Manager'
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