-- =============================================
-- Author:		Bret Pehrson
-- Create date: 08/22/2012
-- Description:	Returns the list of Splunk Event IDs
-- =============================================
CREATE PROCEDURE spSYSA_GetSplunkEventIDs 
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT EventID FROM SYSA_LST_SplunkEventID
	
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_GetSplunkEventIDs] TO [db_executor]
    AS [dbo];

