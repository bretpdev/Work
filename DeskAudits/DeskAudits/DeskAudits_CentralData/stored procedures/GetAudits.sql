CREATE PROCEDURE [deskaudits].[GetAudits]
	@Auditor VARCHAR(250) = NULL,
	@Auditee VARCHAR(250) = NULL,
	@Passed BIT = NULL,
	@CommonFailReasonId INT = NULL,
	@BeginDate DATE = NULL,
	@EndDate DATE = NULL
AS
	
	SELECT
		ADT.Auditid,
		ADT.Auditor,
		ADT.Auditee,
		ADT.Passed,
		ADT.CommonFailReasonId,
		COM.FailReasonDescription AS CommonFailReasonDescription,
		ADT.CustomFailReasonId,
		CUS.FailReasonDescription AS CustomFailReasonDescription,
		ADT.AuditDate
	FROM
		CentralData.deskaudits.Audits ADT
		LEFT JOIN CentralData.deskaudits.CommonFailReasons COM
			ON COM.CommonFailReasonId = ADT.CommonFailReasonId
		LEFT JOIN CentralData.deskaudits.CustomFailReasons CUS
			ON CUS.CustomFailReasonId = ADT.CustomFailReasonId
	WHERE
		ADT.DeletedAt IS NULL
		AND (@BeginDate IS NULL OR CAST(AuditDate AS DATE) >= @BeginDate)
		AND (@EndDate IS NULL OR CAST(AuditDate AS DATE) <= @EndDate)
		AND (@CommonFailReasonId IS NULL OR ADT.CommonFailReasonId = @CommonFailReasonId)
		AND (@Auditor IS NULL OR ADT.Auditor = @Auditor)
		AND (@Auditee IS NULL OR ADT.Auditee = @Auditee)
		AND (@Passed IS NULL OR ADT.Passed = @Passed)

RETURN 0
