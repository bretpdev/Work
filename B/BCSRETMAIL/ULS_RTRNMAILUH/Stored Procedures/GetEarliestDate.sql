CREATE PROCEDURE [rtrnmailuh].[GetEarliestDate]
AS
	SELECT
		MIN(AddedAt)
	FROM
		rtrnmailuh.BarcodeData
RETURN 0