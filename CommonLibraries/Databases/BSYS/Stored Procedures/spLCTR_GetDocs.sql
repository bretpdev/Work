CREATE PROCEDURE dbo.spLCTR_GetDocs

@ID			bigint,
@Type				char(10)/*vchar(6)*/

AS

SELECT A.[ID]
	,A.[Name]
	, COALESCE(A.SearchKey,'')as SearchKey
	, COALESCE(A.Path,'')as Path
	,'test' as test
	
FROM dbo.LCTR_DAT_Docs A 
WHERE A.[ID] = @ID
AND A.Type = @Type
FOR XML AUTO, ELEMENTS