CREATE PROCEDURE [dbo].[GetAllSystemLetterReturnTypes]

AS
	SELECT
		ReturnTypeId,
		ReturnType
	FROM	
		[dbo].[LTDB_SystemLettersReturnType]

RETURN 0
