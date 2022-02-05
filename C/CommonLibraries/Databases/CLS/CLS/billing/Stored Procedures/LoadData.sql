
CREATE PROCEDURE [billing].[LoadData]
(
	@ScriptFileId int,
	@SourceFile VARCHAR(100),
	@CreatedBy VARCHAR(50)
)

AS
	DECLARE 
		@ERROR INT = 0,
		@BorrowerCounterLocation INT = 0,
		@LineDataRowCount INT = 0,
		@ErrorMessage VARCHAR(300),
		@BulkLoadCount INT = 0,
		@BorrowerAccountNumberLocation INT,
		@EndorserAccountNumberLocation INT

BEGIN TRANSACTION

SET @BorrowerCounterLocation = (SELECT BorrowerCounterLocation FROM billing.ScriptFiles WHERE ScriptFileId = @ScriptFileId)
SET @BorrowerAccountNumberLocation = (SELECT BorrowerAccountNumberLocation FROM billing.ScriptFiles WHERE ScriptFileId = @ScriptFileId)
SET @EndorserAccountNumberLocation = (SELECT EndorserAccountNumberLocation FROM billing.ScriptFiles WHERE ScriptFileId = @ScriptFileId)

--Insert data into billing.PrintProcessing from _BulkLoad
INSERT INTO billing.PrintProcessing
(
	AccountNumber, 
	ScriptFileId,
	SourceFile, 
	CreatedBy
) 
SELECT 
	CASE dbo.SplitAndRemoveQuotes(LineData, ',', @EndorserAccountNumberLocation, 1)
		WHEN '' THEN dbo.SplitAndRemoveQuotes(LineData, ',', @BorrowerAccountNumberLocation, 1)
		ELSE dbo.SplitAndRemoveQuotes(LineData, ',', @EndorserAccountNumberLocation, 1)
	END as AccountNumber, 
	@ScriptFileId,
	@SourceFile,--grab from c# param
	@CreatedBy --grab from c# param
FROM
	billing._BulkLoad BL
GROUP BY
	dbo.SplitAndRemoveQuotes(LineData, ',', @BorrowerCounterLocation, 1),
	dbo.SplitAndRemoveQuotes(LineData, ',', @BorrowerAccountNumberLocation, 1),
	dbo.SplitAndRemoveQuotes(LineData, ',', @EndorserAccountNumberLocation, 1)

-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ERROR = @@ERROR

DECLARE @_BulkLoad TABLE
(
	AccountNumber CHAR(10),
	BorrowerCounter VARCHAR(8),
	LineData VARCHAR(MAX),
	LnSeq CHAR(4)
)
INSERT INTO	@_BulkLoad
(
	AccountNumber,
	BorrowerCounter,
	LineData,
	LnSeq
)
SELECT
	CASE dbo.SplitAndRemoveQuotes(LineData, ',', @EndorserAccountNumberLocation, 1)
		WHEN '' THEN dbo.SplitAndRemoveQuotes(LineData, ',', @BorrowerAccountNumberLocation, 1)
		ELSE dbo.SplitAndRemoveQuotes(LineData, ',', @EndorserAccountNumberLocation, 1)
	END as AccountNumber, 
	dbo.SplitAndRemoveQuotes(BL.LineData, ',', @BorrowerCounterLocation, 1) as BorrowerCournt, --BorrowerCounter
	BL.LineData,
	dbo.SplitAndRemoveQuotes(BL.LineData, ',', 13, 1) as LnSeq
FROM 
	billing._BulkLoad BL

-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ERROR = @ERROR + @@ERROR

--Insert data into billing.LineData from _BulkLoad
INSERT INTO billing.LineData
(
	PrintProcessingId,
	LineData
)
SELECT
	PP.PrintProcessingId,
	BL.LineData
FROM
	@_BulkLoad BL
	INNER JOIN billing.PrintProcessing PP on PP.AccountNumber = BL.AccountNumber and PP.SourceFile = @SourceFile


-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ERROR = @ERROR + @@ERROR, @LineDataRowCount = @@ROWCOUNT, @BulkLoadCount = (SELECT COUNT(*) FROM billing._BulkLoad)

IF(@BulkLoadCount != @LineDataRowCount )
	BEGIN
		SET @ERROR = 500 --WE WILL BE CHECKING FOR THIS ERROR BELOW IN THE CODE AND RASING A CUSTOM ERROR MESSAGE
	END

DELETE FROM billing._BulkLoad

-- Save/Set the row count and error number (if any) from the previously executed statement

SELECT @ERROR = @ERROR + @@ERROR

UPDATE
	PP
SET
	PP.OnEcorr = 
		(
			CASE
				WHEN ISNULL(PH05.DI_VLD_CNC_EML_ADR, 'N') = 'N'	THEN 0
				WHEN ISNULL(PH05.DI_CNC_ELT_OPI, 'N') = 'Y' AND ScriptfileId IN (3,4,13,14) THEN 1
				WHEN ISNULL(PH05.DI_CNC_EBL_OPI, 'N') = 'Y' AND ScriptFileId NOT IN (3,4,13,14) THEN 1
				ELSE 0
			END
		)
FROM
	billing.PrintProcessing PP
	LEFT JOIN [CDW].[dbo].PH05_CNC_EML PH05 ON PP.AccountNumber = PH05.DF_SPE_ID
WHERE
	CAST(PP.AddedAt AS DATE) = CAST(GETDATE() AS DATE)

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
			RAISERROR(N'ERROR Executing stored procedure UHEAASQLDB <cls/uls>.billing.LoadData on file %s',16,1, @SourceFile);	
	END