-- =============================================
-- Author:		Jarom Ryan
-- Create date: 02/17/2012
-- Description:	This sp will get the CampId, EmailSubject, Arc, CommentText, 
-- DataFile, HTMLfile, and the FromEmail based upon the CampId sent in
-- =============================================
CREATE PROCEDURE [dbo].[spEMCPGetCampaignData] 
	
	@CampID Int 

AS
BEGIN

	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CampID
	  ,CornerStone
	  ,IncludeAccountNumber
      ,EmailSubjectLine
      ,ARC
      ,CommentText
      ,DataFile
      ,HTMLFile
      ,EmailFrom
      FROM EMCP_DAT_EmailCampaigns
      WHERE CampID = @CampID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spEMCPGetCampaignData] TO [db_executor]
    AS [dbo];



