CREATE PROCEDURE [Noble].GetContactCampaignId
(
	@RegionName VARCHAR(15),
	@Campaign VARCHAR(6)
)

AS
BEGIN

	SELECT CC.ContactCampaignId 
	FROM Noble.ContactCampaigns CC inner join Noble.Groups G on G.GroupId = CC.GroupId
	WHERE CC.CampaignCode = @Campaign and G.GroupName = @RegionName
END;