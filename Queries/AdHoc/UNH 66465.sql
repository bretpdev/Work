--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 14 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'Oregon Severe Storms, Flooding, Landslides, And Mudslides (DR-4519)'
				,@BeginDate DATE = CONVERT(DATE,'20200403') --INCIDENT start date (not declaration date!)
				,@AddedBy VARCHAR(50) = 'UNH 66465' --change to current NH ticket
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters);

		--SET IDENTITY_INSERT [dasforbfed].[Disasters] ON; --OPSDEV ONLY

			INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
			VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 89, @BeginDate), 1, @AddedBy);--1
			-- Save/Set the row count from the previously executed statement
			SELECT @ROWCOUNT = @@ROWCOUNT;		
		
		--SET IDENTITY_INSERT [dasforbfed].[Disasters] OFF; --OPSDEV ONLY
		
		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	
		DECLARE @Zips1 TABLE (ZipCode VARCHAR(5));
		INSERT INTO @Zips1 (ZipCode) VALUES
		('97810'),
		('97835'),
		('97875'),
		('97886'),
		('97801'),
		('97813'),
		('97859'),
		('97862'),
		('97880'),
		('97826'),
		('97838'),
		('97868'),
		('97882')
;
--		DECLARE @Zips2 TABLE (ZipCode VARCHAR(5));
--		INSERT INTO @Zips2 (ZipCode) VALUES
--;


--SELECT * FROM @ZIPS1 
--UNION ALL
--SELECT * FROM @ZIPS2 
--;

	--select count(ZipCode) as all_zips from 
	--(
	--	SELECT * FROM @ZIPS1 
	--	--UNION ALL
	--	--SELECT * FROM @ZIPS2 
	--)z
	----13

	--select count(distinct ZipCode) as distinct_zips from
	--(
	--	SELECT * FROM @ZIPS1 
	--	--UNION ALL
	--	--SELECT * FROM @ZIPS2
	--)z
	----13

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
