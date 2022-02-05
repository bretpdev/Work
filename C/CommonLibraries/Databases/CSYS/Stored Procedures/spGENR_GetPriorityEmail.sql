-- =============================================
-- Author:		Bret Pehrson
-- Create date: 08/27/2012
-- Description:	Returns the email address and priority number for every user in the table
-- =============================================
CREATE PROCEDURE [dbo].[spGENR_GetPriorityEmail] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT b.EMail AS [EmailAddress]
		, a.Priority
	FROM GENR_LST_PriorityEmailRecipient a
	JOIN SYSA_DAT_Users b
	on a.SqlUserID = b.SqlUserId
	
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetPriorityEmail] TO [db_executor]
    AS [dbo];

