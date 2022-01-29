CREATE PROCEDURE [dbo].[GetActiveStatusCodes]

AS
	SELECT 
		StatusCodeId,
		StatusCode,
		StatusCodeName
	FROM	
		StatusCodes
	WHERE
		DeletedAt IS NULL
RETURN 0
