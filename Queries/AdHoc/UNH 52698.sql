/*
Find all accounts where the following ARC was added between and including 1/1/15 - 12/31/16.

ARC = END60 or DLFD1 or DLFDL

Output in Excel - 
Acct #
ARC
ARC Requested Date


*/

USE UDW
GO


SELECT
	PD10.DF_SPE_ACC_ID [Acct #],
	AY10.PF_REQ_ACT [ARC],
	AY10.LD_REQ_RSP_ATY_PRF
FROM
	PD10_PRS_NME PD10
	INNER JOIN AY10_BR_LON_ATY AY10 ON AY10.BF_SSN = PD10.DF_PRS_ID
WHERE
	AY10.PF_REQ_ACT IN 
	(
		'DL201',
		'DL401',
		'DL801',
		'DL911',
		'DL141',
		'DL171',
		'DL203',
		'DL231'
	)
	AND
	AY10.LD_REQ_RSP_ATY_PRF BETWEEN '2015-01-01' AND '2016-12-31'