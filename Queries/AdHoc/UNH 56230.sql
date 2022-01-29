USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 2
	DECLARE @DisasterID INT = 4
	DECLARE @Disaster VARCHAR(100) = 'Tropical Storm Gita'
	DECLARE @BeginDate DATE = '02/07/2018'
	DECLARE @ZipCode VARCHAR(5)= '96799'
	
	INSERT INTO [dasforbfed].[Disasters]
		(DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active)
	VALUES(@DisasterID, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 179, @BeginDate), 1)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	IF NOT EXISTS 
	(
		SELECT 
			ZipId 
		FROM 
			[dasforbfed].[Zips]
		WHERE
			ZipCode = @ZipCode 
			AND DisasterId = @DisasterID
	)
	BEGIN
		INSERT INTO [dasforbfed].[Zips]
			(ZipCode, DisasterId)
		VALUES(@ZipCode, @DisasterID)
	END

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
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