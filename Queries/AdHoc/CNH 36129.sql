USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XX

	UPDATE [dasforbfed].[Disasters]
	SET MaxEndDate = EndDate
	--select * from dasforbfed.Disasters
	WHERE Disaster NOT IN
	--select * from dasforbfed.Disasters where disaster in
	(
		'Previous disasters'--Harvey & Irma
		,'California Wildfire'--California Wildfires (DR-XXXX)
		,'California Mudslide'--California Wildfires, Flooding, Mudflows, And Debris Flows (DR-XXXX)
		,'Hurricane Maria'
	)
	
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
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END