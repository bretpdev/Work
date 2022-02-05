CREATE PROCEDURE [Noble].[GetInvalidateFlag]
(
	@Campaign VARCHAR(6),
	@Group VARCHAR(15)
)

AS
BEGIN

	SELECT CC.Invalidate 
	FROM Noble.ContactCampaigns CC inner join Noble.Groups G on G.GroupId = CC.GroupId
	WHERE CC.CampaignCode = @Campaign and G.GroupName = @Group
END;
GO
GRANT EXECUTE
    ON OBJECT::[Noble].[GetInvalidateFlag] TO [db_executor]
    AS [dbo];

