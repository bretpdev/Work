CREATE PROCEDURE [dbo].[GetCourtData]
	@UserName varchar(50)
AS
	SELECT
		 Request [TicketNumber],
		 'Sacker Script' [System],
		 Script [Title]
	FROM
		BSYS.dbo.SCKR_DAT_ScriptRequests
	WHERE
		Court = @UserName

	UNION

	SELECT
		Request [TicketNumber],
		'Sacker SAS' [System],
		Job [Title]
	FROM
		BSYS.dbo.SCKR_DAT_SASRequests
	WHERE
		Court = @UserName

	UNION

	SELECT
		Request [TicketNumber],
		'Letter Tracking' [System],
		DocName [Title]
	FROM
		BSYS.dbo.LTDB_DAT_Requests
	WHERE
		Court = @UserName

	UNION

	SELECT
		T.Ticket [TicketNumber],
		'Need Help' [System],
		[Subject] [Title]
	FROM
		NeedHelpUheaa.dbo.DAT_Ticket T
		LEFT JOIN NeedHelpUheaa.dbo.DAT_TicketsAssociatedUserID TA ON T.Ticket = TA.Ticket
		LEFT JOIN CSYS.dbo.SYSA_DAT_Users U ON TA.SqlUserID = U.SqlUserID
	Where
		U.FirstName + ' ' + U.LastName = @UserName
		AND TA.[Role] = 'Court'