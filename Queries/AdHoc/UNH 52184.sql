USE ULS
GO

--Add new column to fp.ScriptFiles
ALTER TABLE fp.ScriptFiles
	ADD UsesBulkLoadId INT
GO

--Update the ELSYSDDARC script to set the UsesBulkLoadId to true
UPDATE
	fp.ScriptFiles
SET
	UsesBulkLoadId = 1
WHERE
	SourceFile LIKE '%ULWS28%'
GO

--Create a sproc that will alter the _BulkLoad table if the script uses the bulk load id
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [fp].[AddBulkLoadIdField]
AS
ALTER TABLE fp.[_BulkLoad]
	ADD BulkLoadId BIGINT PRIMARY KEY IDENTITY(1,1)
GO

--Add a column to the FileProcessing table that will hold the bulk load id from the _BulkLoad table
ALTER TABLE fp.FileProcessing
	ADD BulkLoadId BIGINT NULL

--Alter the GetFileNamesForScript to pull the new UsesBulkLoadId column
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [fp].[GetFileNamesForScript]
	@ScriptId varchar(10)
AS
	SELECT
		ScriptFileId,
		SourceFile as [FileName],
		ProcessAllFiles,
		UsesBulkLoadId
	FROM
		[fp].ScriptFiles
	WHERE
		ScriptID = @ScriptId

--LoadData method updated to use the new BulkLoadId column in the fp._BulkLoad table
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

DECLARE @_BulkLoad TABLE
(
	GroupKey VARCHAR(50),
	BulkLoadId BIGINT,
	LineData VARCHAR(MAX)
)

IF (SELECT UsesBulkLoadId FROM [fp].ScriptFiles WHERE ScriptFileId = @ScriptFileId) = 1
	BEGIN --Run this if using the bulk load id
		--Insert data into [FileProcessing]
		INSERT INTO [fp].[FileProcessing]
		(
			GroupKey,
			BulkLoadId,
			ScriptFileId,
			SourceFile,
			CreatedBy
		)
		SELECT
			dbo.SplitAndRemoveQuotes(REPLACE(LineData, '"', ''), ',', @BorrowerAccountNumberLocation, 1) AS GroupKey, --Key Identifier
			BulkLoadId,
			@ScriptFileId,
			@SourceFile,
			@CreatedBy
		FROM
			[fp]._BulkLoad
		GROUP BY
			dbo.SplitAndRemoveQuotes(REPLACE(LineData, '"', ''), ',', @BorrowerAccountNumberLocation, 1),
			BulkLoadId
		
			-- Save/Set the row count and error number (if any) from the previously executed statement
		SELECT @ERROR = @@ERROR

		INSERT INTO @_BulkLoad
		(
			GroupKey,
			BulkLoadId,
			LineData
		)
		SELECT
			dbo.SplitAndRemoveQuotes(BL.LineData, ',', @BorrowerAccountNumberLocation, 1) AS GroupKey, --Key Identifier
			BL.BulkLoadId,
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
					RANK() OVER (PARTITION BY BL.BulkLoadId ORDER BY BL.BulkLoadId) [rnk1],
					RANK() OVER (PARTITION BY FP.FileProcessingId ORDER BY BL.BulkLoadId) [rnk2]
				FROM
					@_BulkLoad BL
					INNER JOIN [fp].FileProcessing FP on FP.GroupKey = BL.GroupKey and FP.SourceFile = @SourceFile
				WHERE
					FP.Active = 1
			) LD
		WHERE
			LD.rnk1 = LD.rnk2
	END
ELSE
	BEGIN --Use this when not using the bulk load id
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
				WHERE
					FP.Active = 1
			) LD
		WHERE
			LD.rnk1 = LD.rnk2
	END

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