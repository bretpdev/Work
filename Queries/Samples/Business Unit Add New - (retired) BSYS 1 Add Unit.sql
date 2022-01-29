--Business Unit Add New - BSYS 1 Add Unit.sql

-- add new business unit to BSYS GENR_LST_BusinessUnits which is the master list of business units in BSYS
-- see notes below to update values

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	INSERT INTO [BSYS].[dbo].[GENR_LST_BusinessUnits]
		(
		   [BusinessUnit]
		  ,[PseudoBU]
		  ,[LetterNo]
		  ,[Type]
		  ,[Parent]
		  ,[Abbreviation]
		)
	VALUES
		(
			'Training & Communications' -- update this with the name of the new business unit
			,'N' -- a real business unit is being added, not an authorization category, so this should be 'N'
			,NULL -- the LetterNo is no longer used so this should be NULL
			,'Group' -- business units are "groups" in the old organizational structure so this should be 'Group'
			,NULL -- since the old organizational structure no longer applies, the unit has no parent so this should be NULL
			,'TC' -- make up a 2 or 3 character abbreviation that makes sense and that hasn't been used before
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