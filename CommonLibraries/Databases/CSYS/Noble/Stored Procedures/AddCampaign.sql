CREATE PROCEDURE [Noble].AddCampaign
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

	INSERT INTO Noble.ContactCampaigns (CampaignCode, CampaignName, GroupId, Status, Invalidate, CallType, ModifiedAt, ModifiedBy)
	SELECT
	@Campaign,
	@Description,
	G.GroupId,
	@Status,
	@Invalidate,
	@CallType,
	getdate(),
	@User
	FROM Noble.Groups G
	WHERE G.GroupName = @Group 

END;
GO
GRANT EXECUTE
    ON OBJECT::[Noble].[AddCampaign] TO [db_executor]
    AS [dbo];

