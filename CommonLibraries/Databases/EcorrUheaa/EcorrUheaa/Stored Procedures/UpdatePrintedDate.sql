-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/28/2013
-- Description:	Updates a given ecorr documents printed date
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePrintedDate]

@DocumentDetailsId int

AS
BEGIN

	SET NOCOUNT ON;

	UPDATE 
		[dbo].[DocumentDetails]
	SET
		[Printed] = GETDATE()
	WHERE 
		[DocumentDetailsId] = @DocumentDetailsId
	

END
