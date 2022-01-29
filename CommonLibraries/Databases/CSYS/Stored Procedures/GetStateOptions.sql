CREATE PROCEDURE [dbo].[GetStateOptions]

AS
	SELECT 
		Code,
		shortDesc,
		[Description],
		IsDomestic
	FROM 
		GENR_LST_States
RETURN 0
