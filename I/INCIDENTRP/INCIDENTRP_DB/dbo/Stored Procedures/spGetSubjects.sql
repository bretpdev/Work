-- =============================================
-- Author:		Daren Beattie
-- Create date: September 9, 2011
-- Description:	Retrieves the subject of all tickets.
-- =============================================
CREATE PROCEDURE [dbo].[spGetSubjects]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CASE TK.TicketType WHEN 'Incident' THEN IR.Narrative ELSE INF.NatureOfCall END AS [Subject]
	FROM DAT_Ticket TK
	LEFT OUTER JOIN DAT_Incident IR
		ON TK.TicketNumber = IR.TicketNumber
		AND TK.TicketType = IR.TicketType
	LEFT OUTER JOIN DAT_ThreatInfo INF
		ON TK.TicketNumber = INF.TicketNumber
		AND TK.TicketType = INF.TicketType
END