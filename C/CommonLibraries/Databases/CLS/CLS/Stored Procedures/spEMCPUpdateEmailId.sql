-- =============================================
-- Author:		Jarom Ryan
-- Create date: 02/21/2012
-- Description:	This sp will update the EMCP_DAT_SPEMLIDs table with
-- the borrowers account number and email address and create an email ID
-- =============================================
CREATE PROCEDURE [dbo].[spEMCPUpdateEmailId] 
	-- Add the parameters for the stored procedure here

	@AccountNumber	varchar(10),
	@CampaignId		int,
	@EmailAddress	varchar(500)
	
AS
BEGIN

	SET NOCOUNT ON;

   INSERT INTO dbo.EMCP_DAT_SPEMLIDs (AccountNumber, EmailCampignID, EmailAddress)
   VALUES (@AccountNumber, @CampaignId, @EmailAddress)
   
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spEMCPUpdateEmailId] TO [db_executor]
    AS [dbo];



