-- =============================================
-- Author:		Jarom Ryan	
-- Create date: 12/20/2013
-- Description:	Inserts a new record in the Batch Email Db
-- =============================================
CREATE PROCEDURE [dbo].[InsertNewBatchEmailCampaign] 

@BatchEmailId int,
@SasFile varchar(100),
@LetterId varchar(50),
@SendingAddress varchar(100),
@SubjectLine varchar(300),
@Arc varchar(5) = null,
@CommentText varchar(1000) = null,
@IncludeAcctNumber bit

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    INSERT INTO [dbo].[BatchEmail]([SASFile],[LetterId],[SendingAddress],[SubjectLine],[ARC],[CommentText],[IncludeAcctNumber])
	VALUES(@SasFile, @LetterId, @SendingAddress, @SubjectLine, @Arc, @CommentText, @IncludeAcctNumber)

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
	WHERE
		BatchEmailId = SCOPE_IDENTITY()

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertNewBatchEmailCampaign] TO [db_executor]
    AS [dbo];



