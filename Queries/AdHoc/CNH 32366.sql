USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		cls..BatchEmail
	SET
		SASFile = 'CurrentNaturalDisasterEmailPop*'
	WHERE
		BatchEmailId = (SELECT BatchEmailId FROM cls..BatchEmail WHERE SASFile = 'NaturalDisasterEmailPop*')

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO cls..BatchEmail(SASFile, LetterId, SendingAddress,  SubjectLine, ARC, CommentText, IncludeAcctNumber)
	VALUES('DelqNaturalDisasterEmailPop*','NDSTDF.html','customerservice@mycornerstoneloan.org','Federal Disaster Relief Area','NDSNF','Disaster email notice sent to borrower',X)

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
