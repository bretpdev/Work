USE CDW
GO

SELECT
	*
FROM
	CDW..AYXX_BR_LON_ATY
WHERE
	PF_REQ_ACT = 'MIDEF'
	AND LF_PRF_BY = 'UTXXXXX'
	AND LD_ATY_RSP > 'XX/XX/XXXX'
