CREATE PROCEDURE [dbo].[GetCorrespondenceFormats]
	
AS
	SELECT 
		CorrespondenceFormatId,
		CorrespondenceFormat
	FROM
		CorrespondenceFormats
RETURN 0
