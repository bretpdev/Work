CREATE PROCEDURE [dbo].[GetLetterNameFromId]
	@ID VARCHAR(10)
AS
	SELECT
		DocName
	FROM
		BSYS..LTDB_DAT_DocDetail
	WHERE
		ID = @ID
RETURN 0