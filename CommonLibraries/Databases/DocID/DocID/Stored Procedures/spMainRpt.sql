CREATE PROCEDURE dbo.spMainRpt 

@TargetDt	datetime

 AS

SELECT DISTINCT PD.UserName,
		PD.DocID, 
		DocIdCounts.DocCount as Docs, 
		COALESCE(POCounts.DocCount,0) as PODocs, 
		COALESCE(BUCounts.DocCount,0) as BUDocs, 
		COALESCE(CFCounts.DocCount,0) as CFDocs,
		COALESCE(OTCounts.DocCount,0) as OTDocs
FROM ProcessingData PD
JOIN (
		SELECT DocID, UserName, COUNT(DocID) as DocCount
		FROM ProcessingData
		WHERE DATEDIFF(d,TimeOfTransaction, @TargetDt) = 0
		GROUP BY DocID, UserName	
	) DocIdCounts ON PD.DocID = DocIdCounts.DocID AND PD.UserName = DocIdCounts.UserName
LEFT JOIN 	(
		SELECT DocID, UserName, COUNT(DocID) as DocCount
		FROM ProcessingData
		WHERE DATEDIFF(d,TimeOfTransaction, @TargetDt) = 0
		AND Source = 'PO'	
		GROUP BY DocID, UserName	
	) POCounts ON PD.DocID = POCounts.DocID AND PD.UserName = POCounts.UserName
LEFT JOIN 	(
		SELECT DocID, UserName, COUNT(DocID) as DocCount
		FROM ProcessingData
		WHERE DATEDIFF(d,TimeOfTransaction, @TargetDt) = 0
		AND Source = 'BU'	
		GROUP BY DocID, UserName
	) BUCounts ON PD.DocID = BUCounts.DocID AND PD.UserName = BUCounts.UserName
LEFT JOIN 	(
		SELECT DocID, UserName, COUNT(DocID) as DocCount
		FROM ProcessingData
		WHERE DATEDIFF(d,TimeOfTransaction, @TargetDt) = 0
		AND Source = 'CF'	 
		GROUP BY DocID, UserName	
	) CFCounts ON PD.DocID = CFCounts.DocID AND PD.UserName = CFCounts.UserName
LEFT JOIN 	(
		SELECT DocID, UserName, COUNT(DocID) as DocCount
		FROM ProcessingData
		WHERE DATEDIFF(d,TimeOfTransaction, @TargetDt) = 0
		AND Source = 'OT'	
		GROUP BY DocID, UserName	
	) OTCounts ON PD.DocID = OTCounts.DocID AND PD.UserName = OTCounts.UserName
WHERE DATEDIFF(d,TimeOfTransaction,@TargetDt) = 0