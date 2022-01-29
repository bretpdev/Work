
/********************************************************
*Routine Name	: [dbo].[spBANKODeleteErroredRecord]
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		08/06/2012	Jarom Ryan		This sp will deleted erroed records from BankoReceiveBankrupcyData
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spBANKODeleteErroredRecord]
	-- Add the parameters for the stored procedure here
	  @RecordNumber as bigint

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Delete
	From dbo.BankoReceiveResponseOutput
	Where RecordNumber = @RecordNumber
	

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spBANKODeleteErroredRecord] TO [db_executor]
    AS [dbo];



