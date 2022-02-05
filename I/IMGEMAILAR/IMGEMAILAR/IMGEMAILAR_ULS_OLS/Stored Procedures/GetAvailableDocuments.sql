CREATE PROCEDURE [imgemailar].[GetAvailableDocuments]
AS

	SELECT
		LetterId, OverrideDescription
	FROM
		imgemailar.AvailableDocuments

RETURN 0
