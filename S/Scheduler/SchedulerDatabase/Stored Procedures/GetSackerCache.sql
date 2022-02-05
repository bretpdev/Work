CREATE PROCEDURE [dbo].[GetSackerCache]
AS

	SELECT
		[SackerCacheId], 
		[RequestTypeId], 
		[Name], 
		[Id],
		[Status], 
		[Priority], 
		[Court],
		[AssignedProgrammer],
		[AssignedTester],
		[DevEstimate], 
		[TestEstimate]
	FROM
		SackerCache

RETURN 0
