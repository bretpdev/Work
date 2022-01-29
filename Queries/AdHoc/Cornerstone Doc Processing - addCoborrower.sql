USE [CDW]
GO
/****** Object:  StoredProcedure [dbo].[CornerStoneDocumentProcessing]    Script Date: 1/18/2019 1:28:01 PM ******/
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
		--	@startDate DATE = '2017-10-2', 
		--	@endDate DATE = '2017-10-4'
/*	END TESTING DATA	*/
	SELECT
		CAST(PP.AddedAt as DATE) [RunDate],
		L.Letter + ' (' + CAST(L.PagesPerDocument as VARCHAR(2)) + ')' [Letter Id],
		COUNT(PP.PrintProcessingId) [LetterCount],
		SUM(CASE 
				WHEN PP.ArcAddProcessingId IS NULL 
				THEN 0 
				ELSE 1 
			END) [SentToArcAddProcessing],
		SUM(CAST(PP.ArcNeeded as TINYINT)) [SentToArcAddProcessingExpected], 
		SUM(CASE 
				WHEN PP.EcorrDocumentCreatedAt IS NULL 
				THEN 0 
				ELSE 1 
		END) [EcorrDocsCreated],
		COUNT(PP.PrintProcessingId) [EcorrDocsCreatedExpected], --count all print processing records because everyting gets ecorred even when the borrower is not on ecorr
		SUM(CASE 
				WHEN PP.ImagedAt IS NULL THEN 0 
				ELSE 1 
		END) [DocsSentToImaging],
		SUM(CAST(PP.ImagingNeeded as TINYINT)) [DocsSentToImagingExpected],
		SUM(CASE 
				WHEN F.ForeignAddress = 0 THEN 
					CASE 
						WHEN PP.PrintedAt IS NULL THEN 0 
						ELSE 1 
					END 
				ELSE 0 
		END) [SentToPrinterDomestic],
		SUM(CASE 
				WHEN F.ForeignAddress = 0 AND PP.OnEcorr = 0 THEN 1 
				ELSE 0 
		END) [SentToPrinterDomesticExpected],    
		SUM(CASE
				WHEN F.ForeignAddress = 1 AND PP.OnEcorr = 0
				THEN 
					CASE WHEN PP.PrintedAt IS NULL THEN 0 ELSE 1 END 
							ELSE 0 
		END) [SentToPrinterForeign],
		SUM(CASE 
				WHEN F.ForeignAddress = 1 AND PP.OnEcorr = 0 THEN 1 
				ELSE 0 
		END) [SentToPrinterForeignExpected]
	FROM
		   CLS.[print].[PrintProcessing] PP
		   INNER JOIN CLS.[print].[ScriptData] SD ON SD.ScriptDataId = PP.ScriptDataId
		   INNER JOIN CLS.[print].Letters L ON L.LetterId = SD.LetterId
		   INNER JOIN 
		   (-- create foreign address indicator
				SELECT
					PP.PrintProcessingId,
					CASE WHEN CLS.dbo.SplitAndRemoveQuotes(PP.LetterData, ',', FH.StateIndex , 1) = '' THEN 1 ELSE 0 END [ForeignAddress] -- if the state is empty then it is a foreign address
				FROM
					CLS.[print].PrintProcessing PP
					INNER JOIN CLS.[print].[ScriptData] SD ON SD.ScriptDataId = PP.ScriptDataId
					INNER JOIN CLS.[print].[FileHeaders] FH ON FH.FileHeaderId = SD.FileHeaderId
			) F ON F.PrintProcessingId = PP.PrintProcessingId
	WHERE
		   CAST(PP.AddedAt as DATE) BETWEEN @startDate AND @endDate
		   AND
		   SD.Active = 1 
		   AND
		   PP.DeletedAt IS NULL
	GROUP BY
		   CAST(PP.AddedAt as DATE),
		   L.Letter + ' (' + CAST(L.PagesPerDocument as VARCHAR(2)) + ')'

	UNION ALL

	SELECT DISTINCT
		CAST(lt20.CreatedAt as DATE) [RunDate],
		LT20.RM_DSC_LTR_PRC + ' (' + ISNULL(CAST(L.PagesPerDocument as VARCHAR(2)), 'NA') + ')' [Letter Id],
		TOTAL.LETTER_COUNT AS [LetterCount],
		0 [SentToArcAddProcessing],
		0 [SentToArcAddProcessingExpected],
		ISNULL(EXPECTED_ECORR.LETTER_COUNT,0) AS [EcorrDocsCreated],
		ISNULL(TOTAL.LETTER_COUNT,0) AS [EcorrDocsCreatedExpected],
		0 [DocsSentToImaging],
		0 [DocsSentToImagingExpected],
		ISNULL(SENT_TO_PRINTER.DOMESTIC,0) AS [SentToPrinterDomestic],
		ISNULL(PRINTED_TOTAL.DOMESTIC_PRINTED,0) [SentToPrinterDomesticExpected],
		ISNULL(SENT_TO_PRINTER.[FOREIGN],0) AS [SentToPrinterForeign],
		ISNULL(PRINTED_TOTAL.FOREIGN_PRINTED,0) AS [SentToPrinterForeignExpected]
	FROM
		CDW..LT20_LTR_REQ_PRC LT20
		LEFT JOIN CLS.[print].Letters L
			on L.Letter = LT20.RM_DSC_LTR_PRC
		LEFT JOIN 
		(
			SELECT
				CAST(LT20.CreatedAt AS DATE) AS CreatedAt,
				LT20.RM_DSC_LTR_PRC, 
				COUNT(PD30D.DF_PRS_ID) + COUNT(PD30F.DF_PRS_ID) AS LETTER_COUNT,
				COUNT(PD30D.DF_PRS_ID) AS DOMESTIC,
				COUNT(PD30F.DF_PRS_ID) AS [FOREIGN]
			FROM 
				CDW..LT20_LTR_REQ_PRC LT20
				LEFT JOIN CDW..PD30_PRS_ADR PD30D
					ON PD30D.DF_PRS_ID = LT20.RF_SBJ_PRC
					AND PD30D.DC_ADR = 'L'
					AND LTRIM(RTRIM(PD30D.DM_FGN_CNY)) = ''
				LEFT JOIN CDW..PD30_PRS_ADR PD30F
					ON PD30F.DF_PRS_ID = LT20.RF_SBJ_PRC
					AND PD30F.DC_ADR = 'L'
					AND LTRIM(RTRIM(PD30F.DM_FGN_CNY)) != ''
			WHERE 
				CAST(LT20.CREATEDAT AS DATE) BETWEEN @startDate AND @endDate
				AND LT20.OnEcorr = 0
				AND LT20.PrintedAt IS NOT NULL
				AND LT20.InactivatedAt IS NULL
			GROUP BY 
				LT20.RM_DSC_LTR_PRC,
				LT20.CreatedAt
		)SENT_TO_PRINTER
					ON SENT_TO_PRINTER.RM_DSC_LTR_PRC = LT20.RM_DSC_LTR_PRC
					AND SENT_TO_PRINTER.CreatedAt = CAST(LT20.CreatedAt AS DATE)
		LEFT JOIN 
		(
			SELECT
				CAST(LT20.CreatedAt AS DATE) AS CreatedAt,
				LT20.RM_DSC_LTR_PRC, 
				COUNT(PD30D.DF_PRS_ID) AS DOMESTIC_PRINTED,
				COUNT(PD30F.DF_PRS_ID) AS FOREIGN_PRINTED
			FROM 
				CDW..LT20_LTR_REQ_PRC LT20
				LEFT JOIN CDW..PD30_PRS_ADR PD30D
					ON PD30D.DF_PRS_ID = LT20.RF_SBJ_PRC
					AND PD30D.DC_ADR = 'L'
					AND LTRIM(RTRIM(PD30D.DM_FGN_CNY)) = ''
				LEFT JOIN CDW..PD30_PRS_ADR PD30F
					ON PD30F.DF_PRS_ID = LT20.RF_SBJ_PRC
					AND PD30F.DC_ADR = 'L'
					AND LTRIM(RTRIM(PD30F.DM_FGN_CNY)) != ''
			WHERE 
				CAST(LT20.CREATEDAT AS DATE) BETWEEN @startDate AND @endDate
				AND LT20.OnEcorr = 0
				AND LT20.InactivatedAt IS NULL
			GROUP BY 
				RM_DSC_LTR_PRC,
				LT20.CreatedAt
		)PRINTED_TOTAL
					ON PRINTED_TOTAL.RM_DSC_LTR_PRC = LT20.RM_DSC_LTR_PRC
					AND PRINTED_TOTAL.CreatedAt = CAST(LT20.CreatedAt AS DATE)
		LEFT JOIN 
		(
			SELECT
				CAST(LT20.CreatedAt AS DATE) AS CreatedAt,
				LT20.RM_DSC_LTR_PRC, 
				COUNT(RN_SEQ_LTR_CRT_PRC) AS LETTER_COUNT
			FROM 
				CDW..LT20_LTR_REQ_PRC LT20
			WHERE 
				CAST(LT20.CREATEDAT AS DATE) BETWEEN @startDate AND @endDate
				AND LT20.EcorrDocumentCreatedAt IS NOT NULL
				AND LT20.InactivatedAt IS NULL
			GROUP BY 
				RM_DSC_LTR_PRC,
				LT20.CreatedAt
		)EXPECTED_ECORR
					ON EXPECTED_ECORR.RM_DSC_LTR_PRC = LT20.RM_DSC_LTR_PRC
					AND EXPECTED_ECORR.CreatedAt = CAST(LT20.CreatedAt AS DATE)
		LEFT JOIN
		(
			SELECT
				CAST(LT20.CreatedAt AS DATE) AS CreatedAt,
				LT20.RM_DSC_LTR_PRC, 
				COUNT( RN_SEQ_LTR_CRT_PRC) AS LETTER_COUNT
			FROM 
				CDW..LT20_LTR_REQ_PRC LT20 
			WHERE 
				  CAST(LT20.CREATEDAT AS DATE) BETWEEN @startDate AND @endDate
				  AND LT20.InactivatedAt IS NULL
			GROUP BY 
				LT20.RM_DSC_LTR_PRC,
				LT20.CreatedAt
		) TOTAL
			ON TOTAL.RM_DSC_LTR_PRC = LT20.RM_DSC_LTR_PRC
			AND TOTAL.CreatedAt = CAST(LT20.CreatedAt AS DATE)
	WHERE
		 CAST(LT20.CreatedAt as DATE) BETWEEN @startDate AND @endDate
		 AND LT20.InactivatedAt IS NULL

/*Coborrower*/
	UNION ALL

	SELECT
		CAST(PP.AddedAt as DATE) [RunDate],
		L.Letter + ' (' + CAST(L.PagesPerDocument as VARCHAR(2)) + ')' [Letter Id],
		COUNT(PP.PrintProcessingId) [LetterCount],
		SUM(CASE 
				WHEN PP.ArcAddProcessingId IS NULL 
				THEN 0 
				ELSE 1 
			END) [SentToArcAddProcessing],
		SUM(CAST(PP.ArcNeeded as TINYINT)) [SentToArcAddProcessingExpected], 
		SUM(CASE 
				WHEN PP.EcorrDocumentCreatedAt IS NULL 
				THEN 0 
				ELSE 1 
		END) [EcorrDocsCreated],
		COUNT(PP.PrintProcessingId) [EcorrDocsCreatedExpected], --count all print processing records because everyting gets ecorred even when the borrower is not on ecorr
		SUM(CASE 
				WHEN PP.ImagedAt IS NULL THEN 0 
				ELSE 1 
		END) [DocsSentToImaging],
		SUM(CAST(PP.ImagingNeeded as TINYINT)) [DocsSentToImagingExpected],
		SUM(CASE 
				WHEN F.ForeignAddress = 0 THEN 
					CASE 
						WHEN PP.PrintedAt IS NULL THEN 0 
						ELSE 1 
					END 
				ELSE 0 
		END) [SentToPrinterDomestic],
		SUM(CASE 
				WHEN F.ForeignAddress = 0 AND PP.OnEcorr = 0 THEN 1 
				ELSE 0 
		END) [SentToPrinterDomesticExpected],    
		SUM(CASE
				WHEN F.ForeignAddress = 1 AND PP.OnEcorr = 0
				THEN 
					CASE WHEN PP.PrintedAt IS NULL THEN 0 ELSE 1 END 
							ELSE 0 
		END) [SentToPrinterForeign],
		SUM(CASE 
				WHEN F.ForeignAddress = 1 AND PP.OnEcorr = 0 THEN 1 
				ELSE 0 
		END) [SentToPrinterForeignExpected]
	FROM
		   CLS.[print].[PrintProcessingCoBorrower] PP
		   INNER JOIN CLS.[print].[ScriptData] SD ON SD.ScriptDataId = PP.ScriptDataId
		   INNER JOIN CLS.[print].Letters L ON L.LetterId = SD.LetterId
		   INNER JOIN 
		   (-- create foreign address indicator
				SELECT
					PP.PrintProcessingId,
					CASE WHEN CLS.dbo.SplitAndRemoveQuotes(PP.LetterData, ',', FH.StateIndex , 1) = '' THEN 1 ELSE 0 END [ForeignAddress] -- if the state is empty then it is a foreign address
				FROM
					CLS.[print].PrintProcessingCoBorrower PP
					INNER JOIN CLS.[print].[ScriptData] SD ON SD.ScriptDataId = PP.ScriptDataId
					INNER JOIN CLS.[print].[FileHeaders] FH ON FH.FileHeaderId = SD.FileHeaderId
			) F ON F.PrintProcessingId = PP.PrintProcessingId
	WHERE
		   CAST(PP.AddedAt as DATE) BETWEEN @startDate AND @endDate
		   AND
		   SD.Active = 1 
		   AND
		   PP.DeletedAt IS NULL
	GROUP BY
		   CAST(PP.AddedAt as DATE),
		   L.Letter + ' (' + CAST(L.PagesPerDocument as VARCHAR(2)) + ')'

	UNION ALL

	SELECT DISTINCT
		CAST(lt20.CreatedAt as DATE) [RunDate],
		LT20.RM_DSC_LTR_PRC + ' (' + ISNULL(CAST(L.PagesPerDocument as VARCHAR(2)), 'NA') + ')' [Letter Id],
		TOTAL.LETTER_COUNT AS [LetterCount],
		0 [SentToArcAddProcessing],
		0 [SentToArcAddProcessingExpected],
		ISNULL(EXPECTED_ECORR.LETTER_COUNT,0) AS [EcorrDocsCreated],
		ISNULL(TOTAL.LETTER_COUNT,0) AS [EcorrDocsCreatedExpected],
		0 [DocsSentToImaging],
		0 [DocsSentToImagingExpected],
		ISNULL(SENT_TO_PRINTER.DOMESTIC,0) AS [SentToPrinterDomestic],
		ISNULL(PRINTED_TOTAL.DOMESTIC_PRINTED,0) [SentToPrinterDomesticExpected],
		ISNULL(SENT_TO_PRINTER.[FOREIGN],0) AS [SentToPrinterForeign],
		ISNULL(PRINTED_TOTAL.FOREIGN_PRINTED,0) AS [SentToPrinterForeignExpected]
	FROM
		CDW..LT20_LTR_REQ_PRC_Coborrower LT20
		LEFT JOIN CLS.[print].Letters L
			on L.Letter = LT20.RM_DSC_LTR_PRC
		LEFT JOIN 
		(
			SELECT
				CAST(LT20.CreatedAt AS DATE) AS CreatedAt,
				LT20.RM_DSC_LTR_PRC, 
				COUNT(PD30D.DF_PRS_ID) + COUNT(PD30F.DF_PRS_ID) AS LETTER_COUNT,
				COUNT(PD30D.DF_PRS_ID) AS DOMESTIC,
				COUNT(PD30F.DF_PRS_ID) AS [FOREIGN]
			FROM 
				CDW..LT20_LTR_REQ_PRC_Coborrower LT20
				LEFT JOIN CDW..PD30_PRS_ADR PD30D
					ON PD30D.DF_PRS_ID = LT20.RF_SBJ_PRC
					AND PD30D.DC_ADR = 'L'
					AND LTRIM(RTRIM(PD30D.DM_FGN_CNY)) = ''
				LEFT JOIN CDW..PD30_PRS_ADR PD30F
					ON PD30F.DF_PRS_ID = LT20.RF_SBJ_PRC
					AND PD30F.DC_ADR = 'L'
					AND LTRIM(RTRIM(PD30F.DM_FGN_CNY)) != ''
			WHERE 
				CAST(LT20.CREATEDAT AS DATE) BETWEEN @startDate AND @endDate
				AND LT20.OnEcorr = 0
				AND LT20.PrintedAt IS NOT NULL
				AND LT20.InactivatedAt IS NULL
			GROUP BY 
				LT20.RM_DSC_LTR_PRC,
				LT20.CreatedAt
		)SENT_TO_PRINTER
					ON SENT_TO_PRINTER.RM_DSC_LTR_PRC = LT20.RM_DSC_LTR_PRC
					AND SENT_TO_PRINTER.CreatedAt = CAST(LT20.CreatedAt AS DATE)
		LEFT JOIN 
		(
			SELECT
				CAST(LT20.CreatedAt AS DATE) AS CreatedAt,
				LT20.RM_DSC_LTR_PRC, 
				COUNT(PD30D.DF_PRS_ID) AS DOMESTIC_PRINTED,
				COUNT(PD30F.DF_PRS_ID) AS FOREIGN_PRINTED
			FROM 
				CDW..LT20_LTR_REQ_PRC_Coborrower LT20
				LEFT JOIN CDW..PD30_PRS_ADR PD30D
					ON PD30D.DF_PRS_ID = LT20.RF_SBJ_PRC
					AND PD30D.DC_ADR = 'L'
					AND LTRIM(RTRIM(PD30D.DM_FGN_CNY)) = ''
				LEFT JOIN CDW..PD30_PRS_ADR PD30F
					ON PD30F.DF_PRS_ID = LT20.RF_SBJ_PRC
					AND PD30F.DC_ADR = 'L'
					AND LTRIM(RTRIM(PD30F.DM_FGN_CNY)) != ''
			WHERE 
				CAST(LT20.CREATEDAT AS DATE) BETWEEN @startDate AND @endDate
				AND LT20.OnEcorr = 0
				AND LT20.InactivatedAt IS NULL
			GROUP BY 
				RM_DSC_LTR_PRC,
				LT20.CreatedAt
		)PRINTED_TOTAL
					ON PRINTED_TOTAL.RM_DSC_LTR_PRC = LT20.RM_DSC_LTR_PRC
					AND PRINTED_TOTAL.CreatedAt = CAST(LT20.CreatedAt AS DATE)
		LEFT JOIN 
		(
			SELECT
				CAST(LT20.CreatedAt AS DATE) AS CreatedAt,
				LT20.RM_DSC_LTR_PRC, 
				COUNT(RN_SEQ_LTR_CRT_PRC) AS LETTER_COUNT
			FROM 
				CDW..LT20_LTR_REQ_PRC_Coborrower LT20
			WHERE 
				CAST(LT20.CREATEDAT AS DATE) BETWEEN @startDate AND @endDate
				AND LT20.EcorrDocumentCreatedAt IS NOT NULL
				AND LT20.InactivatedAt IS NULL
			GROUP BY 
				RM_DSC_LTR_PRC,
				LT20.CreatedAt
		)EXPECTED_ECORR
					ON EXPECTED_ECORR.RM_DSC_LTR_PRC = LT20.RM_DSC_LTR_PRC
					AND EXPECTED_ECORR.CreatedAt = CAST(LT20.CreatedAt AS DATE)
		LEFT JOIN
		(
			SELECT
				CAST(LT20.CreatedAt AS DATE) AS CreatedAt,
				LT20.RM_DSC_LTR_PRC, 
				COUNT( RN_SEQ_LTR_CRT_PRC) AS LETTER_COUNT
			FROM 
				CDW..LT20_LTR_REQ_PRC_Coborrower LT20 
			WHERE 
				  CAST(LT20.CREATEDAT AS DATE) BETWEEN @startDate AND @endDate
				  AND LT20.InactivatedAt IS NULL
			GROUP BY 
				LT20.RM_DSC_LTR_PRC,
				LT20.CreatedAt
		) TOTAL
			ON TOTAL.RM_DSC_LTR_PRC = LT20.RM_DSC_LTR_PRC
			AND TOTAL.CreatedAt = CAST(LT20.CreatedAt AS DATE)
	WHERE
		 CAST(LT20.CreatedAt as DATE) BETWEEN @startDate AND @endDate
		 AND LT20.InactivatedAt IS NULL

