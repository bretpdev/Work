--Issue:
--We’re requesting a query for all notations of doc id “LSCOR” in Compass for borrowers with a California address from 7/1/2021 to 10/31/2021.

--Please have this request completed as soon as possible, 
--as it is urgent information that needs to be relayed over to auditors by the end of today.
SELECT DISTINCT
	PD10.DF_SPE_ACC_ID,
	D.Document,
	AY10.PF_REQ_ACT AS Arc,
	AY10.LN_ATY_SEQ AS ArcSequence,
	CAST(AY10.LD_ATY_REQ_RCV AS DATE) AS ArcRequestDate,
	RTRIM(PD30.DX_STR_ADR_1) AS Address1,
	RTRIM(PD30.DX_STR_ADR_2) AS Address2,
	RTRIM(PD30.DM_CT) AS City,
	RTRIM(PD30.DC_DOM_ST) AS [State],
	RTRIM(PD30.DF_ZIP_CDE) AS Zip
FROM
	ULS.docid.Documents D
	INNER JOIN ULS.docid.DocIdMapping DIM
		ON DIM.DocumentId = D.DocumentsId
	INNER JOIN ULS.docid.Arcs A
		ON A.ArcId = DIM.ArcId
	INNER JOIN UDW..AY10_BR_LON_ATY AY10
		ON AY10.PF_REQ_ACT = A.Arc
		AND CAST(AY10.LD_ATY_REQ_RCV AS DATE) BETWEEN '7/1/2021' AND '10/31/2021'
	INNER JOIN UDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = AY10.BF_SSN
		AND PD30.DC_ADR = 'L'
		AND PD30.DC_DOM_ST = 'CA'
		AND PD30.DI_VLD_ADR = 'Y'
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
WHERE
	D.Document = 'LSCOR'