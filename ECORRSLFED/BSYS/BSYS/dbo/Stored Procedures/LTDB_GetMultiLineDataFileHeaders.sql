
CREATE PROCEDURE [dbo].[LTDB_GetMultiLineDataFileHeaders]
	@LetterId varchar(10),
	@Field VARCHAR(50)
AS
	DECLARE @ID INT = (SELECT DISTINCT DocDetailId FROM LTDB_DAT_DocDetail WHERE ID = @LetterId)
	SELECT
		FH.Header
	FROM
		LTDB_Letter_Header_Mapping MAP
	INNER JOIN LTDB_File_Headers FH
		ON FH.HeaderId = MAP.HeaderId
	WHERE
		MAP.LetterId = @ID
		and MAP.HeaderTypeId =  12

		
RETURN 0