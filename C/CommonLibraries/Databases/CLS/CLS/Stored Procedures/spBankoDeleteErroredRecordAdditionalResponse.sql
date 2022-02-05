

/********************************************************
*Routine Name	: [dbo].[spBankoDeleteErroredRecordAdditionalResponse]
*Purpose		: 
*Used by		: 
*Inputs			: 
*Returns		: 
*Test Code		: EXEC [dbo].[<Procedure_Name, sysname, spTable_ACTION_desc>] @<Param1, sysname, P1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, @<Param2, sysname, P2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
*Note			:  
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		<Date,datetime, Created Date>  <Author, nvarchar(30), Author>
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spBankoDeleteErroredRecordAdditionalResponse]
	-- Add the parameters for the stored procedure here
	  @RecordNumber as bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    Delete
    From dbo.BankoReceiveAdditionalEvents
    Where RecordNumber = @RecordNumber

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spBankoDeleteErroredRecordAdditionalResponse] TO [db_executor]
    AS [dbo];



