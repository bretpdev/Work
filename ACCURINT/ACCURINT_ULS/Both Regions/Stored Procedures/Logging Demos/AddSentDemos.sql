CREATE PROCEDURE [accurint].[AddSentDemos]
	@DemosId INT,
	@Region VARCHAR(10),
	@AccountNumber CHAR(10),
	@SentAddress1 VARCHAR(45), 
	@SentAddress2 VARCHAR(30), 
	@SentCity VARCHAR(20), 
	@SentState VARCHAR(2), 
	@SentZipCode VARCHAR(17), 
	@SentPhoneNumber VARCHAR(27),
	@SentValidity BIT
AS
	
	DECLARE @RegionId INT = (SELECT RegionId FROM accurint.Regions WHERE Region = @Region)

	BEGIN
		INSERT INTO accurint.DemoLogs(RegionId, DemosId, AccountNumber, SentAddress1, SentAddress2, SentCity, SentState, SentZipCode, SentPhoneNumber, SentValidity, SentAt)
		VALUES(@RegionId, @DemosId, @AccountNumber, @SentAddress1, @SentAddress2, @SentCity, @SentState, @SentZipCode, @SentPhoneNumber, @SentValidity, GETDATE())
	END

RETURN 0
