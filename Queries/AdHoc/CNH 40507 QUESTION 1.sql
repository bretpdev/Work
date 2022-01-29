
DROP TABLE IF EXISTS #GRACE
SELECT * INTO #GRACE FROM OPENQUERY(LEGEND,
'
	SELECT DISTINCT
		LNXX.BF_SSN,
		LNXX.LN_SEQ,
		SDXX.LD_SCL_SPR,
		LNXX.LD_END_GRC_PRD_ALI
	FROM
		PKUB.LNXX_LON_STU_OSD  LNXX
		INNER JOIN 
		(
			SELECT 
				BF_SSN,
				LN_SEQ,
				MAX(LN_STU_SPR_SEQ) AS  LN_STU_SPR_SEQ
			FROM 
				PKUB.LNXX_LON_STU_OSD 
			WHERE 
				LC_STA_LONXX = ''A''
			GROUP BY
				BF_SSN,
				LN_SEQ
		) M
			ON M.BF_SSN = LNXX.BF_SSN
			AND M.LN_SEQ = LNXX.LN_SEQ
			AND M.LN_STU_SPR_SEQ = LNXX.LN_STU_SPR_SEQ
		INNER JOIN PKUB.SDXX_STU_SPR SDXX
			ON SDXX.LF_STU_SSN = M.BF_SSN
			AND SDXX.LN_STU_SPR_SEQ = M.LN_STU_SPR_SEQ
	WHERE
		LNXX.LC_STA_LONXX = ''A''
')

select 
	*,
	datediff(day, LD_SCL_SPR, LD_END_GRC_PRD_ali) as grace_days
from 
	#grace
where
	datediff(day, LD_SCL_SPR, LD_END_GRC_PRD_ali) > XXX


