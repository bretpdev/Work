CREATE PROCEDURE [dbo].[LTDB_GetHeader]
	@Letterid varchar(10),
	@Type varchar(50)
AS
	SELECT
		Header
	FROM
		[dbo].[LTDB_File_Headers] FH
		INNER JOIN [dbo].[LTDB_DAT_DocDetail] DD
			ON DD.ID = @Letterid
		INNER JOIN [dbo].[LTDB_LST_HeaderTypes] HT
			ON HT.HeaderType = @Type
			AND HT.Active = 1
		INNER JOIN [dbo].[LTDB_Letter_Header_Mapping] LTM
			ON LTM.LetterId = DD.DocDetailId
			AND LTM.HeaderTypeId = HT.HeaderTypeId
			AND LTM.HeaderId = FH.HeaderId
			AND LTM.Active = 1

		
RETURN 0
