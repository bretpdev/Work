

/********************************************************
*Routine Name	: [dbo].spGetDueDilLetters
*Purpose		: Will get all of the letter 
*Used by		: DTX7L
********************************************************/

Create PROCEDURE [dbo].[spDTX7LGetDueDilLetters]

AS
BEGIN

	SET NOCOUNT ON;


	SELECT 
		 LetterId,
		 Arc
	FROM DTX7LDueDilLetters

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDTX7LGetDueDilLetters] TO [UHEAA\Developers]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDTX7LGetDueDilLetters] TO [UHEAA\SystemAnalysts]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDTX7LGetDueDilLetters] TO [db_executor]
    AS [dbo];

