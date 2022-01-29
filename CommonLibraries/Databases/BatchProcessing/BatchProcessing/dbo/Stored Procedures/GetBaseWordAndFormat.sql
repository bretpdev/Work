CREATE PROCEDURE [dbo].[GetBaseWordAndFormat]
	
AS
	SELECT 
		B.BatchPasswordBaseId,
		dbo.Decryptor(B.BaseWord) AS BaseWord,
		dbo.Decryptor(B.[Format]) AS [Format]
	FROM
		BatchPasswordBase B
	WHERE
		InactivatedAt IS NULL
RETURN 0
