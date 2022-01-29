USE CDW
GO

/****** Object:  View [FsaInvMet].[Daily_BorrowerLevel]    Script Date: 11/11/2019 4:23:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW FsaInvMet.Daily_BorrowerLevel

AS

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
		SUM(ISNULL(BD.LA_OTS_PRI_ELG,0)) AS LA_CUR_PRI,
		SUM(ISNULL(BD.WA_TOT_BRI_OTS,0)) AS WA_TOT_BRI_OTS,
		SUM(ISNULL(BD.LA_OTS_PRI_ELG,0) + ISNULL(BD.WA_TOT_BRI_OTS,0)) AS TOT_AMT,
		CASE WHEN COUNT(DISTINCT BD.IC_LON_PGM) = 1 THEN MAX(BD.IC_LON_PGM)
			 ELSE 'MX' /*mixed loan programs*/
		END AS LON_PGM,
		COUNT(DISTINCT BD.LN_SEQ) AS LOAN_COUNT,
		SUM(CASE WHEN BD.PerformanceCategory = 'PIFPRV' THEN 1 ELSE 0 END) AS PIF_CT_B4_REP_MO, /*first day prev month*/
		SUM(CASE WHEN BD.PerformanceCategory = 'PIF' THEN 1 ELSE 0 END) AS PIF_CT_REP_MO, /*first and last day prev month*/
		SUM(CASE WHEN BD.PerformanceCategory = 'TRNPRV' THEN 1 ELSE 0 END) AS DSTAT_CT_B4_REP_MO, /*first day prev month*/
		SUM(CASE WHEN BD.PerformanceCategory = 'TRN' THEN 1 ELSE 0 END) AS DSTAT_CT_REP_MO,/*first and last day prev month*/
		MAX(BD.LD_PIF_RPT) AS LD_PIF_RPT,
		MAX(PerfLoan.LD_STA_LON10) AS LD_STA_LON10,
		MAX(PerfLoan.ActiveMilitaryIndicator) AS IS_ACTIVE_MILT,
		MIN(PerfLoan.BILL_SATISFIED) AS BILL_SATISFIED,
		MAX(PerfLoan.BorrSegment) AS SEGMENT,
		MAX(PerfLoan.PerformanceCategory) AS PerformanceCategory,
		MAX(BD.DX_ADR_EML) AS DX_ADR_EML,
		MAX(BD.PIF_TRN_DT) AS PIF_TRN_DT
	FROM
		FsaInvMet.Daily_LoanLevel BD
		LEFT JOIN FsaInvMet.Daily_LoanLevel PerfLoan 
			ON PerfLoan.BF_SSN = BD.BF_SSN
			AND PerfLoan.LN_SEQ = BD.LN_SEQ
			AND PerfLoan.LoanStatusPriority = 1
			and PerfLoan.LoanSegmentPriority = 1
	GROUP BY
		BD.BF_SSN
) BorrowerLevel
WHERE 
	BorrowerLevel.PIF_CT_B4_REP_MO + BorrowerLevel.DSTAT_CT_B4_REP_MO != BorrowerLevel.LOAN_COUNT


GO


