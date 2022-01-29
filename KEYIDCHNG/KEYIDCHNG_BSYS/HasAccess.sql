CREATE PROCEDURE [keyidchng].[HasAccess]
	@WindowsUsername VARCHAR(50)
AS

	IF EXISTS(SELECT * FROM [BSYS].[dbo].[GENR_REF_AuthAccess] WHERE [TypeKey] = 'Key ID Agent' AND WinUName = @WindowsUsername)
		SELECT CAST(1 AS BIT)
	ELSE
		SELECT CAST(0 AS BIT)

RETURN 0
