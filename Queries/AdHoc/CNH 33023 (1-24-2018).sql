(
SELECT 
	AYXX.BF_SSN,
	CAST(AYXX.LD_ATY_REQ_RCV AS DATE) AS ProcessOnDate,
	AYXX.LF_USR_REQ_ATY AS RequestedBy,
	AYXX.PF_REQ_ACT AS ARC
FROM
	CDW..AYXX_BR_LON_ATY AYXX
WHERE 
	AYXX.PF_REQ_ACT IN ('IBAPV','ICAPV','REAPV','PEAPV'/*Approvals*/,'IBDNX','IBDNX','IBDNX','IBDNX','IBDNX','IBDNX','IBDNX','IBDNY','IDRID','IDRDN','ICDNY','PEDNY','REDNY','PEDNX','PEDNX','PEDNX','PEDNX','PEDNX','PEDNX'/*Denials*/)
	AND CAST(AYXX.LD_ATY_REQ_RCV AS DATE) BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'

UNION ALL

SELECT 
	PDXX.DF_PRS_ID,
	CAST(AAP.ProcessOn AS DATE) AS ProcessOnDate,
	CreatedBy AS RequestedBy,
	ARC 
FROM
	CLS..ArcAddProcessing AAP
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON AAP.AccountNumber = PDXX.DF_SPE_ACC_ID
WHERE
	AAP.LN_ATY_SEQ IS NULL
	AND AAP.ARC IN ('IBAPV','ICAPV','REAPV','PEAPV'/*Approvals*/,'IBDNX','IBDNX','IBDNX','IBDNX','IBDNX','IBDNX','IBDNX','IBDNY','IDRID','IDRDN','ICDNY','PEDNY','REDNY','PEDNX','PEDNX','PEDNX','PEDNX','PEDNX','PEDNX'/*Denials*/)
	AND CAST(AAP.ProcessOn AS DATE) > 'XXXX-XX-XX'
)
ORDER BY
	ARC,
	ProcessOnDate