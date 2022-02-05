CREATE PROCEDURE [dbo].[LTDB_SearchLetter]
	@LetterId varchar(10)

AS
	SELECT
		DocDetailId,
		ID AS LetterId,
		DocTyp,
		[Status]
	FROM 
		[dbo].[LTDB_DAT_DocDetail]
	WHERE
		ID LIKE @LetterId + '%'

RETURN 0

