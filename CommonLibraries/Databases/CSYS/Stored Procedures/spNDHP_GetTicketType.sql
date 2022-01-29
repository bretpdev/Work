CREATE PROCEDURE [dbo].[spNDHP_GetTicketType]
	@FederalSystem		bit
AS
BEGIN
	SET NOCOUNT ON;
	SELECT FlowID AS Abbreviation, ControlDisplayText AS [Description], UserInterfaceDisplayIndicator
	FROM dbo.FLOW_DAT_Flow
	WHERE ([System] = 'Need Help General Help' AND @FederalSystem = 0 AND FlowID NOT LIKE '% FED')
	OR ([System] = 'Need Help General Help' AND @FederalSystem = 1 AND FlowID LIKE '% FED')
	GROUP BY UserInterfaceDisplayIndicator, FlowID, ControlDisplayText
	ORDER BY ControlDisplayText ASC
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spNDHP_GetTicketType] TO [db_executor]
    AS [dbo];

