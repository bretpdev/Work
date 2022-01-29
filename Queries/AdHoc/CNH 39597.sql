--Please provide a data dump of RSXX, RSXX, RSXX, LNXX, and LNXX for account XXXXXXXXXX. This is needed for an IDR DCR so we can update the preconversion payment counter.

DECLARE @SSN CHAR(X) = (SELECT DF_PRS_ID FROM CDW..PDXX_PRS_NME WHERE DF_SPE_ACC_ID = 'XXXXXXXXXX');

--RSXX
SELECT * 
FROM 
	CDW..RSXX_IBR_RPS 
WHERE 
	BF_SSN = @SSN

--RSXX
SELECT *
FROM
	CDW..RSXX_BR_RPD
WHERE
	BF_SSN = @SSN

--RSXX
SELECT *
FROM
	CDW..RSXX_IBR_IRL_LON
WHERE
	BF_SSN = @SSN

--LNXX
SELECT *
FROM
	CDW..LNXX_LON_RPS
WHERE
	BF_SSN = @SSN

--LNXX
SELECT *
FROM
	CDW..LNXX_RPD_PIO_CVN
WHERE BF_SSN = @SSN
