

/********************************************************
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		<Date,datetime, Created Date>  <Author, nvarchar(30), Author>
*1.0.1		
********************************************************/

CREATE PROCEDURE [dbo].[spSSRS_GetSchoolInfo]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		SchoolName,
		SchoolCode,
		BranchCode,
		ContactNameSchool,
		EmailAddressSchool,
		ContactName3rdParty,
		EmailAddress3rdParty,
		Recipient
	From dbo.SSRS_DAT_ContactInfo
	
	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSSRS_GetSchoolInfo] TO [db_executor]
    AS [dbo];



