--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 68 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'Tennessee Severe Storms, Tornadoes, Straight-line Winds, And Flooding (DR-4476)'
				,@BeginDate DATE = CONVERT(DATE,'20200303') --INCIDENT start date (not declaration date!)
				,@AddedBy VARCHAR(50) = 'UNH 66075' --change to current NH ticket
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
		--Affected Zip Codes
		('37011'),
		('37013'),
		('37070'),
		('37071'),
		('37072'),
		('37076'),
		('37080'),
		('37087'),
		('37088'),
		('37090'),
		('37115'),
		('37116'),
		('37121'),
		('37122'),
		('37136'),
		('37138'),
		('37184'),
		('37189'),
		('37201'),
		('37202'),
		('37203'),
		('37204'),
		('37205'),
		('37206'),
		('37207'),
		('37208'),
		('37209'),
		('37210'),
		('37211'),
		('37212'),
		('37213'),
		('37214'),
		('37215'),
		('37216'),
		('37217'),
		('37218'),
		('37219'),
		('37220'),
		('37221'),
		('37222'),
		('37224'),
		('37227'),
		('37228'),
		('37229'),
		('37230'),
		('37232'),
		('37234'),
		('37235'),
		('37236'),
		('37238'),
		('37240'),
		('37241'),
		('37242'),
		('37243'),
		('37244'),
		('37246'),
		('37250'),
		('38501'),
		('38502'),
		('38503'),
		('38505'),
		('38506'),
		('38544'),
		('38545'),
		('38548'),
		('38574'),
		('38582')
	;

	--select count(ZipCode) as all_zips from @Zips
	--67
	--select count(distinct ZipCode) as distinct_zips from @Zips
	--67

		IF NOT EXISTS 
		(
			SELECT 
				ZipId 
			FROM 
				dasforbfed.Zips Z1
				INNER JOIN @Zips Z2
					ON Z1.ZipCode = Z2.ZipCode
			WHERE
				Z1.DisasterId = @DisasterID
		)
		BEGIN
			INSERT INTO 
				dasforbfed.Zips	(ZipCode, DisasterId)
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
