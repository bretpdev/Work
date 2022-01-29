CREATE PROCEDURE [dbo].[OPSInsertARCs]
	
AS
	
	BEGIN
	
	BEGIN TRANSACTION 
		DECLARE @Error INT = 0,
		@Today DATETIME = GETDATE()

		--Insert OPS ARCs into ArcAddProcessing
		INSERT INTO ArcAddProcessing
		(	
			ArcTypeId,
			AccountNumber, 
			ARC, 
			ScriptId, 
			ProcessOn, 
			Comment, 
			IsReference, 
			IsEndorser, 
			CreatedBy
		)
		SELECT
			1 AS ArcTypeID,
			dbo.SplitAndRemoveQuotes(LD.LineData, ',', 3, 1) AS AccountNumber,
			CASE dbo.SplitAndRemoveQuotes(LD.LineData, ',', 0, 1)
				WHEN 'W' THEN 'OPSWP'
				WHEN 'T' THEN 'OPSTP'
			END [ARC],
			'OPSACTCMAA' AS ScriptId,
			@Today AS ProcessOn,
			'$' + LTRIM(STR(CAST(dbo.SplitAndRemoveQuotes(LD.LineData, ',', 4, 1) AS DECIMAL) / 100, 9, 2)) + ' with an effective date of ' + dbo.SplitAndRemoveQuotes(LD.LineData, ',', 1, 1) + '. The confirmation number is ' + dbo.SplitAndRemoveQuotes(LD.LineData, ',', 8, 1) + '.' AS Comment,
			0 AS IsReference,
			0 AS IsEndorser, 
			SUSER_SNAME() AS CreatedBy	
		FROM
			[fp].FileProcessing FP
			INNER JOIN [fp].LineData LD 
				ON LD.FileProcessingId = FP.FileProcessingId
		WHERE
			FP.SourceFile LIKE 'webpayut%'
			AND FP.ProcessedAt IS NULL
			AND FP.Active = 1
	
		-- Save any error numbers
		SELECT @Error =  @@ERROR;

		-- Mark FileProcessing records as processed
		UPDATE
			[fp].FileProcessing
		SET
			ProcessedAt = @Today
		WHERE
			ProcessedAt IS NULL

		-- Save any error numbers
		SELECT @Error = @Error + @@ERROR;

	-- Check for error and rollback the transaction if any have occurred, otherwise commit the transaction
	IF @ERROR = 0
		BEGIN
			COMMIT TRANSACTION;
		END
	ELSE --ROLLBACK TRANSACTION
		BEGIN
			RAISERROR(N'Failed to import OPS Comments into ArcAddProcessing; [dbo].OPSInsertARCs', 16, 1);
			ROLLBACK TRANSACTION;
		END
END

RETURN 0
