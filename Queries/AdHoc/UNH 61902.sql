--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 130 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'Arkansas Severe Storms And Flooding (DR-4441)'
				,@BeginDate DATE = CONVERT(DATE,'20190521')
				,@AddedBy VARCHAR(50) = 'UNH 61902'
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters);
				--select @Disaster,@BeginDate,@AddedBy,@DisasterID_initial

		INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
		VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 89, @BeginDate), 1, @AddedBy);
		--1

		-- Save/Set the row count from the previously executed statement
		SELECT @ROWCOUNT = @@ROWCOUNT;
	
		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	
		DECLARE @Zips TABLE (ZipCode VARCHAR(5));
		INSERT INTO @Zips (ZipCode) VALUES
--Conway	Crawford	Faulkner	Jefferson	Perry		Pulaski	Sebastian	Yell
('72157'),	('72952'),	('72106'),	('72175'),	('72025'),('72124'),('72913'),('72829'),
('72030'),	('72955'),	('72061'),	('72132'),	('72016'),('72203'),('72908'),('72833'),
('72080'),	('72947'),	('72181'),	('72079'),	('72070'),('72214'),('72916'),('72828'),
('72027'),	('72935'),	('72111'),	('71613'),	('72126'),('72120'),('72914'),('72824'),
('72063'),	('72934'),	('72173'),	('71602'),	('72125'),('72076'),('72906'),('72827'),
('72110'),	('72948'),	('72058'),	('72168'),	('72001'),('72209'),('72903'),('72834'),
('72127'),	('72956'),	('72034'),	('71611'),			 ('72099'),('72901'),('72853'),
('72156'),	('72957'),	('72033'),	('71603'),			 ('72205'),('72905'),('72842'),
('72107'),	('72921'),	('72032'),	('72004'),			 ('72206'),('72904'),('72857'),
			('72932'),	('72035'),	('72133'),			 ('72116'),('72917'),('72838'),
			('72946'),	('72047'),	('71612'),			 ('72135'),('72940'),('72860'),
									('71601'),			 ('72118'),('72938'),
									('72152'),			 ('72215'),('72945'),
									('72073'),			 ('72204'),('72941'),
									('71659'),			 ('72053'),('72937'),
									('72182'),			 ('72078'),('72919'),
														 ('72113'),('72918'),
														 ('72119'),('72936'),
														 ('72114'),('72923'),
														 ('72117'),('72902'),
														 ('72115'),
														 ('72210'),
														 ('72207'),
														 ('72225'),
														 ('72227'),
														 ('72183'),
														 ('72180'),
														 ('72260'),
														 ('72295'),
														 ('72231'),
														 ('72255'),
														 ('72190'),
														 ('72219'),
														 ('72221'),
														 ('72217'),
														 ('72202'),
														 ('72223'),
														 ('72199'),
														 ('72222'),
														 ('72201'),
														 ('72164'),
														 ('72211'),
														 ('72212'),
														 ('72142'),
														 ('72216')
;

	--select count(ZipCode) as all_zips from @Zips
	----129
	--select count(distinct ZipCode) as distinct_zips from @Zips
	----129

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
