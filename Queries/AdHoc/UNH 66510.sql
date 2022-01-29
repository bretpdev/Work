--manager = Andrea Hayhurst 
--SELECT * FROM BSYS..SYSA_LST_Users WHERE FirstName = 'Andrea'
--SELECT * FROM CSYS..SYSA_DAT_Users WHERE FirstName = 'Andrea'

GO
--set the following values
DECLARE @NewUnit VARCHAR(50) = 'Vendor Management'; --name of the new unit
DECLARE @Abbreviation VARCHAR(50) = 'VEN'; --just make this up but try to keep it to 3 characters or less
DECLARE @EffectiveBegin DATE = GETDATE(); --enter a different date if the begin date of the cost center allocation is not today
DECLARE @CostCenter1 VARCHAR(50) = 'LPP Administration'; --name of first cost center >>use this query to get names of cost centers: SELECT * FROM CSYS..COST_DAT_CostCenters
DECLARE @Weight1 int = 100; --percent of unit to allocate to first cost center (1 - 100)
DECLARE @CostCenter2 VARCHAR(50) = ''; --name of second cost center (uncomment second cost center mapping below and change @ExpectedRowCount to 6)
DECLARE @Weight2 int = 50; --percent of unit to allocate to second cost center (1 - 100) **sum of @weight1 and @weight2 should = 100
DECLARE @ExpectedRowCount INT = 5; --change to 6 if there is a second cost center
DECLARE @SqlUserId INT = 4542; --use this query to get the CSYS SqlUserId of the manager:  SELECT * FROM CSYS..SYSA_DAT_Users WHERE FirstName = 'andrea' ( this script updates the business unit of the manager of the new business unit in CSYS..SYSA_DAT_Users; other agents are added to the new unit later by Systems Support)
DECLARE @WindowsUserName VARCHAR(50) = 'ahayhurst' --use this query to get the BSYS WindowsUserName of the manager: SELECT * FROM BSYS..SYSA_LST_Users WHERE FirstName = 'andrea'

--get cost center Ids
DECLARE @CostCenterId1 INT = (SELECT CostCenterId FROM CSYS..COST_DAT_CostCenters WHERE CostCenter = @CostCenter1); -- get id of first cost center above
DECLARE @CostCenterId2 INT = (SELECT CostCenterId FROM CSYS..COST_DAT_CostCenters WHERE CostCenter = @CostCenter2); -- get id of first cost center above

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0


--add new unit to BSYS
	INSERT INTO BSYS..GENR_LST_BusinessUnits(BusinessUnit,PseudoBU,Abbreviation) VALUES(@NewUnit,'N',@Abbreviation)
	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	

--add new unit to CSYS
	INSERT INTO [CSYS].[dbo].[GENR_LST_BusinessUnits] ([Name]) VALUES (@NewUnit) -- update this with the name of the new business unit
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

 --get ID of new unit
DECLARE @UnitId INT = (SELECT ID FROM CSYS..GENR_LST_BusinessUnits WHERE [Name] = @NewUnit);

--add first cost center mapping
	INSERT INTO CSYS..COST_DAT_BusinessUnitCostCenters(BusinessUnitId,CostCenterId,Weight,EffectiveBegin)
	VALUES(	@UnitId,@CostCenterId1,@Weight1,@EffectiveBegin)
	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

--add second cost center mapping
	--INSERT INTO CSYS..COST_DAT_BusinessUnitCostCenters(BusinessUnitId,CostCenterId,Weight,EffectiveBegin)
	--VALUES(	@UnitId,@CostCenterId2,@Weight2,@EffectiveBegin)
	---- Update the row count and error number (if any) from the previously executed statement
	--SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

-- update the business unit of the manager of the new business unit in CSYS..[SYSA_DAT_Users]  (other agents are added to the new unit later by Systems Support)
	UPDATE CSYS..SYSA_DAT_Users	SET BusinessUnit = @UnitId WHERE  SqlUserId = @SqlUserId
	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

-- add a manger for the new business unit to GENR_REF_BU_Agent_Xref in BSYS
	INSERT INTO BSYS..GENR_REF_BU_Agent_Xref(BusinessUnit,WindowsUserID,[Role])	VALUES(@NewUnit, @WindowsUserName, 'Manager')
	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
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



--verify results
--SELECT * FROM BSYS..GENR_LST_BusinessUnits ORDER BY BusinessUnit
--SELECT * FROM CSYS..GENR_LST_BusinessUnits ORDER BY [Name]
--SELECT * FROM CSYS..COST_DAT_CostCenters
--SELECT * FROM CSYS..COST_DAT_BusinessUnitCostCenters ORDER BY BusinessUnitId
--SELECT * FROM CSYS..SYSA_DAT_Users WHERE WindowsUserName = 'whack'
--SELECT * FROM BSYS..GENR_REF_BU_Agent_Xref ORDER BY BusinessUnit
