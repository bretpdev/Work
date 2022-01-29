--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0,
			@ROWCOUNT INT = 0,
			@ExpectedRowCount INT = 80,
			@Disaster VARCHAR(100) = 'Louisiana Hurricane Laura (DR-4559-LA)';
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbuh.Disasters WHERE Disaster = @Disaster);
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbuh.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate);
	--select @Disaster,@DisasterID,@BeginDate;

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(5), DisasterId INT)
	INSERT INTO @Zips_ALL (ZipCode) VALUES
--Affected Zip Codes
('70512'),
('70535'),
('70541'),
('70550'),
('70551'),
('70570'),
('70571'),
('70577'),
('70584'),
('70589'),
('70750'),
('71004'),
('71007'),
('71009'),
('71029'),
('71033'),
('71043'),
('71044'),
('71047'),
('71060'),
('71061'),
('71069'),
('71082'),
('71101'),
('71102'),
('71103'),
('71104'),
('71105'),
('71106'),
('71107'),
('71108'),
('71109'),
('71115'),
('71118'),
('71119'),
('71120'),
('71129'),
('71130'),
('71133'),
('71134'),
('71135'),
('71136'),
('71137'),
('71138'),
('71148'),
('71149'),
('71150'),
('71151'),
('71152'),
('71153'),
('71154'),
('71156'),
('71161'),
('71162'),
('71163'),
('71164'),
('71165'),
('71166'),
('71220'),
('71221'),
('71222'),
('71223'),
('71229'),
('71234'),
('71241'),
('71250'),
('71256'),
('71260'),
('71261'),
('71264'),
('71277'),
('71342'),
('71345'),
('71353'),
('71356'),
('71358'),
('71371'),
('71465'),
('71479'),
('71480')
;
	
	UPDATE @Zips_ALL
	SET DisasterId = @DisasterID;

	--select * from @Zips_ALL;
	--80
	--select ZipCode,count(ZipCode) from @Zips_ALL group by ZipCode having count(ZipCode) > 1;
	--0
	--select distinct * from @Zips_ALL;
	--80

	--remove zips that already exist in database
	DECLARE @Zips TABLE (ZipCode VARCHAR(5))
	INSERT INTO @Zips (ZipCode)
	SELECT DISTINCT
		ZA.ZipCode
	FROM
		@Zips_ALL ZA --all zip codes from spreadsheet
		LEFT JOIN [dasforbuh].[Zips] DZ --zips in database
			ON ZA.ZipCode = DZ.ZipCode
			AND ZA.DisasterId = DZ.DisasterId
	WHERE
		DZ.ZipCode IS NULL --removes zips in database
	--80

	select * from @Zips --new zips to add @ExpectedRowCount
	--80

	IF NOT EXISTS 
	(
		SELECT 
			ZipId 
		FROM 
			[dasforbuh].[Zips] Z1
			INNER JOIN @Zips Z2
				ON Z1.ZipCode = Z2.ZipCode
		WHERE
			Z1.DisasterId = @DisasterID
	)
	BEGIN
		INSERT INTO [dasforbuh].[Zips]	(ZipCode, DisasterId)
		SELECT DISTINCT
			ZipCode
			,@DisasterID 
		FROM 
			@Zips
	END

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END