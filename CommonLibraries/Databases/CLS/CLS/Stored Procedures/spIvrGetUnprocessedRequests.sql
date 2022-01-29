-- =============================================
-- Author:		Jarom Ryan
-- Create date: 01/26/2012
-- Description:	Get all unprocessed records from IvrRequestTracking table
-- =============================================
CREATE PROCEDURE [dbo].[spIvrGetUnprocessedRequests]
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT RecNum,
	AccountNumber,
	Request,
	PaymentAmount
	FROM IvrRequestTracking
	WHERE ProcessedDate IS NULL
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrGetUnprocessedRequests] TO [db_executor]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spIvrGetUnprocessedRequests] TO [UHEAA\BatchScripts]
    AS [dbo];



