CREATE PROCEDURE [cntrprnt].[GetEcorrLetterId]
	@Letter varchar(10)
AS

SELECT
	L.LetterId
FROM
	dbo.Letters L
WHERE
	L.Active = 1
	AND	L.Letter = @Letter

RETURN 0