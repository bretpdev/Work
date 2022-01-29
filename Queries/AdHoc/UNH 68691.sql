--run on UHEAASQLDB
USE ULS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0,
			@ROWCOUNT INT = 0,
			@ExpectedRowCount INT = 196,
			@Disaster VARCHAR(100) = 'Iowa Severe Storms (DR-4557-IA)';
	DECLARE @BeginDate DATE = (SELECT BeginDate FROM dasforbuh.Disasters WHERE Disaster = @Disaster);
	DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbuh.Disasters WHERE Disaster = @Disaster AND BeginDate = @BeginDate);
	--select @Disaster,@DisasterID,@BeginDate;

	DECLARE @Zips_ALL TABLE (ZipCode VARCHAR(5), DisasterId INT)
	INSERT INTO @Zips_ALL (ZipCode) VALUES
--Affected Zip Codes
('50005'),
('50007'),
('50009'),
('50010'),
('50011'),
('50012'),
('50013'),
('50014'),
('50021'),
('50023'),
('50028'),
('50031'),
('50032'),
('50035'),
('50036'),
('50037'),
('50040'),
('50046'),
('50051'),
('50054'),
('50055'),
('50056'),
('50073'),
('50078'),
('50099'),
('50105'),
('50106'),
('50111'),
('50112'),
('50120'),
('50124'),
('50127'),
('50131'),
('50134'),
('50135'),
('50137'),
('50141'),
('50142'),
('50148'),
('50152'),
('50153'),
('50154'),
('50156'),
('50157'),
('50158'),
('50161'),
('50162'),
('50168'),
('50169'),
('50170'),
('50171'),
('50173'),
('50201'),
('50208'),
('50212'),
('50223'),
('50226'),
('50228'),
('50232'),
('50234'),
('50236'),
('50237'),
('50239'),
('50242'),
('50243'),
('50244'),
('50247'),
('50248'),
('50251'),
('50265'),
('50266'),
('50278'),
('50301'),
('50302'),
('50303'),
('50304'),
('50305'),
('50306'),
('50307'),
('50308'),
('50309'),
('50310'),
('50311'),
('50312'),
('50313'),
('50314'),
('50315'),
('50316'),
('50317'),
('50318'),
('50319'),
('50320'),
('50321'),
('50322'),
('50323'),
('50324'),
('50325'),
('50327'),
('50328'),
('50329'),
('50330'),
('50331'),
('50332'),
('50333'),
('50334'),
('50335'),
('50336'),
('50339'),
('50340'),
('50359'),
('50360'),
('50361'),
('50362'),
('50363'),
('50364'),
('50367'),
('50368'),
('50369'),
('50380'),
('50381'),
('50391'),
('50392'),
('50393'),
('50394'),
('50395'),
('50396'),
('50398'),
('50612'),
('50632'),
('50635'),
('50652'),
('50675'),
('50936'),
('50940'),
('50947'),
('50950'),
('50980'),
('50981'),
('50982'),
('50983'),
('52206'),
('52208'),
('52209'),
('52211'),
('52215'),
('52216'),
('52217'),
('52221'),
('52222'),
('52224'),
('52225'),
('52229'),
('52232'),
('52249'),
('52255'),
('52257'),
('52306'),
('52313'),
('52315'),
('52318'),
('52332'),
('52337'),
('52339'),
('52342'),
('52345'),
('52346'),
('52348'),
('52349'),
('52351'),
('52354'),
('52358'),
('52721'),
('52722'),
('52726'),
('52728'),
('52745'),
('52746'),
('52747'),
('52748'),
('52753'),
('52756'),
('52758'),
('52765'),
('52767'),
('52768'),
('52772'),
('52773'),
('52801'),
('52802'),
('52803'),
('52804'),
('52805'),
('52806'),
('52807'),
('52808'),
('52809')

	;
	
	UPDATE @Zips_ALL
	SET DisasterId = @DisasterID;

	--select * from @Zips_ALL;
	----196
	--select ZipCode,count(ZipCode) from @Zips_ALL group by ZipCode having count(ZipCode) > 1;
	----0
	--select distinct * from @Zips_ALL;
	----196

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
	--196

	--select * from @Zips --new zips to add @ExpectedRowCount
	----196

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