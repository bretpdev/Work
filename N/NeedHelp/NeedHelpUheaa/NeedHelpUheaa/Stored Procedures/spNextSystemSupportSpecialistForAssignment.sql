CREATE PROCEDURE [dbo].[spNextSystemSupportSpecialistForAssignment]
	@AssignmentOption	VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT TOP 2 A.SqlUserId,
		COALESCE(B.TheMax, 0) AS TheMax
	FROM LST_AssignToCalculationOption AS A 
	LEFT JOIN
		(
			SELECT --TOP 20 
				DAT_TicketsAssociatedUserID.SqlUserId,
				MAX(DAT_Ticket.Ticket) AS TheMax
			FROM DAT_Ticket
			INNER JOIN DAT_TicketsAssociatedUserID
				ON DAT_Ticket.Ticket = DAT_TicketsAssociatedUserID.Ticket
			WHERE (DAT_TicketsAssociatedUserID.[Role] = 'AssignedTo')
			GROUP BY DAT_TicketsAssociatedUserID.SqlUserId
			--ORDER BY TheMax DESC
		) AS B ON A.SqlUserId = B.SqlUserId
	WHERE A.OptionCode = @AssignmentOption
	ORDER BY TheMax DESC
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spNextSystemSupportSpecialistForAssignment] TO [db_executor]
    AS [dbo];

