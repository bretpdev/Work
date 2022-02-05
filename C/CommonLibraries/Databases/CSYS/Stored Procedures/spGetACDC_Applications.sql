-- =============================================
-- Author:		Bret Pehrson
-- Create date: 02/13/2013
-- Description:	Gets a list of all the applications and their associated files
-- =============================================
CREATE PROCEDURE [dbo].[spGetACDC_Applications] 

AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT
		application_id,
		application_name,
		access_key,
		starting_class,
		starting_file_id
	FROM
		APPLICATIONS
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetACDC_Applications] TO [db_executor]
    AS [dbo];

