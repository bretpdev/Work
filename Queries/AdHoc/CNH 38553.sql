--run on UHEAASQLDB
USE CLS;
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X;
	DECLARE @ROWCOUNT INT = X;
	DECLARE @ExpectedRowCount INT = XX;
	DECLARE @Disaster VARCHAR(XXX) = 'Arkansas Severe Storms And Flooding (DR-XXXX)';
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate);
	--select @Disaster,@DisasterID,@BeginDate;

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(X),DisasterID INT);
	INSERT INTO @Zips_ALL (ZipCode) VALUES 
		--Lincoln:
		('XXXXX'),
		('XXXXX'),
		('XXXXX'),
		('XXXXX')
	;

	--adds disaster id to zip code list
	UPDATE @ZIPS_ALL
	SET DISASTERID = @DISASTERID
	;
	--select * from @Zips_ALL;

	----remove zips that already exist in database
	DECLARE @Zips TABLE (ZipCode VARCHAR(X))
	INSERT INTO @Zips (ZipCode)
	SELECT DISTINCT
		ZA.ZipCode
	FROM
		@Zips_ALL ZA --all zip codes from spreadsheet
		LEFT JOIN [dasforbfed].[Zips] DZ --zips in database
			ON ZA.ZipCode = DZ.ZipCode
			AND DZ.DisasterID = @DisasterID
	WHERE
		DZ.ZipCode IS NULL --removes zips in database
	;
	--select * from @Zips

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
		SELECT DISTINCT
			ZipCode
			,@DisasterID 
		FROM 
			@Zips
	END
	;--X
		-- Save/Set the row count and error number (if any) from the previously executed statement
		SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR;

	DECLARE @WRONG_ARKANSAS INT = (SELECT DisasterId FROM CLS.dasforbfed.Disasters WHERE Disaster = 'Arkansas Severe Storms and Flooding');
	DECLARE @RIGHT_ARKANSAS INT = (SELECT DisasterId FROM CLS.dasforbfed.Disasters WHERE Disaster = 'Arkansas Severe Storms And Flooding (DR-XXXX)');

		UPDATE
			CLS.dasforbfed.ProcessQueue
		SET
			DisasterId = @RIGHT_ARKANSAS
			,EndDate = CONVERT(DATE,'XXXXXXXX')
			,BeginDate = DATEADD(DAY, -(DATEDIFF(DAY, BeginDate, CONVERT(DATE,'XXXXXXXX'))), CONVERT(DATE,'XXXXXXXX'))
		
		WHERE
			DisasterId = @WRONG_ARKANSAS
		;--X
			SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR;

	
		DELETE FROM 
			CLS.dasforbfed.Zips 
		WHERE 
			DisasterId = @WRONG_ARKANSAS 
		;--X
			SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR;

		DELETE FROM
			CLS.dasforbfed.Disasters
		WHERE
			DisasterId = @WRONG_ARKANSAS
			AND Disaster = 'Arkansas Severe Storms and Flooding' 
		;--X
			SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR;
	
	DECLARE @WRONG_MISSOURI INT = (SELECT DisasterId FROM CLS.dasforbfed.Disasters WHERE Disaster = 'Missouri Severe Storms, Tornadoes, And Flooding (DR-XXXX)');

		UPDATE
			CLS.dasforbfed.Disasters 
		SET 
			AddedBy = 'CNH XXXXX' 
		WHERE 
			AddedBy = 'CNH XXXXX' 
			AND DisasterId = @WRONG_MISSOURI 
		;--X
			SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR;

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
