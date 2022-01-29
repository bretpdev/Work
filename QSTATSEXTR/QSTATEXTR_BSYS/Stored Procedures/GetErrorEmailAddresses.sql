CREATE PROCEDURE [qstatsextr].[GetErrorEmailAddresses]
AS
	
	SELECT 
		WinUName 
	FROM 
		GENR_REF_MiscEmailNotif 
	WHERE 
		TypeKey = 'QStats Error Email'

RETURN 0
