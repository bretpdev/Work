CREATE PROCEDURE [dbo].[ArcAdd_GetNextArc]
AS
BEGIN

SET NOCOUNT ON;
BEGIN TRANSACTION

	DECLARE @ArcAddProcessingId BIGINT
	DECLARE @ERROR INT

	DECLARE @MyTableVar 
		TABLE
		(
			ArcAddProcessingId int NOT NULL
		);

	SELECT @ERROR = @@ERROR

	UPDATE TOP (1)
		ArcAddProcessing
	SET
		ProcessedAt = GETDATE()
	OUTPUT inserted.ArcAddProcessingId
	INTO @MyTableVar
	WHERE
		ProcessedAt IS NULL
		AND ProcessOn <= GETDATE()
	
	SELECT @ERROR = @ERROR + @@ERROR

	SELECT
		AAP.ArcAddProcessingId,
		AAP.ArcTypeId,
		AAP.AccountNumber,
		AAP.RecipientId,
		AAP.ARC,
		AAP.ScriptId,
		AAP.Comment,
		AAP.IsReference,
		AAP.IsEndorser,
		AAP.ProcessFrom,
		AAP.ProcessTo,
		AAP.NeededBy,
		AAP.RegardsTo,
		AAP.RegardsCode,
		ARC.ResponseCode
	FROM 
		ArcAddProcessing AAP
	INNER JOIN @MyTableVar MT
		ON MT.ArcAddProcessingId = AAP.ArcAddProcessingId
	LEFT JOIN ArcResponseCodes ARC
		on ARC.ArcResponseCodeId = AAP.ArcResponseCodeId
	
	
	SELECT @ERROR = @ERROR + @@ERROR

	IF @ERROR = 0
		BEGIN
			COMMIT TRANSACTION
		END
	ELSE
		BEGIN
			RAISERROR ('Failed to retrieve dbo.ArcAddProcessing record for processing.', -- error message
				   16, -- Severity.
				   1 -- State.
				   );
			ROLLBACK TRANSACTION
		END
	END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetNextArc] TO [db_executor]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetNextArc] TO [UHEAA\SystemAnalysts]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_GetNextArc] TO [UHEAA\CornerStoneUsers]
    AS [dbo];



