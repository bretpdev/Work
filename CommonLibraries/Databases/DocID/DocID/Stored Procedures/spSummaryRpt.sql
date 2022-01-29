CREATE PROCEDURE dbo.[spSummaryRpt] 

@TargetDt	datetime

 AS

SELECT DISTINCT PD.DocID, 
		DocIdCounts.DocCount as Docs, 
		COALESCE(POCounts.DocCount,0) as PODocs, 
		COALESCE(BUCounts.DocCount,0) as BUDocs, 
		COALESCE(CFCounts.DocCount,0) as CFDocs,
		COALESCE(OTCounts.DocCount,0) as OTDocs
FROM ProcessingData PD
JOIN (
		SELECT DocID, COUNT(DocID) as DocCount
		FROM ProcessingData
		WHERE DATEDIFF(d,TimeOfTransaction, @TargetDt) = 0
		GROUP BY DocID	
	) DocIdCounts ON PD.DocID = DocIdCounts.DocID
LEFT JOIN 	(
		SELECT DocID, COUNT(DocID) as DocCount
		FROM ProcessingData
		WHERE DATEDIFF(d,TimeOfTransaction, @TargetDt) = 0
		AND Source = 'PO'	
		GROUP BY DocID	
	) POCounts ON PD.DocID = POCounts.DocID
LEFT JOIN 	(
		SELECT DocID, COUNT(DocID) as DocCount
		FROM ProcessingData
		WHERE DATEDIFF(d,TimeOfTransaction, @TargetDt) = 0
		AND Source = 'BU'	
		GROUP BY DocID	
	) BUCounts ON PD.DocID = BUCounts.DocID
LEFT JOIN 	(
		SELECT DocID, COUNT(DocID) as DocCount
		FROM ProcessingData
		WHERE DATEDIFF(d,TimeOfTransaction, @TargetDt) = 0
		AND Source = 'CF'	
		GROUP BY DocID	
	) CFCounts ON PD.DocID = CFCounts.DocID
LEFT JOIN 	(
		SELECT DocID, COUNT(DocID) as DocCount
		FROM ProcessingData
		WHERE DATEDIFF(d,TimeOfTransaction, @TargetDt) = 0
		AND Source = 'OT'	
		GROUP BY DocID	
	) OTCounts ON PD.DocID = OTCounts.DocID
WHERE DATEDIFF(d,TimeOfTransaction, @TargetDt) = 0