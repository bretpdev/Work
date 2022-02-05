
CREATE PROCEDURE getLastName
	-- Add the parameters for the stored procedure here
	@userId varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT LastName FROM SYSA_LST_Users WHERE WindowsUserName = @userId
END