USE CDW
GO

SELECT DISTINCT
	T1.BF_SSN,
	CF.CorrespondenceFormat 
FROM 
	CDW..CS_Transfer2 T1

	INNER JOIN CDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = T1.BF_SSN
	INNER  JOIN ECorrFed..BorrowerCorrespondenceFormats BF
		ON BF.AccountNumber = PD10.DF_SPE_ACC_ID
	INNER JOIN ECorrFed..CorrespondenceFormats CF
		ON CF.CorrespondenceFormatId = BF.CorrespondenceFormatId
	INNER JOIN CDW..LN10_LON LN10
		ON LN10.BF_SSN = T1.BF_SSN
WHERE
	BF.CorrespondenceFormatId != 1
	and T1.didtransfer = 1
GROUP BY 
T1.BF_SSN,
	CF.CorrespondenceFormat