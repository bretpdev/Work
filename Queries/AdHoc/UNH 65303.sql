--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 18 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'Puerto Rico Earthquakes (DR-4473)'
				,@BeginDate DATE = CONVERT(DATE,'20191228') --INCIDENT start date (not declaration date!)
				,@AddedBy VARCHAR(50) = 'UNH 65303' --change to current NH ticket
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters);

		--SET IDENTITY_INSERT [dasforbfed].[Disasters] ON; --OPSDEV ONLY

			INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
			VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 89, @BeginDate), 1, @AddedBy);--1
			-- Save/Set the row count from the previously executed statement
			SELECT @ROWCOUNT = @@ROWCOUNT;		
		
		--SET IDENTITY_INSERT [dasforbfed].[Disasters] OFF; --OPSDEV ONLY
		
		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	
		DECLARE @Zips TABLE (ZipCode VARCHAR(5));
		INSERT INTO @Zips (ZipCode) VALUES
		--Guanica	Guayanilla	Penuelas	Ponce		San Gernan	Yauco
		('00647'),	('00656'),	('00624'),	('00715'),	('00636'),	('00698'),
		('00653'),							('00716'),	('00683'),	
											('00717'),		
											('00728'),		
											('00730'),		
											('00731'),		
											('00732'),		
											('00733'),		
											('00734'),		
											('00780')
		;

	--select count(ZipCode) as all_zips from @Zips
	----17
	--select count(distinct ZipCode) as distinct_zips from @Zips
	----17

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
