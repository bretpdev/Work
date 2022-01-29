--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 83 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'Florida Hurricane Sally (DR-4564-FL)'
				,@BeginDate DATE = CONVERT(DATE,'20200923') --declaration date
				,@AddedBy VARCHAR(50) = 'UNH 69138' --change to current NH ticket
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
('32401'),
('32402'),
('32403'),
('32404'),
('32405'),
('32406'),
('32407'),
('32408'),
('32409'),
('32410'),
('32411'),
('32412'),
('32413'),
('32417'),
('32422'),
('32433'),
('32434'),
('32435'),
('32438'),
('32439'),
('32444'),
('32459'),
('32461'),
('32466'),
('32501'),
('32502'),
('32503'),
('32504'),
('32505'),
('32506'),
('32507'),
('32508'),
('32509'),
('32511'),
('32512'),
('32513'),
('32514'),
('32516'),
('32520'),
('32521'),
('32522'),
('32523'),
('32524'),
('32526'),
('32530'),
('32531'),
('32533'),
('32534'),
('32535'),
('32536'),
('32537'),
('32538'),
('32539'),
('32540'),
('32541'),
('32542'),
('32544'),
('32547'),
('32548'),
('32549'),
('32550'),
('32559'),
('32560'),
('32561'),
('32562'),
('32563'),
('32564'),
('32565'),
('32566'),
('32567'),
('32568'),
('32569'),
('32570'),
('32571'),
('32572'),
('32577'),
('32578'),
('32579'),
('32580'),
('32583'),
('32588'),
('32591')
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
----82

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
