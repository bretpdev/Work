CREATE PROCEDURE dbo.[GetMaritalStatus]
AS
BEGIN

	SELECT
		marital_status_id [MaritalStatusId],
		[status] [Status]
	FROM
		dbo.Marital_Status
END