-- =============================================
-- Author:		Jarom Ryan
-- Create date: 11/16/2012
-- Description:	this sp will save new Special Email Campaigns to EMCP_DAT_EmailCampaigns table
-- =============================================
CREATE PROCEDURE [dbo].[spEMCPSaveSpecialEmailCampaign] 
	
	@EmailSubjectLine	Varchar(200),
	@CornerStone		bit,
	@IncludeAccountNumber As bit,
	@ARC				Varchar(5),
	@CommentText		Varchar (1236),
	@DataFile			Varchar(1000),
	@HtmlFile			Varchar(1000),
	@EmailFrom			Varchar(100),
	@DateSetup			DateTime = NULL
AS
BEGIN

	SET NOCOUNT ON;
	
	if @DateSetup is null
	Set @DateSetup = GETDATE()
	
	if((SELECT CampID 
	From dbo.EMCP_DAT_EmailCampaigns
	Where EmailSubjectLine = @EmailSubjectLine And CornerStone = @CornerStone And IncludeAccountNumber = @IncludeAccountNumber And ARC = @ARC And CommentText = @CommentText And DataFile = @DataFile
	And HTMLFile = @HtmlFile And EmailFrom = @EmailFrom) is null)
	BEGIN
		INSERT INTO CLS.dbo.EMCP_DAT_EmailCampaigns (EmailSubjectLine, CornerStone, IncludeAccountNumber, ARC, CommentText, DataFile, HTMLFile, EmailFrom, DateSetup)
		VALUES( @EmailSubjectLine, @CornerStone, @IncludeAccountNumber, @ARC, @CommentText, @DataFile, @HtmlFile,@EmailFrom, @DateSetup)
		
	END
	ELSE
	Return 0

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spEMCPSaveSpecialEmailCampaign] TO [db_executor]
    AS [dbo];



