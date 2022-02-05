
CREATE PROCEDURE [dbo].[LTDBGetSprocsForGivenLetter]
	@LetterId varchar(10)

AS
	SELECT 
		[SystemLettersStoredProcedureId],
        dd.ID AS LetterId,
        [StoredProcedureName],
        rt.ReturnType
    FROM
		[BSYS].[dbo].[LTDB_SystemLettersStoredProcedures] SP
		INNER JOIN [dbo].[LTDB_DAT_DocDetail] DD
			ON DD.DocDetailId = SP.LetterId
			AND DD.ID = @LetterId
		INNER JOIN [dbo].[LTDB_SystemLettersReturnType] RT
			ON RT.ReturnTypeId = SP.ReturnTypeId
RETURN 0