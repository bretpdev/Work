USE [CentralData]
GO
/****** Object:  StoredProcedure [mildefpro].[GetMilitaryDefermentsAppliedLastMonth]    Script Date: 9/16/2021 9:53:40 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [mildefpro].[GetMilitaryDefermentsAppliedLastMonth]
	@Begin DATE, --Defaults to 1st day of previous month
	@End DATE --Defaults to last day of previous month
AS

	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		CONCAT(CentralData.dbo.PascalCase(PD10.DM_PRS_1), ' ', CentralData.dbo.PascalCase(PD10.DM_PRS_LST)) AS BorrowerName,
		LN50.LD_DFR_BEG AS BeginDate,
		LN50.LD_DFR_END AS EndDate,
		ISNULL(ARC.RequestReceivedDate, DF10.LD_CRT_REQ_DFR) AS RequestReceivedDate,
		LN50.LD_DFR_APL AS AppliedDate,
		SUM(LN10.LA_CUR_PRI) AS CurrentBalance
	FROM
		UDW..PD10_PRS_NME PD10
		INNER JOIN UDW..LN10_LON LN10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
			AND LN10.LC_STA_LON10 = 'R' 
			AND LN10.LA_CUR_PRI > 0.00
		INNER JOIN UDW..LN50_BR_DFR_APV LN50
			ON LN50.BF_SSN = PD10.DF_PRS_ID
			AND LN50.LC_STA_LON50 = 'A'
			AND LN50.LN_SEQ = LN10.LN_SEQ
			AND LN50.LD_DFR_APL IS NOT NULL
			AND LN50.LD_DFR_APL BETWEEN @Begin AND @End
		INNER JOIN UDW..DF10_BR_DFR_REQ DF10
			ON DF10.BF_SSN = LN50.BF_SSN
			AND DF10.LC_DFR_TYP IN ('38', '04', '40') --Military deferment types
			AND DF10.LC_DFR_STA = 'A'
			AND DF10.LC_STA_DFR10 = 'A'
			AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
		LEFT JOIN
		(
			SELECT
				LN85.BF_SSN,
				LN85.LN_SEQ,
				MAX(AY10.LD_ATY_REQ_RCV) AS RequestReceivedDate
			FROM
				UDW..AY10_BR_LON_ATY AY10
				INNER JOIN UDW..LN85_LON_ATY LN85
					ON LN85.BF_SSN = AY10.BF_SSN
					AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ
			WHERE
				AY10.PF_REQ_ACT = 'MIDEF'
				AND AY10.LC_STA_ACTY10 = 'A'
			GROUP BY
				LN85.BF_SSN,
				LN85.LN_SEQ
		) ARC
			ON ARC.BF_SSN = LN10.BF_SSN
			AND ARC.LN_SEQ = LN10.LN_SEQ
	GROUP BY
		PD10.DF_SPE_ACC_ID, 
		CONCAT(CentralData.dbo.PascalCase(PD10.DM_PRS_1), ' ', CentralData.dbo.PascalCase(PD10.DM_PRS_LST)),
		LN50.LD_DFR_BEG,
		LN50.LD_DFR_END,
		ISNULL(ARC.RequestReceivedDate, DF10.LD_CRT_REQ_DFR),
		LN50.LD_DFR_APL
	ORDER BY
		PD10.DF_SPE_ACC_ID ASC

RETURN 0
