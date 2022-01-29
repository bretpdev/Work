USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = X

	UPDATE [dasforbfed].[Disasters]
	SET MaxEndDate = DATEADD(DAY,XXX,BeginDate)
	WHERE Disaster IN
	--select * from dasforbfed.Disasters where disaster in
	(
		 'Tropical Storm Gita'
		,'Alabama Storms and Tornadoes'
		,'Indiana Severe Storms and Flooding'
		,'North Carolina Tornado and Severe Storms'
		,'Hawaii Severe Storms, Flooding, Landslides, and Mudslides'
		,'Hawaii Kilauea Volcanic Eruption and Earthquakes'
		,'Texas Severe Storms and Flooding'
		,'California Wildfires And High Winds (DR-XXXX)'
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