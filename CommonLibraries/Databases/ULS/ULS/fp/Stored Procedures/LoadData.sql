CREATE PROCEDURE [fp].[LoadData]
( 
	@ScriptFileId int,
	@SourceFile varchar(100),
	@CreatedBy varchar(50)
)
AS
BEGIN
	DECLARE @AccountNumberLocation INT
	SET @AccountNumberLocation = (SELECT AccountNumberLocation FROM [fp].ScriptFiles WHERE ScriptFileId = @ScriptFileId)

	--Insert data into [FileProcessing]
	INSERT INTO [fp].[FileProcessing]
	(
		GroupKey,
		ScriptFileId,
		SourceFile,
		CreatedBy
	)
	SELECT
		dbo.SplitAndRemoveQuotes(LineData, ',', @AccountNumberLocation, 1) AS GroupKey, --Key Identifier
		@ScriptFileId,
		@SourceFile,
		@CreatedBy
	FROM
		[fp]._BulkLoad
	GROUP BY
		dbo.SplitAndRemoveQuotes(LineData, ',', @AccountNumberLocation, 1)

	DECLARE @_BulkLoad TABLE
	(
		GroupKey VARCHAR(50),
		LineData VARCHAR(MAX)
	)


	INSERT INTO @_BulkLoad
	(
		GroupKey,
		LineData
	)
	SELECT
		dbo.SplitAndRemoveQuotes(BL.LineData, ',', @AccountNumberLocation, 1) AS GroupKey, --Key Identifier
		BL.LineData
	FROM
		[fp]._BulkLoad BL


	INSERT INTO [fp].LineData
	(
		FileProcessingId,
		LineData
	)
	SELECT
		LD.FileProcessingId,
		LD.LineData
	FROM
		(
			SELECT
				FP.FileProcessingId,
				BL.LineData,
				RANK() OVER (PARTITION BY BL.GroupKey ORDER BY FP.FileProcessingId, BL.LineData) [rnk1],
				RANK() OVER (PARTITION BY FP.FileProcessingId ORDER BY FP.FileProcessingId, BL.LineData) [rnk2]
			FROM
				@_BulkLoad BL
				INNER JOIN [fp].FileProcessing FP on FP.GroupKey = BL.GroupKey and FP.SourceFile = @SourceFile
		) LD
	WHERE
		LD.rnk1 = LD.rnk2
END

DELETE FROM [fp]._BulkLoad
;

RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[fp].[LoadData] TO [db_executor]
    AS [UHEAA\Developers];

