CREATE PROCEDURE [dbo].[LTDB_GetClosingParagraph]
	@LetterId varchar(10)

AS
	SELECT 
		ClosingParagraph
	FROM
		LTDB_DAT_DocDetail
	WHERE
		ID = @LetterId
RETURN 0
