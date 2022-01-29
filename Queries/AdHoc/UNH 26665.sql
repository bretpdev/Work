USE NeedHelpUheaa

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	SELECT 'Perform first step here.'

	UPDATE 
		dbo.DAT_Ticket
	SET
		Issue = REPLACE(REPLACE(CAST(Issue as VARCHAR(MAX)), '0479755085', 'XXXXXXXXXX'), '5552208844', 'XXXXXXXXXX'),
		History = REPLACE(REPLACE(CAST(History as VARCHAR(MAX)), '0479755085', 'XXXXXXXXXX'), '5552208844', 'XXXXXXXXXX')
	WHERE
		Ticket in (26665, 26244)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 2 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		--COMMIT TRANSACTION
		ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END
