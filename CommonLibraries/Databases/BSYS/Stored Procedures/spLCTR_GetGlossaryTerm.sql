CREATE PROCEDURE dbo.spLCTR_GetGlossaryTerm

@TermID			bigint

AS

SELECT A.[ID]
	,A.Term
	,COALESCE(A.Definition,'') as Definition
	,'test' as test
	
FROM dbo.LCTR_DAT_Glossary A 
WHERE A.[ID] = @TermID
FOR XML AUTO, ELEMENTS