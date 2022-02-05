CREATE PROCEDURE [dbo].[InsertLenderUpdate]
	@Mod CHAR(1),
	@LenderId VARCHAR(6),
	@FullName VARCHAR(40),
	@ShortName VARCHAR(20),
	@Address1 VARCHAR(30),
	@Address2 VARCHAR(30) = NULL,
	@City VARCHAR(20),
	@State CHAR(2),
	@Zip VARCHAR(9),
	@Valid BIT,
	@Type CHAR(2) = NULL,
	@AddedBy VARCHAR(100)

AS
	INSERT INTO LenderUpdates([MOD], LenderId, FullName, ShortName, Address1, Address2, City, [State],Zip, Valid, [Type], AddedBy)
	VALUES(@Mod, @LenderId, @FullName, @ShortName, @Address1, @Address2, @City, @State, @Zip, @Valid, @Type, @AddedBy)
RETURN 0
