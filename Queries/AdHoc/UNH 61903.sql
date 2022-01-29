--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 83 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'South Dakota Severe Winter Storm, Snowstorm, And Flooding (DR-4440)'
				,@BeginDate DATE = CONVERT(DATE,'20190313')
				,@AddedBy VARCHAR(50) = 'UNH 61903'
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters);
				--select @Disaster,@BeginDate,@AddedBy,@DisasterID_initial

		INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
		VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 89, @BeginDate), 1, @AddedBy);
		--1

		-- Save/Set the row count from the previously executed statement
		SELECT @ROWCOUNT = @@ROWCOUNT;
	
		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	
		DECLARE @Zips TABLE (ZipCode VARCHAR(5));
		INSERT INTO @Zips (ZipCode) VALUES
--Bennett	Bon Homme	CharlesMix	CheyenneRiverIndianReservation	Dewey	Hutchinson	Jackson	  Mellette	Minnehaha		OglalaLakota Oglala sioux tribe of the pine ridge reservation	rosebud indian reservation	todd		yankton	  ziebach
('57714'),	('57315'),('57361'),	('57625'),						('57661'),('57354'),('57543'),('57560'),	('57035'),	('57794'),	('57770'),											('57570'),					('57566'),	('57046'),('57623'),
('57551'),	('57066'),('57356'),									('57630'),('57366'),('57547'),('57579'),	('57018'),	('57772'),																					('57570'),	('57072'),('57622'),
('57574'),	('57063'),('57329'),									('57625'),('57376'),('57521'),('57585'),	('57033'),	('57716'),																					('57572'),	('57040'),	
			('57059'),('57367'),									('57636'),('57331'),('57577'),				('57041'),	('57770'),																					('57563'),	('57067'),	
			('57062'),('57369'),									('57652'),('57029'),('57750'),				('57020'),	('57756'),																					('57555'),	('57078'), 	
					  ('57380'),									('57633'),('57045'),						('57118'),	('57752'),																								('57031'),	
					  ('57342'),									('57656'),('57052'),						('57186'),	('57764'),					
																												('57117'),						
																												('57109'),						
																												('57110'),						
																												('57107'),						
																												('57105'),						
																												('57103'),						
																												('57104'),						
																												('57030'),						
																												('57022'),						
																												('57106'),						
																												('57068'),						
																												('57055'),						
																												('57005'),						
																												('57101'),						
																												('57003'),						
																												('57197'),						
																												('57198'),						
																												('57193')					

;

	--select count(ZipCode) as all_zips from @Zips
	----85
	--select count(distinct ZipCode) as distinct_zips from @Zips
	----82

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
			INSERT INTO [dasforbfed].[Zips]	(ZipCode, DisasterId)
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
