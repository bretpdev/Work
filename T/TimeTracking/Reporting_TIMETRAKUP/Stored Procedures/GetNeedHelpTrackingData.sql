CREATE PROCEDURE [dbo].[GetNeedHelpTrackingData]
AS
	SELECT DISTINCT
		DT.Ticket [TicketNumber],
		DT.TicketCode [Title],
		DT.[Subject] [RequestName],
		U.[Court]
	FROM
		NeedHelpUheaa..DAT_Ticket DT
		LEFT JOIN
		(
			SELECT
				TA.Ticket,
				U.FirstName + ' ' + U.LastName [Court]
			FROM
				NeedHelpUheaa..DAT_TicketsAssociatedUserID TA
				JOIN CSYS..SYSA_DAT_Users U
					ON TA.SqlUserId = U.SqlUserId
			WHERE
				TA.[Role] = 'Court'
		) U ON DT.Ticket = U.Ticket