
CREATE PROCEDURE [dbo].[CheckIfLetterExists]
	@Letter varchar(10)
AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		Letter
	FROM
		Letters
	WHERE
		Letter = @Letter

END

