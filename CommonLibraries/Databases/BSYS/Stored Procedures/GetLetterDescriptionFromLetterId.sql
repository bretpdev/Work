CREATE PROCEDURE [dbo].[GetLetterDescriptionFromLetterId]
	@LetterId VARCHAR(10)
AS
	SELECT 
		DocName
	FROM
		LTDB_DAT_DocDetail
	WHERE
		ID = @LetterId
		AND [Status] = 'Active'
RETURN 0
