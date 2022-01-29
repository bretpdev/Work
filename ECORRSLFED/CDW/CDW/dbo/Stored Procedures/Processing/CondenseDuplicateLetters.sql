CREATE PROCEDURE [dbo].[CondenseDuplicateLetters]
AS

DECLARE @DuplicateReasonId INT = (SELECT SystemLetterExclusionReasonId FROM CLS..SystemLetterExclusionReasons WHERE SystemLetterExclusionReason = 'Duplicate Removal')

--We are inactivating all but the most current record for the run. The form field/loan detail sproc should then get 
-- ay10 records that map to all the loans that need to go onto the document
UPDATE 
	LT20
SET
	InactivatedAt = GETDATE(),
	SystemLetterExclusionReasonId = @DuplicateReasonId
FROM
	CDW..LT20_LTR_REQ_PRC LT20
	INNER JOIN
	(	
		SELECT
			LT20.DF_SPE_ACC_ID,
			DL.Letter,
			MAX(LT20.RT_RUN_SRT_DTS_PRC) AS RT_RUN_SRT_DTS_PRC
		FROM
			CLS.dbo.DuplicateLetters DL
			INNER JOIN CDW..LT20_LTR_REQ_PRC LT20
				ON DL.Letter = LT20.RM_DSC_LTR_PRC
		WHERE
			LT20.InactivatedAt IS NULL
			AND LT20.EcorrDocumentCreatedAt IS NULL
			AND LT20.PrintedAt IS NULL
		GROUP BY
			LT20.DF_SPE_ACC_ID,
			DL.Letter,
			CAST(LT20.RT_RUN_SRT_DTS_PRC AS DATE)
		HAVING
			COUNT(*) > 1
	) EXCLUDE_LETTERS
		ON LT20.DF_SPE_ACC_ID = EXCLUDE_LETTERS.DF_SPE_ACC_ID
		AND LT20.RM_DSC_LTR_PRC = EXCLUDE_LETTERS.Letter
		AND CAST(LT20.RT_RUN_SRT_DTS_PRC AS DATE) = CAST(EXCLUDE_LETTERS.RT_RUN_SRT_DTS_PRC AS DATE)
		AND LT20.RT_RUN_SRT_DTS_PRC != EXCLUDE_LETTERS.RT_RUN_SRT_DTS_PRC
WHERE
	LT20.InactivatedAt IS NULL
	AND LT20.EcorrDocumentCreatedAt IS NULL
	AND LT20.PrintedAt IS NULL

UPDATE 
	LT20
SET
	InactivatedAt = GETDATE(),
	SystemLetterExclusionReasonId = @DuplicateReasonId
FROM
	CDW..LT20_LTR_REQ_PRC_Coborrower LT20
	INNER JOIN
	(	
		SELECT
			LT20.DF_SPE_ACC_ID,
			DL.Letter,
			MAX(LT20.RT_RUN_SRT_DTS_PRC) AS RT_RUN_SRT_DTS_PRC
		FROM
			CLS.dbo.DuplicateLetters DL
			INNER JOIN CDW..LT20_LTR_REQ_PRC_Coborrower LT20
				ON DL.Letter = LT20.RM_DSC_LTR_PRC
		WHERE
			LT20.InactivatedAt IS NULL
			AND LT20.EcorrDocumentCreatedAt IS NULL
			AND LT20.PrintedAt IS NULL
		GROUP BY
			LT20.DF_SPE_ACC_ID,
			DL.Letter,
			CAST(LT20.RT_RUN_SRT_DTS_PRC AS DATE)
		HAVING
			COUNT(*) > 1
	) EXCLUDE_LETTERS
		ON LT20.DF_SPE_ACC_ID = EXCLUDE_LETTERS.DF_SPE_ACC_ID
		AND LT20.RM_DSC_LTR_PRC = EXCLUDE_LETTERS.Letter
		AND CAST(LT20.RT_RUN_SRT_DTS_PRC AS DATE) = CAST(EXCLUDE_LETTERS.RT_RUN_SRT_DTS_PRC AS DATE)
		AND LT20.RT_RUN_SRT_DTS_PRC != EXCLUDE_LETTERS.RT_RUN_SRT_DTS_PRC
WHERE
	LT20.InactivatedAt IS NULL
	AND LT20.EcorrDocumentCreatedAt IS NULL
	AND LT20.PrintedAt IS NULL