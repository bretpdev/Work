CREATE PROCEDURE [dbo].[ArcAdd_GetAllArcs]
AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT
		ArcAddProcessingId,
		ArcTypeId,
		AccountNumber,
		RecipientId,
		ARC,
		ScriptId,
		Comment,
		IsReference,
		IsEndorser,
		ProcessFrom,
		ProcessTo,
		NeededBy,
		RegardsTo,
		RegardsCode
	FROM 
		ArcAddProcessing
	WHERE
		ProcessOn <= GETDATE()

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetAllArcs] TO [db_executor]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetAllArcs] TO [UHEAA\CornerStoneUsers]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetAllArcs] TO [UHEAA\SystemAnalysts]
    AS [dbo];



