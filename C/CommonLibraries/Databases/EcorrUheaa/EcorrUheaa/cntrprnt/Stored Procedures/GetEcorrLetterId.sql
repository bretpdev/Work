CREATE PROCEDURE [cntrprnt].[GetEcorrLetterId]
	@Letter varchar(10)
AS

SELECT
	l.LetterId
FROM
	dbo.Letters l
WHERE
	Active = 1
AND
	Letter = @Letter

RETURN 0