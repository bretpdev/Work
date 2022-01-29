CREATE PROCEDURE [payhistlpp].[GetUserAccess]
AS
	SELECT
		UserAccessId,
		UserName
	FROM
		ULS.payhistlpp.UserAccess