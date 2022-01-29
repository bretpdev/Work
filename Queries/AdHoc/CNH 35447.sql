USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		[CLS].[billing].[SpecialMessages]
	SET
		FirstSpecialMessage = 'LIVE CHAT WITH ONE OF OUR LOAN SPECIALISTS TODAY!
		We are excited to announce the release of CornerStone live chat! To utilize this new method of
		communication, navigate to our homepage and click on the “Chat Now” button located at the top of the page.'
	WHERE
		FirstSpecialMessageTitle NOT LIKE '%interest%'
		AND FirstSpecialMessageTitle IS NOT NULL

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = XX AND @ERROR = X
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