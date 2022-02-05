CREATE PROCEDURE [dbo].[ArcAdd_GetNextOnelinkRecord]
AS
BEGIN
SET NOCOUNT ON;
BEGIN TRANSACTION

	DECLARE @ArcAddProcessingOnelinkId BIGINT
	DECLARE @ERROR INT

	DECLARE @MyTableVar 
		TABLE
		(
			ArcAddProcessingOnelinkId int NOT NULL
		);

	SELECT @ERROR = @@ERROR

	UPDATE TOP (1)
		ArcAddProcessingOnelink
	SET
		ProcessedAt = GETDATE()
	OUTPUT inserted.ArcAddProcessingOnelinkId
	INTO @MyTableVar
	WHERE
		ProcessedAt IS NULL
		AND ProcessOn <= GETDATE()
	
	SELECT @ERROR = @ERROR + @@ERROR

	SELECT
		AAP.ArcAddProcessingOnelinkId,
        AAP.AccountIdentifer,
        AAP.AssociatedPersonID,
        AAP.ActionCode,
        AAP.ActivityType,
        AAP.ActivityContactType,
        AAP.DocumentID,
        AAP.ActivityDateTime,
		AAP.ActivityCloseDate,
        AAP.UniqueID,
        AAP.InstitutionID,
        AAP.UserID,
        AAP.ClaimPackageCreateDate,
        AAP.UserIDClaimPackage,
        AAP.Comment,
        AAP.ScriptID
	FROM 
		ArcAddProcessingOnelink AAP
	INNER JOIN @MyTableVar MT
		ON MT.ArcAddProcessingOnelinkId = AAP.ArcAddProcessingOnelinkId
	
	
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
