-- =============================================
-- Author:		Jarom Ryan
-- Create date: 12/20/2013
-- Description:	This Sproc will update an existing Batch Email record
-- =============================================
CREATE PROCEDURE [dbo].[UpdateBatchEmail] 

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

	SET NOCOUNT ON;


	UPDATE
		[dbo].[BatchEmail]
	SET
		[SASFile] = @SasFile,
		[LetterId] = @LetterId,
		[SendingAddress] = @SendingAddress,
		[SubjectLine] = @SubjectLine,
		[ARC] = @Arc,
		[CommentText] = @CommentText,
		[IncludeAcctNumber] = @IncludeAcctNumber
	WHERE
		[BatchEmailId] = @BatchEmailId

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateBatchEmail] TO [db_executor]
    AS [dbo];



