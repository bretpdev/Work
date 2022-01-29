CREATE FUNCTION [dbo].[Encrypt]
(
	@EncryptValue VARCHAR(50)
)
RETURNS VARBINARY(128)
AS
BEGIN
	RETURN EncryptByCert(Cert_ID('USHE_Financial_Encryption_Certificate'), @EncryptValue)
END
