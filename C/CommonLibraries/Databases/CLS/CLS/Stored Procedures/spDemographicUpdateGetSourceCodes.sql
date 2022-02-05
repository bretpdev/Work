-- =============================================
-- Author:		Bret Pehrson
-- Create date: 10/9/2013
-- Description:	Returns a list of source code names
-- =============================================
CREATE PROCEDURE spDemographicUpdateGetSourceCodes
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		SourceName
	FROM
		DemographicUpdateSystemCodes

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDemographicUpdateGetSourceCodes] TO [db_executor]
    AS [dbo];



