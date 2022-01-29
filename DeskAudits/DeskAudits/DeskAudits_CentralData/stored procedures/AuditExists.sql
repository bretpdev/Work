CREATE PROCEDURE [deskaudits].[AuditExists]
	@Auditor VARCHAR(250),
	@Auditee VARCHAR(250),
	@Passed BIT,
	@CommonFailReasonId INT = NULL,
	@CustomFailReasonDescription VARCHAR(2000) = NULL,
	@AuditDate DATETIME
AS
	
		SELECT
			CASE WHEN COUNT(1) > 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT)
			END AS AuditExists
		FROM
			deskaudits.Audits ADT
			LEFT JOIN deskaudits.CustomFailReasons CFR
				ON CFR.CustomFailReasonId = ADT.CustomFailReasonId
		WHERE
			ADT.Auditor = @Auditor
			AND ADT.Auditee = @Auditee
			AND ADT.Passed = @Passed
			AND (@CommonFailReasonId IS NULL OR ADT.CommonFailReasonId = @CommonFailReasonId)
			AND (@CustomFailReasonDescription IS NULL OR CFR.FailReasonDescription = @CustomFailReasonDescription)
			AND ADT.AuditDate = @AuditDate

RETURN 0

