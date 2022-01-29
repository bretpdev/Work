CREATE PROCEDURE [dbo].[AddApplication]
	@ApplicationName varchar(100),
	@AccessKey varchar(50),
	@StartingClass varchar(50),
	@StartingDll varchar(256),
	@AddedBy int,
	@SourcePath varchar(256)
AS
	INSERT INTO Applications(ApplicationName, AccessKey, AddedBy, SourcePath, StartingClass, StartingDll)
	VALUES(@ApplicationName, @AccessKey, @AddedBy, @SourcePath, @StartingClass, @StartingDll)

	SELECT CAST(SCOPE_IDENTITY() AS int)
RETURN 0

GRANT EXECUTE ON AddApplication TO db_executor