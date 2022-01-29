CREATE PROCEDURE [dbo].[spGetTicket]
	@TicketID			bigint,
	@TicketCode			varchar(50)
AS
BEGIN 

	SET NOCOUNT ON;

	SELECT T.Ticket as TicketID,
			Coalesce(TicketCode,'') as TicketCode,
			Coalesce([Subject],'') as [Subject],
			Coalesce(Requested, GetDate()) as Requested,
			Coalesce(Unit, 0) as Unit,
			Coalesce(Area,'') as Area,
			Coalesce([Required], GetDate()) as [Required],
			Coalesce(Issue,'') as Issue,
			Coalesce(ResolutionCause,'') as ResolutionCause,
			Coalesce(ResolutionFix,'') as ResolutionFix,
			Coalesce(ResolutionPrevention,'') as ResolutionPrevention,
			Coalesce([Status],'') as [Status],
			Coalesce(StatusDate, GetDate()) as StatusDate,
			Coalesce(CourtDate, GetDate()) as CourtDate,
			Coalesce(IssueUpdate,'') as IssueUpdate,
			Coalesce(History,'') as History,
			Coalesce(PreviousStatus,'') as PreviousStatus,
			Coalesce(UrgencyOption,'') as UrgencyOption,
			Coalesce(CatOption,'') as CatOption,
			Coalesce(Priority,'') as Priority,
			Coalesce(LastUpdated, GetDate()) as LastUpdated,
			Coalesce(CCCIssue,'') as CCCIssue,
			Coalesce(RequestProjectNum,'') as RequestProjectNum,
			Coalesce(Comments,'') as Comments,
			Coalesce(RelatedQCIssues,'') as RelatedQCIssues,
			Coalesce(RelatedProcedures,'') as RelatedProcedures,
			Coalesce(AT.SqlUserId, 0) as AssignedToID,
			Coalesce(PC.SqlUserId, 0) as PreviousCourt,
			Coalesce(C.SqlUserId, 0) as CourtID,
			Coalesce(R.SqlUserId, 0) as RequesterID
	FROM dbo.DAT_Ticket T
	LEFT JOIN dbo.DAT_TicketsAssociatedUserID AT
		ON AT.Ticket = T.Ticket
		AND AT.Role = 'AssignedTo'
	LEFT JOIN dbo.DAT_TicketsAssociatedUserID PC
		ON PC.Ticket = T.Ticket
		AND PC.Role = 'PreviousCourt'
	LEFT JOIN dbo.DAT_TicketsAssociatedUserID C
			ON C.Ticket = T.Ticket
			AND C.Role = 'Court'
	LEFT JOIN dbo.DAT_TicketsAssociatedUserID R
			ON R.Ticket = T.Ticket
			AND R.Role = 'Requester'
	WHERE T.Ticket = @TicketID
			and T.TicketCode = @TicketCode

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGetTicket] TO [db_executor]
    AS [dbo];

