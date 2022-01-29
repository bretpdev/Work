--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 137 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'Louisiana Hurricane Delta (DR-4570-LA)'
				,@BeginDate DATE = CONVERT(DATE,'20201016') --declaration date
				,@AddedBy VARCHAR(50) = 'UNH 69530' --change to current NH ticket
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
('22001'),
('22011'),
('22019'),
('22023'),
('22053'),
('22055'),
('22079'),
('22097'),
('22099'),
('22113'),
('70501'),
('70502'),
('70503'),
('70504'),
('70505'),
('70506'),
('70507'),
('70508'),
('70509'),
('70510'),
('70511'),
('70512'),
('70516'),
('70517'),
('70518'),
('70519'),
('70520'),
('70521'),
('70525'),
('70526'),
('70527'),
('70528'),
('70529'),
('70531'),
('70532'),
('70533'),
('70534'),
('70535'),
('70537'),
('70541'),
('70542'),
('70543'),
('70546'),
('70548'),
('70549'),
('70550'),
('70551'),
('70555'),
('70556'),
('70558'),
('70559'),
('70570'),
('70571'),
('70575'),
('70577'),
('70578'),
('70581'),
('70582'),
('70583'),
('70584'),
('70589'),
('70591'),
('70592'),
('70593'),
('70596'),
('70598'),
('70601'),
('70602'),
('70605'),
('70606'),
('70607'),
('70609'),
('70611'),
('70612'),
('70615'),
('70616'),
('70629'),
('70630'),
('70631'),
('70632'),
('70633'),
('70634'),
('70637'),
('70640'),
('70643'),
('70645'),
('70646'),
('70647'),
('70650'),
('70652'),
('70653'),
('70657'),
('70660'),
('70661'),
('70662'),
('70663'),
('70664'),
('70665'),
('70668'),
('70669'),
('70750'),
('71301'),
('71302'),
('71303'),
('71306'),
('71307'),
('71309'),
('71315'),
('71325'),
('71328'),
('71330'),
('71345'),
('71346'),
('71348'),
('71353'),
('71356'),
('71358'),
('71359'),
('71360'),
('71361'),
('71365'),
('71405'),
('71409'),
('71424'),
('71427'),
('71430'),
('71431'),
('71433'),
('71438'),
('71447'),
('71448'),
('71455'),
('71466'),
('71472'),
('71477'),
('71485'),
('22001'),
('22011'),
('22019'),
('22023'),
('22053'),
('22055'),
('22079'),
('22097'),
('22099'),
('22113'),
('70501'),
('70502'),
('70503'),
('70504'),
('70505'),
('70506'),
('70507'),
('70508'),
('70509'),
('70510'),
('70511'),
('70512'),
('70516'),
('70517'),
('70518'),
('70519'),
('70520'),
('70521'),
('70525'),
('70526'),
('70527'),
('70528'),
('70529'),
('70531'),
('70532'),
('70533'),
('70534'),
('70535'),
('70537'),
('70541'),
('70542'),
('70543'),
('70546'),
('70548'),
('70549'),
('70550'),
('70551'),
('70555'),
('70556'),
('70558'),
('70559'),
('70570'),
('70571'),
('70575'),
('70577'),
('70578'),
('70581'),
('70582'),
('70583'),
('70584'),
('70589'),
('70591'),
('70592'),
('70593'),
('70596'),
('70598'),
('70601'),
('70602'),
('70605'),
('70606'),
('70607'),
('70609'),
('70611'),
('70612'),
('70615'),
('70616'),
('70629'),
('70630'),
('70631'),
('70632'),
('70633'),
('70634'),
('70637'),
('70640'),
('70643'),
('70645'),
('70646'),
('70647'),
('70650'),
('70652'),
('70653'),
('70657'),
('70660'),
('70661'),
('70662'),
('70663'),
('70664'),
('70665'),
('70668'),
('70669'),
('70750'),
('71301'),
('71302'),
('71303'),
('71306'),
('71307'),
('71309'),
('71315'),
('71325'),
('71328'),
('71330'),
('71345'),
('71346'),
('71348'),
('71353'),
('71356'),
('71358'),
('71359'),
('71360'),
('71361'),
('71365'),
('71405'),
('71409'),
('71424'),
('71427'),
('71430'),
('71431'),
('71433'),
('71438'),
('71447'),
('71448'),
('71455'),
('71466'),
('71472'),
('71477'),
('71485')
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
----272, but 136 distinct

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
