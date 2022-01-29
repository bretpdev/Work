USE IncidentReportingUHEAA
GO

ALTER PROCEDURE [dbo].[IncidentReportingCancellations]
	@ticketType VARCHAR(200),
	@beginDate DATE,
	@endDate DATE
AS

IF @beginDate IS NULL
BEGIN
	SET @beginDate = DATEADD(DAY, 1, EOMONTH(DATEADD(DAY , -1, GETDATE()), -1))
END

IF @endDate IS NULL
BEGIN
	SET @endDate = EOMONTH(DATEADD(DAY , -1, GETDATE()))
END


SELECT
	*
FROM
	(
		SELECT
			'UHEAA' AS Portfolio,
			CANC.*,
			USR.FirstName+' '+USR.LastName AS AgentName,
			CASE
				WHEN CANC.TicketType = 'Incident' THEN 'Security Incident'
				ELSE 'Physical Threat'
			END AS IncidentType
		FROM
			IncidentReportingUHEAA.dbo.DAT_CanceledTickets CANC
			LEFT JOIN CSYS.dbo.SYSA_DAT_Users USR
				ON CANC.SqlUserId = USR.SqlUserId
	) Tickets
WHERE
	TicketType IN (SELECT value FROM STRING_SPLIT(@ticketType, ','))
	AND Portfolio IN ('UHEAA')
	AND CAST(CreateDateTime AS Date) BETWEEN @beginDate AND @endDate
ORDER BY
	IncidentType,
	AgentName

GO