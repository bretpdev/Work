USE ULS
GO

ALTER TABLE ULS.fp.ScriptFiles
	ADD BorrowerAccountNumberLocation INT NOT NULL DEFAULT(0)
GO

INSERT INTO ULS.fp.ScriptFiles(ScriptID, SourceFile, LetterId, DocId, ArcId, FileHeaderId, AddedBy, AddedAt, Active, ProcessAllFiles, BorrowerAccountNumberLocation)
VALUES('OPSACTCMAA', 'webpayut', null, null, null, null, USER_NAME(), GETDATE(), 1, 1, 3)
GO

ALTER TABLE ULS.fp.FileProcessing
	ADD DeletedAt DATETIME NULL,
	DeletedBy VARCHAR(50) NULL

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [fp].[LoadData]
( 
	@ScriptFileId int,
	@SourceFile varchar(100),
	@CreatedBy varchar(50)
)
AS
	DECLARE 
		@ERROR INT = 0,
		@LineDataRowCount INT = 0,
		@ErrorMessage VARCHAR(300),
		@BulkLoadCount INT = 0,
		@BorrowerAccountNumberLocation INT

BEGIN TRANSACTION

SET @BorrowerAccountNumberLocation = (SELECT BorrowerAccountNumberLocation FROM [fp].ScriptFiles WHERE ScriptFileId = @ScriptFileId)

--Insert data into [FileProcessing]
INSERT INTO [fp].[FileProcessing]
(
	GroupKey,
	ScriptFileId,
	SourceFile,
	CreatedBy
)
SELECT
	dbo.SplitAndRemoveQuotes(REPLACE(LineData, '"', ''), ',', @BorrowerAccountNumberLocation, 1) AS GroupKey, --Key Identifier
	@ScriptFileId,
	@SourceFile,
	@CreatedBy
FROM
	[fp]._BulkLoad
GROUP BY
	dbo.SplitAndRemoveQuotes(REPLACE(LineData, '"', ''), ',', @BorrowerAccountNumberLocation, 1)
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ERROR = @@ERROR

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
	dbo.SplitAndRemoveQuotes(BL.LineData, ',', @BorrowerAccountNumberLocation, 1) AS GroupKey, --Key Identifier
	BL.LineData
FROM
	[fp]._BulkLoad BL

-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ERROR = @ERROR + @@ERROR


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

-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ERROR = @ERROR + @@ERROR, @LineDataRowCount = @@ROWCOUNT, @BulkLoadCount = (SELECT COUNT(*) FROM [fp]._BulkLoad)

DELETE FROM [fp]._BulkLoad

-- Save/Set the row count and error number (if any) from the previously executed statement

SELECT @ERROR = @ERROR + @@ERROR

IF @ERROR = 0
	BEGIN
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		ROLLBACK TRANSACTION
		IF(@ERROR = 500)
		BEGIN
			SET @ErrorMessage = 'The number of records inserted into LineData did not match the number in _BulkLoad.'
			RAISERROR(N'The number of records intserted into _BulkLoad:%d. The number of records inserted into LineData:%d. %s', 16, 2, @BulkLoadCount, @LineDataRowCount, @ErrorMessage)  
		END
		ELSE
			RAISERROR(N'ERROR Executing stored procedure UHEAASQLDB <uls>.[fp].LoadData',16,1);	
	END
