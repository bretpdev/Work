CREATE PROCEDURE [dbo].[GetApplicationArguments]
	@ApplicationID int
AS
	SELECT
		A.ArgumentId,
		ARG.Argument,
		ARG.ArgumentDescription,
		A.ArgumentOrder
	FROM
		[ApplicationArguments] A
		INNER JOIN ARGUMENTS ARG
			ON A.ArgumentId = ARG.ArgumentId
	WHERE
		A.ApplicationId = @ApplicationID
RETURN 0

GRANT EXECUTE ON [GetApplicationArguments] TO db_executor