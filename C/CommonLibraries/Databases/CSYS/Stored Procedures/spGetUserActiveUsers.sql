-- =============================================
-- Author:		Bret Pehrson
-- Create date: 02/12/2013
-- Description:	Returns a list of active users and their SqlUserID
-- =============================================
CREATE PROCEDURE spGetUserActiveUsers 

AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT
		SqlUserId,
		FirstName + ' ' + LastName
	FROM
		SYSA_DAT_Users
	WHERE
		[Status] = 'Active' --Get only active employees
END
