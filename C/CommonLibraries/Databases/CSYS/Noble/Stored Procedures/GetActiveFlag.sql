CREATE PROCEDURE [Noble].[GetActiveFlag]
(
	@Campaign VARCHAR(6),
	@Group VARCHAR(15)
)

AS
BEGIN

	SELECT CC.status 
	FROM Noble.ContactCampaigns CC inner join Noble.Groups G on G.GroupId = CC.GroupId
	WHERE CC.CampaignCode = @Campaign and G.GroupName = @Group
END;
GO
GRANT EXECUTE
    ON OBJECT::[Noble].[GetActiveFlag] TO [db_executor]
    AS [dbo];

