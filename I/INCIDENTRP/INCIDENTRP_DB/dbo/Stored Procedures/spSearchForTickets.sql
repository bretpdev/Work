
-- =============================================
-- Author:		Daren Beattie
-- Create date: September 19, 2011
-- Description:	Searches for tickets in the database that meet the given criteria.
-- =============================================
CREATE PROCEDURE [dbo].[spSearchForTickets]
	@TicketNumber BIGINT = NULL,
	@KeyWordSearchScope VARCHAR(10) = NULL,
	@KeyWord VARCHAR(MAX) = NULL,
	@Subject VARCHAR(MAX) = NULL,
	@Court INT = NULL,
	@Requester INT = NULL,
	@Status VARCHAR(50) = NULL,
	@BusinessUnitId INT = NULL,
	@TicketType VARCHAR(50) = NULL,
	@FunctionalArea VARCHAR(50) = NULL,
	@AssignedTo INT = NULL,
	@CreateDateRangeStart DATETIME,
	@CreateDateRangeEnd DATETIME
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT
		CASE TK.TicketType WHEN 'Incident' THEN 'IR FED' ELSE 'THR FED' END AS TicketCode,
		TK.TicketNumber,
		CASE TK.TicketType WHEN 'Incident' THEN IR.Narrative ELSE INF.NatureOfCall END AS [Subject],
		TK.Status,
		TK.Priority,
		MAX(HIST.UpdateDateTime) AS LastUpdateDate
	FROM DAT_Ticket TK
	INNER JOIN DATHistory HIST
		ON HIST.TicketNumber = TK.TicketNumber
		AND HIST.TicketType = TK.TicketType
	LEFT OUTER JOIN DAT_Incident IR
		ON TK.TicketNumber = IR.TicketNumber
		AND TK.TicketType = IR.TicketType
	LEFT OUTER JOIN DAT_ThreatInfo INF
		ON TK.TicketNumber = INF.TicketNumber
		AND TK.TicketType = INF.TicketType
	LEFT OUTER JOIN CSYS.dbo.SYSA_DAT_UserKeyAssignment BU
		ON BU.UserKey IN ('Manager', 'Member', 'Supervisor')
		AND BU.EndDate IS NULL
		AND TK.Requester = BU.SqlUserId
	WHERE (
		@TicketNumber IS NULL
		OR TK.TicketNumber = @TicketNumber
	)
	AND (
		@KeyWordSearchScope IS NULL
		OR (
			@KeyWordSearchScope IN ('Subject', 'All')
			AND (
				(TK.TicketType = 'Incident' AND IR.Narrative LIKE '%' + @KeyWord + '%')
				OR (TK.TicketType = 'Threat' AND INF.NatureOfCall LIKE '%' + @KeyWord + '%')
			)
		)
		OR (
			@KeyWordSearchScope IN ('History', 'All')
			AND EXISTS (
				SELECT *
				FROM DATHistory
				WHERE [UpdateText] LIKE '%' + @KeyWord + '%'
			)
		)
	)
	AND (
		@Subject IS NULL
		OR (TK.TicketType = 'Incident' AND @Subject = IR.Narrative)
		OR (TK.TicketType = 'Threat' AND @Subject = INF.NatureOfCall)
	)
	AND (
		@Court IS NULL
		OR @Court = TK.Court
	)
	AND (
		@Requester IS NULL
		OR @Requester = TK.Requester
	)
	AND (
		@Status IS NULL
		OR @Status = TK.[Status]
	)
	AND (
		@BusinessUnitId IS NULL
		OR @BusinessUnitId = BU.BusinessUnit
	)
	AND (
		@TicketType IS NULL
		OR @TicketType = TK.TicketType
	)
	AND (
		@FunctionalArea IS NULL
		OR @FunctionalArea = TK.FunctionalArea
	)
	AND (
		@AssignedTo IS NULL
		OR @AssignedTo = TK.AssignedTo
	)
	AND TK.CreateDateTime BETWEEN @CreateDateRangeStart AND @CreateDateRangeEnd
	GROUP BY
		TK.TicketType,
		TK.TicketNumber,
		IR.Narrative,
		INF.NatureOfCall,
		TK.[Status],
		TK.Priority
END