CREATE PROCEDURE [idrxmldata].[GetExistingEAppXmlData]
	@Eapp char(10)
AS
	SELECT DISTINCT
		e_application_id AS EappId
	FROM
		[dbo].Applications 
	WHERE
		e_application_id = @Eapp