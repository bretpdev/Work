USE [CDW]
GO
/****** Object:  StoredProcedure [dbo].[CornerStoneDocumentProcessing]    Script Date: X/XX/XXXX X:XX:XX AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CornerStoneDocumentProcessing]
	@startDate DATE,
	@endDate DATE
AS
BEGIN
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
/*	TESTING DATA		*/
		--DECLARE 
		--	@startDate DATE = 'XXXX-XX-X', 
		--	@endDate DATE = 'XXXX-XX-X'
/*	END TESTING DATA	*/
	SELECT
		CAST(PP.AddedAt as DATE) [RunDate],
		RTRIM(L.Letter) + ' (' + CAST(L.PagesPerDocument as VARCHAR(X)) + ')' [Letter Id],
		COUNT(PP.PrintProcessingId) [LetterCount],
		SUM(CASE 
				WHEN PP.ArcAddProcessingId IS NULL 
				THEN X 
				ELSE X 
			END) [SentToArcAddProcessing],
		SUM(CAST(PP.ArcNeeded as TINYINT)) [SentToArcAddProcessingExpected], 
		SUM(CASE 
				WHEN PP.EcorrDocumentCreatedAt IS NULL 
				THEN X 
				ELSE X 
			END) [EcorrDocsCreated],
		COUNT(PP.PrintProcessingId) [EcorrDocsCreatedExpected], --count all print processing records because everyting gets ecorred even when the borrower is not on ecorr
		SUM(CASE 
				WHEN PP.ImagedAt IS NULL THEN X 
				ELSE X 
			END) [DocsSentToImaging],
		SUM(CAST(PP.ImagingNeeded as TINYINT)) [DocsSentToImagingExpected],
		SUM(CASE 
			WHEN F.ForeignAddress = X THEN 
				CASE 
					WHEN PP.PrintedAt IS NULL THEN X 
					ELSE X 
				END 
			ELSE X 
			END) [SentToPrinterDomestic],
		SUM(CASE 
				WHEN F.ForeignAddress = X AND PP.OnEcorr = X THEN X 
				ELSE X 
			END) [SentToPrinterDomesticExpected],    
		SUM(CASE
				WHEN F.ForeignAddress = X AND PP.OnEcorr = X
				THEN 
					CASE WHEN PP.PrintedAt IS NULL THEN X ELSE X END 
					ELSE X 
			END) [SentToPrinterForeign],
		SUM(CASE 
				WHEN F.ForeignAddress = X AND PP.OnEcorr = X THEN X 
				ELSE X 
			END) [SentToPrinterForeignExpected]
	FROM
		CLS.[print].[PrintProcessing] PP
		INNER JOIN CLS.[print].[ScriptData] SD ON SD.ScriptDataId = PP.ScriptDataId
		INNER JOIN CLS.[print].Letters L ON L.LetterId = SD.LetterId
		INNER JOIN 
		(	-- create foreign address indicator
			SELECT
				PP.PrintProcessingId,
				CASE WHEN CLS.dbo.SplitAndRemoveQuotes(PP.LetterData, ',', FH.StateIndex , X) = '' THEN X ELSE X END [ForeignAddress] -- if the state is empty then it is a foreign address
			FROM
				CLS.[print].PrintProcessing PP
				INNER JOIN CLS.[print].[ScriptData] SD ON SD.ScriptDataId = PP.ScriptDataId
				INNER JOIN CLS.[print].[FileHeaders] FH ON FH.FileHeaderId = SD.FileHeaderId
		) F ON F.PrintProcessingId = PP.PrintProcessingId
	WHERE
		CAST(PP.AddedAt as DATE) BETWEEN @startDate AND @endDate
		AND
		SD.Active = X 
		AND
		PP.DeletedAt IS NULL
	GROUP BY
		CAST(PP.AddedAt as DATE),
		RTRIM(L.Letter) + ' (' + CAST(L.PagesPerDocument as VARCHAR(X)) + ')'


	UNION ALL


	SELECT DISTINCT
		--*
		CAST(LTXX.CreatedAt as DATE) [RunDate],
		RTRIM(LTXX.RM_DSC_LTR_PRC) + ' (' + ISNULL(CAST(L.PagesPerDocument as VARCHAR(X)), 'NA') + ')   ' + ISNULL(L.Instructions, '') [Letter Id],
		TOTAL.LETTER_COUNT AS [LetterCount],
		X [SentToArcAddProcessing],
		X [SentToArcAddProcessingExpected],
		ISNULL(EXPECTED_ECORR.LETTER_COUNT,X) AS [EcorrDocsCreated],
		ISNULL(TOTAL.LETTER_COUNT,X) AS [EcorrDocsCreatedExpected],
		X [DocsSentToImaging],
		X [DocsSentToImagingExpected],
		ISNULL(SENT_TO_PRINTER_DOM.DOMESTIC_COUNT,X) AS [SentToPrinterDomestic],
		ISNULL(PRINTED_DOM.DOMESTIC_COUNT,X) [SentToPrinterDomesticExpected],
		ISNULL(SENT_TO_PRINTER_FOR.[FOREIGN_COUNT],X) AS [SentToPrinterForeign],
		ISNULL(PRINTED_FOR.FOREIGN_COUNT,X) AS [SentToPrinterForeignExpected]
	FROM
		CDW..LTXX_LTR_REQ_PRC LTXX
		LEFT JOIN CLS..SystemLetterExclusions SLE
			ON SLE.LetterId = ltXX.RM_DSC_LTR_PRC
			AND SLE.SystemLetterExclusionReasonId = X
		LEFT JOIN CLS.[print].Letters L
			ON L.Letter = LTXX.RM_DSC_LTR_PRC
		LEFT JOIN 
		(
			SELECT
				DOM_POP.CreatedAt,
				DOM_POP.RM_DSC_LTR_PRC,
				COUNT(*) AS DOMESTIC_COUNT
			FROM
			(
				SELECT
					CAST(LTXX.CreatedAt AS DATE) AS CreatedAt,
					LTXX.RM_DSC_LTR_PRC, 
					COUNT(*)    AS LETTER_COUNT

				FROM 
					CDW..LTXX_LTR_REQ_PRC LTXX
					INNER JOIN CDW..PDXX_PRS_ADR PDXXD
						ON PDXXD.DF_PRS_ID = LTXX.RF_SBJ_PRC
						AND PDXXD.DC_ADR = 'L'
						AND PDXXD.DI_VLD_ADR = 'Y'
						AND LTRIM(RTRIM(PDXXD.DM_FGN_CNY)) = ''
				WHERE 
					CAST(LTXX.CREATEDAT AS DATE) BETWEEN @startDate AND @endDate
					AND LTXX.OnEcorr = X
					AND LTXX.PrintedAt IS NOT NULL
					AND LTXX.InactivatedAt IS NULL
				GROUP BY
					CAST(LTXX.CreatedAt AS DATE),
					RM_DSC_LTR_PRC, 
					RM_APL_PGM_PRC, 
					RT_RUN_SRT_DTS_PRC, 
					RN_SEQ_LTR_CRT_PRC
			) DOM_POP
			GROUP BY
				DOM_POP.CreatedAt,
				DOM_POP.RM_DSC_LTR_PRC
		)SENT_TO_PRINTER_DOM
			ON SENT_TO_PRINTER_DOM.RM_DSC_LTR_PRC = LTXX.RM_DSC_LTR_PRC
			AND SENT_TO_PRINTER_DOM.CreatedAt = CAST(LTXX.CreatedAt AS DATE)
		LEFT JOIN 
		(
			SELECT
				DOM_POP.CreatedAt,
				DOM_POP.RM_DSC_LTR_PRC,
				COUNT(*) AS [FOREIGN_COUNT]
			FROM
			(
				SELECT
					CAST(LTXX.CreatedAt AS DATE) AS CreatedAt,
					LTXX.RM_DSC_LTR_PRC, 
					COUNT(*)    AS LETTER_COUNT

				FROM 
					CDW..LTXX_LTR_REQ_PRC LTXX
					INNER JOIN CDW..PDXX_PRS_ADR PDXXD
						ON PDXXD.DF_PRS_ID = LTXX.RF_SBJ_PRC
						AND PDXXD.DC_ADR = 'L'
						AND PDXXD.DI_VLD_ADR = 'Y'
						AND LTRIM(RTRIM(PDXXD.DM_FGN_CNY)) != ''
				WHERE 
					CAST(LTXX.CREATEDAT AS DATE) BETWEEN @startDate AND @endDate
					AND LTXX.OnEcorr = X
					AND LTXX.PrintedAt IS NOT NULL
					AND LTXX.InactivatedAt IS NULL
				GROUP BY
					CAST(LTXX.CreatedAt AS DATE),
					RM_DSC_LTR_PRC, 
					RM_APL_PGM_PRC, 
					RT_RUN_SRT_DTS_PRC, 
					RN_SEQ_LTR_CRT_PRC
			) DOM_POP
			GROUP BY
				DOM_POP.CreatedAt,
				DOM_POP.RM_DSC_LTR_PRC
		)SENT_TO_PRINTER_FOR
			ON SENT_TO_PRINTER_FOR.RM_DSC_LTR_PRC = LTXX.RM_DSC_LTR_PRC
			AND SENT_TO_PRINTER_FOR.CreatedAt = CAST(LTXX.CreatedAt AS DATE)
		LEFT JOIN 
		(
			SELECT
				DOM_POP.CreatedAt,
				DOM_POP.RM_DSC_LTR_PRC,
				COUNT(*) AS DOMESTIC_COUNT
			FROM
			(
				SELECT
					CAST(LTXX.CreatedAt AS DATE) AS CreatedAt,
					LTXX.RM_DSC_LTR_PRC, 
					COUNT(*)    AS LETTER_COUNT

				FROM 
					CDW..LTXX_LTR_REQ_PRC LTXX
					INNER JOIN CDW..PDXX_PRS_ADR PDXXD
						ON PDXXD.DF_PRS_ID = LTXX.RF_SBJ_PRC
						AND PDXXD.DC_ADR = 'L'
						AND LTRIM(RTRIM(PDXXD.DM_FGN_CNY)) = ''
				WHERE 
					CAST(LTXX.CREATEDAT AS DATE) BETWEEN @startDate AND @endDate
					AND LTXX.OnEcorr = X
					AND LTXX.InactivatedAt IS NULL
				GROUP BY
					CAST(LTXX.CreatedAt AS DATE),
					RM_DSC_LTR_PRC, 
					RM_APL_PGM_PRC, 
					RT_RUN_SRT_DTS_PRC, 
					RN_SEQ_LTR_CRT_PRC
			) DOM_POP
			GROUP BY
				DOM_POP.CreatedAt,
				DOM_POP.RM_DSC_LTR_PRC
		)PRINTED_DOM
			ON PRINTED_DOM.RM_DSC_LTR_PRC = LTXX.RM_DSC_LTR_PRC
			AND PRINTED_DOM.CreatedAt = CAST(LTXX.CreatedAt AS DATE)
		LEFT JOIN 
		(
			SELECT
				DOM_POP.CreatedAt,
				DOM_POP.RM_DSC_LTR_PRC,
				COUNT(*) AS [FOREIGN_COUNT]
			FROM
			(
				SELECT
					CAST(LTXX.CreatedAt AS DATE) AS CreatedAt,
					LTXX.RM_DSC_LTR_PRC, 
					COUNT(*)    AS LETTER_COUNT

				FROM 
					CDW..LTXX_LTR_REQ_PRC LTXX
					INNER JOIN CDW..PDXX_PRS_ADR PDXXD
						ON PDXXD.DF_PRS_ID = LTXX.RF_SBJ_PRC
						AND PDXXD.DC_ADR = 'L'
						AND LTRIM(RTRIM(PDXXD.DM_FGN_CNY)) != ''
				WHERE 
					CAST(LTXX.CREATEDAT AS DATE) BETWEEN @startDate AND @endDate
					AND LTXX.OnEcorr = X
					AND LTXX.InactivatedAt IS NULL
				GROUP BY
					CAST(LTXX.CreatedAt AS DATE),
					RM_DSC_LTR_PRC, 
					RM_APL_PGM_PRC, 
					RT_RUN_SRT_DTS_PRC, 
					RN_SEQ_LTR_CRT_PRC
			) DOM_POP
			GROUP BY
				DOM_POP.CreatedAt,
				DOM_POP.RM_DSC_LTR_PRC
		)PRINTED_FOR
			ON PRINTED_FOR.RM_DSC_LTR_PRC = LTXX.RM_DSC_LTR_PRC
			AND PRINTED_FOR.CreatedAt = CAST(LTXX.CreatedAt AS DATE)
		LEFT JOIN 
		(
			SELECT
				CAST(LTXX.CreatedAt AS DATE) AS CreatedAt,
				LTXX.RM_DSC_LTR_PRC, 
				COUNT(RN_SEQ_LTR_CRT_PRC) AS LETTER_COUNT
			FROM 
				CDW..LTXX_LTR_REQ_PRC LTXX
			WHERE 
				CAST(LTXX.CREATEDAT AS DATE) BETWEEN @startDate AND @endDate
				AND LTXX.EcorrDocumentCreatedAt IS NOT NULL
				AND LTXX.InactivatedAt IS NULL
			GROUP BY 
				RM_DSC_LTR_PRC,
				LTXX.CreatedAt
		)EXPECTED_ECORR
			ON EXPECTED_ECORR.RM_DSC_LTR_PRC = LTXX.RM_DSC_LTR_PRC
			AND EXPECTED_ECORR.CreatedAt = CAST(LTXX.CreatedAt AS DATE)
		LEFT JOIN
		(
			SELECT
				CAST(LTXX.CreatedAt AS DATE) AS CreatedAt,
				LTXX.RM_DSC_LTR_PRC, 
				COUNT( RN_SEQ_LTR_CRT_PRC) AS LETTER_COUNT
			FROM 
				CDW..LTXX_LTR_REQ_PRC LTXX 
			WHERE 
				  CAST(LTXX.CREATEDAT AS DATE) BETWEEN @startDate AND @endDate
				  AND LTXX.InactivatedAt IS NULL
			GROUP BY 
				LTXX.RM_DSC_LTR_PRC,
				LTXX.CreatedAt
		) TOTAL
			ON TOTAL.RM_DSC_LTR_PRC = LTXX.RM_DSC_LTR_PRC
			AND TOTAL.CreatedAt = CAST(LTXX.CreatedAt AS DATE)
	WHERE
		 CAST(LTXX.CreatedAt as DATE) BETWEEN @startDate AND @endDate
		 AND LTXX.InactivatedAt IS NULL
		 AND SLE.LetterId IS NULL
	ORDER BY
		RunDate

END