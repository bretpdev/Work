CREATE PROCEDURE [lnderlettr].[GetPendingWork]
AS
	SELECT
		LettersId,
		BF_SSN,
		CASE 
			WHEN RTRIM(WF_ORG_LDR) IN ('814817', '818334', '824421', '826079', '831495', '832733', '801871', '802176', '805317', '806746', '807674','811735') THEN '814817'
			ELSE RTRIM(WF_ORG_LDR)
		END AS WF_ORG_LDR,
		II_LDR_VLD_ADR,
		LDR_STR_ADR_1,
		InLenderList,
		[Population],
		LetterCreatedAt,
		ArcAddProcessingId,
		QueueClosedAt
	FROM
		[lnderlettr].Letters
	WHERE
		DeletedAt IS NULL
		AND	ErroredAt IS NULL
		AND 
		(
			(
				LetterCreatedAt IS NULL 
				AND 
				(
					InLenderList = 1 
					OR II_LDR_VLD_ADR = 'N'
				)
			)
			OR ISNULL(ArcAddProcessingId,0) = 0
			OR QueueClosedAt IS NULL
		)