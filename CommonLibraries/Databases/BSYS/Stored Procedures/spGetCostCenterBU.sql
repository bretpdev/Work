CREATE PROCEDURE [dbo].[spGetCostCenterBU]
	@CostCenterCode varchar(10)

AS
	SELECT Name FROM GENR_LST_UHEAACostCenters where Code = @CostCenterCode
RETURN 0
