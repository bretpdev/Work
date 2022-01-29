DECLARE @ACCOUNTNUMBER VARCHAR(10) = '5530110400'
DECLARE @SSN VARCHAR(9) = '520046355'

SELECT
	L.Letter,
	DD.Ssn,
	DD.ADDR_ACCT_NUM,
	DD.DocDate
FROM
	EcorrUheaa..DocumentDetails DD
	INNER JOIN EcorrUheaa..Letters L
		ON L.LetterId = DD.LetterId
WHERE
	SSN = @SSN --ENDORSER
	and dd.Active = 1
UNION

SELECT
	L.Letter,
	DD.Ssn,
	DD.ADDR_ACCT_NUM,
	DD.DocDate
FROM
	EcorrUheaa..DocumentDetails DD
	INNER JOIN EcorrUheaa..Letters L
		ON L.LetterId = DD.LetterId
WHERE 
	ADDR_ACCT_NUM = @ACCOUNTNUMBER
	and dd.Active = 1