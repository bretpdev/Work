CREATE PROCEDURE [dbo].[InsertProject]
	@Name NVARCHAR(MAX),
	@Notes NVARCHAR(MAX)
AS
	
INSERT INTO Projects ([Name], Notes)
VALUES (@Name, @Notes)

SELECT SCOPE_IDENTITY()
