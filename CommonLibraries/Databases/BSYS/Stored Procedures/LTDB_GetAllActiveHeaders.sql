CREATE PROCEDURE [dbo].[LTDB_GetAllActiveHeaders]

AS
	SELECT 
		HeaderId,
		Header
	FROM	
		LTDB_File_Headers
	WHERE Active = 1
RETURN 0
