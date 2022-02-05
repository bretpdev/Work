-- =============================================
-- Author:		Daren Beattie
-- Create date: October 3, 2011
-- Description:	Retrieves the list of relationships from the LST_Relationship table.
-- =============================================
CREATE PROCEDURE [dbo].[spGetRelationships]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Relationship
	FROM LST_Relationship
END