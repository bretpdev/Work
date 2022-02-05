CREATE PROCEDURE [dbo].[spMD_GetRelationshipBeginDate]
	@AccountNumber char(10)
AS
	
	select getdate() as RelationshipBeginDate

RETURN 0