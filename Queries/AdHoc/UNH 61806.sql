--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 159 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'Oklahoma Severe Storms, Straight-line Winds, Tornadoes, And Flooding (DR-4438)'
				,@BeginDate DATE = CONVERT(DATE,'20190507')
				,@AddedBy VARCHAR(50) = 'UNH 61806'
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
--Canadian	Creek	Logan	   Muskogee	  osege 	Ottawa	Rogers		Tulsa	Wagoner		washington
('73090'),('74068'),('73027'),('74401'),('74084'),('74355'),('74031'),('74008'),('74014'),('74003'),
('73014'),('74071'),('73050'),('74402'),('74060'),('74331'),('74080'),('74011'),('74429'),('74006'),
('73085'),('74131'),('73058'),('74403'),('74054'),('74354'),('74016'),('74012'),('74446'),('74004'),
('73036'),('74067'),('73063'),('74422'),('74056'),('74343'),('74015'),('74013'),('74454'),('74005'),
('73022'),('74030'),('73073'),('74423'),('74633'),('74370'),('74017'),('74021'),('74458'),('74082'),
('73078'),('74039'),('73044'),('74428'),('74652'),('74360'),('74019'),('74033'),('74467'),('74051'),
('73064'),('74046'),('73056'),('74434'),('74001'),('74363'),('74018'),('74037'),('74477'),('74061'),
('73099'),('74041'),('73028'),('74436'),('74002'),('74358'),('74053'),('74043'),		  ('74022'),
		  ('74044') 		 ,('74439'),('74637'),('74335'),('74036'),('74050'),		  ('74029'),
		  ('74028') 		 ,('74450'),('74035'),('74339'),		  ('74055'),	
		  ('74052') 		 ,('74455'),							  ('74063'),		
		  ('74066') 		 ,('74463'),							  ('74070'),		
		  ('74010') 		 ,('74468'),							  ('74073'),		
		  ('74047') 		 ,('74469'),							  ('74101'),		
							  ('74470'),							  ('74102'),		
																	  ('74103'),		
																	  ('74104'),		
																	  ('74105'),		
																	  ('74106'),		
																	  ('74107'),		
																	  ('74108'),		
																	  ('74110'),		
																	  ('74112'),		
																	  ('74114'),		
																	  ('74115'),		
																	  ('74116'),		
																	  ('74117'),		
																	  ('74119'),		
																	  ('74120'),		
																	  ('74121'),		
																	  ('74126'),		
																	  ('74127'),		
																	  ('74128'),		
																	  ('74129'),		
																	  ('74130'),		
																	  ('74132'),		
																	  ('74133'),		
																	  ('74134'),		
																	  ('74135'),		
																	  ('74136'),		
																	  ('74137'),		
																	  ('74141'),		
																	  ('74145'),		
																	  ('74146'),		
																	  ('74147'),		
																	  ('74148'),		
																	  ('74149'),		
																	  ('74150'),		
																	  ('74152'),		
																	  ('74153'),		
																	  ('74155'),		
																	  ('74156'),		
																	  ('74157'),		
																	  ('74158'),		
																	  ('74159'),		
																	  ('74169'),		
																	  ('74170'),		
																	  ('74171'),		
																	  ('74172'),		
																	  ('74182'),		
																	  ('74183'),		
																	  ('74184'),		
																	  ('74186'),		
																	  ('74187'),		
																	  ('74189'),		
																	  ('74192'),		
																	  ('74193'),		
																	  ('74194')		

;

	--select count(ZipCode) as all_zips from @Zips
	----158
	--select count(distinct ZipCode) as distinct_zips from @Zips
	----158

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
