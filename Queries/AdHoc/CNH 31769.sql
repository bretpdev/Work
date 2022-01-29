USE [CLS]
GO
/****** Object:  StoredProcedure [dbo].[OPSInsertARCs]    Script Date: X/XX/XXXX X:XX:XX AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[OPSInsertARCs]
AS
BEGIN
	
	BEGIN TRANSACTION 
		DECLARE @Error int = X

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
			X [ArcTypeID],
			FP.GroupKey [AccountNumber],
			CASE dbo.SplitAndRemoveQuotes(LD.LineData, ',', X, X)
				WHEN 'W' THEN 'OPSWP'
				WHEN 'T' THEN 'OPSTP'
			END [ARC],
			'OPSACTCMAA' [ScriptId],
			GETDATE() [ProcessOn],
			'$' + REPLACE(LTRIM(STR(CAST(dbo.SplitAndRemoveQuotes(LD.LineData, ',', X, X) AS decimal) / XXX, X, X)), '"', '') + ' with an effective date of ' + REPLACE(dbo.SplitAndRemoveQuotes(LD.LineData, ',', X, X), '"', '') + '. The confirmation number is ' + REPLACE(dbo.SplitAndRemoveQuotes(LD.LineData, ',', X, X), '"', '') + '.' [Comment],
			X [IsReference],
			X [IsEndorser], 
			SUSER_SNAME() [CreatedBy]	
		FROM
			[fp].FileProcessing FP
			JOIN [fp].LineData LD ON FP.FileProcessingId = LD.FileProcessingId
			JOIN [fp].ScriptFiles SF ON SF.ScriptFileId = FP.ScriptFileId
		WHERE
			SF.SourceFile LIKE 'NFP_WEB_LETTER_REQ%'
			AND FP.ProcessedAt IS NULL
			AND FP.DeletedAt IS NULL

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
	IF @ERROR = X
		BEGIN
			COMMIT TRANSACTION
			--ROLLBACK TRANSACTION
		END
	ELSE
		BEGIN
			RAISERROR(N'Failed to import OPS Comments into ArcAddProcessing; [dbo].OPSInsertARCs', XX, X)
			ROLLBACK TRANSACTION
		END
END
