CREATE PROCEDURE [dbo].[GetEncryptedCredential]
	@EncryptedCredentialId int
AS
	
select 
	EncryptedCredential 
from 
	EncryptedCredentials
where
	EncryptedCredentialId = @EncryptedCredentialId


RETURN 0
