

/********************************************************
*Routine Name	: [dbo].spGetExpiredLetters
*Purpose		: Will get all of the letter id and Arcs
*Used by		: DTX7L
********************************************************/

CREATE PROCEDURE [dbo].[spDTX7LGetExpiredLetters]

AS
BEGIN

	SET NOCOUNT ON;


	SELECT 
		 LetterId,
		 Arc
	FROM DTX7LExpiredLetters

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDTX7LGetExpiredLetters] TO [UHEAA\Developers]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDTX7LGetExpiredLetters] TO [UHEAA\SystemAnalysts]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDTX7LGetExpiredLetters] TO [db_executor]
    AS [dbo];

