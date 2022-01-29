CREATE PROCEDURE [dbo].[GetSprocsForGivenLetter]
	@LetterId int
	
AS
	SELECT
		SP.SystemLettersStoredProcedureId,
		SP.StoredProcedureName,
		SPRT.ReturnType,
		SP.Active
	FROM
		[dbo].[LTDB_SystemLettersStoredProcedures] SP
	INNER JOIN [dbo].[LTDB_SystemLettersReturnType] SPRT
		ON SP.ReturnTypeId = SPRT.ReturnTypeId
	WHERE
		LetterId = @LetterId
RETURN 0
