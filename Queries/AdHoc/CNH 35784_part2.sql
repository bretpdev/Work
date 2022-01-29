USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = X
	DECLARE @DisasterId INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = 'South Carolina Hurricane Florence (DR-XXXX)')
	--SELECT @DisasterId

	--SELECT * FROM dasforbfed.Zips WHERE ZipCode IN ('XXXXX','XXXXX','XXXXX','XXXXX')

	UPDATE CLS.dasforbfed.Zips
	SET DisasterId = @DisasterId
	WHERE ZipCode IN ('XXXXX','XXXXX','XXXXX','XXXXX')
	--X

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END