CREATE PROCEDURE [accurint].[AddReceivedDemos]
	@DemosId INT,
	@Region VARCHAR(10),
	@ReceivedAddress1 VARCHAR(45), 
	@ReceivedAddress2 VARCHAR(30), 
	@ReceivedCity VARCHAR(20), 
	@ReceivedState VARCHAR(2), 
	@ReceivedZipCode VARCHAR(17), 
	@ReceivedPhoneNumber VARCHAR(27)
AS
	
	DECLARE @RegionId INT = (SELECT RegionId FROM accurint.Regions WHERE Region = @Region)

	UPDATE
		accurint.DemoLogs 
	SET
		ReceivedAddress1 = @ReceivedAddress1, 
		ReceivedAddress2 = @ReceivedAddress2, 
		ReceivedCity = @ReceivedCity, 
		ReceivedState = @ReceivedState, 
		ReceivedZipCode = @ReceivedZipCode, 
		ReceivedPhoneNumber = @ReceivedPhoneNumber,
		ReceivedAt = GETDATE()
	WHERE	
		RegionId = @RegionId
		AND DemosId = @DemosId
	
RETURN 0