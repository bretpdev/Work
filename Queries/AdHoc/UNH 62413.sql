--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 305 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'Missouri Severe Storms, Tornadoes, And Flooding (DR-4451)'
				,@BeginDate DATE = CONVERT(DATE,'20190429')
				,@AddedBy VARCHAR(50) = 'UNH 62413'
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbfed.Disasters);
				--select @Disaster,@BeginDate,@AddedBy,@DisasterID_initial

		SET IDENTITY_INSERT [dasforbfed].[Disasters] ON
			INSERT INTO [dasforbfed].[Disasters] (DisasterId, Disaster, BeginDate, EndDate, MaxEndDate, Active, AddedBy)
			VALUES(@DisasterID_initial, @Disaster, @BeginDate, DATEADD(DAY, 89, @BeginDate), DATEADD(DAY, 89, @BeginDate), 1, @AddedBy);
			--1
			-- Save/Set the row count from the previously executed statement
			SELECT @ROWCOUNT = @@ROWCOUNT;
		SET IDENTITY_INSERT [dasforbfed].[Disasters] OFF
	
		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbfed.Disasters WHERE Disaster = @Disaster);
	
		DECLARE @Zips TABLE (ZipCode VARCHAR(5),DisasterID INT);
		INSERT INTO @Zips (ZipCode) VALUES
--Andrew	Atchison	Boone	Buchanan Carroll	Andrew	Atchinson	Boone	Buchanan	Carroll		Chariton	Cole		Greene		Holt		Jackson		Jasper	Lafayette	Lincoln	Livingston	Miller		Osage		Pike		Platte	Pulaski		St. Charles
('64421'),('64446'),('65010'),('64401'),('64622'),('64483'),('64491')	,('65212'),('64502'),('64668'),('64660')	,('65023'),	('65604'),	('64437'),	('64013'),('64755'),('64001'),('63343'),('64601')	,('65017')	,('65001'),	('65001'),	('64018'),('65452')	,('63301'),
('64427'),('64482'),('65039'),('64440'),('64623'),('64436'),('64482')	,('65215'),('64501'),('64643'),('64676')	,('65032'),	('65612'),	('64451'),	('64014'),('64801'),('64011'),('63347'),('64635')	,('65026')	,('65016'),	('65016'),	('64028'),('65457')	,('63302'),
('64436'),('64491'),('65201'),('64443'),('64633'),('64449'),('64496')	,('65203'),('64505'),('64623'),('64681')	,('65040'),	('65619'),	('64466'),	('64015'),('64802'),('64020'),('63349'),('64638')	,('65047')	,('65024'),	('65024'),	('64079'),('65459')	,('63303'),
('64449'),('64496'),('65202'),('64448'),('64639'),('64480'),('64446')	,('65205'),('64504'),('64633'),('65236')	,('65053'),	('65648'),	('64470'),	('64016'),('64803'),('64021'),('63362'),('64656')	,('65064')	,('65035'),	('63330'),	('64092'),('65473')	,('63304'),
('64459'),('64498'),('65203'),('64484'),('64643'),('64459'),('64498')	,('65211'),('64503'),('64639'),('65246')	,('65074'),	('65725'),	('64473'),	('64029'),('64804'),('64022'),('63369'),('64664')	,('65075')	,('65048'),	('63334'),	('64098'),('65534')	,('63332'),
('64480'),			('65205'),('64501'),('64668'),('64421')				,('65279'),('64443'),('64622'),('65261')	,('65076'),	('65738'),				('64030'),('64830'),('64037'),('63370'),('64686')	,('65082')	,('65051'),	('63336'),	('64150'),('65556')	,('63338'),
('64483'),			('65211'),('64502'),('64680'),('64427')				,('65256'),('64440'),('64682'),('65281')	,('65101'),	('65757'),				('64034'),('64833'),('64067'),('63377'),('64688')	,('65083')	,('65054'),	('63339'),	('64151'),('65583')	,('63341'),
('64485'),			('65212'),('64503'),('64682'),('64485')				,('65299'),('64448'),('64680'),('65286')	,('65102'),	('65765'),				('64050'),('64834'),('64071'),('63379')				,('65486')	,('65058'),	('63344'),	('64152'),('65584')	,('63346'),
					('65215'),('64504'),								 ('65284'),('64484')						,('65103'),	('65770'),				('64051'),('64835'),('64074'),('63381')							,('65085'),	('63353'),	('64153')			,('63348'),
					('65216'),('64505'),								 ('65255'),('64401')						,('65104'),	('65781'),				('64052'),('64836'),('64076'),('63387'),									('63433'),	('64154')			,('63365'),
					('65217'),('64506'),								 ('65217'),('64506')						,('65105'),	('65801'),				('64053'),('64841'),('64096'),('63389'),									('63441'),	('64163')			,('63366'),
					('65218'),('64507'),								 ('65216'),('64507')						,('65106'),	('65802'),				('64054'),('64849'),('64097'),															('64164')			,('63367'),
					('65240'),('64508'),								 ('65240'),('64508')						,('65107'),	('65803'),				('64055'),('64855'),('65327'),															('64168')			,('63368'),
					('65255'),											 ('65218')									,('65108'),	('65804'),				('64056'),('64857'),																	('64190')			,('63373'),
					('65256'),											 ('65202')									,('65109'),	('65805'),				('64057'),('64859'),																	('64195')			,('63376'),
					('65279'),											 ('65039')									,('65110'),	('65806'),				('64058'),('64862'),																	('64439')			,('63385'),
					('65284'),											 ('65010')									,('65111'),	('65807'),				('64063'),('64869'),																	('64444')			,('63386'),
					('65299'),											 ('65201'),												('65808'),				('64064'),('64870'),								
																																('65809'),				('64065'),										
																																('65810'),				('64066'),										
																																('65814'),				('64070'),										
																																('65817'),				('64075'),										
																																('65890'),				('64081'),										
																																('65897'),				('64082'),										
																																('65898'),				('64086'),										
																																('65899'),				('64088'),										
																																						('64101'),										
																																						('64102'),										
																																						('64105'),										
																																						('64106'),										
																																						('64108'),										
																																						('64109'),										
																																						('64110'),										
																																						('64111'),										
																																						('64112'),										
																																						('64113'),										
																																						('64114'),										
																																						('64120'),										
																																						('64121'),										
																																						('64123'),										
																																						('64124'),										
																																						('64125'),										
																																						('64126'),										
																																						('64127'),										
																																						('64128'),										
																																						('64129'),										
																																						('64130'),										
																																						('64131'),										
																																						('64132'),										
																																						('64133'),										
																																						('64134'),										
																																						('64136'),										
																																						('64137'),										
																																						('64138'),										
																																						('64139'),										
																																						('64141'),										
																																						('64145'),										
																																						('64146'),										
																																						('64147'),										
																																						('64148'),										
																																						('64149'),										
																																						('64170'),										
																																						('64171'),										
																																						('64172'),										
																																						('64179'),										
																																						('64180'),										
																																						('64183'),										
																																						('64184'),										
																																						('64185'),										
																																						('64187'),										
																																						('64191'),										
																																						('64192'),										
																																						('64193'),										
																																						('64194'),										
																																						('64196'),										
																																						('64197'),										
																																						('64198'),										
																																						('64199'),										
																																						('64944'),										
																																						('64999')								


;

	--select count(ZipCode) as all_zips from @Zips
	--359
	--select count(distinct ZipCode) as distinct_zips from @Zips
	--304
	--select ZipCode from @Zips group by ZipCode having count(ZipCode) > 1;
	--55

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
