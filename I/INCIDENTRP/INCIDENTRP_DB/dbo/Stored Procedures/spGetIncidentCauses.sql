-- =============================================
-- Author:		Daren Beattie
-- Create date: September 1, 2011
-- Description:	Returns a list of possible incident causes
-- =============================================
CREATE PROCEDURE [dbo].[spGetIncidentCauses]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Cause
	FROM LST_IncidentCause
END