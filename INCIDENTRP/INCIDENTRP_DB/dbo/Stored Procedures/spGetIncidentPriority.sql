-- =============================================
-- Author:		Daren Beattie
-- Create date: September 9, 2011
-- Description:	Retrieves the numeric priority for an incident based on the urgency.
-- =============================================
CREATE PROCEDURE [dbo].[spGetIncidentPriority]
	-- Add the parameters for the stored procedure here
	@Urgency VARCHAR(10) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT CAST(NumericValue AS INT)
	FROM LST_IncidentPriority
	WHERE Priority = @Urgency
END