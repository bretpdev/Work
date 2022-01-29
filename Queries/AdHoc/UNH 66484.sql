/****  FOR FUTURE TIME TRACKING DEVS:
----TODO: inactivate these:
--Account Services Subbranch
--Administrative Services Division
--Business Operations Branch
--External Training and Communications
--Policy and Training
--System Operations

--Physical Facilities – TODO: combine with Facilities
--LPP Support Services - TODO:  change to Technology & Innovation
*****/

--run on NOCHOUSE
BEGIN TRY
	BEGIN TRANSACTION

	DECLARE @ROWCOUNT INT = 0,
			@ExpectedRowCount INT = 13;

	INSERT INTO CSYS..COST_DAT_BusinessUnitCostCenters
	(
	   BusinessUnitId
      ,CostCenterId
      ,[Weight]
      ,EffectiveBegin
      ,EffectiveEnd
	)
	VALUES
	(
		 44 --Business Development Division
		,1	--LPP Administration
		,100
		,CONVERT(DATE,'20200101')
		,NULL
	),
	(
		 6  --Communications
		,32	--Training
		,100
		,CONVERT(DATE,'20200101')
		,NULL
	),
	(
		 8  --Compliance and Program Review
		,28	--Compliance & QC
		,100
		,CONVERT(DATE,'20200101')
		,NULL
	),
	(
		 13 --Facilities
		,15	--LGP Administration
		,50
		,CONVERT(DATE,'20200101')
		,NULL
	),
	(
		 13 --Facilities
		,1	--LPP Administration
		,50
		,CONVERT(DATE,'20200101')
		,NULL
	),
	(
	     27 --Physical Facilities
		,15 --LGP Administration
		,50
		,CONVERT(DATE,'20200101')
		,NULL
	),
	(
		 27 --Physical Facilities
		,1  --LPP Administration
		,50
		,CONVERT(DATE,'20200101')
		,NULL
	),
	(
		 9  --IT
		,2	--LPP Information Technology
		,100
		,CONVERT(DATE,'20200101')
		,NULL
	),
	(
		 23 --OCHE
		,23 --OCHE
		,100
		,CONVERT(DATE,'20200101')
		,NULL
	),
	(
		 32 --Reception
		,5	--Human Resources
		,100
		,CONVERT(DATE,'20200101')
		,NULL
	),
	(
		 34 --Strategic Projects
		,7	--LPP Support Services  --same as "Technology & Innovation"???
		,100
		,CONVERT(DATE,'20200101')
		,NULL
	),
	(
		 35 --Systems Support
		,7	--LPP Support Services --same as "Technology & Innovation"???
		,100
		,CONVERT(DATE,'20200101')
		,NULL
	)
	;		
	--12 ROWS
	SELECT @ROWCOUNT = @@ROWCOUNT;

	--all OCHE to overhead
	UPDATE 
		CSYS..COST_DAT_CostCenters
	SET 
		IsBillable = 0
	WHERE
		CostCenterId = 23
		AND	CostCenter = 'OCHE'
	;
	--1 ROW
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