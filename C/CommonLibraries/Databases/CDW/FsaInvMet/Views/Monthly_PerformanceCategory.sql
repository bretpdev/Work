
CREATE VIEW [FsaInvMet].Monthly_PerformanceCategory

AS

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
				WHEN BD.ORD = 0 THEN
					CASE 
						WHEN M.ActiveMilitaryIndicator = 1 THEN '4'
						WHEN BD.WC_DW_LON_STA IN ('02','23') AND BD.LN_DLQ_MAX < 1 THEN '1'
						WHEN BD.WC_DW_LON_STA = '01' AND BD.LN_DLQ_MAX < 1 THEN '2'
						WHEN BD.WC_DW_LON_STA IN ('03','09','10','11','15','16','17','18','19','20','21','22') THEN 
							CASE
								WHEN BD.SPEC_FORB_IND = 1 AND BD.LN_DLQ_MAX = 0 THEN '6'
								WHEN BD.LN_DLQ_MAX = 0 AND BD.LC_CAM_LON_STA = '02' THEN '1'
								WHEN BD.LN_DLQ_MAX = 0 AND BD.LC_CAM_LON_STA = '01' THEN '2'
								WHEN BD.LN_DLQ_MAX BETWEEN 0 AND 5 THEN '3'
								WHEN BD.LN_DLQ_MAX <= 30 THEN '7'
								WHEN BD.LN_DLQ_MAX <= 90 THEN '8'
								WHEN BD.LN_DLQ_MAX <= 150 THEN '9'
								WHEN BD.LN_DLQ_MAX <= 270 THEN '10'
								WHEN BD.LN_DLQ_MAX <= 360 THEN '11'
								WHEN BD.LN_DLQ_MAX > 360 THEN '12'
								ELSE '99'
							END
						WHEN BD.WC_DW_LON_STA IN ('04', '05') THEN 
							CASE
								WHEN BD.LN_DLQ_MAX = 0 AND BD.BILL_SATISFIED = 1 THEN '3'
								WHEN BD.LN_DLQ_MAX = 0 THEN CAST(CAST(BD.WC_DW_LON_STA AS TINYINT) + 1 AS VARCHAR)--status 4 is category 5 etc.
								WHEN BD.LN_DLQ_MAX BETWEEN 1 AND 5 THEN '3'
								WHEN BD.LN_DLQ_MAX <= 30 THEN '7'
								WHEN BD.LN_DLQ_MAX <= 90 THEN '8'
								WHEN BD.LN_DLQ_MAX <= 150 THEN '9'
								WHEN BD.LN_DLQ_MAX <= 270 THEN '10'
								WHEN BD.LN_DLQ_MAX <= 360 THEN '11'
								WHEN BD.LN_DLQ_MAX > 360 THEN '12'
								ELSE '99'
							END
						WHEN BD.LN_DLQ_MAX <= 30 THEN '7'
						WHEN BD.LN_DLQ_MAX <= 90 THEN '8'
						WHEN BD.LN_DLQ_MAX <= 150 THEN '9'
						WHEN BD.LN_DLQ_MAX <= 270 THEN '10'
						WHEN BD.LN_DLQ_MAX <= 360 THEN '11'
						WHEN BD.LN_DLQ_MAX > 360 THEN '12'
						ELSE '99'
					END
				WHEN BD.ORD = 1 THEN 'PIF'
				WHEN BD.ORD = 2 THEN 'TRN'
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