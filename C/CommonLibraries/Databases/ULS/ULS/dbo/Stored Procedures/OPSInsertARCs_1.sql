CREATE PROCEDURE [dbo].[OPSInsertARCs]
AS
BEGIN
	
	BEGIN TRANSACTION 
		DECLARE @Error int = 0

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
			1 [ArcTypeID],
			dbo.SplitAndRemoveQuotes(LD.LineData, ',', 3, 1) [AccountNumber],
			CASE dbo.SplitAndRemoveQuotes(LD.LineData, ',', 0, 1)
				WHEN 'W' THEN 'OPSWP'
				WHEN 'T' THEN 'OPSTP'
			END [ARC],
			'OPSACTCMAA' [ScriptId],
			GETDATE() [ProcessOn],
			'$' + LTRIM(STR(CAST(dbo.SplitAndRemoveQuotes(LD.LineData, ',', 4, 1) AS decimal) / 100, 9, 2)) + ' with an effective date of ' + dbo.SplitAndRemoveQuotes(LD.LineData, ',', 1, 1) + '. The confirmation number is ' + dbo.SplitAndRemoveQuotes(LD.LineData, ',', 8, 1) + '.' [Comment],
			0 [IsReference],
			0 [IsEndorser], 
			SUSER_SNAME() [CreatedBy]	
		FROM
			[fp].FileProcessing FP
			JOIN [fp].LineData LD ON FP.FileProcessingId = LD.FileProcessingId
			JOIN [fp].ScriptFiles SF ON SF.ScriptFileId = FP.ScriptFileId
		WHERE
			SF.SourceFile LIKE 'webpayut%'
			AND FP.ProcessedAt is null
			AND FP.Active = 1
	
		-- Save any error numbers
		SELECT @Error =  @@ERROR

		-- Mark FileProcessing records as processed
		UPDATE
			[fp].FileProcessing
		SET
			ProcessedAt = GETDATE()
		WHERE
			ProcessedAt is NULL

		-- Save any error numbers
		SELECT @Error = @Error + @@ERROR

	-- Check for error and rollback the transaction if any have occurred, otherwise commit the transaction
	IF @ERROR = 0
		BEGIN
			COMMIT TRANSACTION
			--ROLLBACK TRANSACTION
		END
	ELSE
		BEGIN
			RAISERROR(N'Failed to import OPS Comments into ArcAddProcessing; [dbo].OPSInsertARCs', 16, 1)
			ROLLBACK TRANSACTION
		END
END