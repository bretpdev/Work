CREATE PROCEDURE [deskaudits].[AddAudit]
	@Auditor VARCHAR(250),
	@Auditee VARCHAR(250),
	@Passed BIT,
	@CommonFailReasonId INT = NULL,
	@CustomFailReasonDescription VARCHAR(2000) = NULL,
	@AuditDate DATETIME
AS
	
	BEGIN TRANSACTION

		DECLARE	@ERROR INT = 0;
		DECLARE @CustomFailReasonId INT;
		DECLARE @OtherCommonReasonId INT = (SELECT CommonFailReasonId FROM deskaudits.CommonFailReasons WHERE SUBSTRING(FailReasonDescription, 1, 5) = 'Other')

		IF (@CommonFailReasonId = @OtherCommonReasonId)
			BEGIN
				IF NOT EXISTS (SELECT CustomFailReasonId FROM CustomFailReasons WHERE FailReasonDescription = @CustomFailReasonDescription)
					BEGIN
						INSERT INTO [deskaudits].CustomFailReasons (FailReasonDescription)
						VALUES (@CustomFailReasonDescription)
						SET @CustomFailReasonId = SCOPE_IDENTITY()
						SELECT @ERROR = @@ERROR;
					END
				ELSE
					SET @CustomFailReasonId = (SELECT CustomFailReasonId FROM CustomFailReasons WHERE FailReasonDescription = @CustomFailReasonDescription)
			END

		INSERT INTO [deskaudits].Audits (Auditor, Auditee, Passed, CommonFailReasonId, CustomFailReasonId, AuditDate)
		VALUES (@Auditor, @Auditee, @Passed, @CommonFailReasonId, @CustomFailReasonId, @AuditDate)
		SELECT @ERROR = @ERROR + @@ERROR;

		IF (@ERROR = 0)
			BEGIN
				COMMIT TRANSACTION
				SELECT CAST(1 AS BIT) AS WasSuccessful; --Succeeded
			END
		ELSE
			BEGIN
				ROLLBACK TRANSACTION
				SELECT CAST(0 AS BIT) AS WasSuccessful; --Failed
			END

RETURN 0
