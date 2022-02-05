-- =============================================
-- Author:		Bret Pehrson
-- Create date: 08/29/2012
-- Description:	Updates user in the SYSA_DAT_Users table
-- =============================================
CREATE PROCEDURE [dbo].[spSYSA_UpdateUser] 
	-- Add the parameters for the stored procedure here
	@SqlUserID int = 0, 
	@WindowsUserName varchar(50) = '', 
	@FirstName varchar(50) = '', 
	@MiddleInitial char(1) = '', 
	@LastName varchar(50) = '', 
	@EMail varchar(100) = '', 
	@Extension varchar(4) = '', 
	@Extension2 varchar(4) = '', 
	@AesAccountId char(10) = '', 
	@BusinessUnit int = 0, 
	@Role int = 0,
	@Status varchar(50) = '',
	@Title varchar(50) = '',
	@AesUserId varchar(7) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE SYSA_DAT_Users
	SET WindowsUserName = @WindowsUserName
		, FirstName = @FirstName
		, MiddleInitial = @MiddleInitial
		, LastName = @LastName
		, EMail = @EMail
		, Extension = @Extension
		, Extension2 = @Extension2
		, AesAccountId = @AesAccountId
		, BusinessUnit = @BusinessUnit
		, [Role] = @Role
		, [Status] = @Status
		, Title = @Title
		, AesUserId = @AesUserId
	WHERE SqlUserId = @SqlUserID
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spSYSA_UpdateUser] TO [db_executor]
    AS [dbo];

