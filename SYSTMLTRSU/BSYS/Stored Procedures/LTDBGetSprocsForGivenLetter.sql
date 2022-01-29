CREATE PROCEDURE [dbo].[LTDBGetSprocsForGivenLetter]
	@LetterId VARCHAR(10)
AS
	SELECT 
		SystemLettersStoredProcedureId,
        dd.ID AS LetterId,
        StoredProcedureName,
        rt.ReturnType
    FROM
		LTDB_SystemLettersStoredProcedures SP
		INNER JOIN LTDB_DAT_DocDetail DD
			ON DD.DocDetailId = SP.LetterId
			AND DD.ID = @LetterId
		INNER JOIN LTDB_SystemLettersReturnType RT
			ON RT.ReturnTypeId = SP.ReturnTypeId
