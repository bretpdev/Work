CREATE PROCEDURE [fp].[GetLineDataForAccount]
	@FileProcessingId int
AS
	SELECT
		LD.LineData
	FROM
		[fp].LineData LD
		INNER JOIN [fp].FileProcessing FP
			ON LD.FileProcessingId = FP.FileProcessingId
	WHERE
		FP.FileProcessingId = @FileProcessingId
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[fp].[GetLineDataForAccount] TO [db_executor]
    AS [dbo];

