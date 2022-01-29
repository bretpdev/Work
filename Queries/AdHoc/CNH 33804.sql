--select * from openquery(legend,
--'
--SELECT
--	*
--FROM
--	PKUB.GRSX_TRF_SER_DAT


--')
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT DISTINCT
	FSXX.BF_SSN AS SSN,
	FSXX.LN_SEQ AS LOAN_SEQ,
	FSXX.LF_FED_AWD + RIGHT('XXX' +CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(X)),X) AS AWARD_ID
FROM CDW..LNXX_LON LNXX
INNER JOIN
(
	SELECT * FROM OPENQUERY(LEGEND,
	'
	SELECT DISTINCT
		*
	FROM
		PKUB.BTXX_MAJ_BCH
	WHERE
		PM_SER_PRV = ''XXXXXX''

	')
) OQ
	ON OQ.PF_MAJ_BCH = LNXX.PF_MAJ_BCH
INNER JOIN CDW..FSXX_DL_LON FSXX
	ON FSXX.BF_SSN = LNXX.BF_SSN
	AND FSXX.LN_SEQ = LNXX.LN_SEQ
INNER JOIN CDW..PDXX_PRS_NME PDXX
	ON PDXX.DF_PRS_ID = LNXX.BF_SSN
WHERE
	LNXX.LC_STA_LONXX = 'R'