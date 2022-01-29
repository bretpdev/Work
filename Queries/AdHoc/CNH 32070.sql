select COUNT(*), SUM(PaymentAmount), ProcessedDate from CLS..CheckByPhone where ProcessedDate >= 'XXXX-XX-XX' group by ProcessedDate order by ProcessedDate desc -- XX
select * from CLS..CheckByPhone where ProcessedDate >= 'XXXX-XX-XX' order by ProcessedDate desc -- XX
select * from openquery(legend, 'select * from webflsX.RMXX_ONL_PAY')

SELECT 
	CBP.PaymentAmount,
	CBP.EffectiveDate,
	CBP.FileName,
	RMXX.* 
FROM 
	CDW..RMXX_BR_RMT_PST RMXX
	LEFT OUTER JOIN 
	(
		SELECT 
			PDXX.DF_PRS_ID, 
			CBP.PaymentAmount,
			CBP.EffectiveDate,
			CBP.FileName
		FROM 
			CLS..CheckByPhone CBP 
			INNER JOIN CDW..PDXX_PRS_NME PDXX 
				ON PDXX.DF_SPE_ACC_ID = CBP.AccountNumber
		WHERE 
			CBP.ProcessedDate >= 'XXXX-XX-XX' 
	) CBP 
		ON RMXX.BF_SSN = CBP.DF_PRS_ID
WHERE 
	RMXX.LD_RMT_BCH_INI ='XXXX-XX-XX XX:XX:XX.XXX' 
	AND RMXX.LC_RMT_BCH_SRC_IPT = 'T' 
	AND RMXX.LN_RMT_BCH_SEQ IN(XX)
	AND CBP.PaymentAmount IS NULL	

SELECT 
	*
FROM 
	CDW..RMXX_BR_RMT_PST RMXX
	LEFT OUTER JOIN 
	(
		SELECT 
			PDXX.DF_PRS_ID, 
			CBP.PaymentAmount,
			CBP.EffectiveDate,
			CBP.FileName
		FROM 
			CLS..CheckByPhone CBP 
			INNER JOIN CDW..PDXX_PRS_NME PDXX 
				ON PDXX.DF_SPE_ACC_ID = CBP.AccountNumber
		WHERE 
			CBP.ProcessedDate >= 'XXXX-XX-XX' 
	) CBP 
		ON RMXX.BF_SSN = CBP.DF_PRS_ID
WHERE 
	RMXX.LD_RMT_BCH_INI ='XXXX-XX-XX XX:XX:XX.XXX' 
	AND RMXX.LC_RMT_BCH_SRC_IPT = 'T' 
	AND RMXX.LN_RMT_BCH_SEQ IN(XX)
	--AND CBP.PaymentAmount IS NOT NULL