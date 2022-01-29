CREATE PROCEDURE [dbo].[LTDB_GetAllActiveHeaderTypes]
	
AS
	SELECT 
		HeaderTypeId,
		HeaderType
	FROM	
		LTDB_LST_HeaderTypes
	WHERE Active = 1
RETURN 0
