-- =============================================
-- Author:		Jarom Ryan
-- Create date: 02/21/2012
-- Description:	This sp will update the EMCP_DAT_EmailCampaigns table
-- =============================================
CREATE PROCEDURE [dbo].[spEMCPUpdateSpecialEmailCampaign] 
	@CampId				bigint,
	@EmailSubjectLine	Varchar(200),
	@CornerStone		As bit,
	@IncludeAccountNumber As bit,
	@ARC				Varchar(5),
	@CommentText		Text,
	@DataFile			Varchar(1000),
	@HtmlFile			Varchar(1000),
	@EmailFrom			Varchar(100)
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE EMCP_DAT_EmailCampaigns
	SET EmailSubjectLine = @EmailSubjectLine,
		CornerStone = @CornerStone,
		IncludeAccountNumber = @IncludeAccountNumber,
		ARC = @ARC,
		CommentText = @CommentText,
		DataFile = @DataFile,
		HTMLFile = @HtmlFile,
		EmailFrom = @EmailFrom
	WHERE CampID = @CampId
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spEMCPUpdateSpecialEmailCampaign] TO [db_executor]
    AS [dbo];



