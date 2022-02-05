-- =============================================
-- Author:		Bret Pehrson
-- Create date: 02/20/2013
-- Description:	Returns a list of users ID's and their full name
-- =============================================
CREATE PROCEDURE [dbo].[spGetUserIdAndFullName] 
	@Status bit
AS
BEGIN

	DECLARE @StatusText Varchar(50)
	
	SET @StatusText = 
		CASE WHEN @Status = 1
			THEN 'Active'
		ELSE
			'Inactive'
		END

	SET NOCOUNT ON;
	
	SELECT
		WindowsUserName,
		SqlUserId,
		FirstName + ' ' + LastName AS [FullName]
	FROM
		SYSA_DAT_Users
	WHERE
		[Status] = @StatusText
END
