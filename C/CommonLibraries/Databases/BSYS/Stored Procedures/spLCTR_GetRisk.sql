CREATE PROCEDURE dbo.spLCTR_GetRisk

@TopicID			bigint,
@Type				char(10)/*vchar(6)*/

AS

SELECT B.[ID]
	,COALESCE(A.Risk,'None') as Risk
	,'test' as test
	
FROM dbo.LCTR_DAT_Topic B
left outer join dbo.LCTR_DAT_Risk A 
	on A.ID = B.ID
	and A.Type = @Type
WHERE B.[ID] = @TopicID

FOR XML AUTO, ELEMENTS