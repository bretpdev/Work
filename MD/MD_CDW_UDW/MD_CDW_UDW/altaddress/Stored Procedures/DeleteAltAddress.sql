CREATE PROCEDURE [altaddress].[DeleteAltAddress]
	@AlternateAddressId int
AS

	delete from [altaddress].AlternateAddresses 
	where AlternateAddressId = @AlternateAddressId

RETURN 0
