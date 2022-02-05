CREATE PROCEDURE [dbo].[GetLetterRequestNumbers]

AS

SELECT
	Request
FROM
	BSYS.dbo.LTDB_DAT_Requests
	
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetLetterRequestNumbers] TO [db_executor]
    AS [dbo];

