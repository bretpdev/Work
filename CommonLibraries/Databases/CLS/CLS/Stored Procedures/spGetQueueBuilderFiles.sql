-- =============================================
-- Author:		Daren Beattie
-- Create date: October 13, 2011
-- Description:	Retrieves the contents of the QueueBuilderFile table.
-- =============================================
CREATE PROCEDURE spGetQueueBuilderFiles
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		[FileName],
		EmptyFileIsOk,
		MissingFileIsOk,
		ProcessMultipleFiles
	FROM QueueBuilderFile
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetQueueBuilderFiles] TO [db_executor]
    AS [dbo];



