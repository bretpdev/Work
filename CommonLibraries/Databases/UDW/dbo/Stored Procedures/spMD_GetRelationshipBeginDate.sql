CREATE PROCEDURE [dbo].[spMD_GetRelationshipBeginDate]
	@AccountNumber char(10)
AS
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	select getdate() as RelationshipBeginDate

RETURN 0