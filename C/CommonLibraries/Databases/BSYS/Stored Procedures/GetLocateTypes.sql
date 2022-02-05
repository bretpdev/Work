CREATE PROCEDURE [dbo].[GetLocateTypes]

AS
	SELECT 
		LocateType,
		ShortDescription,
		LongDescription
	FROM 
		dbo.GENR_LST_LocateTypes
RETURN 0
