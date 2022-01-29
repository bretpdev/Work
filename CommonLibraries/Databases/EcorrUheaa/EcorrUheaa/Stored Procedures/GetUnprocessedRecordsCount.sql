-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/28/2013
-- Description:	Will Gather all of the unprocessed records from the e corr db
-- =============================================
CREATE PROCEDURE [dbo].[GetUnprocessedRecordsCount] 

AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		count(*)

	FROM 
		[dbo].[DocumentDetails] DD
	INNER JOIN [dbo].[Letters] LTRS
		ON LTRS.LetterId = DD.LetterId
	WHERE 
		[Printed] IS NULL
END

