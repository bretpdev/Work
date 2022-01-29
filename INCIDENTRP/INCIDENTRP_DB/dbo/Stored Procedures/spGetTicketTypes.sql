-- =============================================
-- Author:		Daren Beattie
-- Create date: August 16, 2011
-- Description:	Retrieves the full list of ticket types.
-- =============================================
CREATE PROCEDURE [dbo].[spGetTicketTypes]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Description], FlowID AS Abbreviation
	FROM CSYS.dbo.FLOW_DAT_Flow
	WHERE [System] = 'Incident Reporting Module'
	AND FlowID LIKE('% Fed')
END