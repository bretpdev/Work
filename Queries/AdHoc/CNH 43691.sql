USE CDW
GO

SELECT DISTINCT
	TX.BF_SSN,
	CF.CorrespondenceFormat 
FROM 
	CDW..CS_TransferX TX

	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = TX.BF_SSN
	INNER  JOIN ECorrFed..BorrowerCorrespondenceFormats BF
		ON BF.AccountNumber = PDXX.DF_SPE_ACC_ID
	INNER JOIN ECorrFed..CorrespondenceFormats CF
		ON CF.CorrespondenceFormatId = BF.CorrespondenceFormatId
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = TX.BF_SSN
WHERE
	BF.CorrespondenceFormatId != X
	and TX.didtransfer = X
GROUP BY 
TX.BF_SSN,
	CF.CorrespondenceFormat