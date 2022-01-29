CREATE PROCEDURE [dbo].[LTDB_GetAllMultiLineFieldsForLetter]
	@LetterId varchar(10)
AS
	DECLARE @ID INT = (SELECT DISTINCT DocDetailId FROM LTDB_DAT_DocDetail WHERE ID = @LetterId)
	SELECT
		FH.Header
	FROM
		LTDB_Letter_Header_Mapping MAP
	INNER JOIN LTDB_File_Headers FH
		ON FH.HeaderId = MAP.HeaderId
	WHERE
		LetterId = @ID
		AND HeaderTypeId = 15
RETURN 0
