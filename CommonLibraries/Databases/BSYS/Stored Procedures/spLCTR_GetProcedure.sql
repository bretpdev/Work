CREATE PROCEDURE dbo.spLCTR_GetProcedure

@pID			bigint

AS

SELECT A.[ID]
	, COALESCE(A.SearchKey,'')as SearchKey
	,'test' as test
	
FROM dbo.LCTR_DAT_Procedures A 
WHERE A.[ID] = @pID
FOR XML AUTO, ELEMENTS