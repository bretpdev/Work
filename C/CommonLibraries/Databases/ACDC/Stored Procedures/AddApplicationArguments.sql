CREATE PROCEDURE [dbo].[AddApplicationArguments]
	@ApplicationId int,
	@ArgumentId int,
	@ArgumentOrder int
AS
	INSERT INTO ApplicationArguments(ApplicationId, ArgumentId, ArgumentOrder)
	VALUES(@ApplicationId, @ArgumentId, @ArgumentOrder)
RETURN 0

GRANT EXECUTE ON AddApplicationArguments TO db_executor