CREATE PROCEDURE [dbo].[InsertPathType]
	@Description NVARCHAR(32),
	@RootPath NVARCHAR(256)
AS
	
INSERT INTO dbo.PathTypes ([Description], RootPath)
VALUES (@Description, @RootPath)

SELECT CAST(SCOPE_IDENTITY() AS INT)