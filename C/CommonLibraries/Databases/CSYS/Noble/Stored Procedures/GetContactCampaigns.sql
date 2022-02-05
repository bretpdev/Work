CREATE PROCEDURE [Noble].[GetContactCampaigns]
(
	@RegionName VARCHAR(20)
)
AS
BEGIN

	SELECT CC.CampaignCode 
	FROM Noble.ContactCampaigns CC
		INNER JOIN Noble.Groups G ON G.GroupId = CC.GroupId
	WHERE G.GroupName = @RegionName
END;
GO
GRANT EXECUTE
    ON OBJECT::[Noble].[GetContactCampaigns] TO [db_executor]
    AS [dbo];

