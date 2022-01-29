-- =============================================
-- Author:		Jarom Ryan
-- Create date: 01/30/2013
-- Description:	Will pull all data from the PhoneConsentUpdate table
-- =============================================
CREATE PROCEDURE [dbo].[spGetPhoneConsentQueues] 

AS
BEGIN

	SET NOCOUNT ON;

	Select
		[Queue],
		[SubQueue],
		[Arc],
		[Comment],
		[SourceCode]
	From dbo.PhoneConsentUpdate
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetPhoneConsentQueues] TO [db_executor]
    AS [dbo];



