
-- =============================================
-- Author:		Eric Lynes
-- Create date: 3/9/2012
-- Description:	Decrypts a value on the fly
-- =============================================
CREATE FUNCTION [dbo].[Decryptor]
(
	@encValue varbinary(128)
)
RETURNS varchar(25)
AS
BEGIN	
	RETURN CONVERT(varchar(25), 
		DecryptByKeyAutoCert(cert_id('USHE_Financial_Encryption_Certificate'), null, @encValue))
END
