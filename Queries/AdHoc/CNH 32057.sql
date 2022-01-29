USE CDW
GO
SELECT
	PDXX.DF_SPE_ACC_ID [Acct #],
	LNXX.LN_SEQ [Loan seq],
	LNXX.LC_STA_LONXX [LNXX status],
	LNXX.LA_CUR_PRI [Current Prin],
	LNXX.LC_STA_LONXX [Repayment status],
	LNXX.LN_RPS_SEQ
FROM
	PDXX_PRS_NME PDXX
	INNER JOIN LNXX_LON LNXX ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN DWXX_DW_CLC_CLU DWXX ON DWXX.BF_SSN = LNXX.BF_SSN AND DWXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN LNXX_LON_RPS LNXX ON LNXX.BF_SSN = LNXX.BF_SSN AND LNXX.LN_SEQ = LNXX.LN_SEQ
WHERE
	LNXX.LC_STA_LONXX = 'R'
	AND
	LNXX.LA_CUR_PRI > X.XX
	AND
	DWXX.WC_DW_LON_STA = 'XX'
ORDER BY
	[Acct #],
	[Loan seq]


--LNXX.LC-STA-LONXX = R & LNXX.LA-CUR-PRI > X Death Status DWXX.WC_DW_LON_STA = �XX�

--Output - in Excel
--Acct # = PDXX.
--Loan seq = LNXX.LN-SEQ
--LNXX status = LNXX.LC-STA-LONXX
--Current Prin = LNXX.LA-CUR-PRI
--Repayment status = LNXX.LC-STA-LONXX
