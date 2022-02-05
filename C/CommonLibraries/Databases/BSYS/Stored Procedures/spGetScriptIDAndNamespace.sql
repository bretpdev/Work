

/********************************************************
*Routine Name	: [dbo].[<Procedure_Name, sysname, spTable_ACTION_desc>]
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

CREATE PROCEDURE [dbo].[spGetScriptIDAndNamespace]

	  @ScriptName Varchar(50)

AS
BEGIN

	SET NOCOUNT ON;


	SELECT 
	ScriptId,
	StartingNamespaceAndClass
	from dbo.DLLS_DAT_CSharpScript
	where ScriptName = @ScriptName

	SET NOCOUNT OFF;
END