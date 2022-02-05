
CREATE PROCEDURE [dbo].[spSearchForOpenTickets]
	@SearchOption						VARCHAR(50) = 'OpenTickets',
	@TicketNumber						BIGINT = 0,
	@FunctionalArea						VARCHAR(100) = '',
	@KeyWord							VARCHAR(100) = '',
	@KeyWordScope						VARCHAR(10) = 'None',
	@AssignedTo							INT = NULL,
	@Court								INT = NULL,
	@Type								VARCHAR(50) = '',
	@Requester							INT = NULL,
	@Status								VARCHAR(50) = '',
	@BusinessUnit						INT = NULL,
	@BeginCreateDate					VARCHAR(20) = '',
	@EndCreateDate						VARCHAR(20) = ''
	
AS
BEGIN
	SET NOCOUNT ON;


	SELECT tk.Ticket as TicketNumber,
			Coalesce(tk.TicketCode,'') as TicketCode,
			Coalesce(tk.[Subject],'') as [Subject],
			tk.Requested,
			Coalesce(tk.Unit, 0) as Unit,
			Coalesce(tk.Area, '') as Area,
			Coalesce(tk.[Required], '') as [Required],
			Coalesce(tk.[Status], '') as [Status],
			Coalesce(tk.Issue, '') as Issue,
			Coalesce(tk.StatusDate, '') as StatusDate,
			Coalesce(tk.CourtDate, '') as CourtDate,
			Coalesce(tk.Priority, 0) as Priority,
			Coalesce(tk.LastUpdated, '') as LastUpdate,
			Coalesce(tk.CCCIssue, '') as CCCIssue,
			Coalesce(tk.RequestProjectNum, '') as RequestProjectNum,
			Coalesce(fl.UserInterfaceDisplayIndicator, '') as UserInterfaceDisplayIndicator
	FROM dbo.DAT_Ticket tk
	LEFT JOIN dbo.DAT_TicketsAssociatedUserID ato
		ON tk.Ticket = ato.Ticket AND ato.Role = 'AssignedTo'
	LEFT JOIN dbo.DAT_TicketsAssociatedUserID crt
		ON tk.Ticket = crt.Ticket AND crt.Role = 'Court'
	LEFT JOIN dbo.DAT_TicketsAssociatedUserID rqst
		ON tk.Ticket = rqst.Ticket AND rqst.Role = 'Requester'
	LEFT JOIN
		( 
			SELECT Ticket, MIN(BeginDate) as CreateDate 
			FROM dbo.REF_Status sta
			GROUP BY Ticket
		) as sta
		ON tk.Ticket = sta.Ticket
	left join CSYS.dbo.FLOW_DAT_Flow fl
		on tk.TicketCode = fl.FlowID
	WHERE	--If all tickets was selected then return all tickets without any other filtering criteria
			(@SearchOption = 'AllTickets') OR
			--If open tickets was selected then return all open tickets (I tried to use all current status for closing a ticket
			--and added a few more that I thought they may use sometime).
			(@SearchOption = 'OpenTickets' AND tk.[Status] NOT IN ('Withdrawn', 'Resolved', 'Complete', 'Closed', 'Verified')) OR
			--If search criteria was provided then return all tickets that match the criteria
			(@SearchOption = 'UserSearchCriteria' AND (
													@TicketNumber = 0 OR @TicketNumber = tk.Ticket
												) AND (
													@FunctionalArea = '' OR @FunctionalArea = tk.Area
												) AND (
													@KeyWordScope = 'None' OR (
																			(@KeyWordScope = 'Subject' AND tk.[Subject] LIKE '%' + @KeyWord + '%') OR
																			(@KeyWordScope = 'Issue' AND tk.Issue LIKE '%' + @KeyWord + '%') OR
																			(@KeyWordScope = 'History' AND tk.History LIKE '%' + @KeyWord + '%') OR
																			(@KeyWordScope = 'All' AND (
																											tk.History LIKE '%' + @KeyWord + '%' OR
																											tk.Issue LIKE '%' + @KeyWord + '%' OR
																											tk.[Subject] LIKE '%' + @KeyWord + '%'
																										))
																		)
												) AND (
													@AssignedTo IS NULL OR @AssignedTo = ato.SqlUserId
												) AND (
													@Court IS NULL OR @Court = crt.SqlUserId
												) AND (
													@Type = '' OR @Type = tk.TicketCode
												) AND (
													@Requester IS NULL OR @Requester = rqst.SqlUserId
												) AND (
													@Status = '' OR @Status = tk.[Status]
												) AND (
													@BusinessUnit IS NULL OR @BusinessUnit = tk.Unit
												) AND (
													@BeginCreateDate = '' OR sta.CreateDate BETWEEN @BeginCreateDate AND @EndCreateDate
												)
			)

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSearchForOpenTickets] TO [db_executor]
    AS [dbo];

