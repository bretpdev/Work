--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 209 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'California Wildfires (DR-4558-CA)'
				,@BeginDate DATE = CONVERT(DATE,'20200822') --declaration date
				,@AddedBy VARCHAR(50) = 'UNH 68578' --change to current NH ticket
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbuh.Disasters)
				,@DelinquencyOverride BIT = 0; --set to 1 for COVID, 0 for all others

		INSERT INTO [dasforbuh].[Disasters]
		(
			DisasterId, 
			Disaster, 
			BeginDate, 
			EndDate, 
			MaxEndDate, 
			DelinquencyOverride, --set to 1 for COVID, 0 for all others
			Active, 
			AddedBy
		)
		VALUES
		(
			@DisasterID_initial, 
			@Disaster, 
			@BeginDate, 
			DATEADD(DAY, 89, @BeginDate), 
			DATEADD(DAY, 89, @BeginDate), 
			@DelinquencyOverride, 
			1, 
			@AddedBy
		);--1
		-- Save/Set the row count from the previously executed statement
		SELECT @ROWCOUNT = @@ROWCOUNT;		
		
		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbuh.Disasters WHERE Disaster = @Disaster);

		DECLARE @Zips1 TABLE (ZipCode VARCHAR(5)); 
		INSERT INTO @Zips1 (ZipCode) VALUES
--Affected Zip Codes
('93426'),
('93450'),
('93901'),
('93902'),
('93905'),
('93906'),
('93907'),
('93908'),
('93912'),
('93915'),
('93920'),
('93921'),
('93922'),
('93923'),
('93924'),
('93925'),
('93926'),
('93927'),
('93928'),
('93930'),
('93932'),
('93933'),
('93940'),
('93942'),
('93943'),
('93944'),
('93950'),
('93953'),
('93954'),
('93955'),
('93960'),
('93962'),
('94002'),
('94005'),
('94010'),
('94011'),
('94014'),
('94015'),
('94016'),
('94017'),
('94018'),
('94019'),
('94020'),
('94021'),
('94025'),
('94026'),
('94027'),
('94028'),
('94030'),
('94037'),
('94038'),
('94044'),
('94060'),
('94061'),
('94062'),
('94063'),
('94064'),
('94065'),
('94066'),
('94070'),
('94074'),
('94080'),
('94083'),
('94128'),
('94401'),
('94402'),
('94403'),
('94404'),
('94497'),
('94503'),
('94508'),
('94510'),
('94512'),
('94515'),
('94533'),
('94534'),
('94535'),
('94558'),
('94559'),
('94562'),
('94567'),
('94571'),
('94573'),
('94574'),
('94576'),
('94581'),
('94585'),
('94589'),
('94590'),
('94591'),
('94592'),
('94599'),
('94922'),
('94923'),
('94926'),
('94927'),
('94928'),
('94931'),
('94951'),
('94952'),
('94953'),
('94954'),
('94955'),
('94972'),
('94975'),
('94999'),
('95001'),
('95003'),
('95004'),
('95005'),
('95006'),
('95007'),
('95010'),
('95012'),
('95017'),
('95018'),
('95019'),
('95033'),
('95039'),
('95041'),
('95060'),
('95061'),
('95062'),
('95063'),
('95064'),
('95065'),
('95066'),
('95067'),
('95073'),
('95076'),
('95077'),
('95401'),
('95402'),
('95403'),
('95404'),
('95405'),
('95406'),
('95407'),
('95409'),
('95412'),
('95416'),
('95419'),
('95421'),
('95422'),
('95423'),
('95424'),
('95425'),
('95426'),
('95430'),
('95431'),
('95433'),
('95435'),
('95436'),
('95439'),
('95441'),
('95442'),
('95443'),
('95444'),
('95446'),
('95448'),
('95450'),
('95451'),
('95452'),
('95453'),
('95457'),
('95458'),
('95461'),
('95462'),
('95464'),
('95465'),
('95467'),
('95471'),
('95472'),
('95473'),
('95476'),
('95480'),
('95485'),
('95486'),
('95487'),
('95492'),
('95493'),
('95497'),
('95605'),
('95606'),
('95607'),
('95612'),
('95616'),
('95617'),
('95618'),
('95620'),
('95625'),
('95627'),
('95637'),
('95645'),
('95653'),
('95679'),
('95687'),
('95688'),
('95691'),
('95694'),
('95695'),
('95696'),
('95697'),
('95698'),
('95776'),
('95798'),
('95799'),
('95937')

;

--		DECLARE @Zips2 TABLE (ZipCode VARCHAR(5));
--		INSERT INTO @Zips2 (ZipCode) VALUES 
--;

--		DECLARE @Zips3 TABLE (ZipCode VARCHAR(5));
--		INSERT INTO @Zips3 (ZipCode) VALUES
--;


--;WITH Z AS
--(
--	SELECT * FROM @ZIPS1 
--	--UNION ALL
--	--SELECT * FROM @ZIPS2 
--	--UNION ALL
--	--SELECT * FROM @ZIPS3
--)
--	select 'all_zips' as category, count(ZipCode) as tally from z
--	union all
--	select 'distinct_zips' as category, count(distinct ZipCode) as tally from z
--;
----209

		IF NOT EXISTS 
		(
			SELECT 
				ZipId 
			FROM 
				[dasforbuh].[Zips] Z1
				INNER JOIN 
				(
					SELECT * FROM @ZIPS1 
					--UNION ALL
					--SELECT * FROM @ZIPS2 
					--UNION ALL
					--SELECT * FROM @ZIPS3 			
				) Z2
					ON Z1.ZipCode = Z2.ZipCode
			WHERE
				Z1.DisasterId = @DisasterID
		)
		BEGIN
			INSERT INTO 
				[dasforbuh].[Zips]	(ZipCode, DisasterId)
			SELECT DISTINCT
				ZipCode
				,@DisasterID 
			FROM 
				(
					SELECT * FROM @ZIPS1 
					--UNION ALL
					--SELECT * FROM @ZIPS2 
					--UNION ALL
					--SELECT * FROM @ZIPS3 			
				)z
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
