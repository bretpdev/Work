
CREATE PROCEDURE [dbo].[spDocIdGetProcessingSummary]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    DECLARE @LastProcessingDate DATETIME
    SET @LastProcessingDate= (SELECT MAX(DateTimeStamp) FROM DocIdProcessed)
    
	SELECT DISTINCT
		DATEADD(DD, 0, DATEDIFF(DD, 0, GEN.DateTimeStamp)) AS 'Date',
		GEN.DocId,
		COALESCE(PO.Total, 0) AS PostOffice,
		COALESCE(IH.Total, 0) AS InHouse,
		COALESCE(FA.Total, 0) AS Fax,
		COALESCE(OT.Total, 0) AS Other,
		COALESCE(TOT.Total, 0) AS Total
	FROM DocIdProcessed GEN
	LEFT OUTER JOIN (
		SELECT DocId, COUNT(DocId) AS Total
		FROM DocIdProcessed
		WHERE DATEDIFF(DD, DateTimeStamp, @LastProcessingDate) = 0
		AND Source = 'PO'
		GROUP BY DocId
	) PO
		ON PO.DocId = GEN.DocId
	LEFT OUTER JOIN (
		SELECT DocId, COUNT(DocId) AS Total
		FROM DocIdProcessed
		WHERE DATEDIFF(DD, DateTimeStamp, @LastProcessingDate) = 0
		AND Source = 'IH'
		GROUP BY DocId
	) IH
		ON IH.DocId = GEN.DocId
	LEFT OUTER JOIN (
		SELECT DocId, COUNT(DocId) AS Total
		FROM DocIdProcessed
		WHERE DATEDIFF(DD, DateTimeStamp, @LastProcessingDate) = 0
		AND Source = 'FA'
		GROUP BY DocId
	) FA
		ON FA.DocId = GEN.DocId
	LEFT OUTER JOIN (
		SELECT DocId, COUNT(DocId) AS Total
		FROM DocIdProcessed
		WHERE DATEDIFF(DD, DateTimeStamp, @LastProcessingDate) = 0
		AND Source = 'OT'
		GROUP BY DocId
	) OT
		ON OT.DocId = GEN.DocId
	LEFT OUTER JOIN (
		SELECT DocId, COUNT(DocId) AS Total
		FROM DocIdProcessed
		WHERE DATEDIFF(DD, DateTimeStamp, @LastProcessingDate) = 0
		GROUP BY DocId
	) TOT
		ON TOT.DocId = GEN.DocId
	WHERE DATEDIFF(DD, DateTimeStamp, @LastProcessingDate) = 0
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDocIdGetProcessingSummary] TO [db_executor]
    AS [dbo];



