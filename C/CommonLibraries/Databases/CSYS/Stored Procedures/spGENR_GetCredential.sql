
CREATE PROCEDURE [dbo].[spGENR_GetCredential]
	@CredentialKey	VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	dbo.Decryptor([Credential])
	FROM	dbo.GENR_DAT_Credentials
	WHERE	CredentialKey = @CredentialKey

	SET NOCOUNT OFF;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spGENR_GetCredential] TO [db_executor]
    AS [dbo];

