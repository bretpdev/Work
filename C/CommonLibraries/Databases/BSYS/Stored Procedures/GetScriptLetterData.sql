CREATE PROCEDURE [dbo].[GetScriptLetterData]
	@LetterId VARCHAR(10)

AS
	SELECT 
		MAP.LetterHeaderMappingId,
		MAP.HeaderId,
		H.Header,
		MAP.HeaderTypeId,
		HT.HeaderType,
		MAP.[Order],
		map.Active
	FROM
		LTDB_Letter_Header_Mapping MAP
		INNER JOIN LTDB_File_Headers H
			ON H.HeaderId = MAP.HeaderId
		INNER JOIN LTDB_LST_HeaderTypes HT
			ON HT.HeaderTypeId = MAP.HeaderTypeId
		INNER JOIN LTDB_DAT_DocDetail DD
			ON DD.DocDetailId = MAP.LetterId
			AND DD.ID = @LetterId
RETURN 0
