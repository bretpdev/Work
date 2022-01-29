CREATE PROCEDURE [dbo].[GetExistingEAppXmlData]
	@Eapp char(10)
AS
	SELECT DISTINCT
		e_application_id AS EappId
	FROM
		Applications 
	WHERE
		e_application_id = @Eapp
		
RETURN 0