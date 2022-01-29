CREATE PROCEDURE spDocIdGetDocumentDetails
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		DocId,
		[Description],
		Arc
	FROM DocIdDocument
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDocIdGetDocumentDetails] TO [db_executor]
    AS [dbo];



