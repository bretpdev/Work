CREATE PROCEDURE [dbo].[GetArguments]
AS
	SELECT
		ArgumentId,
		Argument,
		ArgumentDescription
	FROM
		Arguments
RETURN 0

GRANT EXECUTE ON GetArguments TO db_executor