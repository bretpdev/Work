USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		DAT_Ticket
	SET
		History = 'Alyssa Despain - XX/XX/XXXX XX:XX AM - Discussion

Submitted ccc ISXXXXXX

Alyssa Despain - XX/XX/XXXX XX:XX AM - Discussion

INSERT TO LNGXX FAILED - XXXX NXX

Updating the other half of the error message

Alyssa Despain - XX/XX/XXXX XX:XX AM - Discussion

submitted cnoc XXXXX

Alyssa Despain - XX/XX/XXXX XX:XX AM - Discussion

Court changed from David Halladay to Alyssa Despain

Alyssa Despain - XX/XX/XXXX XX:XX AM - Discussion

Issue:
Fixing batch critical UTTSJNO TSNOXXXX 

'
	WHERE
		Ticket = XXXXX

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT =  AND @ERROR = X
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
