CREATE PROCEDURE [duplrefs].[InactivateRecord]
	@BorrowerQueueId INT

AS

	DECLARE @EXPECTED_ROWCOUNT INT = --Number of RQ records being targeted
		(
			SELECT 
				COUNT(1) C
			FROM
				duplrefs.BorrowerQueue BQ
				INNER JOIN duplrefs.ReferenceQueue RQ
					ON RQ.BorrowerQueueId = BQ.BorrowerQueueId
			WHERE
				BQ.BorrowerQueueId = @BorrowerQueueId 
				AND RQ.DeletedAt IS NULL
				AND 
				(
					RQ.ProcessedAt IS NULL
					OR RQ.Lp2fProcessedAt IS NULL
				)
		)

	BEGIN
		BEGIN TRANSACTION
		DECLARE @ERROR INT = 0
		DECLARE @ROWCOUNT INT = 0

		UPDATE
			BQ
		SET
			BQ.DeletedAt = GETDATE(),
			BQ.DeletedBy = SUSER_NAME()
		FROM
			duplrefs.BorrowerQueue BQ
		WHERE
			BQ.BorrowerQueueId = @BorrowerQueueId
			AND BQ.DeletedAt IS NULL

		SELECT @ERROR = @@ERROR, @ROWCOUNT = @@ROWCOUNT
		SELECT @EXPECTED_ROWCOUNT = @EXPECTED_ROWCOUNT + @ROWCOUNT --Add BQ record to expected count if it was updated

		UPDATE
			RQ
		SET
			RQ.DeletedAt = GETDATE(),
			RQ.DeletedBy = SUSER_NAME()
		FROM
			duplrefs.BorrowerQueue BQ
			INNER JOIN duplrefs.ReferenceQueue RQ
				ON RQ.BorrowerQueueId = BQ.BorrowerQueueId
		WHERE
			BQ.BorrowerQueueId = @BorrowerQueueId
			AND RQ.DeletedAt IS NULL
			AND 
			(
				RQ.ProcessedAt IS NULL
				OR RQ.Lp2fProcessedAt IS NULL
			)

		SELECT @ERROR = @ERROR + @@ERROR, @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT

		IF(@ERROR = 0 AND @EXPECTED_ROWCOUNT = @ROWCOUNT)
			BEGIN
				COMMIT TRANSACTION
				SELECT CAST(1 AS BIT) [WasSuccessful]
			END
		ELSE
			BEGIN
				ROLLBACK TRANSACTION
				SELECT CAST(0 AS BIT) [WasSuccessful]
			END
	END
		
RETURN 0
