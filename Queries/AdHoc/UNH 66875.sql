--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 41 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'Tennessee Severe Storms, Tornadoes, Straight-line Winds, And Flooding (DR-4541)'
				,@BeginDate DATE = CONVERT(DATE,'20200424')
				,@AddedBy VARCHAR(50) = 'UNH 66875' --change to current NH ticket
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters);

		SET IDENTITY_INSERT [dasforbfed].[Disasters] ON; --OPSDEV ONLY

			INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
			VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 89, @BeginDate), 1, @AddedBy);--1
			-- Save/Set the row count from the previously executed statement
			SELECT @ROWCOUNT = @@ROWCOUNT;		
		
		SET IDENTITY_INSERT [dasforbfed].[Disasters] OFF; --OPSDEV ONLY
		
		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster);

		DECLARE @Zips1 TABLE (ZipCode VARCHAR(5));
		INSERT INTO @Zips1 (ZipCode) VALUES
--Affected Zip Codes
('37302'),
('37304'),
('37308'),
('37310'),
('37311'),
('37312'),
('37315'),
('37320'),
('37323'),
('37341'),
('37343'),
('37350'),
('37351'),
('37353'),
('37363'),
('37364'),
('37373'),
('37377'),
('37379'),
('37384'),
('37401'),
('37402'),
('37403'),
('37404'),
('37405'),
('37406'),
('37407'),
('37408'),
('37409'),
('37410'),
('37411'),
('37412'),
('37414'),
('37415'),
('37416'),
('37419'),
('37421'),
('37422'),
('37424'),
('37450')
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
----40

		IF NOT EXISTS 
		(
			SELECT 
				ZipId 
			FROM 
				[dasforbfed].[Zips] Z1
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
				[dasforbfed].[Zips]	(ZipCode, DisasterId)
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
