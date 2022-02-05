

/********************************************************
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		09/19/2012	Jarom Ryan		will delete records from the BANKOReponse table after it has been processed
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spBANKODeleteProcessedRecord]

@RecordNumber as BigInt

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	Delete
	From BankoReceiveResponseOutput
	Where RecordNumber = @RecordNumber

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spBANKODeleteProcessedRecord] TO [db_executor]
    AS [dbo];



