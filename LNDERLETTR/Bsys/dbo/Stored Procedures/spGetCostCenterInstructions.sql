-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/04/2012
-- Description:	The sp will get Cost Center Instructions from the Letter Tracking DB
-- =============================================
CREATE PROCEDURE [dbo].[spGetCostCenterInstructions] 

	@letterId As Varchar(10)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		COALESCE(Instructions, '') AS Instructions 
	FROM 
		LTDB_DAT_CentralPrintingDocData 
	WHERE 
		ID = @letterId

END