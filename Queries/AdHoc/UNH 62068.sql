--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 165 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'Ohio Severe Storms, Straight-line Winds, Tornadoes, Flooding, Landslides, And Mudslide (DR-4447)'
				,@BeginDate DATE = CONVERT(DATE,'20190527')
				,@AddedBy VARCHAR(50) = 'UNH 62068'
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters);
				--select @Disaster,@BeginDate,@AddedBy,@DisasterID_initial

		INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
		VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 89, @BeginDate), 1, @AddedBy);
		--1

		-- Save/Set the row count from the previously executed statement
		SELECT @ROWCOUNT = @@ROWCOUNT;
	
		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	
		DECLARE @Zips TABLE (ZipCode VARCHAR(5),DisasterID INT);
		INSERT INTO @Zips (ZipCode) VALUES
--Auglaize	Darke	Greene	  Hocking	Mercer	   Miami	Montgomery	Muskingum	Perry	Pickaway
('45806'),('45303'),('45301'),('43111'),('45310'),('45308'),('45309'),('43701'),('43076'),('43103'),
('45819'),('45304'),('45305'),('43127'),('45822'),('45312'),('45315'),('43701'),('43730'),('43113'),
('45865'),('45328'),('45307'),('43135'),('45826'),('45317'),('45322'),('43702'),('43731'),('43116'),
('45869'),('45331'),('45314'),('43138'),('45828'),('45318'),('45325'),('43702'),('43739'),('43117'),
('45870'),('45332'),('45316'),('43144'),('45846'),('45326'),('45327'),('43720'),('43748'),('43145'),
('45871'),('45346'),('45324'),('43149'),('45860'),('45337'),('45342'),('43727'),('43760'),('43146'),
('45884'),('45348'),('45335'),('43152'),('45862'),('45339'),('45343'),('43734'),('43761'),('43156'),
('45885'),('45350'),('45370'),('43158'),('45866'),('45356'),('45345'),('43735'),('43764'),('43164'),
('45888'),('45351'),('45384'),		    ('45882'),('45359'),('45354'),('43738'),('43766'),
('45895'),('45352'),('45385'),		    ('45883'),('45361'),('45377'),('43746'),('43782'),
('45896'),('45358'),('45387'),					  ('45371'),('45401'),('43762'),('43783'),
		  ('45362'),('45431'),					  ('45373'),('45402'),('43767')
		 ,('45380'),('45432'),					  ('45374'),('45403'),('43771')
		 ,('45388'),('45433'),					  ('45383'),('45404'),('43777')
		 ,('45390'),('45434')							   ,('45405'),('43791')
				   ,('45435')							   ,('45406'),('43802')
														   ,('45408'),('43821')
														   ,('45409'),('43822')
														   ,('45410'),('43830')
														   ,('45412'),('43842')
														   ,('45413')
														   ,('45414')
														   ,('45415')
														   ,('45416')
														   ,('45417')
														   ,('45418')
														   ,('45419')
														   ,('45420')
														   ,('45422')
														   ,('45423')
														   ,('45424')
														   ,('45426')
														   ,('45427')
														   ,('45428')
														   ,('45429')
														   ,('45430')
														   ,('45437')
														   ,('45439')
														   ,('45440')
														   ,('45441')
														   ,('45448')
														   ,('45449')
														   ,('45454')
														   ,('45458')
														   ,('45459')
														   ,('45463')
														   ,('45469')
														   ,('45470')
														   ,('45475')
														   ,('45479')
														   ,('45481')
														   ,('45482')
														   ,('45490')

;

	--select count(ZipCode) as all_zips from @Zips
	--166
	--select count(distinct ZipCode) as distinct_zips from @Zips
	--164
	--select ZipCode from @Zips group by ZipCode having count(ZipCode) > 1;

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
