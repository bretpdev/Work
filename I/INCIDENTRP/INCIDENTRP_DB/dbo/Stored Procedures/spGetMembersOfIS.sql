-- =============================================
-- Author:		Bret Pehrson
-- Create date: 6/19/13
-- Description:	Returns a list of all the users in the Information Security BU
-- =============================================
CREATE PROCEDURE spGetMembersOfIS
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT
		FirstName + ' ' + LastName
	FROM
		CSYS.dbo.SYSA_DAT_Users
	WHERE
		BusinessUnit = 16
		OR BusinessUnit = 9
		AND [Status] = 'Active'
END