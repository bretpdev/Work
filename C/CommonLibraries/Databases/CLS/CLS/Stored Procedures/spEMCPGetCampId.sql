-- =============================================
-- Author:		Jarom Ryan
-- Create date: 12/05/2012
-- Description:	This will get the Campaign Id for a given campaign
-- =============================================
CREATE PROCEDURE [dbo].[spEMCPGetCampId] 
	-- Add the parameters for the stored procedure here
	@EmailSubject	As Varchar (200),
	@CornerStone	As bit,
	@IncludeAccountNumber As bit,
	@Arc			As Varchar(5),
	@CommentText	As Varchar (1238),
	@DataFile		As Varchar(1000),
	@HtmlFile		As Varchar(1000),
	@EmailFrom		As Varchar(200)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CampID
	From dbo.EMCP_DAT_EmailCampaigns
	Where EmailSubjectLine = @EmailSubject and CornerStone = @CornerStone and IncludeAccountNumber = @IncludeAccountNumber and ARC = @Arc and CommentText = @CommentText
	and DataFile = @DataFile and HTMLFile = @HtmlFile and EmailFrom = @EmailFrom
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spEMCPGetCampId] TO [db_executor]
    AS [dbo];



