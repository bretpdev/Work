CREATE PROCEDURE [clmpmtpst].[AddError]
	@ClaimPaymentId INT = NULL,
	@ErrorDescription VARCHAR(1000)
AS
	
	BEGIN TRANSACTION
		DECLARE @ERROR INT = 0,
			@ROWCOUNT INT = 0,
			@ErrorId INT;

	/* Create error description if there is not already a matching one */
	IF NOT EXISTS (SELECT DescriptionId FROM clmpmtpst.ErrorDescriptions ED WHERE ED.[Description] = @ErrorDescription)
		BEGIN
			INSERT INTO clmpmtpst.ErrorDescriptions
			VALUES (@ErrorDescription);
			SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
		END

	/* Add error record to Errors table */
	INSERT INTO clmpmtpst.Errors (ErrorDescriptionId)
	SELECT
		DescriptionId
	FROM
		ErrorDescriptions ED
	WHERE
		ED.[Description] = @ErrorDescription

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	SET @ErrorId = SCOPE_IDENTITY

	IF (@ClaimPaymentId IS NOT NULL) --If error is tied to claim record, record that in main processing table
		BEGIN
			UPDATE
				clmpmtpst.ClaimPayments
			SET
				ErrorId = @ErrorId
			WHERE
				ClaimPaymentId = @ClaimPaymentId

			SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
		END

	IF @ROWCOUNT >= 1 AND @ERROR = 0
		BEGIN
			COMMIT TRANSACTION
			SELECT CAST(1 AS BIT) AS Succeeded
		END
	ELSE
		BEGIN
			PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
			PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
			PRINT 'Transaction NOT committed'
			ROLLBACK TRANSACTION
			SELECT CAST(0 AS BIT) AS Succeeded
		END

RETURN 0
