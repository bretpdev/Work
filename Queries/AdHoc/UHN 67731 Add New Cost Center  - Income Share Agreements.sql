--Brand New unit (new unit name does not exist)

--update these parameters
  DECLARE @NewUnit varchar(50) = 'ISA'; -- name of new unit
  DECLARE @CostCenter varchar(50) = 'Income Share Agreements';

USE BSYS --change here and on line ##

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0


--BSYS
	--add new unit to list of units
	INSERT INTO GENR_LST_BusinessUnits (BusinessUnit,PseudoBU)
	VALUES (@NewUnit,'N')
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	--add new unit to apps that use it (assumes all apps use it, delete lines that don't apply)
	INSERT INTO BSYS..GENR_REF_BUsAndAppKey(BusinessUnit,ApplicationKey)
	VALUES
	('ISA','Procedures'),
	('ISA','DataSpot'),
	('ISA','Document Generation'),
	('ISA','Letters'),
	('ISA','NeedHelp'),
	('ISA','PJ'),
	('ISA','Sacker'),
	('ISA','System Access DB')
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

 --CSYS
 USE CSYS
	--create table variable to capture ID of new unit
    DECLARE @BUIds TABLE(ID INT);

	--add new unit to list of units
	INSERT INTO GENR_LST_BusinessUnits ([Name])
	VALUES (@NewUnit)
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	--pass ID of new unit to parameter to be used to update COST_DAT_BusinessUnitCostCenters
	DECLARE @BUID INT = (SELECT ID FROM CSYS..GENR_LST_BusinessUnits WHERE [Name] = 'ISA');


	--add new ost center
	INSERT INTO CSYS..COST_DAT_CostCenters(CostCenter,IsBillable,IsChargedOverHead)
	VALUES(@CostCenter,1,1)
	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	--pass ID of new costcenter to parameter to be used to update COST_DAT_BusinessUnitCostCenters
	DECLARE @CCId INT = (SELECT CostCenterId FROM CSYS..COST_DAT_CostCenters WHERE CostCenter = @CostCenter);  -- ID from COST_DAT_CostCenters of cost center to which the new unit should be assigned

	--add new record to COST_DAT_BusinessUnitCostCenters to assign new unit to a cost center
	INSERT INTO COST_DAT_BusinessUnitCostCenters(BusinessUnitId,CostCenterId,[Weight],EffectiveBegin)
	VALUES(@BUID,@CCId,100,GETDATE())
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ERROR = 0 AND @ROWCOUNT = 12
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END

--SELECT * FROM BSYS..GENR_LST_BusinessUnits ORDER BY BusinessUnit
--SELECT * FROM CSYS..GENR_LST_BusinessUnits ORDER BY [Name]
--SELECT * FROM CSYS..COST_DAT_CostCenters ORDER BY CostCenter
--SELECT * FROM CSYS..COST_DAT_BusinessUnitCostCenters