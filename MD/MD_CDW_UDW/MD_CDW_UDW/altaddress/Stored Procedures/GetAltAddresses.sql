CREATE PROCEDURE [altaddress].[GetAltAddresses]
	@AccountNumber char(10)
AS

	select AlternateAddressId, Address1, Address2, City, State, Zip, Country
	from [altaddress].AlternateAddresses
	where AccountNumber = @AccountNumber

RETURN 0
