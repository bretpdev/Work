CREATE PROCEDURE [Noble].[UpdateCampaign]
(
	@Group VARCHAR(15),
	@Campaign VARCHAR(6),
	@Description VARCHAR(100),
	@CallType int,
	@Status int,
	@Invalidate int,
	@User VARCHAR(75)
)

AS
BEGIN

	UPDATE CC SET 
	CC.CampaignName = @Description,
	CC.CallType = @CallType,
	CC.Status = @Status,
	CC.Invalidate = @Invalidate,
	CC.ModifiedAt = getdate(),
	CC.ModifiedBy = @User
	FROM Noble.ContactCampaigns CC 
		INNER JOIN Noble.Groups G ON CC.GroupId = G.GroupId
	WHERE G.GroupName = @Group AND CC.CampaignCode = @Campaign
END;
GO
GRANT EXECUTE
    ON OBJECT::[Noble].[UpdateCampaign] TO [db_executor]
    AS [dbo];

