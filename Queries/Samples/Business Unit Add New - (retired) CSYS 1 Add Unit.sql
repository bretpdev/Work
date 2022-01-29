-- Business Unit Add New - CSYS 1 Add Unit.sql

-- add new business unit to CSYS GENR_LST_BusinessUnits which is the master list of business units in CSYS
-- see notes below to update values

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	INSERT INTO [CSYS].[dbo].[GENR_LST_BusinessUnits] ([Name]) VALUES ('Training & Communications') -- update this with the name of the new business unit

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