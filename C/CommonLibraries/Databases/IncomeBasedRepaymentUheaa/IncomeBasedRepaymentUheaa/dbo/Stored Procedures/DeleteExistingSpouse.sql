-- =============================================
-- Author:		Jarom Ryan
-- Create date: 09/04/2013
-- Description:	will delete a spouse from the Applications table and Spouse table
-- =============================================
CREATE PROCEDURE [dbo].[DeleteExistingSpouse]

@SpouseId INT

AS
BEGIN

	SET NOCOUNT ON;
	
	UPDATE
		dbo.Applications
	SET spouse_id = NULL
	WHERE spouse_id = @SpouseId
	
	
	DELETE 
	FROM
		 dbo.Spouses
	WHERE 
		spouse_id = @SpouseId
END
