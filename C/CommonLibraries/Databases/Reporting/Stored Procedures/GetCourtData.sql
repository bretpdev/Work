CREATE PROCEDURE [dbo].[GetCourtData]
	@UserName varchar(50)
AS
	SELECT
		 Request [TicketNumber],
		 'Sacker Script' [System],
		 Script [Title],
		 CASE
			WHEN Script LIKE '%fed' OR Script LIKE '%FED' OR Script LIKE '%(FED)' THEN
				1
				ELSE
				0
			END [IsFederal]
	FROM
		BSYS.dbo.SCKR_DAT_ScriptRequests
	WHERE
		Court = @UserName

	UNION

	SELECT
		Request [TicketNumber],
		'Sacker SAS' [System],
		Job [Title],
		 CASE
			WHEN Job LIKE '%fed' OR Job LIKE '%FED' OR Job LIKE '%(FED)' THEN
				1
				ELSE
				0
			END [IsFederal]
	FROM
		BSYS.dbo.SCKR_DAT_SASRequests
	WHERE
		Court = @UserName

	UNION

	SELECT
		Request [TicketNumber],
		'Letter Tracking' [System],
		DocName [Title],
		CASE
			WHEN DocName LIKE '%fed' OR DocName LIKE '%FED' OR DocName LIKE '%(FED)' THEN
				1
				ELSE
				0
			END [IsFederal]
	FROM
		BSYS.dbo.LTDB_DAT_Requests
	WHERE
		Court = @UserName

	UNION

	SELECT
		T.Ticket [TicketNumber],
		'Need Help' [System],
		[Subject] [Title],
		0 [IsFederal]
	FROM
		NeedHelpUheaa.dbo.DAT_Ticket T
		LEFT JOIN NeedHelpUheaa.dbo.DAT_TicketsAssociatedUserID TA ON T.Ticket = TA.Ticket
		LEFT JOIN CSYS.dbo.SYSA_DAT_Users U ON TA.SqlUserID = U.SqlUserID
	Where
		U.FirstName + ' ' + U.LastName = @UserName
		AND TA.[Role] = 'Court'

	UNION

	SELECT
		T.Ticket [TicketNumber],
		'Need Help' [System],
		[Subject] [Title],
		1 [IsFederal]
	FROM
		NeedHelpCornerStone.dbo.DAT_Ticket T
		LEFT JOIN NeedHelpCornerStone.dbo.DAT_TicketsAssociatedUserID TA ON T.Ticket = TA.Ticket
		LEFT JOIN CSYS.dbo.SYSA_DAT_Users U ON TA.SqlUserID = U.SqlUserID
	Where
		U.FirstName + ' ' + U.LastName = @UserName
		AND TA.[Role] = 'Court'
	ORDER BY
		[System]

RETURN 0

GRANT EXECUTE ON [dbo].[GetCourtData] TO db_executor
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCourtData] TO [db_executor]
    AS [dbo];

