CREATE PROCEDURE [dbo].[GetDocDetailIdFromLetter]
	@Letter VARCHAR(250)
AS
	SELECT
		dd.DocumentDetailsId
	FROM
		DocumentDetails DD
	WHERE 
		DD.[Path] LIKE '%' + @Letter
RETURN 0
