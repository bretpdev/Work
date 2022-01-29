
CREATE PROCEDURE [dbo].[GetMissingDocumentationReasons]
	
AS
	SELECT 
		REA.MissingDocumentationReason
	FROM
		MissingDocumentationReasons REA
RETURN 0