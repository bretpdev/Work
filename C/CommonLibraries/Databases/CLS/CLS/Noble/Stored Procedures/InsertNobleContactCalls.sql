CREATE PROCEDURE [Noble].[InsertNobleContactCalls]
(
	@RegionName VARCHAR(20),
	@Telephone VARCHAR(10),
	@Category int, 
	@ListId VARCHAR(10),
	@Campaign VARCHAR(6),
	@CampaignId int,
	@AgentCode VARCHAR(7),
	@Disposition VARCHAR(3),
	@AgentDisposition VARCHAR(3),
	@CallDate date,
	@CallTime VARCHAR(20),
	@TimeConnected VARCHAR(8),
	@TimeHold VARCHAR(8),
	@TimeACW VARCHAR(8),
	@Filler1 VARCHAR(20),
	@SSN VARCHAR(9),
	@AccountNumber VARCHAR(10),
	@Filler2 VARCHAR(10),
	@AgentCode2 VARCHAR(50),
	@AgentName VARCHAR(100)
)
AS
BEGIN
	INSERT INTO Noble.CornerStoneContactCalls(AccountNumber, SSN, Telephone, Category, ListId, ContactCampaignId, AgentCode, AgentCode2, AgentName, Status, AddiStatus, CallDate, CallTime, TimeConnected, TimeACW, TimeHold, Filler1, Filler2)
	VALUES(@AccountNumber,@SSN,@Telephone,@Category,@ListId,@CampaignId,@AgentCode,@AgentCode2,@AgentName,@Disposition,@AgentDisposition,@CallDate,@CallTime,@TimeConnected,@TimeACW,@TimeHold,@Filler1,@Filler2)
END;
GO
GRANT EXECUTE
    ON OBJECT::[Noble].[InsertNobleContactCalls] TO [db_executor]
    AS [dbo];

