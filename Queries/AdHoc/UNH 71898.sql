USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		NeedHelpUheaa..DAT_Ticket
	SET
		TicketCode = 'Phone System Issues'
	WHERE
		TicketCode = 'Noble Issues'

	SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

	DELETE FROM CSYS..FLOW_DAT_FlowStep
	WHERE FlowID LIKE 'Noble%'

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

	DELETE FROM CSYS..FLOW_DAT_Flow
	WHERE FlowID LIKE 'Noble%'

	SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

		UPDATE
		NeedHelpUheaa..DAT_Ticket
	SET
		TicketCode = 'PSCC'
	WHERE
		TicketCode = 'NCC'

	SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

	DELETE FROM CSYS..FLOW_DAT_FlowStep
	WHERE FlowID LIKE 'NCC'

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

	DELETE FROM CSYS..FLOW_DAT_Flow
	WHERE FlowID LIKE 'NCC'

	SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

IF @ROWCOUNT = 164 AND @ERROR = 0
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