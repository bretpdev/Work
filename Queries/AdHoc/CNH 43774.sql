SELECT
	R.*
FROM
	CDW..CS_TransferX TX
	INNER JOIN CDW..PDXX_PRS_DTH R
		ON R.DF_PRS_ID = TX.BF_SSN
WHERE
	TX.DidTransfer = X

SELECT
	R.*
FROM
	CDW..CS_TransferX TX
	INNER JOIN CDW..PDXX_PRS_DSA R
		ON R.DF_PRS_ID = TX.BF_SSN
WHERE
	TX.DidTransfer = X


SELECT
	R.*
FROM
	CDW..CS_TransferX TX
	INNER JOIN CDW..PDXX_PRS_BKR R
		ON R.DF_PRS_ID = TX.BF_SSN
WHERE
	TX.DidTransfer = X