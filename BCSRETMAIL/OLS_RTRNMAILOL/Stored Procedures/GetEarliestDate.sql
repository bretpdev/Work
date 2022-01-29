CREATE PROCEDURE [rtrnmailol].[GetEarliestDate]
AS
	SELECT
		MIN(AddedAt)
	FROM
		rtrnmailol.BarcodeData
RETURN 0