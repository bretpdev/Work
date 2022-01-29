USE NeedHelpCornerStone
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		DAT_Ticket
	SET
		History = 
		'Jeremy Blair - XX/XX/XXXX XX:XX AM - Discussion

		The DCR fix for XXXXX was completed. Are these Batch Criticals still showing up on the report?

		Adam Isom - XX/XX/XXXX XX:XX AM - Discussion

		Moving to my court

		Jeremy Blair - XX/XX/XXXX XX:XX AM - Discussion

		The fix for ticket XXXXX should fix these as well.

		Alyssa Despain - XX/XX/XXXX XX:XX PM - Discussion

		"You are missing LNXX records for the time period referenced in the error. These LNXX records need to be inserted using the attached template. You will also need to ensure there is LPXX set up for that time period as well."

		These appear to be the same accounts as last time, sending to Jeremy to verify

		Alyssa Despain - XX/XX/XXXX XX:XX AM - Discussion

		submitted ccc XXXXX

		Alyssa Despain - XX/XX/XXXX XX:XX AM - Discussion

		Court changed from Jeremy Blair to Alyssa Despain

		Alyssa Despain - XX/XX/XXXX XX:XX AM - Discussion

		Issue:
		TSXGX TSXGF INVALID RETURN CODE FROM INT ACC SUBROUTINE'
	WHERE
		Ticket = XXXXX

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

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