
CREATE PROCEDURE spIvrGetUnprocessedRequests

AS
BEGIN
	SET NOCOUNT ON;
	SELECT RecNum, AccountNumber, Request FROM IvrRequestTracking WHERE ProcessedDate IS NULL
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrGetUnprocessedRequests] TO [UHEAA\UHEAAUsers]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrGetUnprocessedRequests] TO [db_executor]
    AS [dbo];

