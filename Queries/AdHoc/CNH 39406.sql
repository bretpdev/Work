USE NeedHelpCornerStone
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		NeedHelpCornerStone..DAT_Ticket
	SET
		[Status] = 'Discussion',
		PreviousStatus = 'Submitting',
		History = 'Wendy Hack - XX/XX/XXXX XX:XX AM - Discussion

	We are looking into this. 

	Mary Nkirote Kavila - XX/XX/XXXX XX:XX AM - Discussion

	Issue:
	Cornerstone dialer files did not load, could you please assist in loading these files in the dialer?

	'
	WHERE
		Ticket = XXXXX

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		NeedHelpCornerStone..DAT_TicketsAssociatedUserID
	SET
		SqlUserId = XXXX
	WHERE
		[Role] = 'Court'
		AND Ticket = XXXXX

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = X AND @ERROR = X
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