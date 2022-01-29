--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 32 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'Iowa Severe Storms (DR-4557-IA)'
				,@BeginDate DATE = CONVERT(DATE,'20200817') --declaration date
				,@AddedBy VARCHAR(50) = 'UNH 68557' --change to current NH ticket
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
('52202'),
('52213'),
('52214'),
('52218'),
('52219'),
('52227'),
('52228'),
('52233'),
('52253'),
('52302'),
('52314'),
('52324'),
('52328'),
('52336'),
('52341'),
('52344'),
('52352'),
('52401'),
('52402'),
('52403'),
('52404'),
('52405'),
('52406'),
('52407'),
('52408'),
('52409'),
('52410'),
('52411'),
('52497'),
('52498'),
('52499')
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
----31

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
