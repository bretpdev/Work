CREATE PROCEDURE dbo.spLCTR_GetRiskPolicy

@TopicID			bigint,
@Type				char(10)/*vchar(6)*/

AS

IF (@Type = 'Topic')
	BEGIN
		SELECT B.[ID]
			,COALESCE(A.Risk,'None') as Risk
			,COALESCE(C.Policy,'None') as Policy
			,COALESCE(D.[Description],'None') as [Description]
			,'test' as test
			
		FROM dbo.LCTR_DAT_Topic B
		left outer join dbo.LCTR_DAT_Risk A 
			on A.ID = B.ID
			and A.Type = @Type
		left outer join dbo.LCTR_DAT_Policy C
			on C.ID = B.ID
			and C.Type = @Type
		left outer join dbo.LCTR_DAT_FlowChart D
			on D.ID = B.ID
			and D.Type = @Type
		WHERE B.[ID] = @TopicID
		FOR XML AUTO, ELEMENTS
	END
ELSE
	BEGIN
		SELECT B.[ID]
			,COALESCE(A.Risk,'None') as Risk
			,COALESCE(C.Policy,'None') as Policy
			,COALESCE(D.[Description],'None') as [Description]
			,'test' as test
			
		FROM dbo.LCTR_DAT_Procedures B
		left outer join dbo.LCTR_DAT_Risk A 
			on A.ID = B.ID
			and A.Type = @Type
		left outer join dbo.LCTR_DAT_Policy C
			on C.ID = B.ID
			and C.Type = @Type
		left outer join dbo.LCTR_DAT_FlowChart D
			on D.ID = B.ID
			and D.Type = @Type
		WHERE B.[ID] = @TopicID
		FOR XML AUTO, ELEMENTS
	END