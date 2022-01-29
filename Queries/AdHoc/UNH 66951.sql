--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 242 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'COVID-19 Pandemic VT (DR-4532)'
				,@BeginDate DATE = CONVERT(DATE,'20200408') --declaration date
				,@AddedBy VARCHAR(50) = 'UNH 66951' --change to current NH ticket
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbuh.Disasters);

		--SET IDENTITY_INSERT [dasforbuh].[Disasters] ON; --OPSDEV ONLY

			INSERT INTO [dasforbuh].[Disasters]
			(
				DisasterId, 
				Disaster, 
				BeginDate, 
				EndDate, 
				MaxEndDate, 
				DelinquencyOverride, --set to 1 for COVID
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
				1, 
				1, 
				@AddedBy
			);--1
			-- Save/Set the row count from the previously executed statement
			SELECT @ROWCOUNT = @@ROWCOUNT;		
		
		--SET IDENTITY_INSERT [dasforbuh].[Disasters] OFF; --OPSDEV ONLY
		
		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbuh.Disasters WHERE Disaster = @Disaster);

		DECLARE @Zips1 TABLE (ZipCode VARCHAR(5)); 
		INSERT INTO @Zips1 (ZipCode) VALUES 
--Affected Zip Codes
('05745'),
('05823'),
('05778'),
('05905'),
('05768'),
('05859'),
('05873'),
('05738'),
('05079'),
('05757'),
('05857'),
('05678'),
('05731'),
('05672'),
('05253'),
('05072'),
('05761'),
('05903'),
('05732'),
('05452'),
('05656'),
('05403'),
('05648'),
('05455'),
('05075'),
('05833'),
('05734'),
('05252'),
('05758'),
('05739'),
('05042'),
('05653'),
('05481'),
('05441'),
('05904'),
('05650'),
('05045'),
('05662'),
('05069'),
('05861'),
('05826'),
('05633'),
('05821'),
('05766'),
('05667'),
('05460'),
('05473'),
('05486'),
('05730'),
('05670'),
('05845'),
('05850'),
('05819'),
('05822'),
('05765'),
('05465'),
('05862'),
('05902'),
('05250'),
('05451'),
('05839'),
('05679'),
('05855'),
('05478'),
('05658'),
('05651'),
('05471'),
('05261'),
('05830'),
('05837'),
('05260'),
('05820'),
('05620'),
('05453'),
('05494'),
('05446'),
('05257'),
('05472'),
('05086'),
('05350'),
('05737'),
('05054'),
('05408'),
('05469'),
('05490'),
('05401'),
('05824'),
('05604'),
('05655'),
('05671'),
('05702'),
('05085'),
('05842'),
('05770'),
('05449'),
('05774'),
('05838'),
('05039'),
('05492'),
('05673'),
('05776'),
('05847'),
('05462'),
('05041'),
('05860'),
('05764'),
('05874'),
('05641'),
('05406'),
('05868'),
('05485'),
('05750'),
('05832'),
('05440'),
('05036'),
('05476'),
('05076'),
('05666'),
('05050'),
('05479'),
('05836'),
('05447'),
('05450'),
('05254'),
('05058'),
('05046'),
('05402'),
('05489'),
('05463'),
('05654'),
('05682'),
('05777'),
('05649'),
('05033'),
('05663'),
('05495'),
('05851'),
('05444'),
('05733'),
('05448'),
('05603'),
('05828'),
('05352'),
('05081'),
('05906'),
('05491'),
('05665'),
('05077'),
('05660'),
('05051'),
('05255'),
('05747'),
('05458'),
('05736'),
('05152'),
('05445'),
('05038'),
('05609'),
('05735'),
('05640'),
('05657'),
('05060'),
('05043'),
('05829'),
('05759'),
('05669'),
('05674'),
('05457'),
('05074'),
('05681'),
('05652'),
('05762'),
('05849'),
('05677'),
('05468'),
('05744'),
('05680'),
('05456'),
('05602'),
('05853'),
('05740'),
('05676'),
('05404'),
('05464'),
('05251'),
('05340'),
('05848'),
('05488'),
('05763'),
('05647'),
('05664'),
('05841'),
('05601'),
('05858'),
('05442'),
('05907'),
('05474'),
('05201'),
('05701'),
('05040'),
('05061'),
('05875'),
('05748'),
('05863'),
('05827'),
('05083'),
('05773'),
('05867'),
('05487'),
('05439'),
('05070'),
('05751'),
('05407'),
('05866'),
('05483'),
('05843'),
('05477'),
('05262'),
('05482'),
('05454'),
('05846'),
('05742'),
('05901'),
('05872'),
('05871'),
('05470'),
('05741'),
('05443'),
('05459'),
('05769'),
('05825'),
('05775'),
('05753'),
('05840'),
('05461'),
('05661'),
('05743'),
('05466'),
('05405'),
('05675'),
('05760')
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
----241

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
