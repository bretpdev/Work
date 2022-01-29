USE NeedHelpCornerStone
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
		
	UPDATE
		T
	SET
		T.PreviousStatus = 'BS Approval',
		T.[Status] = FS.[Status]
	FROM
		NeedHelpCornerStone..DAT_Ticket T
		LEFT JOIN NeedHelpCornerStone..DAT_TicketsAssociatedUserID ID
			ON T.Ticket = ID.Ticket
		LEFT JOIN CSYS..SYSA_DAT_Users U
			ON ID.SqlUserId = U.SqlUserId
		LEFT JOIN CSYS..FLOW_DAT_FlowStep FS
			ON T.TicketCode = FS.FlowID
			AND FS.FlowStepSequenceNumber = (SELECT MAX(FlowStepSequenceNumber) FROM CSYS..FLOW_DAT_FlowStep WHERE FlowID = T.TicketCode)
	WHERE
		(ID.[Role] = 'Court' AND U.WindowsUserName = 'bcox')
		OR
		(ID.[Role] = 'Court' AND ID.SqlUserId IS NULL AND T.[Status] = 'BS Approval')


	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		ID
	SET
		ID.SqlUserId = NULL
	FROM
		NeedHelpCornerStone..DAT_TicketsAssociatedUserID ID
		LEFT JOIN CSYS..SYSA_DAT_Users U
			ON ID.SqlUserId = U.SqlUserId
	WHERE
		ID.[Role] = 'Court'
		AND U.WindowsUserName = 'bcox'

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = XXXX AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END