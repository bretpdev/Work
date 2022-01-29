USE [CDW]
GO

/****** Object:  View [FsaInvMet].[Monthly_Military]    Script Date: X/XX/XXXX XX:XX:XX AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--CREATE VIEW [FsaInvMet].[Monthly_Military]

--AS
SELECT
	LNXX.BF_SSN,
	LNXX.LN_SEQ, 
	X AS ActiveMilitaryIndicator
FROM
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..DWXX_DW_CLC_CLU DWXX
		ON DWXX.BF_SSN = LNXX.BF_SSN
		AND DWXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN 
	(
		SELECT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LR_ITR,
			LNXX.LC_INT_RDC_PGM,
			ROW_NUMBER() OVER (PARTITION BY LNXX.BF_SSN, LNXX.LN_SEQ ORDER BY LNXX.LD_STA_LONXX DESC) AS CurrentRow
		FROM
			CDW..LNXX_INT_RTE_HST LNXX
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END /*last day prev month*/
	) LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		AND LNXX.CurrentRow = X
	LEFT JOIN
	(
		SELECT DISTINCT
			DWXX.BF_SSN,
			DWXX.LN_SEQ
		FROM
			CDW..DWXX_DW_CLC_CLU DWXX
			INNER JOIN CDW..LNXX_BR_DFR_APV LNXX
				ON DWXX.BF_SSN = LNXX.BF_SSN
				AND DWXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LC_STA_LONXX = 'A'
				AND CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END /*last day prev month*/
			INNER JOIN CDW..DFXX_BR_DFR_REQ DFXX
				ON DWXX.BF_SSN = DFXX.BF_SSN
				AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
				AND DFXX.LC_DFR_STA = 'A'
				AND DFXX.LC_STA_DFRXX = 'A'
				AND DFXX.LC_DFR_TYP IN('XX','XX')
				AND LNXX.LC_DFR_RSP != 'XXX'
		WHERE 
			DWXX.WC_DW_LON_STA = 'XX'
	) DefermentDate 
		ON DefermentDate.BF_SSN = LNXX.BF_SSN
		AND DefermentDate.LN_SEQ = LNXX.LN_SEQ
	LEFT JOIN
	(
		SELECT DISTINCT
			SP.BorrSSN,
			SP.Loan
		FROM
			CLS.scra.ScriptProcessing SP
			INNER JOIN CLS.scra.DataComparison DC
				ON SP.DataComparisonId = DC.DataComparisonId
				AND SP.BorrSSN = DC.BorrSSN
				AND SP.Loan = DC.Loan
				AND DC.ActiveRow = X
		WHERE
			CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) BETWEEN SP.TXCXBegin AND SP.TXCXEnd
			AND SP.LNXXDisb <= SP.TXCXBegin
	) MilitaryDatabase
		ON MilitaryDatabase.BorrSSN = LNXX.BF_SSN
		AND MilitaryDatabase.Loan = LNXX.LN_SEQ
WHERE
	LNXX.LC_STA_LONXX = 'R'
	AND
	(
		(LNXX.LR_ITR = X AND LNXX.LC_INT_RDC_PGM = 'S') --danger zone borrower
		OR (LNXX.LR_ITR <= X AND LNXX.LC_INT_RDC_PGM = 'M') 
		OR DefermentDate.BF_SSN IS NOT NULL --has military deferment
		OR MilitaryDatabase.BorrSSN IS NOT NULL --Active in new database
	)


GO

/****** Object:  View [FsaInvMet].[Monthly_BasePopulation]    Script Date: X/XX/XXXX XX:XX:XX AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
--CREATE VIEW [FsaInvMet].[Monthly_BasePopulation]

--AS
SELECT
	LNXX.BF_SSN,
	LNXX.LN_SEQ,
	LNXX.LC_STA_LONXX,
	LNXX.LD_STA_LONXX,
	LNXX.LD_LON_EFF_ADD,
	LNXX.LD_LON_ACL_ADD,
	LNXX.LD_PIF_RPT,
	CASE 
		WHEN LNXX.LA_CUR_PRI > X THEN LNXX.LC_CAM_LON_STA
		ELSE '' 
	END AS LC_CAM_LON_STA, --UHEAA ONLY?
	PDXX.DX_ADR_EML,
	CASE 
		WHEN LNXX.IC_LON_PGM IN('DLPCNS','DLPLGB','DLPLUS','DLSCNS','DLSPCN','DLSSPL','DLSTFD','DLUCNS','DLUNST','DLUSPL','TEACH' ) THEN 'DL'
		WHEN LNXX.IC_LON_PGM IN('CNSLDN','PLUS','PLUSGB','SLS','STFFRD','SUBCNS','SUBSPC','UNCNS','UNSPC','UNSTFD','FISL') THEN 'FL'
		ELSE ''
	END AS IC_LON_PGM,
	LNXX.LA_CUR_PRI AS LA_OTS_PRI_ELG,
	COALESCE(DWXX.WA_TOT_BRI_OTS,DWXX.LA_NSI_OTS,X) AS WA_TOT_BRI_OTS,
	CASE 
		WHEN LNXX.LA_CUR_PRI <= X THEN X
		WHEN LNXX.LC_STA_LONXX = 'D' THEN DATEDIFF(DAY,LNXX.LD_DLQ_OCC, GETDATE()) + X - (DATEDIFF(DAY, CAST(DATEADD(DAY,-DAY(GETDATE()), CAST(GETDATE() AS DATE)) AS DATETIME),GETDATE())) /* get delinquency as of beginning of month*/
		WHEN DWXX.WC_DW_LON_STA IN ('XX','XX') THEN X
		WHEN FORB.BF_SSN != '' THEN X
        WHEN COALESCE(LNXX.LN_DLQ_MAX - (DATEDIFF(DAY, CAST(DATEADD(DAY,-DAY(GETDATE()), CAST(GETDATE() AS DATE)) AS DATETIME),GETDATE())),X) < X THEN X
		ELSE COALESCE(LNXX.LN_DLQ_MAX - (DATEDIFF(DAY, CAST(DATEADD(DAY,-DAY(GETDATE()), CAST(GETDATE() AS DATE)) AS DATETIME),GETDATE())),X)
	END AS LN_DLQ_MAX,
	CASE 
		WHEN FORB.BF_SSN != '' THEN 'X' 
		ELSE 'X' 
	END AS SPEC_FORB_IND,
	DWXX.WC_DW_LON_STA,
	CASE 
		WHEN LNXX.LC_STA_LONXX = 'D' AND LNXX.LA_CUR_PRI = X THEN X 
		WHEN LNXX.LD_PIF_RPT IS NOT NULL THEN X ELSE X 
	END AS ORD,
	CASE 
		WHEN LNXX.BF_SSN IS NULL THEN X 
		ELSE X
	END AS BILL_SATISFIED,
	SegmentLogic.Segment,
	LNXX.LF_LON_CUR_OWN,
	CASE
		WHEN InDefer.BF_SSN IS NULL THEN X 
		ELSE X
	END AS DefermentIndicator,
	CASE
		WHEN InDeferBorr.BF_SSN IS NULL THEN X 
		ELSE X
	END AS BorrDefermentIndicator,
	CASE 
		WHEN LNXX.LC_STA_LONXX = 'D' AND LNXX.LA_CUR_PRI = X THEN LNXX.LD_STA_LONXX
		WHEN LNXX.LD_PIF_RPT IS NOT NULL THEN LNXX.LD_PIF_RPT ELSE NULL 
	END AS PIF_TRN_DT,
	SegmentLogic.SepDate,
	LNXX.LD_END_GRC_PRD AS GraceEnd
FROM 
	CDW..LNXX_LON LNXX
	LEFT JOIN CDW..DWXX_DW_CLC_CLU DWXX 
		ON LNXX.BF_SSN = DWXX.BF_SSN 
		AND LNXX.LN_SEQ = DWXX.LN_SEQ
	LEFT JOIN 
	(
		SELECT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LD_DLQ_OCC,
			MAX(LNXX.LN_DLQ_MAX) + X [LN_DLQ_MAX]
		FROM	
			CDW..LNXX_LON_DLQ_HST LNXX
		WHERE
			LNXX.LC_STA_LONXX = 'X'
			AND LNXX.LC_DLQ_TYP = 'P'
			AND 
			(
				DATEADD(DAY,-X,CAST(GETDATE() AS DATE)) <= LNXX.LD_DLQ_MAX --Account for AES holidays that we dont honor
				OR LNXX.LN_DLQ_MAX >= XXX
			)
		GROUP BY
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LD_DLQ_OCC
	) LNXX ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	LEFT JOIN 
	( -- email address
		SELECT
			*,
			ROW_NUMBER() OVER (PARTITION BY DF_PRS_ID ORDER BY PriorityNumber) [EmailPriority]
		FROM
		(
			SELECT
				PDXX.DF_PRS_ID,
				PDXX.DX_ADR_EML,
				CASE 
					WHEN DC_ADR_EML = 'H' THEN X
					WHEN DC_ADR_EML = 'A' THEN X
					WHEN DC_ADR_EML = 'W' THEN X
				END [PriorityNumber]
			FROM
				CDW..PDXX_PRS_ADR_EML PDXX
			WHERE
				PDXX.DI_VLD_ADR_EML = 'Y'
				AND PDXX.DC_STA_PDXX = 'A'
		) Email
	) PDXX 
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN 
		AND PDXX.EmailPriority = X --highest priority email only
	LEFT JOIN
	( --Deferment indicator
		SELECT
			DFXX.BF_SSN,
			LNXX.LN_SEQ
		FROM
			CDW..DFXX_BR_DFR_REQ DFXX
			INNER JOIN CDW..LNXX_BR_DFR_APV LNXX 
				ON DFXX.BF_SSN = LNXX.BF_SSN 
				AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM
		WHERE
			DFXX.LC_DFR_STA = 'A'
			AND LNXX.LC_STA_LONXX = 'A'
			AND DFXX.LC_STA_DFRXX = 'A'
			AND LNXX.LC_DFR_RSP != 'XXX'     
			AND LNXX.LD_DFR_END >= CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) /*Last Day Prev Month*/
			AND LNXX.LD_DFR_APL <= CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) /*Last Day Prev Month*/
			AND LNXX.LD_DFR_BEG <= CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) /*Last Day Prev Month*/
	) InDefer 
		ON InDefer.BF_SSN = LNXX.BF_SSN
		AND InDefer.LN_SEQ = LNXX.LN_SEQ
	LEFT JOIN
	( --Deferment indicator borr
		SELECT DISTINCT
			DFXX.BF_SSN
		FROM
			CDW..DFXX_BR_DFR_REQ DFXX
			INNER JOIN CDW..LNXX_BR_DFR_APV LNXX 
				ON DFXX.BF_SSN = LNXX.BF_SSN 
				AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM
		WHERE
			DFXX.LC_DFR_STA = 'A'
			AND LNXX.LC_STA_LONXX = 'A'
			AND DFXX.LC_STA_DFRXX = 'A'
			AND LNXX.LC_DFR_RSP != 'XXX'     
			AND LNXX.LD_DFR_END >= CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) /*Last Day Prev Month*/
			AND LNXX.LD_DFR_APL <= CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) /*Last Day Prev Month*/
			AND LNXX.LD_DFR_BEG <= CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) /*Last Day Prev Month*/
	) InDeferBorr 
		ON InDeferBorr.BF_SSN = LNXX.BF_SSN
	LEFT JOIN
	( -- forbearance indicator
		SELECT 
			FBXX.BF_SSN,
			LNXX.LN_SEQ                                       
		FROM 
			CDW..FBXX_BR_FOR_REQ FBXX
			INNER JOIN CDW..LNXX_BR_FOR_APV LNXX 
				ON FBXX.BF_SSN = LNXX.BF_SSN 
				AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
		WHERE
			FBXX.LC_FOR_STA = 'A'
			AND LNXX.LC_STA_LONXX = 'A'
			AND FBXX.LC_STA_FORXX = 'A'
			AND LNXX.LC_FOR_RSP != 'XXX'      
			--AND FBXX.LC_FOR_TYP IN ('XX','XX','XX','XX','XX')
			AND LNXX.LD_FOR_END >= CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) /*Last Day Prev Month*/
			AND LNXX.LD_FOR_APL <= CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) /*Last Day Prev Month*/
			AND LNXX.LD_FOR_BEG <= CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) /*Last Day Prev Month*/
	) FORB 
		ON FORB.BF_SSN = LNXX.BF_SSN
		AND FORB.LN_SEQ = LNXX.LN_SEQ
	LEFT JOIN
	( 
		SELECT
			LNXX.BF_SSN,
			LNXX.LN_SEQ
		FROM
			CDW..BLXX_BR_BIL BLXX
			INNER JOIN CDW..LNXX_LON_BIL_CRF LNXX
				ON BLXX.BF_SSN = LNXX.BF_SSN
				AND BLXX.LD_BIL_CRT = LNXX.LD_BIL_CRT
				AND BLXX.LN_SEQ_BIL_WI_DTE = LNXX.LN_SEQ_BIL_WI_DTE
		WHERE
			BLXX.LD_BIL_DU BETWEEN CAST(DATEADD(MONTH, DATEDIFF(MONTH, X, GETDATE())-X, X) AS DATE)/*first day prev month*/ AND CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) /*Last Day Prev Month*/
			AND BLXX.LC_IND_BIL_SNT != 'X' --Exclude paid ahead bills from counting
			AND LNXX.LD_BIL_DU_LON BETWEEN CAST(DATEADD(MONTH, DATEDIFF(MONTH, X, GETDATE())-X, X) AS DATE)/*first day prev month*/ AND CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) /*Last Day Prev Month*/
			AND LNXX.LA_TOT_BIL_STS >= COALESCE(LNXX.LA_BIL_CUR_DU,X)
			AND LNXX.LC_STA_LONXX = 'A'
			AND LNXX.LC_BIL_TYP_LON = 'P'
	) LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
	LEFT JOIN
	( -- segment
		SELECT
			LNXX.BF_SSN AS SSN,
			LNXX.LN_SEQ AS LN_SEQ,
			COALESCE(GroupedSep.SepDateSchool, GroupedSep.SepDateDef, GroupedSep.SepDateDisb) AS SepDate,
			CASE 
				WHEN LNXX.BF_SSN IS NOT NULL THEN X
				WHEN LNXXConPlus.BF_SSN IS NOT NULL THEN X
				WHEN LNXXConPlus.BF_SSN IS NULL AND SDXXMax.LC_REA_SCL_SPR = 'XX' /*graduated*/
					AND COALESCE(SDXXMax.LD_SCL_SPR, LNXXDeferPlus.GradDeferDate, LNXXGradPlus.GradRepayDate) > CAST(DATEADD(YEAR, -X, GETDATE()) AS DATE) THEN X /*less than X years ago*/
				WHEN LNXXConPlus.BF_SSN IS NULL
					AND SDXXMax.LC_REA_SCL_SPR = 'XX' /*graduated*/
					AND COALESCE(SDXXMax.LD_SCL_SPR, LNXXDeferPlus.GradDeferDate, LNXXGradPlus.GradRepayDate) <=  CAST(DATEADD(YEAR, -X, GETDATE()) AS DATE) THEN X /*More than X years ago*/
				WHEN LNXXConPlus.BF_SSN IS NULL
					AND SDXXMax.LC_REA_SCL_SPR != 'XX' /*didnt graduate*/
					AND SDXXMax.LD_SCL_SPR >  CAST(DATEADD(YEAR, -X, GETDATE()) AS DATE)  THEN X /*Less than X years ago*/
				WHEN LNXXConPlus.BF_SSN IS NULL
					AND SDXXMax.LC_REA_SCL_SPR != 'XX' /*didnt graduate*/
					AND SDXXMax.LD_SCL_SPR <=  CAST(DATEADD(YEAR, -X, GETDATE()) AS DATE)  THEN X /*More than X years ago*/
				WHEN COALESCE(SDXXMax.LD_SCL_SPR, LNXXDeferPlus.GradDeferDate, LNXXGradPlus.GradRepayDate) >  CAST(DATEADD(YEAR, -X, GETDATE()) AS DATE) THEN X /*Less than X years ago*/
				WHEN COALESCE(SDXXMax.LD_SCL_SPR, LNXXDeferPlus.GradDeferDate, LNXXGradPlus.GradRepayDate) <=  CAST(DATEADD(YEAR, -X, GETDATE()) AS DATE) THEN X /*More than X years ago*/
				ELSE X
			END [Segment]
		FROM 
			CDW..LNXX_LON LNXX
			LEFT JOIN
			( 
				SELECT
					BF_SSN,
					LN_SEQ
				FROM
					CDW..LNXX_LON
				WHERE
					IC_LON_PGM IN ('DLPLUS', 'DLCNSL', 'DLPCNS', 'DLSCCN', 'DLSCNS', 'DLSCPL', 'DLSCSC', 'DLSCSL', 'DLSCST', 'DLSCUC', 'DLSCUN', 'DLSPCN', 'DLSSPL', 'DLUCNS', 'DLUSPL', 'DSCON', 'DUCON', 'DLSCPG', 'DLPLGB')
			) LNXXConPlus 
				ON LNXXConPlus.BF_SSN = LNXX.BF_SSN
				AND LNXXConPlus.LN_SEQ = LNXX.LN_SEQ
			LEFT JOIN
			(
				SELECT
					LNXX.BF_SSN,
					LNXX.LN_SEQ,
					CAST(DATEADD(DAY, -XXX, MAX(LNXX.LD_DSB)) AS DATE) AS GradRepayDate
				FROM
					CDW..LNXX_LON LNXX
					INNER JOIN CDW..LNXX_DSB LNXX 
						ON LNXX.BF_SSN = LNXX.BF_SSN 
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
				WHERE
					LNXX.IC_LON_PGM IN ('DGPLUS','DLPLGB')
					AND LNXX.LC_STA_LONXX = 'X'
					AND LNXX.LD_DSB <= CAST(GETDATE() AS DATE)
				GROUP BY
					LNXX.BF_SSN,
					LNXX.LN_SEQ
			) LNXXGradPlus 
				ON LNXXGradPlus.BF_SSN = LNXX.BF_SSN
				AND LNXXGradPlus.LN_SEQ = LNXX.LN_SEQ
			LEFT JOIN
			(
				SELECT
					LNXX.BF_SSN,
					LNXX.LN_SEQ,
					CAST(DATEADD(DAY, -XXX, MAX(LNXX.LD_DFR_END)) AS DATE) AS GradDeferDate
				FROM
					CDW..LNXX_LON LNXX
					INNER JOIN CDW..LNXX_BR_DFR_APV LNXX 
						ON LNXX.BF_SSN = LNXX.BF_SSN 
						AND LNXX.LN_SEQ = LNXX.LN_SEQ
					INNER JOIN CDW..DFXX_BR_DFR_REQ DFXX 
						ON DFXX.BF_SSN = LNXX.BF_SSN 
						AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM
				WHERE
					LNXX.IC_LON_PGM IN ('DGPLUS','DLPLGB')
					AND LNXX.LC_STA_LONXX = 'A'
					AND DFXX.LC_DFR_TYP IN ('XX','XX','XX')
					AND DFXX.LC_DFR_STA = 'A'
					AND DFXX.LC_STA_DFRXX = 'A'
					AND LNXX.LD_DFR_END <= CAST(GETDATE() AS DATE)
				GROUP BY
					LNXX.BF_SSN,
					LNXX.LN_SEQ
			) LNXXDeferPlus 
				ON LNXXDeferPlus.BF_SSN = LNXX.BF_SSN
				AND LNXXDeferPlus.LN_SEQ = LNXX.LN_SEQ
			LEFT JOIN 
			(	
				SELECT
					LNXX.BF_SSN,
					LNXX.LN_SEQ,
					MAX(SDXX.LN_STU_SPR_SEQ) AS LN_STU_SPR_SEQ
				FROM
					CDW..SDXX_STU_SPR SDXX
					INNER JOIN CDW..LNXX_LON_STU_OSD LNXX
						ON LNXX.LF_STU_SSN = SDXX.LF_STU_SSN
						AND LNXX.LN_STU_SPR_SEQ = SDXX.LN_STU_SPR_SEQ
						AND LNXX.LC_STA_LONXX = 'A'
				WHERE
					SDXX.LC_STA_STUXX = 'A'
				GROUP BY 
					LNXX.BF_SSN,
					LNXX.LN_SEQ
			) SDXXSEQ
				ON SDXXSEQ.BF_SSN = LNXX.BF_SSN
				AND SDXXSEQ.LN_SEQ =LNXX.LN_SEQ
			LEFT JOIN 
			(	
				SELECT
					LNXX.BF_SSN,
					LNXX.LN_SEQ,
					SDXX.LN_STU_SPR_SEQ,
					SDXX.LC_REA_SCL_SPR,
					SDXX.LD_SCL_SPR
				FROM
					CDW..SDXX_STU_SPR SDXX
					INNER JOIN CDW..LNXX_LON_STU_OSD LNXX
						ON LNXX.LF_STU_SSN = SDXX.LF_STU_SSN
						AND LNXX.LN_STU_SPR_SEQ = SDXX.LN_STU_SPR_SEQ
						AND LNXX.LC_STA_LONXX = 'A'
				WHERE
					SDXX.LC_STA_STUXX = 'A'
					AND SDXX.LD_SCL_SPR <= CAST(GETDATE() AS DATE)
			) SDXXMax
				ON SDXXMax.BF_SSN = LNXX.BF_SSN
				AND SDXXMax.LN_SEQ = LNXX.LN_SEQ
				AND SDXXMAX.LN_STU_SPR_SEQ = SDXXSEQ.LN_STU_SPR_SEQ
			LEFT JOIN
			(
				SELECT
					LNXX.BF_SSN,
					LNXX.LN_SEQ
				FROM 
					CDW..LNXX_RPD_PIO_CVN LNXX
				WHERE
					LNXX.LD_LON_RHB_PCV IS NOT NULL OR COALESCE(LNXX.IF_LON_SRV_DFL_LON,'                ') != '                '
			) LNXX 
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
			LEFT JOIN
			( --Getting valid separation date for in school borrowers not returning in school performance categories
					SELECT
						BF_SSN,
						LN_SEQ,
						MAX(LN_STU_SPR_SEQ) AS LN_STU_SPR_SEQ,
						MAX(SepDateDisb) AS SepDateDisb,
						MAX(SepDateDef) AS SepDateDef,
						MAX(SepDateSchool) AS SepDateSchool
					FROM
					(
					SELECT
						LNXX.BF_SSN,
						LNXX.LN_SEQ,
						X AS LN_STU_SPR_SEQ,
						CAST(DATEADD(DAY, -XXX, MAX(LNXX.LD_DSB)) AS DATE) AS SepDateDisb,
						NULL AS SepDateDef,
						NULL AS SepDateSchool
					FROM
						CDW..LNXX_LON LNXX
						INNER JOIN CDW..LNXX_DSB LNXX 
							ON LNXX.BF_SSN = LNXX.BF_SSN 
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
					WHERE
						LNXX.IC_LON_PGM IN ('DGPLUS','DLPLGB')
						AND LNXX.LC_STA_LONXX = 'X'
					GROUP BY
						LNXX.BF_SSN,
						LNXX.LN_SEQ

					UNION ALL

					SELECT
						LNXX.BF_SSN,
						LNXX.LN_SEQ,
						X AS LN_STU_SPR_SEQ,
						NULL AS SepDateDisb,
						CAST(DATEADD(DAY, -XXX, MAX(LNXX.LD_DFR_END)) AS DATE) AS SepDateDef,
						NULL AS SepDateSchool
					FROM
						CDW..LNXX_LON LNXX
						INNER JOIN CDW..LNXX_BR_DFR_APV LNXX 
							ON LNXX.BF_SSN = LNXX.BF_SSN 
							AND LNXX.LN_SEQ = LNXX.LN_SEQ
						INNER JOIN CDW..DFXX_BR_DFR_REQ DFXX 
							ON DFXX.BF_SSN = LNXX.BF_SSN 
							AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM
					WHERE
						LNXX.IC_LON_PGM IN ('DGPLUS','DLPLGB')
						AND LNXX.LC_STA_LONXX = 'A'
						AND DFXX.LC_DFR_TYP IN ('XX','XX','XX')
						AND DFXX.LC_DFR_STA = 'A'
						AND DFXX.LC_STA_DFRXX = 'A'
					GROUP BY
						LNXX.BF_SSN,
						LNXX.LN_SEQ

					UNION ALL

					SELECT
						LNXX.BF_SSN,
						LNXX.LN_SEQ,
						SDXX.LN_STU_SPR_SEQ,
						NULL AS SepDateDisb,
						NULL AS SepDateDef,
						SDXX.LD_SCL_SPR As SepDateSchool
					FROM
						CDW..SDXX_STU_SPR SDXX
						INNER JOIN CDW..LNXX_LON_STU_OSD LNXX
							ON LNXX.LF_STU_SSN = SDXX.LF_STU_SSN
							AND LNXX.LN_STU_SPR_SEQ = SDXX.LN_STU_SPR_SEQ
							AND LNXX.LC_STA_LONXX = 'A'
					WHERE
						SDXX.LC_STA_STUXX = 'A'
				) SepDate
				GROUP BY
					BF_SSN,
					LN_SEQ
			) GroupedSep
				ON GroupedSep.BF_SSN = LNXX.BF_SSN
				AND GroupedSep.LN_SEQ = LNXX.LN_SEQ
				AND 
				(	
					GroupedSep.LN_STU_SPR_SEQ = SDXXSEQ.LN_STU_SPR_SEQ
					OR GroupedSep.LN_STU_SPR_SEQ = X
				)
	) SegmentLogic 
		ON SegmentLogic.SSN = LNXX.BF_SSN
		AND SegmentLogic.LN_SEQ = LNXX.LN_SEQ 
WHERE 
	LNXX.LC_STA_LONXX IN ('R','D','L')
	AND LNXX.LD_LON_ACL_ADD <= CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) /*last day prev month*/
	AND DWXX.WC_DW_LON_STA NOT IN ('XX','XX','XX') --These are considered status errors and output to an error file in the sas

GO

/****** Object:  View [FsaInvMet].[Monthly_PerformanceCategory]    Script Date: X/XX/XXXX XX:XX:XX AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--CREATE VIEW [FsaInvMet].[Monthly_PerformanceCategory]

--AS

-- set performance category
SELECT
	BD.BF_SSN,
	BD.LN_SEQ,
	PF.PerformanceCategory
FROM
	[FsaInvMet].[Monthly_BasePopulation] BD
	INNER JOIN
	(
		SELECT
			BD.BF_SSN,
			BD.LN_SEQ,
			CASE
				WHEN BD.ORD = X THEN
					CASE 
						WHEN M.ActiveMilitaryIndicator = X THEN 'XX'
						WHEN BD.WC_DW_LON_STA IN ('XX','XX') AND BD.LN_DLQ_MAX < X THEN 'XX'
						WHEN BD.WC_DW_LON_STA = 'XX' AND BD.LN_DLQ_MAX < X THEN 'XX'
						WHEN BD.WC_DW_LON_STA IN ('XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX','XX') THEN 
							CASE
								WHEN BD.LN_DLQ_MAX = X AND BD.BILL_SATISFIED = X THEN 'XX'
								WHEN BD.SPEC_FORB_IND = X AND BD.LN_DLQ_MAX = X THEN 'XX'
								WHEN BD.DefermentIndicator = X AND BD.LN_DLQ_MAX = X THEN 'XX'
								WHEN BD.LN_DLQ_MAX = X AND (BD.LC_CAM_LON_STA = 'XX' OR BD.SepDate >= CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE)) THEN 'XX'
								WHEN BD.LN_DLQ_MAX = X AND (BD.LC_CAM_LON_STA = 'XX' OR (BD.SepDate < CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) AND BD.GraceEnd >= CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE))) THEN 'XX'
								WHEN BD.LN_DLQ_MAX BETWEEN X AND X THEN 'XX'
								WHEN BD.LN_DLQ_MAX <= XX THEN 'XX'
								WHEN BD.LN_DLQ_MAX <= XX THEN 'XX'
								WHEN BD.LN_DLQ_MAX <= XXX THEN 'XX'
								WHEN BD.LN_DLQ_MAX <= XXX THEN 'XX'
								WHEN BD.LN_DLQ_MAX <= XXX THEN 'XX'
								WHEN BD.LN_DLQ_MAX > XXX THEN 'XX'
								ELSE 'XX'
							END
						WHEN BD.WC_DW_LON_STA IN ('XX', 'XX') THEN 
							CASE
								WHEN BD.LA_OTS_PRI_ELG <= X THEN RIGHT('XX'+ CAST(CAST(BD.WC_DW_LON_STA AS TINYINT) + X AS VARCHAR),X) --Paid off, but showing defer/forb (needs BU review)
								WHEN BD.LN_DLQ_MAX = X AND BD.BILL_SATISFIED = X THEN 'XX'
								WHEN BD.LN_DLQ_MAX = X THEN RIGHT('XX'+ CAST(CAST(BD.WC_DW_LON_STA AS TINYINT) + X AS VARCHAR),X)--status X is category X etc.
								WHEN BD.LN_DLQ_MAX BETWEEN X AND X THEN 'XX'
								WHEN BD.LN_DLQ_MAX <= XX THEN 'XX'
								WHEN BD.LN_DLQ_MAX <= XX THEN 'XX'
								WHEN BD.LN_DLQ_MAX <= XXX THEN 'XX'
								WHEN BD.LN_DLQ_MAX <= XXX THEN 'XX'
								WHEN BD.LN_DLQ_MAX <= XXX THEN 'XX'
								WHEN BD.LN_DLQ_MAX > XXX THEN 'XX'
								ELSE 'XX'
							END
						WHEN BD.LN_DLQ_MAX <= XX THEN 'XX'
						WHEN BD.LN_DLQ_MAX <= XX THEN 'XX'
						WHEN BD.LN_DLQ_MAX <= XXX THEN 'XX'
						WHEN BD.LN_DLQ_MAX <= XXX THEN 'XX'
						WHEN BD.LN_DLQ_MAX <= XXX THEN 'XX'
						WHEN BD.LN_DLQ_MAX > XXX THEN 'XX'
						ELSE 'XX'
					END
				WHEN BD.ORD = X AND LD_PIF_RPT BETWEEN CAST(DATEADD(MONTH, DATEDIFF(MONTH, X, GETDATE())-X, X) AS DATE) AND CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) THEN 'PIF'
				WHEN BD.ORD = X AND LC_STA_LONXX = 'D' AND LA_OTS_PRI_ELG  = X AND LD_STA_LONXX BETWEEN CAST(DATEADD(MONTH, DATEDIFF(MONTH, X, GETDATE())-X, X) AS DATE) AND CAST(DATEADD(D,-(DAY(GETDATE())),GETDATE()) AS DATE) THEN 'TRN'
				WHEN BD.ORD = X THEN 'PIFPRV'
				WHEN BD.ORD = X THEN 'TRNPRV'
				ELSE 'PRV'
			END [PerformanceCategory]
		FROM
			[FsaInvMet].[Monthly_BasePopulation] BD
			LEFT OUTER JOIN [FsaInvMet].[Monthly_Military] M
				ON M.BF_SSN = BD.BF_SSN
				AND M.LN_SEQ = BD.LN_SEQ
	) PF 
		ON PF.BF_SSN = BD.BF_SSN
		AND PF.LN_SEQ = BD.LN_SEQ
GO

USE [CDW]
GO

--TRUNCATE TABLE [FsaInvMet].[Monthly_LoanLevel]
--INSERT INTO [FsaInvMet].[Monthly_LoanLevel]
SELECT 
	LoanPerf.BF_SSN,
	LoanPerf.LN_SEQ,
	LoanPerf.LC_STA_LONXX,
	LoanPerf.LD_STA_LONXX,
	LoanPerf.LD_LON_EFF_ADD,
	LoanPerf.LD_LON_ACL_ADD,
	LoanPerf.LD_PIF_RPT,
	LoanPerf.LC_CAM_LON_STA,
	LoanPerf.DX_ADR_EML,
	LoanPerf.IC_LON_PGM,
	LoanPerf.LA_OTS_PRI_ELG,
	LoanPerf.WA_TOT_BRI_OTS,
	LoanPerf.LN_DLQ_MAX,
	LoanPerf.SPEC_FORB_IND,
	LoanPerf.WC_DW_LON_STA,
	LoanPerf.ORD,
	LoanPerf.BILL_SATISFIED,
	LoanPerf.Segment,
	BorrSeg.Segment AS BorrSegment,
	LoanPerf.LF_LON_CUR_OWN,
	LoanPerf.DefermentIndicator,
	LoanPerf.BorrDefermentIndicator,
	LoanPerf.PIF_TRN_DT,
	LoanPerf.PerformanceCategory,
	LoanPerf.ActiveMilitaryIndicator,
	LoanPerf.LoanStatusPriority,
	BorrSeg.LoanSegmentPriority

FROM
	( --Loan level performance category with priority
		SELECT
			Perf.*,
			ROW_NUMBER() OVER (PARTITION BY Perf.BF_SSN ORDER BY Perf.PerfCategoryPriority, Perf.LN_SEQ) [LoanStatusPriority]
		FROM
		( --Loan level performance category
			SELECT
				BP.*,
				PF.PerformanceCategory,
				COALESCE(M.ActiveMilitaryIndicator,X) AS ActiveMilitaryIndicator,
				CASE 
					WHEN PerformanceCategory = 'XX' THEN X /*military*/
					WHEN PerformanceCategory = 'XX' THEN X /*repayment XXX+*/
					WHEN PerformanceCategory = 'XX' THEN X /*repayment XXX-XXX*/
					WHEN PerformanceCategory = 'XX' THEN X /*repayment XXX-XXX*/
					WHEN PerformanceCategory = 'XX' THEN X /*repayment XX-XXX*/
					WHEN PerformanceCategory = 'XX' THEN X /*repayment XX-XX*/
					WHEN PerformanceCategory = 'XX' THEN X /*repayment X-XX*/
					WHEN PerformanceCategory = 'XX' THEN X /*repayment current*/
					WHEN PerformanceCategory = 'XX' AND BP.BorrDefermentIndicator = X THEN X /*in school*/
					WHEN PerformanceCategory = 'XX' THEN XX /*in grace*/
					WHEN PerformanceCategory = 'XX' THEN XX /*in forb*/
					WHEN PerformanceCategory = 'XX' THEN XX /*in defer*/
					WHEN PerformanceCategory = 'XX' THEN XX /*in school with active defer/forb but no defer/forb status*/
					WHEN PerformanceCategory = 'XX' THEN XX /*catch all active*/
					WHEN PerformanceCategory = 'PIF' THEN XX /*pif*/
					WHEN PerformanceCategory = 'TRN' THEN XX /*transfered*/
					WHEN PerformanceCategory = 'PIFPRV' THEN XX /*pif previous*/
					WHEN PerformanceCategory = 'TRNPRV' THEN XX /*transfered previous*/
					WHEN PerformanceCategory = 'PRV' THEN XX /*All actions previous to this month*/
					ELSE XX
				END [PerfCategoryPriority]
			FROM
				[FsaInvMet].Monthly_BasePopulation BP
				INNER JOIN [FsaInvMet].Monthly_PerformanceCategory PF
					ON PF.BF_SSN = BP.BF_SSN
					AND PF.LN_SEQ = BP.LN_SEQ
				LEFT OUTER JOIN [FsaInvMet].Monthly_Military M
					ON M.BF_SSN = BP.BF_SSN
					AND M.LN_SEQ = BP.LN_SEQ
		) Perf
	) LoanPerf
	INNER JOIN
	( --Borrower level segment with priority
		SELECT
			Seg.BF_SSN,
			Seg.Segment,
			ROW_NUMBER() OVER (PARTITION BY Seg.BF_SSN ORDER BY Seg.SegmentPriority) [LoanSegmentPriority]
		FROM
			( --Borrower level segment
				SELECT
					BP.BF_SSN,
					BP.Segment,
					CASE 
						WHEN BP.Segment = X THEN X /*Rehab*/
						WHEN BP.Segment = X THEN X /*ConPlus*/
						WHEN BP.Segment = X THEN X /*Grad <X*/
						WHEN BP.Segment = X THEN X /*Grad >X*/
						WHEN BP.Segment = X THEN X /*NonGrad <X*/
						WHEN BP.Segment = X THEN X /*NonGrad >X*/
						WHEN BP.Segment = X THEN X /*Non Categorized*/
						ELSE X
					END [SegmentPriority]
				FROM
					[FsaInvMet].Monthly_BasePopulation BP
			) Seg	 
	) BorrSeg 
	ON BorrSeg.BF_SSN = LoanPerf.BF_SSN
	AND BorrSeg.LoanSegmentPriority = X

GO

/****** Object:  View [FsaInvMet].[Monthly_BorrowerLevel]    Script Date: X/XX/XXXX XX:XX:XX AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

--CREATE VIEW [FsaInvMet].[Monthly_BorrowerLevel]

--AS

SELECT 
	* 
FROM
(
	SELECT 
		BD.BF_SSN,
		MIN(BD.LD_LON_EFF_ADD) AS LD_LON_EFF_ADD,
		MIN(BD.LD_LON_ACL_ADD) AS LD_LON_ACL_ADD,
		MAX(PerfLoan.WC_DW_LON_STA) AS WC_DW_LON_STA,
		MAX(PerfLoan.SPEC_FORB_IND) AS SPEC_FORB_IND,
		MAX(PerfLoan.BorrDefermentIndicator) AS DefermentIndicator,
		MAX(BD.LN_DLQ_MAX) AS LN_DLQ_MAX,
		MAX(PerfLoan.LC_CAM_LON_STA) AS LC_CAM_LON_STA,
		SUM(ISNULL(BD.LA_OTS_PRI_ELG,X)) AS LA_CUR_PRI,
		SUM(ISNULL(BD.WA_TOT_BRI_OTS,X)) AS WA_TOT_BRI_OTS,
		SUM(ISNULL(BD.LA_OTS_PRI_ELG,X) + ISNULL(BD.WA_TOT_BRI_OTS,X)) AS TOT_AMT,
		CASE WHEN COUNT(DISTINCT BD.IC_LON_PGM) = X THEN MAX(BD.IC_LON_PGM)
			 ELSE 'MX' /*mixed loan programs*/
		END AS LON_PGM,
		COUNT(DISTINCT BD.LN_SEQ) AS LOAN_COUNT,
		SUM(CASE WHEN BD.PerformanceCategory = 'PIFPRV' THEN X ELSE X END) AS PIF_CT_BX_REP_MO, /*first day prev month*/
		SUM(CASE WHEN BD.PerformanceCategory = 'PIF' THEN X ELSE X END) AS PIF_CT_REP_MO, /*first and last day prev month*/
		SUM(CASE WHEN BD.PerformanceCategory = 'TRNPRV' THEN X ELSE X END) AS DSTAT_CT_BX_REP_MO, /*first day prev month*/
		SUM(CASE WHEN BD.PerformanceCategory = 'TRN' THEN X ELSE X END) AS DSTAT_CT_REP_MO,/*first and last day prev month*/
		MAX(BD.LD_PIF_RPT) AS LD_PIF_RPT,
		MAX(PerfLoan.LD_STA_LONXX) AS LD_STA_LONXX,
		MAX(PerfLoan.ActiveMilitaryIndicator) AS IS_ACTIVE_MILT,
		MIN(PerfLoan.BILL_SATISFIED) AS BILL_SATISFIED,
		MAX(PerfLoan.BorrSegment) AS SEGMENT,
		MAX(PerfLoan.PerformanceCategory) AS PerformanceCategory,
		MAX(BD.DX_ADR_EML) AS DX_ADR_EML,
		MAX(BD.PIF_TRN_DT) AS PIF_TRN_DT
	FROM
		[FsaInvMet].[Monthly_LoanLevel] BD
		LEFT JOIN [FsaInvMet].[Monthly_LoanLevel] PerfLoan 
			ON PerfLoan.BF_SSN = BD.BF_SSN
			AND PerfLoan.LN_SEQ = BD.LN_SEQ
			AND PerfLoan.LoanStatusPriority = X
			AND PerfLoan.LoanSegmentPriority = X
	GROUP BY
		BD.BF_SSN
) BorrowerLevel
WHERE 
	BorrowerLevel.PIF_CT_BX_REP_MO + BorrowerLevel.DSTAT_CT_BX_REP_MO != BorrowerLevel.LOAN_COUNT
GO
