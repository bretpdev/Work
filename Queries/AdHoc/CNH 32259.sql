USE [BSYS]
GO
/****** Object:  StoredProcedure [dbo].[LTDB_GetHeader]    Script Date: X/XX/XXXX XX:XX:XX AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[LTDB_GetHeader]
	@Letterid varchar(XX),
	@Type varchar(XX)
AS
	SELECT
		Header
	FROM
		[dbo].[LTDB_File_Headers] FH
		INNER JOIN [dbo].[LTDB_DAT_DocDetail] DD
			ON DD.ID = @Letterid
		INNER JOIN [dbo].[LTDB_LST_HeaderTypes] HT
			ON HT.HeaderType = @Type
			AND HT.Active = X
		INNER JOIN [dbo].[LTDB_Letter_Header_Mapping] LTM
			ON LTM.LetterId = DD.DocDetailId
			AND LTM.HeaderTypeId = HT.HeaderTypeId
			AND LTM.HeaderId = FH.HeaderId
			AND LTM.Active = X
	ORDER BY
		LTM.[Order]

		
RETURN X