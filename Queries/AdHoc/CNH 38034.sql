--query based on CNH XXXXX
USE CDW
GO

DECLARE @BEGIN DATE = 'XX/XX/XXXX';
DECLARE @END DATE = 'XX/XX/XXXX';

SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID AS ACCOUNTNUMBER
	,LNXX.LN_SEQ AS LOANSEQUENCE
	,CAST(LNXX.LD_DLQ_OCC+XXX AS DATE) AS DATEXXXDAYSDELINQUENT
FROM
	PDXX_PRS_NME PDXX
	INNER JOIN LNXX_LON LNXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN LNXX_LON_DLQ_HST LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
WHERE
	LNXX.LN_DLQ_MAX >= 'XXX'
	AND (LNXX.LD_DLQ_OCC)+XXX BETWEEN @BEGIN AND @END
;