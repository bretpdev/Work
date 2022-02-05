
/********************************************************
*Routine Name	: [dbo].[spGENR_InsertUser]
*Purpose		: Insert new user into SYSA_DAT_Users table
*Used by		: Systems Support
*Revision History
*Version	Date		Person			Description
*=======	==========	============	================
*1.0.0		6/13/2012	Bret Pehrson
********************************************************/

CREATE PROCEDURE [dbo].[spGENR_InsertUser]
@WindowsUserName Varchar(50),
@FirstName Varchar(50),
@MiddleInitial Char(1) = '',
@LastName Varchar(50),
@EMail Varchar(100),
@Extension Varchar(4) = '',
@Extension2 Varchar(4) = '',
@BusinessUnit int,
@Role int,
@Status Varchar(50),
@Title Varchar(50),
@AesUserID Varchar(7)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO SYSA_DAT_Users(WindowsUserName
		, FirstName
		, MiddleInitial
		, LastName
		, EMail
		, Extension
		, Extension2
		, BusinessUnit
		, [Role]
		, [Status]
		, [Title]
		, AesUserId
	)
	VALUES(@WindowsUserName
		, @FirstName
		, @MiddleInitial
		, @LastName
		, @EMail
		, @Extension
		, @Extension2
		, @BusinessUnit
		, @Role
		, @Status
		, @Title
		, @AesUserID
	)

	SET NOCOUNT OFF;
END