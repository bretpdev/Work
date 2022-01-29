
CREATE PROCEDURE [fp].[LoadData]
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

DECLARE @FieldName varchar(25)
SET @FieldName = 'PH05.' + (SELECT EcorrFieldName FROM [fp].EcorrCategories EC WHERE EC.ScriptFileId = @ScriptFileId)

DECLARE @Sql varchar(1000)
SET @Sql = N'
UPDATE
	FP
SET
	FP.OnEcorr = 
		(
			CASE
				WHEN ISNULL(' + @FieldName + ', 0) = 1 AND ISNULL(PH05.DI_VLD_CNC_EML_ADR, 0) = 1
					THEN 1
				ELSE 0
			END
		)
FROM
	[CLS].[fp].FileProcessing FP
	LEFT JOIN [CDW].[dbo].PH05_ContactEmail PH05 ON FP.GroupKey = PH05.DF_SPE_ACC_ID
WHERE
	FP.AddedAt > DATEADD(WEEK,-1,GETDATE())'
exec sp_sqlexec @Sql

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
			RAISERROR(N'ERROR Executing stored procedure UHEAASQLDB <cls>.[fp].LoadData',16,1);	
	END
GO
GRANT EXECUTE
    ON OBJECT::[fp].[LoadData] TO [db_executor]
    AS [dbo];

