--CNH XXXXX

--We need to query the system for completed unemployment deferments and economic hardship deferments. 
--I'll also need two different output files for query. 
--X. (Unemployment Deferment between XX/XX - current) SSN. ARC: LSXXX and WRDXX. Date requested, Date Responded, USER ID. 
--X. (EH Deferment between XX/XX - current). SSN, ARC: LSXXX. Date Requested, Date Responded, USER ID. 

--X. (Unemployment Deferment between XX/XX - current) SSN. ARC: LSXXX and WRDXX. Date requested, Date Responded, USER ID. 
SELECT
	AYXX.BF_SSN AS SSN,
	AYXX.PF_REQ_ACT AS ARC,
	CAST(AYXX.LD_ATY_REQ_RCV AS DATE) AS [Date Requested],
	CAST(AYXX.LD_ATY_RSP AS DATE) AS [Date Responded],
	AYXX.LF_PRF_BY AS [USER ID]
FROM
	CDW..AYXX_BR_LON_ATY AYXX
WHERE
	AYXX.LD_ATY_REQ_RCV BETWEEN 'XXXX-XX-XX' AND CAST(GETDATE() AS DATE)
	AND AYXX.PF_REQ_ACT IN ('LSXXX', 'WRDXX')
ORDER BY PF_REQ_ACT


--X. (EH Deferment between XX/XX - current). SSN, ARC: LSXXX. Date Requested, Date Responded, USER ID.
SELECT
	AYXX.BF_SSN AS SSN,
	AYXX.PF_REQ_ACT AS ARC,
	CAST(AYXX.LD_ATY_REQ_RCV AS DATE) AS [Date Requested],
	CAST(AYXX.LD_ATY_RSP AS DATE) AS [Date Responded],
	AYXX.LF_PRF_BY AS [USER ID]
FROM
	CDW..AYXX_BR_LON_ATY AYXX
WHERE
	AYXX.LD_ATY_REQ_RCV BETWEEN 'XXXX-XX-XX' AND CAST(GETDATE() AS DATE)
	AND AYXX.PF_REQ_ACT IN ('LSXXX')
ORDER BY PF_REQ_ACT