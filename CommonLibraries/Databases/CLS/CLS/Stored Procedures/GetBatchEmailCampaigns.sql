
-- =============================================
-- Author:		<Jarom Ryan>
-- Create date: <12/21/2011>
-- Description:	<Gets all data from BatchEmail>
-- =============================================
CREATE PROCEDURE [dbo].[GetBatchEmailCampaigns]

AS
BEGIN

	SELECT 
		BatchEmailId,
		SASFile,
		LetterId,
		SendingAddress,
		SubjectLine,
		ARC,
		CommentText,
		IncludeAcctNumber
	FROM 
		BatchEmail
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBatchEmailCampaigns] TO [db_executor]
    AS [dbo];



