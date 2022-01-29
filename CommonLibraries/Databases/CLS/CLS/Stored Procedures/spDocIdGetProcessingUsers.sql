
CREATE PROCEDURE [dbo].[spDocIdGetProcessingUsers]
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
		GEN.UserName,
		GEN.DocId,
		COALESCE(PO.Total, 0) AS PostOffice,
		COALESCE(IH.Total, 0) AS InHouse,
		COALESCE(FA.Total, 0) AS Fax,
		COALESCE(OT.Total, 0) AS Other,
		COALESCE(TOT.Total, 0) AS Total
	FROM DocIdProcessed GEN
	LEFT OUTER JOIN (
		SELECT UserName, DocId, COUNT(DocId) AS Total
		FROM DocIdProcessed
		WHERE DATEDIFF(DD, DateTimeStamp, @LastProcessingDate) = 0
		AND Source = 'PO'
		GROUP BY UserName, DocId
	) PO
		ON PO.UserName = GEN.UserName AND PO.DocId = GEN.DocId
	LEFT OUTER JOIN (
		SELECT UserName, DocId, COUNT(DocId) AS Total
		FROM DocIdProcessed
		WHERE DATEDIFF(DD, DateTimeStamp, @LastProcessingDate) = 0
		AND Source = 'IH'
		GROUP BY UserName, DocId
	) IH
		ON IH.UserName = GEN.UserName AND IH.DocId = GEN.DocId
	LEFT OUTER JOIN (
		SELECT UserName, DocId, COUNT(DocId) AS Total
		FROM DocIdProcessed
		WHERE DATEDIFF(DD, DateTimeStamp, @LastProcessingDate) = 0
		AND Source = 'FA'
		GROUP BY UserName, DocId
	) FA
		ON FA.UserName = GEN.UserName AND FA.DocId = GEN.DocId
	LEFT OUTER JOIN (
		SELECT UserName, DocId, COUNT(DocId) AS Total
		FROM DocIdProcessed
		WHERE DATEDIFF(DD, DateTimeStamp, @LastProcessingDate) = 0
		AND Source = 'OT'
		GROUP BY UserName, DocId
	) OT
		ON OT.UserName = GEN.UserName AND OT.DocId = GEN.DocId
	LEFT OUTER JOIN (
		SELECT UserName, DocId, COUNT(DocId) AS Total
		FROM DocIdProcessed
		WHERE DATEDIFF(DD, DateTimeStamp, @LastProcessingDate) = 0
		GROUP BY UserName, DocId
	) TOT
		ON TOT.UserName = GEN.UserName AND TOT.DocId = GEN.DocId
	WHERE DATEDIFF(DD, DateTimeStamp, @LastProcessingDate) = 0
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDocIdGetProcessingUsers] TO [db_executor]
    AS [dbo];



