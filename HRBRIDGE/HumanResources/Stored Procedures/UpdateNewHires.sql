CREATE PROCEDURE [hrbridge].[UpdateNewHires]
AS
	UPDATE
		hrbridge.EmployeeCustomFields
	SET
		NeedsUpdated = 1,
		NewHire = 'No'
	WHERE
		HireDate <= DATEADD(DAY, -31, GETDATE()) 
		AND NewHire = 'Yes'
