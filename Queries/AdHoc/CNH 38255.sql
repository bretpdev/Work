--run on UHEAASQLDB
USE CLS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = X
				,@ExpectedRowCount INT = XX --# of distinct zips + X disaster forb
				,@Disaster VARCHAR(XXX) = 'South Dakota Severe Winter Storm, Snowstorm, And Flooding (DR-XXXX)'
				,@BeginDate DATE = CONVERT(DATE,'XXXXXXXX')
				,@AddedBy VARCHAR(XX) = 'CNH XXXXX'
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+X FROM dasforbfed.Disasters);

		INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
		VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, XX, @BeginDate), DATEADD(DAY, XX, @BeginDate), X, @AddedBy);
		--X

		-- Save/Set the row count from the previously executed statement
		SELECT @ROWCOUNT = @@ROWCOUNT;
	
		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	
		DECLARE @Zips TABLE (ZipCode VARCHAR(X));
		INSERT INTO @Zips (ZipCode) VALUES
--Bennett	Bon Homme	Charles Mix	Cheyenne River indian reservation	Dewey	Hutchinson	Jackson	Mellette	Minnehaha	Oglala lakota	Oglala sioux tribe of the pine ridge reservation	rosebud indian reservation	todd	yankton		ziebach
('XXXXX'),	('XXXXX'),	('XXXXX'),	('XXXXX'),							('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),	('XXXXX'),		('XXXXX'),											('XXXXX'),					('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),	('XXXXX'),	('XXXXX'),										('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),	('XXXXX'),																						('XXXXX'),('XXXXX'),('XXXXX'),
('XXXXX'),	('XXXXX'),	('XXXXX'),										('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),('XXXXX'),	('XXXXX'),																						('XXXXX'),('XXXXX'),	
			('XXXXX'),	('XXXXX'),										('XXXXX'),('XXXXX'),('XXXXX'),			('XXXXX'),	('XXXXX'),																						('XXXXX'),('XXXXX'),	
			('XXXXX'),	('XXXXX'),										('XXXXX'),('XXXXX'),('XXXXX'),			('XXXXX'),	('XXXXX'),																						('XXXXX'),('XXXXX'),	
						('XXXXX'),										('XXXXX'),('XXXXX'),					('XXXXX'),	('XXXXX'),																								  ('XXXXX'),	
						('XXXXX'),										('XXXXX'),('XXXXX'),					('XXXXX'),	('XXXXX'),					
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX'),						
																												('XXXXX')				

			;

	--select count(ZipCode) as all_zips from @Zips
	----XX
	--select count(distinct ZipCode) as distinct_zips from @Zips
	----XX

		IF NOT EXISTS 
		(
			SELECT 
				ZipId 
			FROM 
				[dasforbfed].[Zips] ZX
				INNER JOIN @Zips ZX
					ON ZX.ZipCode = ZX.ZipCode
			WHERE
				ZX.DisasterId = @DisasterID
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
			PRINT 'Expected row count not met. Expecting ' +  CAST(@ExpectedRowCount AS VARCHAR(XX)) + ' rows, but returned ' + CAST(@ROWCOUNT AS VARCHAR(XX))+ ' rows.';
			ROLLBACK TRANSACTION;
		END
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed. Errors found in SQL statement.';
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;