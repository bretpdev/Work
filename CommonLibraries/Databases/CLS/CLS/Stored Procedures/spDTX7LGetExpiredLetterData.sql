


/********************************************************

*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		09/04/2012	Jarom Ryan		Will gather letter data from dbo.DTX7LExpiredLetters
		
********************************************************/

CREATE PROCEDURE [dbo].[spDTX7LGetExpiredLetterData]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		 LetterId,
		 Arc
	FROM dbo.DTX7LExpiredLetters
	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDTX7LGetExpiredLetterData] TO [db_executor]
    AS [dbo];



