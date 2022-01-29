USE CDW
GO

/****** Object:  View [FsaInvMet].[Daily_BasePopulation]    Script Date: 11/5/2019 1:35:15 PM ******/

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW FsaInvMet.Daily_BasePopulation

AS

SELECT
	LN10.BF_SSN,
	LN10.LN_SEQ,
	LN10.LC_STA_LON10,
	LN10.LD_STA_LON10,
	LN10.LD_LON_EFF_ADD,
	LN10.LD_LON_ACL_ADD,
	LN10.LD_PIF_RPT,
	CASE 
		WHEN LN10.LA_CUR_PRI > 0 THEN LN10.LC_CAM_LON_STA
		ELSE '' 
	END AS LC_CAM_LON_STA,
	PD32.DX_ADR_EML,
	CASE 
		WHEN LN10.IC_LON_PGM IN('DLPCNS','DLPLGB','DLPLUS','DLSCNS','DLSPCN','DLSSPL','DLSTFD','DLUCNS','DLUNST','DLUSPL','TEACH' ) THEN 'DL'
		WHEN LN10.IC_LON_PGM IN('CNSLDN','PLUS','PLUSGB','SLS','STFFRD','SUBCNS','SUBSPC','UNCNS','UNSPC','UNSTFD','FISL') THEN 'FL'
		ELSE ''
	END AS IC_LON_PGM,
	LN10.LA_CUR_PRI AS LA_OTS_PRI_ELG,
	COALESCE(DW01.WA_TOT_BRI_OTS,DW01.LA_NSI_OTS,0) AS WA_TOT_BRI_OTS,
	CASE 
		WHEN LN10.LA_CUR_PRI <= 0 THEN 0
		WHEN LN10.LC_STA_LON10 = 'D' THEN COALESCE(DATEDIFF(DAY,LN16.LD_DLQ_OCC, GETDATE()),-1) + 1 /* get current day delinquency*/
		WHEN DW01.WC_DW_LON_STA IN ('04','05') THEN 0
		WHEN FORB.BF_SSN != '' THEN 0
        ELSE COALESCE(LN16.LN_DLQ_MAX, 0)
	END AS LN_DLQ_MAX,
	CASE 
		WHEN FORB.BF_SSN != '' THEN '1' 
		ELSE '0' 
	END AS SPEC_FORB_IND,
	DW01.WC_DW_LON_STA,
	CASE 
		WHEN LN10.LC_STA_LON10 = 'D' AND LN10.LA_CUR_PRI = 0 THEN 2 
		WHEN LN10.LD_PIF_RPT IS NOT NULL THEN 1 ELSE 0 
	END AS ORD,
	CASE 
		WHEN LN80.BF_SSN IS NULL THEN 0 
		ELSE 1
	END AS BILL_SATISFIED,
	SegmentLogic.Segment,
	LN10.LF_LON_CUR_OWN,
	CASE
		WHEN InDefer.BF_SSN IS NULL THEN 0 
		ELSE 1
	END AS DefermentIndicator,
	CASE
		WHEN InDeferBorr.BF_SSN IS NULL THEN 0 
		ELSE 1
	END AS BorrDefermentIndicator,
	CASE 
		WHEN LN10.LC_STA_LON10 = 'D' AND LN10.LA_CUR_PRI = 0 THEN LN10.LD_STA_LON10
		WHEN LN10.LD_PIF_RPT IS NOT NULL THEN LN10.LD_PIF_RPT ELSE NULL 
	END AS PIF_TRN_DT,
	SegmentLogic.SepDate,
	LN10.LD_END_GRC_PRD AS GraceEnd,
	CASE
		WHEN CR_5505.BF_SSN IS NOT NULL THEN 1 ELSE 0
	END AS CR_5505
FROM 
	LN10_LON LN10
	LEFT JOIN DW01_DW_CLC_CLU DW01 
		ON LN10.BF_SSN = DW01.BF_SSN 
		AND LN10.LN_SEQ = DW01.LN_SEQ
	LEFT JOIN 
	(
		SELECT
			LN16.BF_SSN,
			LN16.LN_SEQ,
			LN16.LD_DLQ_OCC,
			MAX(LN16.LN_DLQ_MAX) + 1 AS LN_DLQ_MAX
		FROM	
			LN16_LON_DLQ_HST LN16
		WHERE
			LN16.LC_STA_LON16 = '1'
			AND LN16.LC_DLQ_TYP = 'P'
			AND 
			(
				--Account for AES holidays that we dont honor
				--DATEADD(DAY,-5,CAST(GETDATE() AS DATE)) <= LN16.LD_DLQ_MAX 
				LN16.LD_DLQ_MAX >= DATEADD(DAY,-5,CAST(GETDATE() AS DATE)) --Reversed due to Sargability considerationsDATEADD(DAY,-5,CAST(GETDATE() AS DATE)) <= LN16.LD_DLQ_MAX --Account for AES holidays that we dont honor
				OR LN16.LN_DLQ_MAX >= 360
			)
		GROUP BY
			LN16.BF_SSN,
			LN16.LN_SEQ,
			LN16.LD_DLQ_OCC
	) LN16 
		ON LN16.BF_SSN = LN10.BF_SSN
		AND LN16.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN 
	( -- email address
		SELECT
			DF_PRS_ID,
			DX_ADR_EML,
			ROW_NUMBER() OVER (PARTITION BY DF_PRS_ID ORDER BY PriorityNumber) AS EmailPriority
		FROM
		(
			SELECT
				PD32.DF_PRS_ID,
				PD32.DX_ADR_EML,
				CASE 
					WHEN DC_ADR_EML = 'H' THEN 1
					WHEN DC_ADR_EML = 'A' THEN 2
					WHEN DC_ADR_EML = 'W' THEN 3
				END AS PriorityNumber
			FROM
				PD32_PRS_ADR_EML PD32
			WHERE
				PD32.DI_VLD_ADR_EML = 'Y'
				AND PD32.DC_STA_PD32 = 'A'
		) Email
	) PD32 
		ON PD32.DF_PRS_ID = LN10.BF_SSN 
		AND PD32.EmailPriority = 1 --highest priority email only
	LEFT JOIN
	( --Deferment indicator
		SELECT
			DF10.BF_SSN,
			LN50.LN_SEQ
		FROM
			DF10_BR_DFR_REQ DF10
			INNER JOIN LN50_BR_DFR_APV LN50 
				ON DF10.BF_SSN = LN50.BF_SSN 
				AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
		WHERE
			DF10.LC_DFR_STA = 'A'
			AND LN50.LC_STA_LON50 = 'A'
			AND DF10.LC_STA_DFR10 = 'A'
			AND LN50.LC_DFR_RSP != '003'      
			AND LN50.LD_DFR_END >= CAST(GETDATE() AS DATE)
			AND LN50.LD_DFR_APL <= CAST(GETDATE() AS DATE)
			AND LN50.LD_DFR_BEG <= CAST(GETDATE() AS DATE)
	) InDefer 
		ON InDefer.BF_SSN = LN10.BF_SSN
		AND InDefer.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	( --Deferment indicator borr
		SELECT DISTINCT
			DF10.BF_SSN
		FROM
			DF10_BR_DFR_REQ DF10
			INNER JOIN LN50_BR_DFR_APV LN50 
				ON DF10.BF_SSN = LN50.BF_SSN 
				AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
		WHERE
			DF10.LC_DFR_STA = 'A'
			AND LN50.LC_STA_LON50 = 'A'
			AND DF10.LC_STA_DFR10 = 'A'
			AND LN50.LC_DFR_RSP != '003'      
			AND LN50.LD_DFR_END >= CAST(GETDATE() AS DATE)
			AND LN50.LD_DFR_APL <= CAST(GETDATE() AS DATE)
			AND LN50.LD_DFR_BEG <= CAST(GETDATE() AS DATE)
	) InDeferBorr 
		ON InDeferBorr.BF_SSN = LN10.BF_SSN
	LEFT JOIN
	( -- forbearance indicator
		SELECT 
			FB10.BF_SSN,
			LN60.LN_SEQ                                       
		FROM 
			FB10_BR_FOR_REQ FB10
			INNER JOIN LN60_BR_FOR_APV LN60 
				ON FB10.BF_SSN = LN60.BF_SSN 
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
		WHERE
			FB10.LC_FOR_STA = 'A'
			AND LN60.LC_STA_LON60 = 'A'
			AND FB10.LC_STA_FOR10 = 'A'      
			AND LN60.LC_FOR_RSP != '003'
			AND CAST(LN60.LD_FOR_END AS DATE) >= CAST(GETDATE() AS DATE)
			AND CAST(LN60.LD_FOR_APL AS DATE) <= CAST(GETDATE() AS DATE)
			AND CAST(LN60.LD_FOR_BEG AS DATE) <= CAST(GETDATE() AS DATE)
	) FORB 
		ON FORB.BF_SSN = LN10.BF_SSN
		AND FORB.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	( -- forbearance category 5505
		SELECT 
			FB10.BF_SSN,
			LN60.LN_SEQ                                       
		FROM 
			FB10_BR_FOR_REQ FB10
			INNER JOIN LN60_BR_FOR_APV LN60 
				ON FB10.BF_SSN = LN60.BF_SSN 
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
		WHERE
			FB10.LC_FOR_STA = 'A'
			AND LN60.LC_STA_LON60 = 'A'
			AND FB10.LC_STA_FOR10 = 'A'
			AND LN60.LC_FOR_RSP != '003'      
			AND FB10.LC_FOR_TYP = '41'
			AND CAST(LN60.LD_FOR_END AS DATE) >= CAST(GETDATE() AS DATE)
			AND CAST(LN60.LD_FOR_APL AS DATE) <= CAST(GETDATE() AS DATE)
			AND CAST(LN60.LD_FOR_BEG AS DATE) <= CAST(GETDATE() AS DATE)
	) CR_5505 
		ON CR_5505.BF_SSN = LN10.BF_SSN
		AND CR_5505.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT
			LN80.BF_SSN,
			LN80.LN_SEQ
		FROM
			BL10_BR_BIL BL10
			INNER JOIN LN80_LON_BIL_CRF LN80
				ON BL10.BF_SSN = LN80.BF_SSN
				AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
				AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
		WHERE
			BL10.LD_BIL_DU BETWEEN CAST(DATEADD(DAY,1,EOMONTH(GETDATE(),-1)) AS DATE) /*first day of current month*/ AND CAST(EOMONTH(GETDATE(),0) AS DATE) -- last day of current month
			AND BL10.LC_IND_BIL_SNT != '5' --Exclude paid ahead bills from counting
			AND LN80.LD_BIL_DU_LON BETWEEN CAST(DATEADD(DAY,1,EOMONTH(GETDATE(),-1)) AS DATE) /*first day of current month*/ AND CAST(EOMONTH(GETDATE(),0) AS DATE) -- last day of current month
			AND LN80.LA_TOT_BIL_STS >= COALESCE(LN80.LA_BIL_CUR_DU,0)
			AND LN80.LC_STA_LON80 = 'A'
			AND LN80.LC_BIL_TYP_LON = 'P'
	) LN80
		ON LN10.BF_SSN = LN80.BF_SSN
		AND LN10.LN_SEQ = LN80.LN_SEQ	
	LEFT JOIN
	( -- segment
		SELECT 
			LN10.BF_SSN AS SSN,
			LN10.LN_SEQ AS LN_SEQ,
			COALESCE(GroupedSep.SepDateSchool, GroupedSep.SepDateDef, GroupedSep.SepDateDisb) AS SepDate,
			CASE 
				WHEN LN09.BF_SSN IS NOT NULL THEN 6
				WHEN LN10ConPlus.BF_SSN IS NOT NULL THEN 1
				WHEN LN10ConPlus.BF_SSN IS NULL AND SD10Max.LC_REA_SCL_SPR = '01' /*graduated*/
					AND COALESCE(SD10Max.LD_SCL_SPR, LN10DeferPlus.GradDeferDate, LN10GradPlus.GradRepayDate) > CAST(DATEADD(YEAR, -3, GETDATE()) AS DATE) THEN 2 /*less than 3 years ago*/
				WHEN LN10ConPlus.BF_SSN IS NULL
					AND SD10Max.LC_REA_SCL_SPR = '01' /*graduated*/
					AND COALESCE(SD10Max.LD_SCL_SPR, LN10DeferPlus.GradDeferDate, LN10GradPlus.GradRepayDate) <= CAST(DATEADD(YEAR, -3, GETDATE()) AS DATE) THEN 3 /*More than 3 years ago*/
				WHEN LN10ConPlus.BF_SSN IS NULL
					AND SD10Max.LC_REA_SCL_SPR != '01' /*didnt graduate*/
					AND SD10Max.LD_SCL_SPR > CAST(DATEADD(YEAR, -3, GETDATE()) AS DATE)  THEN 4 /*Less than 3 years ago*/
				WHEN LN10ConPlus.BF_SSN IS NULL
					AND SD10Max.LC_REA_SCL_SPR != '01' /*didnt graduate*/
					AND SD10Max.LD_SCL_SPR <= CAST(DATEADD(YEAR, -3, GETDATE()) AS DATE)  THEN 5 /*More than 3 years ago*/
				WHEN COALESCE(SD10Max.LD_SCL_SPR, LN10DeferPlus.GradDeferDate, LN10GradPlus.GradRepayDate) > CAST(DATEADD(YEAR, -3, GETDATE()) AS DATE) THEN 2 /*Less than 3 years ago*/
				WHEN COALESCE(SD10Max.LD_SCL_SPR, LN10DeferPlus.GradDeferDate, LN10GradPlus.GradRepayDate) <= CAST(DATEADD(YEAR, -3, GETDATE()) AS DATE) THEN 3 /*More than 3 years ago*/
				ELSE 7
			END AS Segment
		FROM 
			LN10_LON LN10
			LEFT JOIN
			( 
				SELECT
					BF_SSN,
					LN_SEQ
				FROM
					LN10_LON
				WHERE
					IC_LON_PGM IN ('DLPLUS', 'DLCNSL', 'DLPCNS', 'DLSCCN', 'DLSCNS', 'DLSCPL', 'DLSCSC', 'DLSCSL', 'DLSCST', 'DLSCUC', 'DLSCUN', 'DLSPCN', 'DLSSPL', 'DLUCNS', 'DLUSPL', 'DSCON', 'DUCON', 'DLSCPG', 'DLPLGB')
			) LN10ConPlus 
				ON LN10ConPlus.BF_SSN = LN10.BF_SSN
				AND LN10ConPlus.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN
			(
				SELECT
					LN10.BF_SSN,
					LN10.LN_SEQ,
					CAST(DATEADD(DAY, -180, MAX(LN15.LD_DSB)) AS DATE) AS GradRepayDate
				FROM
					LN10_LON LN10
					INNER JOIN LN15_DSB LN15 
						ON LN15.BF_SSN = LN10.BF_SSN 
						AND LN15.LN_SEQ = LN10.LN_SEQ
				WHERE
					LN10.IC_LON_PGM IN ('DGPLUS','DLPLGB')
					AND LN15.LC_STA_LON15 = '1'
					AND LN15.LD_DSB <= CAST(GETDATE() AS DATE)
				GROUP BY
					LN10.BF_SSN,
					LN10.LN_SEQ
			) LN10GradPlus 
				ON LN10GradPlus.BF_SSN = LN10.BF_SSN
				AND LN10GradPlus.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN
			(
				SELECT
					LN10.BF_SSN,
					LN10.LN_SEQ,
					CAST(DATEADD(DAY, -180, MAX(LN50.LD_DFR_END)) AS DATE) AS GradDeferDate
				FROM
					LN10_LON LN10
					INNER JOIN LN50_BR_DFR_APV LN50 
						ON LN50.BF_SSN = LN10.BF_SSN 
						AND LN50.LN_SEQ = LN10.LN_SEQ
					INNER JOIN DF10_BR_DFR_REQ DF10 
						ON DF10.BF_SSN = LN50.BF_SSN 
						AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
				WHERE
					LN10.IC_LON_PGM IN ('DGPLUS','DLPLGB')
					AND LN50.LC_STA_LON50 = 'A'
					AND DF10.LC_DFR_TYP IN ('15','16','18')
					AND DF10.LC_DFR_STA = 'A'
					AND DF10.LC_STA_DFR10 = 'A'
					AND LN50.LD_DFR_END <= CAST(GETDATE() AS DATE)
				GROUP BY
					LN10.BF_SSN,
					LN10.LN_SEQ
			) LN10DeferPlus 
				ON LN10DeferPlus.BF_SSN = LN10.BF_SSN
				AND LN10DeferPlus.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN 
			(	
				SELECT
					LN13.BF_SSN,
					LN13.LN_SEQ,
					MAX(SD10.LN_STU_SPR_SEQ) AS LN_STU_SPR_SEQ
				FROM
					SD10_STU_SPR SD10
					INNER JOIN LN13_LON_STU_OSD LN13
						ON LN13.LF_STU_SSN = SD10.LF_STU_SSN
						AND LN13.LN_STU_SPR_SEQ = SD10.LN_STU_SPR_SEQ
						AND LN13.LC_STA_LON13 = 'A'
				WHERE
					SD10.LC_STA_STU10 = 'A'
				GROUP BY 
					LN13.BF_SSN,
					LN13.LN_SEQ
			) SD10SEQ
				ON SD10SEQ.BF_SSN = LN10.BF_SSN
				AND SD10SEQ.LN_SEQ =LN10.LN_SEQ
			LEFT JOIN 
			(	
				SELECT
					LN13.BF_SSN,
					LN13.LN_SEQ,
					SD10.LN_STU_SPR_SEQ,
					SD10.LC_REA_SCL_SPR,
					SD10.LD_SCL_SPR
				FROM
					SD10_STU_SPR SD10
					INNER JOIN LN13_LON_STU_OSD LN13
						ON LN13.LF_STU_SSN = SD10.LF_STU_SSN
						AND LN13.LN_STU_SPR_SEQ = SD10.LN_STU_SPR_SEQ
						AND LN13.LC_STA_LON13 = 'A'
				WHERE
					SD10.LC_STA_STU10 = 'A'
					AND SD10.LD_SCL_SPR <= CAST(GETDATE() AS DATE)
			) SD10Max
				ON SD10Max.BF_SSN = LN10.BF_SSN
				AND SD10Max.LN_SEQ = LN10.LN_SEQ
				AND SD10MAX.LN_STU_SPR_SEQ = SD10SEQ.LN_STU_SPR_SEQ
			LEFT JOIN
			(
				SELECT
					LN09.BF_SSN,
					LN09.LN_SEQ
				FROM 
					LN09_RPD_PIO_CVN LN09
				WHERE
					LN09.LD_LON_RHB_PCV IS NOT NULL OR COALESCE(LN09.IF_LON_SRV_DFL_LON,'                ') != '                '
			) LN09 
				ON LN09.BF_SSN = LN10.BF_SSN
				AND LN09.LN_SEQ = LN10.LN_SEQ
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
						LN10.BF_SSN,
						LN10.LN_SEQ,
						0 AS LN_STU_SPR_SEQ,
						CAST(DATEADD(DAY, -180, MAX(LN15.LD_DSB)) AS DATE) AS SepDateDisb,
						NULL AS SepDateDef,
						NULL AS SepDateSchool
					FROM
						LN10_LON LN10
						INNER JOIN LN15_DSB LN15 
							ON LN15.BF_SSN = LN10.BF_SSN 
							AND LN15.LN_SEQ = LN10.LN_SEQ
					WHERE
						LN10.IC_LON_PGM IN ('DGPLUS','DLPLGB')
						AND LN15.LC_STA_LON15 = '1'
					GROUP BY
						LN10.BF_SSN,
						LN10.LN_SEQ

					UNION ALL

					SELECT
						LN10.BF_SSN,
						LN10.LN_SEQ,
						0 AS LN_STU_SPR_SEQ,
						NULL AS SepDateDisb,
						CAST(DATEADD(DAY, -180, MAX(LN50.LD_DFR_END)) AS DATE) AS SepDateDef,
						NULL AS SepDateSchool
					FROM
						LN10_LON LN10
						INNER JOIN LN50_BR_DFR_APV LN50 
							ON LN50.BF_SSN = LN10.BF_SSN 
							AND LN50.LN_SEQ = LN10.LN_SEQ
						INNER JOIN DF10_BR_DFR_REQ DF10 
							ON DF10.BF_SSN = LN50.BF_SSN 
							AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
					WHERE
						LN10.IC_LON_PGM IN ('DGPLUS','DLPLGB')
						AND LN50.LC_STA_LON50 = 'A'
						AND DF10.LC_DFR_TYP IN ('15','16','18')
						AND DF10.LC_DFR_STA = 'A'
						AND DF10.LC_STA_DFR10 = 'A'
					GROUP BY
						LN10.BF_SSN,
						LN10.LN_SEQ

					UNION ALL

					SELECT
						LN13.BF_SSN,
						LN13.LN_SEQ,
						SD10.LN_STU_SPR_SEQ,
						NULL AS SepDateDisb,
						NULL AS SepDateDef,
						SD10.LD_SCL_SPR As SepDateSchool
					FROM
						SD10_STU_SPR SD10
						INNER JOIN LN13_LON_STU_OSD LN13
							ON LN13.LF_STU_SSN = SD10.LF_STU_SSN
							AND LN13.LN_STU_SPR_SEQ = SD10.LN_STU_SPR_SEQ
							AND LN13.LC_STA_LON13 = 'A'
					WHERE
						SD10.LC_STA_STU10 = 'A'
				) SepDate
				GROUP BY
					BF_SSN,
					LN_SEQ
			) GroupedSep
				ON GroupedSep.BF_SSN = LN10.BF_SSN
				AND GroupedSep.LN_SEQ = LN10.LN_SEQ
				AND 
				(	
					GroupedSep.LN_STU_SPR_SEQ = SD10SEQ.LN_STU_SPR_SEQ
					OR GroupedSep.LN_STU_SPR_SEQ = 0
				)
	) SegmentLogic 
		ON SegmentLogic.SSN = LN10.BF_SSN
		AND SegmentLogic.LN_SEQ = LN10.LN_SEQ
WHERE 
	LN10.LC_STA_LON10 IN ('R','D','L')
	AND LN10.LD_LON_ACL_ADD <= CAST(GETDATE() AS DATE)
	AND DW01.WC_DW_LON_STA NOT IN ('06','88','98') --These are considered status errors and output to an error file in the sas


GO


