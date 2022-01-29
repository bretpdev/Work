CREATE PROCEDURE [dbo].[GetFormatNameFromCode]
	@FormatCode int
	
AS
	SELECT 
		CorrespondenceFormat
	FROM
		CorrespondenceFormats
	WHERE
		NTISCode = @FormatCode
RETURN 0
