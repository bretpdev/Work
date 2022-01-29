--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 59 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'Texas Severe Storms And Flooding (DR-4454)'
				,@BeginDate DATE = CONVERT(DATE,'20190624') --INCIDENT start date (not declaration date!)
				,@AddedBy VARCHAR(50) = 'UNH 62537' --change to current NH ticket
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters);

		INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
		VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 89, @BeginDate), 1, @AddedBy);
		--1

		-- Save/Set the row count from the previously executed statement
		SELECT @ROWCOUNT = @@ROWCOUNT;

		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	
		DECLARE @Zips TABLE (ZipCode VARCHAR(5));
		INSERT INTO @Zips (ZipCode) VALUES
--Cameron	Hidalgo	  Willacy
('78568'),('78503'),('78561'),
('78593'),('78502'),('78594'),
('78566'),('78501'),('78598'),
('78597'),('78504'),('78590'),
('78559'),('78537'),('78569'),
('78567'),('78516'),('78580'),
('78552'),('78505'),	
('78551'),('78573'),	
('78553'),('78574'),	
('78592'),('78576'),	
('78535'),('78565'),	
('78578'),('78570'),	
('78583'),('78572'),	
('78523'),('78595'),	
('78521'),('78596'),	
('78522'),('78599'),	
('78526'),('78577'),	
('78520'),('78579'),	
('78550'),('78589'),	
('78575'),('78541'),	
('78586'),('78542'),	
		  ('78543'),	
		  ('78538'),	
		  ('78539'),	
		  ('78540'),	
		  ('78560'),	
		  ('78562'),	
		  ('78563'),	
		  ('78549'),	
		  ('78557'),	
		  ('78558')
		;
	--select count(ZipCode) as all_zips from @Zips
	--58
	--select count(distinct ZipCode) as distinct_zips from @Zips
	--58

		IF NOT EXISTS 
		(
			SELECT 
				ZipId 
			FROM 
				[dasforbfed].[Zips] Z1
				INNER JOIN @Zips Z2
					ON Z1.ZipCode = Z2.ZipCode
			WHERE
				Z1.DisasterId = @DisasterID
		)
		BEGIN
			INSERT INTO 
				[dasforbfed].[Zips]	(ZipCode, DisasterId)
			SELECT DISTINCT
				ZipCode
				,@DisasterID 
			FROM 
				@Zips
		END;

		-- Save/Set the row count from the previously executed statement
		SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT;

	IF @ROWCOUNT = @ExpectedRowCount
		BEGIN
			PRINT 'Transaction committed.'
			COMMIT TRANSACTION
			--ROLLBACK TRANSACTION
		END
	ELSE
		BEGIN
			PRINT 'Transaction NOT committed.';
			PRINT 'Expected row count not met. Expecting ' +  CAST(@ExpectedRowCount AS VARCHAR(10)) + ' rows, but returned ' + CAST(@ROWCOUNT AS VARCHAR(10))+ ' rows.';
			ROLLBACK TRANSACTION;
		END
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed. Errors found in SQL statement.';
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;
