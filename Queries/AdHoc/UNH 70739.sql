USE MauiDUDE 
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	DECLARE @CategoryId INT

	INSERT INTO MauiDUDE.calls.Categories(Title)
	VALUES('Text Message')

	SET @CategoryId = SCOPE_IDENTITY()

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO MauiDUDE.calls.Reasons(CategoryId, ReasonText, Uheaa, Cornerstone, Inbound, Outbound, [Enabled])
	VALUES(@CategoryId, 'Delinquency', 1, 0, 1, 1, 1)
	,(@CategoryId, 'IDR', 1, 0, 1, 1, 1)
	,(@CategoryId, 'Opt-In', 1, 0, 1, 1, 1)
	,(@CategoryId, 'Opt-Out', 1, 0, 1, 1, 1)
	,(@CategoryId, 'Other', 1, 0, 1, 1, 1)

	SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

IF @ROWCOUNT = 6 AND @ERROR = 0
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