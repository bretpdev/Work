


/********************************************************

*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		09/04/2012	Jarom Ryan		Will gather letter data from dbo.DTX7LDueDilLetters
		
********************************************************/

CREATE PROCEDURE [dbo].[spDTX7LGetDueDilLetterData]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		 LetterId,
		 Arc
	FROM dbo.DTX7LDueDilLetters
	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDTX7LGetDueDilLetterData] TO [db_executor]
    AS [dbo];



