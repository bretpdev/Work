CREATE PROCEDURE [emailcampaigns].[CampaignDataGetAll]
AS

	select CampaignDataId, SasFile, HtmlFile, SendingAddress, SubjectLine, Arc, ActionCode, CommentText
	  from emailcampaigns.CampaignData

RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[emailcampaigns].[CampaignDataGetAll] TO [UHEAA\UHEAAUsers]
    AS [dbo];

