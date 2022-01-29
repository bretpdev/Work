-- =============================================
-- Author:		Eric Lynes
-- Create date: 3/9/2012
-- Description:	Decrypts a value on the fly
-- =============================================
CREATE FUNCTION [dbo].[Decryptor]
(
	@encValue varbinary(128)
)
RETURNS varchar(50)
AS
BEGIN	
	RETURN  
		CONVERT(VARCHAR (50),DECRYPTBYCERT(CERT_ID('USHE_Financial_Encryption_Certificate'), @encValue))
END