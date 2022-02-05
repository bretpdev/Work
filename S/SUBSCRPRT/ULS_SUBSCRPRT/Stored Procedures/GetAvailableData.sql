CREATE PROCEDURE [subscrprt].[GetAvailableData]
AS
	SELECT
		PrintDataId,
		BF_SSN,
		CLUID
	FROM
		subscrprt.PrintData
	WHERE
		ProcessedAt IS NULL
		AND DeletedAt IS NULL
	ORDER BY
		DM_PRS_LST
RETURN 0