-- =============================================
-- Author:		Jarom Ryan
-- Create date: 02/21/2012
-- Description:	This sp will get the EmailId based upon the Account Number, Email Campaign ID
-- and email address 
-- =============================================
CREATE PROCEDURE [dbo].[spEMCPGetEmailId]
	-- Add the parameters for the stored procedure here
	@AccountNumber		varchar(10),
	@EmailCampaignId	int,
	@EmailAddress		varchar(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT EmailId
	FROM dbo.EMCP_DAT_SPEMLIDs
	WHERE AccountNumber = @AccountNumber And EmailCampignId = @EmailCampaignId And EmailAddress = @EmailAddress
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spEMCPGetEmailId] TO [db_executor]
    AS [dbo];



