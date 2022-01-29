USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
		
	UPDATE
		T
	SET
		T.PreviousStatus = 'BS Approval',
		T.[Status] = FS.[Status]
	FROM
		NeedHelpUheaa..DAT_Ticket T
		LEFT JOIN NeedHelpUheaa..DAT_TicketsAssociatedUserID ID
			ON T.Ticket = ID.Ticket
		LEFT JOIN CSYS..SYSA_DAT_Users U
			ON ID.SqlUserId = U.SqlUserId
		LEFT JOIN CSYS..FLOW_DAT_FlowStep FS
			ON T.TicketCode = FS.FlowID
			AND FS.FlowStepSequenceNumber = (SELECT MAX(FlowStepSequenceNumber) FROM CSYS..FLOW_DAT_FlowStep WHERE FlowID = T.TicketCode)
	WHERE
		ID.[Role] = 'Court'
		AND U.WindowsUserName = 'bcox'
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		ID
	SET
		ID.SqlUserId = NULL
	FROM
		NeedHelpUheaa..DAT_TicketsAssociatedUserID ID
		LEFT JOIN CSYS..SYSA_DAT_Users U
			ON ID.SqlUserId = U.SqlUserId
	WHERE
		ID.[Role] = 'Court'
		AND U.WindowsUserName = 'bcox'

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 6098 AND @ERROR = 0
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