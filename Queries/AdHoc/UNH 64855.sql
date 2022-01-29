--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 93 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'South Dakota Severe Storms, Tornadoes, And Flooding (DR-4469)'
				,@BeginDate DATE = CONVERT(DATE,'20190909')
				,@AddedBy VARCHAR(50) = 'UNH 64855'
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters);


		INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedAt, AddedBy)
		VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 89, @BeginDate), 1, GETDATE(), @AddedBy);
		--1

		-- Save/Set the row count from the previously executed statement
		SELECT @ROWCOUNT = @@ROWCOUNT;
	
		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	
		DECLARE @Zips TABLE (ZipCode VARCHAR(5));
		INSERT INTO @Zips (ZipCode) VALUES
			--Brookings
			('57002'),
			('57006'),
			('57007'),
			('57026'),
			('57061'),
			('57071'),
			('57220'),
			('57276'),
			--Charles Mix
			('57329'),
			('57342'),
			('57356'),
			('57361'),
			('57367'),
			('57369'),
			('57380'),
			--Davison
			('57301'),
			('57334'),
			('57363'),
			--Flandreau Indian Reservation
			('57028'),
			--Hanson
			('57311'),
			('57332'),
			('57340'),
			--Hutchinson
			('57029'),
			('57045'),
			('57052'),
			('57331'),
			('57354'),
			('57366'),
			('57376'),
			--Lake
			('57016'),
			('57042'),
			('57050'),
			('57054'),
			('57057'),
			('57075'),
			('57076'),
			--Lincoln
			('57013'),
			('57027'),
			('57032'),
			('57034'),
			('57039'),
			('57064'),
			('57077'),
			('57108'),
			--McCook
			('57012'),
			('57048'),
			('57058'),
			('57319'),
			('57374'),
			--Minnehaha
			('57003'),
			('57005'),
			('57018'),
			('57020'),
			('57022'),
			('57030'),
			('57033'),
			('57035'),
			('57041'),
			('57055'),
			('57056'),
			('57068'),
			('57101'),
			('57103'),
			('57104'),
			('57105'),
			('57106'),
			('57107'),
			('57109'),
			('57110'),
			('57117'),
			('57118'),
			('57186'),
			('57188'),
			('57189'),
			('57192'),
			('57193'),
			('57194'),
			('57195'),
			('57196'),
			('57197'),
			('57198'),
			--Moody
			('57017'),
			('57024'),
			('57028'),
			('57065'),
			--Yankton
			('57031'),
			('57040'),
			('57046'),
			('57067'),
			('57072'),
			('57078'),
			('57079'),
			--('Yankton Indian Reservation'),
			('57570')
		;


	--select count(ZipCode) as all_zips from @Zips
	----93
	--select count(distinct ZipCode) as distinct_zips from @Zips
	----92

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
