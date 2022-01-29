CREATE PROCEDURE [altaddress].[AddAltAddress]
	@AccountNumber char(10),
	@Address1 nvarchar(1000),
	@Address2 nvarchar(1000),
	@City nvarchar(100),
	@State nvarchar(100),
	@Zip nvarchar(100),
	@Country nvarchar(100)
AS

	insert into [altaddress].AlternateAddresses (AccountNumber, Address1, Address2, City, State, Zip, Country)
	values (@AccountNumber, @Address1, @Address2, @City, @State, @Zip, @Country)

	select CAST(SCOPE_IDENTITY() as int) as AlternateAddressId

RETURN 0