CREATE PROCEDURE [dbo].[AddBusinessUnitByID]
	@ID int,
	@Name varchar(50)
AS
	INSERT INTO BusinessUnits(BusinessUnitId, BusinessUnitName)
	VALUES(@ID, @Name)
RETURN 0