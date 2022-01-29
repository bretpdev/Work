--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 320 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'COVID-19 Pandemic ID (DR-4534)'
				,@BeginDate DATE = CONVERT(DATE,'20200409') --declaration date
				,@AddedBy VARCHAR(50) = 'UNH 66952' --change to current NH ticket
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
('83824'),
('83854'),
('83717'),
('83404'),
('83323'),
('83636'),
('83209'),
('83210'),
('83235'),
('83469'),
('83287'),
('83253'),
('83832'),
('83539'),
('83263'),
('83445'),
('83252'),
('83847'),
('83647'),
('83542'),
('83867'),
('83851'),
('83254'),
('83530'),
('83726'),
('83303'),
('83320'),
('83830'),
('83217'),
('83602'),
('83634'),
('83716'),
('83864'),
('83615'),
('83801'),
('83449'),
('83641'),
('83347'),
('83549'),
('83654'),
('83233'),
('83340'),
('83855'),
('83841'),
('83853'),
('83810'),
('83804'),
('83547'),
('83443'),
('83535'),
('83228'),
('83202'),
('83211'),
('83402'),
('83637'),
('83554'),
('83630'),
('83850'),
('83283'),
('83318'),
('83705'),
('83545'),
('83450'),
('83874'),
('83438'),
('83712'),
('83272'),
('83424'),
('83327'),
('83520'),
('83633'),
('83822'),
('83540'),
('83876'),
('83428'),
('83522'),
('83548'),
('83652'),
('83866'),
('83466'),
('83839'),
('83646'),
('83321'),
('83836'),
('83330'),
('83333'),
('83455'),
('83335'),
('83332'),
('83713'),
('83623'),
('83687'),
('83701'),
('83870'),
('83720'),
('83255'),
('83313'),
('83218'),
('83706'),
('83644'),
('83619'),
('83431'),
('83338'),
('83735'),
('83722'),
('83626'),
('83703'),
('83622'),
('83803'),
('83256'),
('83860'),
('83729'),
('83877'),
('83806'),
('83610'),
('83611'),
('83234'),
('83468'),
('83845'),
('83204'),
('83638'),
('83237'),
('83555'),
('83422'),
('83236'),
('83350'),
('83406'),
('83421'),
('83533'),
('83732'),
('83229'),
('83226'),
('83656'),
('83334'),
('83728'),
('83342'),
('83657'),
('83869'),
('83871'),
('83814'),
('83353'),
('83857'),
('83835'),
('83337'),
('83627'),
('83328'),
('83629'),
('83203'),
('83436'),
('83221'),
('83324'),
('83861'),
('83865'),
('83460'),
('83872'),
('83314'),
('83631'),
('83873'),
('83276'),
('83446'),
('83834'),
('83281'),
('83531'),
('83686'),
('83821'),
('83526'),
('83215'),
('83322'),
('83648'),
('83635'),
('83553'),
('83442'),
('83316'),
('83650'),
('83465'),
('83714'),
('83501'),
('83434'),
('83809'),
('83274'),
('83846'),
('83341'),
('83524'),
('83655'),
('83452'),
('83812'),
('83639'),
('83709'),
('83435'),
('83423'),
('83808'),
('83464'),
('83708'),
('83525'),
('83666'),
('83612'),
('83311'),
('83628'),
('83232'),
('83702'),
('83852'),
('83239'),
('83815'),
('83262'),
('83451'),
('83205'),
('83463'),
('83201'),
('83420'),
('83523'),
('83325'),
('83642'),
('83719'),
('83606'),
('83403'),
('83355'),
('83837'),
('83285'),
('83401'),
('83546'),
('83448'),
('83212'),
('83462'),
('83440'),
('83244'),
('83617'),
('83660'),
('83427'),
('83607'),
('83405'),
('83214'),
('83206'),
('83825'),
('83227'),
('83250'),
('83616'),
('83653'),
('83467'),
('83811'),
('83715'),
('83415'),
('83543'),
('83302'),
('83858'),
('83277'),
('83843'),
('83707'),
('83805'),
('83645'),
('83868'),
('83536'),
('83848'),
('83354'),
('83213'),
('83312'),
('83669'),
('83823'),
('83724'),
('83826'),
('83711'),
('83230'),
('83241'),
('83643'),
('83537'),
('83336'),
('83756'),
('83278'),
('83429'),
('83672'),
('83220'),
('83541'),
('83425'),
('83849'),
('83454'),
('83245'),
('83827'),
('83816'),
('83433'),
('83671'),
('83624'),
('83632'),
('83731'),
('83352'),
('83286'),
('83604'),
('83670'),
('83840'),
('83552'),
('83833'),
('83348'),
('83343'),
('83223'),
('83344'),
('83725'),
('83261'),
('83349'),
('83271'),
('83677'),
('83842'),
('83243'),
('83441'),
('83246'),
('83651'),
('83676'),
('83605'),
('83251'),
('83544'),
('83704'),
('83301'),
('83802'),
('83844'),
('83813'),
('83856'),
('83799'),
('83346'),
('83444'),
('83238'),
('83661'),
('83680')
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
----319

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
