CREATE PROCEDURE [dbo].[Dtx7lGetDeletedRecords]
	
AS
	SELECT
		DTX7LDeletedRecordId,
		Ssn,
		Arc,
		RequestDate,
		LetterId,
		IsDueDiligence
	FROM
		DTX7LDeletedRecords
	WHERE
		ProcessedAt IS NULL
RETURN 0
