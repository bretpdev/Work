--run on UHEAASQLDB
USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedRowCount INT = XX

	DECLARE @Disaster VARCHAR(XXX) = 'Hawaii Severe Storms, Flooding, Landslides, and Mudslides'
	DECLARE @BeginDate DATE = 'XX/XX/XXXX'

	DECLARE @DisasterID_initial INT = (SELECT MAX(DisasterId)+X FROM dasforbfed.Disasters)

	INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active)
	VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, XX, @BeginDate), DATEADD(DAY, XXX, @BeginDate), X)
	--X

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	
	--Affected zip codes:
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster)
	
	DECLARE @Zips TABLE (ZipCode VARCHAR(X))
	INSERT INTO @Zips (ZipCode) VALUES
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX')
	;

--select  * from @Zips order by ZipCode
--XX

	IF NOT EXISTS 
	(
		SELECT 
			ZipId 
		FROM 
			[dasforbfed].[Zips] ZX
			INNER JOIN @Zips ZX
				ON ZX.ZipCode = ZX.ZipCode
		WHERE
			ZX.DisasterId = @DisasterID
	)
	BEGIN
		INSERT INTO [dasforbfed].[Zips]	(ZipCode, DisasterId)
		SELECT 
			ZipCode
			,@DisasterID 
		FROM 
			@Zips
	END

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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