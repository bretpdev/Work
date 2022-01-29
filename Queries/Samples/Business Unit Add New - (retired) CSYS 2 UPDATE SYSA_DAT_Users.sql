-- Business Unit Add New - CSYS 2 UPDATE SYSA_DAT_Users.sql

-- update the business unit of the manager of the new business unit in CSYS..[SYSA_DAT_Users]  (other agents are added to the new unit later by Systems Support)
-- see notes below to update values


GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	--Set values for row to update
	DECLARE @UNIT VARCHAR(50) = 'Training & Communications' -- update this with the name of the new business unit
	DECLARE @SQUID INT = 4155                           -- update this with the SqlUserId of the manager of the new unit

	UPDATE
		CSYS..[SYSA_DAT_Users]
	SET BusinessUnit = (SELECT ID FROM [CSYS]..[GENR_LST_BusinessUnits]	WHERE Name = @UNIT)
	WHERE  SqlUserId = @SQUID

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