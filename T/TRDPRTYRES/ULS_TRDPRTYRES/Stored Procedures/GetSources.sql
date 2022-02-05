CREATE PROCEDURE [trdprtyres].[GetSources]
	@IsOnelink BIT
AS
	SELECT
		Source,
		SourceCode
	FROM
		trdprtyres.Sources
	WHERE
		IsOnelink = @IsOnelink
		AND DeletedAt IS NULL