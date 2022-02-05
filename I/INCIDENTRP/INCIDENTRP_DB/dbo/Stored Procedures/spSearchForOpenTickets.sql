-- =============================================
-- Author:		Daren Beattie
-- Create date: September 16, 2011
-- Description:	Searches for open tickets in the database.
-- =============================================
CREATE PROCEDURE [dbo].[spSearchForOpenTickets] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		CASE TK.TicketType WHEN 'Incident' THEN 'IR FED' ELSE 'THR FED' END AS TicketCode,
		TK.TicketNumber,
		CASE TK.TicketType WHEN 'Incident' THEN IR.Narrative ELSE INF.NatureOfCall END AS [Subject],
		TK.Status,
		TK.Priority,
		(
			SELECT MAX(UpdateDateTime)
			FROM DATHistory HIST
			WHERE HIST.TicketNumber = TK.TicketNumber
			AND HIST.TicketType = TK.TicketType
		) AS LastUpdateDate
	FROM DAT_Ticket TK
	LEFT OUTER JOIN DAT_Incident IR
		ON TK.TicketNumber = IR.TicketNumber
		AND TK.TicketType = IR.TicketType
	LEFT OUTER JOIN DAT_ThreatInfo INF
		ON TK.TicketNumber = INF.TicketNumber
		AND TK.TicketType = INF.TicketType
	WHERE TK.Status != 'Closed'
END