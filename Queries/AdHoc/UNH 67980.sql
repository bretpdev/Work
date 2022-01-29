--run on UHEAASQLDB
USE ULS
GO

BEGIN TRY
	BEGIN TRANSACTION;

		DECLARE @ROWCOUNT INT = 0
				,@ExpectedRowCount INT = 138 --# of distinct zips + 1 disaster forb
				,@Disaster VARCHAR(100) = 'Michigan Severe Storms And Flooding (DR-4547); Utah Earthquake And Aftershocks (DR-4548)'
				,@BeginDate DATE = CONVERT(DATE,'20200709') --declaration date
				,@AddedBy VARCHAR(50) = 'UNH 67980' --change to current NH ticket
				,@DisasterID_initial INT = (SELECT MAX(DisasterId)+1 FROM dasforbuh.Disasters)
				,@DelinquencyOverride BIT = 0; --set to 1 for COVID, 0 for all others

		INSERT INTO [dasforbuh].[Disasters]
		(
			DisasterId, 
			Disaster, 
			BeginDate, 
			EndDate, 
			MaxEndDate, 
			DelinquencyOverride, --set to 1 for COVID, 0 for all others
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
			@DelinquencyOverride, 
			1, 
			@AddedBy
		);--1
		-- Save/Set the row count from the previously executed statement
		SELECT @ROWCOUNT = @@ROWCOUNT;		
		
		--Affected zip codes:
		DECLARE @DisasterID INT = (SELECT DisasterId FROM dasforbuh.Disasters WHERE Disaster = @Disaster);

		DECLARE @Zips1 TABLE (ZipCode VARCHAR(5)); 
		INSERT INTO @Zips1 (ZipCode) VALUES
--Affected Zip Codes
('84006'),
('84009'),
('84010'),
('84011'),
('84014'),
('84015'),
('84016'),
('84020'),
('84025'),
('84037'),
('84040'),
('84041'),
('84044'),
('84047'),
('84054'),
('84056'),
('84065'),
('84070'),
('84075'),
('84081'),
('84084'),
('84087'),
('84088'),
('84089'),
('84090'),
('84091'),
('84092'),
('84093'),
('84094'),
('84095'),
('84096'),
('84101'),
('84102'),
('84103'),
('84104'),
('84105'),
('84106'),
('84107'),
('84108'),
('84109'),
('84110'),
('84111'),
('84112'),
('84113'),
('84114'),
('84115'),
('84116'),
('84117'),
('84118'),
('84119'),
('84120'),
('84121'),
('84122'),
('84123'),
('84124'),
('84125'),
('84126'),
('84127'),
('84128'),
('84129'),
('84130'),
('84131'),
('84132'),
('84133'),
('84134'),
('84138'),
('84139'),
('84141'),
('84143'),
('84145'),
('84147'),
('84148'),
('84150'),
('84151'),
('84152'),
('84157'),
('84158'),
('84165'),
('84170'),
('84171'),
('84180'),
('84184'),
('84190'),
('84199'),
('48415'),
('48417'),
('48601'),
('48602'),
('48603'),
('48604'),
('48605'),
('48606'),
('48607'),
('48608'),
('48609'),
('48610'),
('48612'),
('48614'),
('48616'),
('48618'),
('48620'),
('48623'),
('48624'),
('48626'),
('48628'),
('48637'),
('48638'),
('48640'),
('48641'),
('48642'),
('48649'),
('48652'),
('48655'),
('48657'),
('48658'),
('48659'),
('48663'),
('48667'),
('48670'),
('48674'),
('48686'),
('48703'),
('48722'),
('48724'),
('48730'),
('48734'),
('48739'),
('48743'),
('48748'),
('48749'),
('48750'),
('48763'),
('48764'),
('48765'),
('48766'),
('48770'),
('48787')
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
----137

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
