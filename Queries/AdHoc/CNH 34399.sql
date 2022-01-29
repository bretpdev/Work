SELECT
	MONTH(AYXX.LD_ATY_REQ_RCV) AS [Month],
	YEAR(AYXX.LD_ATY_REQ_RCV) AS [Year],
	SUM(CASE WHEN DID.DocId IN ('CRDFM', 'CRDDS', 'CREHD', 'CRGFD', 'CRSCD', 'CRMLD', 'CRPPD', 'CRPSD', 'CRRTD', 'CRUND', 'CRWMD') THEN X ELSE X END) AS Deferment,
	SUM(CASE WHEN DID.DocId IN ('CRFRB', 'CRMFB', 'CRSDB', 'CRTFF') THEN X ELSE X END) AS Forbearance,
	SUM(CASE WHEN DID.DocId IN ('CRIDR', 'CRIBR', 'CRINC', 'CRADI') THEN X ELSE X END) AS IDR,
	SUM(CASE WHEN DID.DocId IN ('CRRPC') THEN X ELSE X END) AS RepaymentChanges,
	SUM(CASE WHEN DID.DocId IN ('CRBKP') THEN X ELSE X END) AS Bankruptcy,
	SUM(CASE WHEN DID.DocId IN ('CRMIL') THEN X ELSE X END) AS Military,
	SUM(CASE WHEN DID.DocId IN ('CRLCD', 'CRDIS') THEN X ELSE X END) AS CreditDisputes,
	SUM(CASE WHEN DID.DocId IN ('CRBCR') THEN X ELSE X END) AS Other
FROM 
	CLS..DocIdDocument DID
	INNER JOIN CDW..AYXX_BR_LON_ATY AYXX
		ON AYXX.PF_REQ_ACT = DID.Arc
WHERE 
	DID.DocId IN 
	(
		'CRDFM', 'CRDDS', 'CREHD', 'CRGFD', 'CRSCD', 'CRMLD', 'CRPPD', 'CRPSD', 'CRRTD', 'CRUND', 'CRWMD', --Deferments
		'CRFRB', 'CRMFB', 'CRSDB', 'CRTFF', --Forbearances
		'CRIDR', 'CRIBR', 'CRINC', 'CRADI', --IDR
		'CRRPC', --Repayment Changes
		'CRBKP', --Bankruptcy
		'CRMIL', --Military
		'CRLCD', 'CRDIS', --Credit disputes
		'CRBCR' --All Other Docs
	)
	AND AYXX.LC_STA_ACTYXX = 'A'
	AND CAST(AYXX.LD_ATY_REQ_RCV AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'
GROUP BY
	YEAR(AYXX.LD_ATY_REQ_RCV),
	MONTH(AYXX.LD_ATY_REQ_RCV)
ORDER BY 
	[Year],
	[Month]