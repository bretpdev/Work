CREATE PROCEDURE [altaddress].[UpdateAltAddress]
	@AlternateAddressId int,
	@Address1 nvarchar(1000),
	@Address2 nvarchar(1000),
	@City nvarchar(100),
	@State nvarchar(100),
	@Zip nvarchar(100),
	@Country nvarchar(100)
AS

	update [altaddress].AlternateAddresses
	set Address1 = @Address1, Address2 = @Address2, City = @City, State = @State, Zip = @Zip, Country = @Country
	where AlternateAddressId = @AlternateAddressId

RETURN 0