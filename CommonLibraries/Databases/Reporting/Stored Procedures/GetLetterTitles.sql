CREATE PROCEDURE [dbo].[GetLetterTitles]
AS

SELECT
	Title
FROM
	BSYS.dbo.LTDB_DAT_Requests

RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetLetterTitles] TO [db_executor]
    AS [dbo];