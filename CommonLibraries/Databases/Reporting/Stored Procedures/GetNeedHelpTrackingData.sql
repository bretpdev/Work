CREATE PROCEDURE [dbo].[GetNeedHelpTrackingData]
	@IsFederal bit
AS
	IF (@IsFederal = 1)
		BEGIN
			SELECT DISTINCT
				DT.Ticket [TicketNumber],
				DT.TicketCode [Title],
				DT.[Subject] [RequestName],
				U.[User Name]
			FROM
				NeedHelpCornerStone..DAT_Ticket DT
				LEFT JOIN
				(
					SELECT
						TA.Ticket,
						U.FirstName + ' ' + U.LastName [User Name]
					FROM
						NeedHelpCornerStone..DAT_TicketsAssociatedUserID TA
						JOIN CSYS..SYSA_DAT_Users U
							ON TA.SqlUserId = U.SqlUserId
					WHERE
						TA.[Role] = 'Court'
				) U ON DT.Ticket = U.Ticket
		END
	ELSE
		BEGIN
			SELECT DISTINCT
				DT.Ticket [TicketNumber],
				DT.TicketCode [Title],
				DT.[Subject] [RequestName],
				U.[User Name]
			FROM
				NeedHelpUheaa..DAT_Ticket DT
				LEFT JOIN
				(
					SELECT
						TA.Ticket,
						U.FirstName + ' ' + U.LastName [User Name]
					FROM
						NeedHelpUheaa..DAT_TicketsAssociatedUserID TA
						JOIN CSYS..SYSA_DAT_Users U
							ON TA.SqlUserId = U.SqlUserId
					WHERE
						TA.[Role] = 'Court'
				) U ON DT.Ticket = U.Ticket
		END
RETURN 0