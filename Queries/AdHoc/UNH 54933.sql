USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		DAT_Ticket
	SET
		History = 'David Halladay - 01/02/2018 04:48 PM - Discussion

	These accounts also need to be reviewed. 

	80 7683 6690
	96 2340 3263
	56 1623 1063
	58 1557 4155
	21 6994 7551

	Candice Cole - 12/22/2017 04:20 PM - Discussion

	Issue:
	The accounts below appeared on the ARC Reconcillation report for the RWCLM arc. Can you please let us know which accounts or which particular loans on the accounts require the ARC and we''ll get it added. 

	Accounts:
	XXX-XX-8322 
	XXX-XX-8918 
	XXX-XX-8511 
	XXX-XX-6003 '
	WHERE
		Ticket = 54789

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