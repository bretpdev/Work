CREATE PROCEDURE [dbo].[InsertPathType]
	@Description nvarchar(32),
	@RootPath nvarchar(256)
AS
	insert into dbo.PathTypes ([Description], RootPath)
	values (@Description, @RootPath)
SELECT cast(SCOPE_IDENTITY() as int)
