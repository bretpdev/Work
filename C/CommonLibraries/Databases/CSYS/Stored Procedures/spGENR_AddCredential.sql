
CREATE PROCEDURE [dbo].[spGENR_AddCredential]
	@CredentialKey	VARCHAR(50),
	@Credential		VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	OPEN SYMMETRIC KEY USHE_Financial_Data_Key DECRYPTION BY CERTIFICATE USHE_Financial_Encryption_Certificate;
	
	DECLARE @EncryptedCredential	VARBINARY(128)

	SET @EncryptedCredential = ENCRYPTBYKEY(Key_GUID('USHE_Financial_Data_Key'), @Credential)

	INSERT INTO dbo.GENR_DAT_Credentials ([CredentialKey], [Credential])
	VALUES (@CredentialKey, @EncryptedCredential)

	SET NOCOUNT OFF;
	CLOSE SYMMETRIC KEY USHE_Financial_Data_Key;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_AddCredential] TO [db_executor]
    AS [dbo];

