-- =============================================
-- Author:		Jarom Ryan
-- Create date: 08/19/2013
-- Description:	Updates the Spouse id of a given app
-- =============================================
CREATE PROCEDURE [dbo].[spUpdateSpouseId] 

@SpouseId Int,
@AppId int

AS
BEGIN

	SET NOCOUNT ON;


	UPDATE 
		dbo.Applications
	SET
		spouse_id = @SpouseId
	WHERE application_id = @AppId

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spUpdateSpouseId] TO [db_executor]
    AS [dbo];

