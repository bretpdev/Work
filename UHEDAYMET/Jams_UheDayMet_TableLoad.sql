USE [UDW]
GO
--UHEAA MONTHLY LOAN LEVEL LOAD

TRUNCATE TABLE [uhedaymet].[Daily_LoanLevel] 
INSERT INTO [uhedaymet].[Daily_LoanLevel]

SELECT DISTINCT
	LoanPerf.BF_SSN AS BF_SSN,
	LoanPerf.LN_SEQ AS LN_SEQ,
	LoanPerf.LC_STA_LON10 AS LC_STA_LON10,
	LoanPerf.LD_STA_LON10 AS LD_STA_LON10,
	LoanPerf.LD_LON_EFF_ADD AS LD_LON_EFF_ADD,
	LoanPerf.LD_LON_ACL_ADD AS LD_LON_ACL_ADD,
	LoanPerf.LD_PIF_RPT AS LD_PIF_RPT,
	LoanPerf.LC_CAM_LON_STA AS LC_CAM_LON_STA,
	LoanPerf.DX_ADR_EML AS DX_ADR_EML,
	LoanPerf.IC_LON_PGM AS IC_LON_PGM,
	LoanPerf.LA_OTS_PRI_ELG AS LA_OTS_PRI_ELG,
	LoanPerf.WA_TOT_BRI_OTS AS WA_TOT_BRI_OTS,
	LoanPerf.LN_DLQ_MAX AS LN_DLQ_MAX,
	LoanPerf.SPEC_FORB_IND AS SPEC_FORB_IND,
	LoanPerf.WC_DW_LON_STA AS WC_DW_LON_STA,
	LoanPerf.ORD AS ORD,
	LoanPerf.BILL_SATISFIED AS BILL_SATISFIED,
	LoanPerf.Segment AS Segment,
	BorrSeg.Segment AS BorrSegment,
	LoanPerf.LF_LON_CUR_OWN AS LF_LON_CUR_OWN,
	LoanPerf.DefermentIndicator AS DefermentIndicator,
	LoanPerf.BorrDefermentIndicator AS BorrDefermentIndicator,
	LoanPerf.PIF_TRN_DT AS PIF_TRN_DT,
	LoanPerf.PerformanceCategory AS PerformanceCategory,
	LoanPerf.ActiveMilitaryIndicator AS ActiveMilitaryIndicator,
	LoanPerf.LoanStatusPriority AS LoanStatusPriority,
	BorrSeg.LoanSegmentPriority AS LoanSegmentPriority

FROM
	( --Loan level performance category with priority
		SELECT
			Perf.BF_SSN AS BF_SSN,
			Perf.BILL_SATISFIED AS BILL_SATISFIED,
			Perf.BorrDefermentIndicator AS BorrDefermentIndicator,
			Perf.DefermentIndicator AS DefermentIndicator,
			Perf.DX_ADR_EML AS DX_ADR_EML,
			Perf.GraceEnd AS GraceEnd,
			Perf.IC_LON_PGM AS IC_LON_PGM,
			Perf.LA_OTS_PRI_ELG AS LA_OTS_PRI_ELG,
			Perf.LC_CAM_LON_STA AS LC_CAM_LON_STA,
			Perf.LC_STA_LON10 AS LC_STA_LON10,
			Perf.LD_LON_ACL_ADD AS LD_LON_ACL_ADD,
			Perf.LD_LON_EFF_ADD AS LD_LON_EFF_ADD,
			Perf.LD_PIF_RPT AS LD_PIF_RPT,
			Perf.LD_STA_LON10 AS LD_STA_LON10,
			Perf.LF_LON_CUR_OWN AS LF_LON_CUR_OWN,
			Perf.LN_DLQ_MAX AS LN_DLQ_MAX,
			Perf.LN_SEQ AS LN_SEQ,
			Perf.ORD AS ORD,
			Perf.PIF_TRN_DT AS PIF_TRN_DT,
			Perf.Segment AS Segment,
			Perf.SepDate AS SepDate,
			Perf.SPEC_FORB_IND AS SPEC_FORB_IND,
			Perf.WA_TOT_BRI_OTS AS WA_TOT_BRI_OTS,
			Perf.WC_DW_LON_STA AS WC_DW_LON_STA,
			Perf.PerformanceCategory AS PerformanceCategory,
			Perf.ActiveMilitaryIndicator AS ActiveMilitaryIndicator,
			Perf.PerfCategoryPriority AS PerfCategoryPriority,
			ROW_NUMBER() OVER (PARTITION BY Perf.BF_SSN ORDER BY Perf.PerfCategoryPriority, Perf.LN_SEQ) [LoanStatusPriority]
		FROM
		( --Loan level performance category
			SELECT DISTINCT
				BP.BF_SSN AS BF_SSN,
				BP.BILL_SATISFIED AS BILL_SATISFIED,
				BP.BorrDefermentIndicator AS BorrDefermentIndicator,
				BP.DefermentIndicator AS DefermentIndicator,
				BP.DX_ADR_EML AS DX_ADR_EML,
				BP.GraceEnd AS GraceEnd,
				BP.IC_LON_PGM AS IC_LON_PGM,
				BP.LA_OTS_PRI_ELG AS LA_OTS_PRI_ELG,
				BP.LC_CAM_LON_STA AS LC_CAM_LON_STA,
				BP.LC_STA_LON10 AS LC_STA_LON10,
				BP.LD_LON_ACL_ADD AS LD_LON_ACL_ADD,
				BP.LD_LON_EFF_ADD AS LD_LON_EFF_ADD,
				BP.LD_PIF_RPT AS LD_PIF_RPT,
				BP.LD_STA_LON10 AS LD_STA_LON10,
				BP.LF_LON_CUR_OWN AS LF_LON_CUR_OWN,
				BP.LN_DLQ_MAX AS LN_DLQ_MAX,
				BP.LN_SEQ AS LN_SEQ,
				BP.ORD AS ORD,
				BP.PIF_TRN_DT AS PIF_TRN_DT,
				BP.Segment AS Segment,
				BP.SepDate AS SepDate,
				BP.SPEC_FORB_IND AS SPEC_FORB_IND,
				BP.WA_TOT_BRI_OTS AS WA_TOT_BRI_OTS,
				BP.WC_DW_LON_STA AS WC_DW_LON_STA,
				PF.PerformanceCategory AS PerformanceCategory,
				COALESCE(M.ActiveMilitaryIndicator,0) AS ActiveMilitaryIndicator,
				CASE 
					WHEN PerformanceCategory = '04' THEN 1 /*military*/
					WHEN PerformanceCategory = '12' THEN 2 /*repayment 361+*/
					WHEN PerformanceCategory = '11' THEN 3 /*repayment 271-360*/
					WHEN PerformanceCategory = '10' THEN 4 /*repayment 151-270*/
					WHEN PerformanceCategory = '09' THEN 5 /*repayment 91-150*/
					WHEN PerformanceCategory = '08' THEN 6 /*repayment 31-90*/
					WHEN PerformanceCategory = '07' THEN 7 /*repayment 6-30*/
					WHEN PerformanceCategory = '03' THEN 8 /*repayment current*/
					WHEN PerformanceCategory = '01' AND BP.BorrDefermentIndicator = 0 THEN 9 /*in school*/
					WHEN PerformanceCategory = '02' THEN 10 /*in grace*/
					WHEN PerformanceCategory = '06' THEN 11 /*in forb*/
					WHEN PerformanceCategory = '05' THEN 12 /*in defer*/
					WHEN PerformanceCategory = '01' THEN 13 /*in school with active defer/forb but no defer/forb status*/
					WHEN PerformanceCategory = '99' THEN 14 /*catch all active*/
					WHEN PerformanceCategory = 'PIF' THEN 15 /*pif*/
					WHEN PerformanceCategory = 'TRN' THEN 16 /*transfered*/
					WHEN PerformanceCategory = 'PIFPRV' THEN 17 /*pif previous*/
					WHEN PerformanceCategory = 'TRNPRV' THEN 18 /*transfered previous*/
					WHEN PerformanceCategory = 'PRV' THEN 19 /*All actions previous to this month*/
					ELSE 20
				END AS [PerfCategoryPriority]
			FROM
				[uhedaymet].Daily_BasePopulation BP
				INNER JOIN [uhedaymet].Daily_PerformanceCategory PF
					ON PF.BF_SSN = BP.BF_SSN
					AND PF.LN_SEQ = BP.LN_SEQ
				LEFT OUTER JOIN [uhedaymet].Daily_Military M
					ON M.BF_SSN = BP.BF_SSN
					AND M.LN_SEQ = BP.LN_SEQ
		) AS Perf
	) AS LoanPerf
	INNER JOIN
	( --Borrower level segment with priority
		SELECT
			Seg.BF_SSN AS BF_SSN,
			Seg.Segment AS Segment,
			ROW_NUMBER() OVER (PARTITION BY Seg.BF_SSN ORDER BY Seg.SegmentPriority) AS [LoanSegmentPriority]
		FROM
			( --Borrower level segment
				SELECT
					BP.BF_SSN,
					BP.Segment,
					CASE 
						WHEN BP.Segment = 6 THEN 1 /*Rehab*/
						WHEN BP.Segment = 1 THEN 2 /*ConPlus*/
						WHEN BP.Segment = 2 THEN 3 /*Grad <3*/
						WHEN BP.Segment = 3 THEN 4 /*Grad >3*/
						WHEN BP.Segment = 4 THEN 5 /*NonGrad <3*/
						WHEN BP.Segment = 5 THEN 6 /*NonGrad >3*/
						WHEN BP.Segment = 7 THEN 7 /*Non Categorized*/
						ELSE 8
					END [SegmentPriority]
				FROM
					[uhedaymet].Daily_BasePopulation BP
			) AS Seg	 
	) AS BorrSeg 
	ON BorrSeg.BF_SSN = LoanPerf.BF_SSN
	AND BorrSeg.LoanSegmentPriority = 1
	

