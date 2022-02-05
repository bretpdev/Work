-- =============================================
-- Author:		Bret Pehrson
-- Create date: 8/6/2013
-- Description:	Checks a users access to a given stored procedure
-- =============================================
CREATE PROCEDURE CheckSprocAccess 
	-- Add the parameters for the stored procedure here
	@Database varchar(25),
	@SprocName varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM fn_my_permissions(@Database + '.dbo.' + @SprocName, 'OBJECT') where permission_name='EXECUTE';
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckSprocAccess] TO [db_executor]
    AS [dbo];



