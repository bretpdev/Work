CREATE PROCEDURE dbo.spLCTR_GetTopic

@TopicID			bigint

AS

SELECT A.[ID]
	,A.[Name]
	,COALESCE(A.Narrative,'') as Narrative
	, COALESCE(A.SearchKey,'')as SearchKey
	,'test' as test
	
FROM dbo.LCTR_DAT_Topic A 
WHERE A.[ID] = @TopicID
FOR XML AUTO, ELEMENTS