-- =============================================
-- Author:		Jarom Ryan
-- Create date: 02/15/2012
-- Description:	This sp will get all email campaigns that do not have a date complete
-- =============================================
CREATE PROCEDURE [dbo].[spEMCPGetEmailCampaigns]

AS
BEGIN
	
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [CampID]
      ,[EmailSubjectLine]
      ,[DataFile]
      ,[HTMLFile]
      FROM EMCP_DAT_EmailCampaigns
      WHERE [DateComplete] IS NULL 
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spEMCPGetEmailCampaigns] TO [db_executor]
    AS [dbo];



