CREATE PROCEDURE [trdprtyres].[GetRelationships]
	@IsOnelink BIT
AS
	SELECT
		Relationship,
		RelationshipCode
	FROM
		trdprtyres.Relationships
	WHERE
		IsOnelink = @IsOnelink
		AND DeletedAt IS NULL