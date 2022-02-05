
CREATE VIEW [FsaInvMet].Monthly_LoanLevel

AS

SELECT
	DLL.*,
	ROW_NUMBER() OVER (PARTITION BY DLL.BF_SSN ORDER BY DLL.PerfCategoryPriority) [LoanStatusPriority]
FROM
	(
		SELECT
			BP.*,
			PF.PerformanceCategory,
			COALESCE(M.ActiveMilitaryIndicator,0) AS ActiveMilitaryIndicator,
			ROW_NUMBER() OVER (PARTITION BY PF.BF_SSN, PF.PerformanceCategory ORDER BY PF.LN_SEQ) [PerformanceSequence],
			CASE 
				WHEN PerformanceCategory = '4' THEN 1 /*military*/
				WHEN PerformanceCategory = '12' THEN 2 /*repayment 361+*/
				WHEN PerformanceCategory = '11' THEN 3 /*repayment 271-360*/
				WHEN PerformanceCategory = '10' THEN 4 /*repayment 151-270*/
				WHEN PerformanceCategory = '9' THEN 5 /*repayment 91-150*/
				WHEN PerformanceCategory = '8' THEN 6 /*repayment 31-90*/
				WHEN PerformanceCategory = '7' THEN 7 /*repayment 6-30*/
				WHEN PerformanceCategory = '3' THEN 8 /*repayment current*/
				WHEN PerformanceCategory = '1' THEN 9 /*in school*/
				WHEN PerformanceCategory = '2' THEN 10 /*in grace*/
				WHEN PerformanceCategory = '6' THEN 11 /*in forb*/
				WHEN PerformanceCategory = '5' THEN 12 /*in defer*/
				WHEN PerformanceCategory = '99' THEN 13 /*catch all active*/
				WHEN PerformanceCategory = 'PIF' THEN 14 /*pif*/
				WHEN PerformanceCategory = 'TRN' THEN 15 /*transfered*/
				WHEN PerformanceCategory = 'PRV' THEN 16 /*All actions previous to this month*/
				ELSE 17 
			END [PerfCategoryPriority]
		FROM
			[FsaInvMet].Monthly_BasePopulation BP
			INNER JOIN [FsaInvMet].Monthly_PerformanceCategory PF
				ON PF.BF_SSN = BP.BF_SSN
				AND PF.LN_SEQ = BP.LN_SEQ
			LEFT OUTER JOIN [FsaInvMet].Monthly_Military M
				ON M.BF_SSN = BP.BF_SSN
				AND M.LN_SEQ = BP.LN_SEQ
	) DLL