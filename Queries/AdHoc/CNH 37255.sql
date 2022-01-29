/*Find all accounts where:

PF_REQ_ACT 
AYXX.PF_REQ_ACT = DITLF
AYXX.LD_ATY_REQ_RCV = X/X/XXXX � X/X/XXXX  (TLF Discharge Date)

AND:
LNXX.LON_CRB_RPT 
LNXX.LC_RPT_STA_CRB = XX-XX  (XX to XXX+ Days Delinquent)
LD_RPT_CRB = XX/X/XXXX - X/XX/XXXX  (Reported on or after  the LD_ATY_REQ_RCV  dates of X/X/XXXX � X/X/XXXX)


OUTPUT
BF_SSN =  (Borrower SSN)
LD_ATY_REQ_RCV = X/X/XXXX � X/X/XXXX   (Teacher Loan Forgiveness Date)

Issue:
As part of a Corrective Action Plan for an FSA Audit, we are requesting the following:

Any DITLF arcs from X/X/XXXX through X/X/XXXX, in which any negative credit reporting occurred after the DITLF date.

Requested output: SSN. Date of DITLF.

Let me know if this isn't a possibility or if we need to modify the request.
*/

SELECT * FROM OPENQUERY(LEGEND,
'
SELECT DISTINCT
	AYXX.BF_SSN,
	CAST(AYXX.LD_ATY_REQ_RCV AS DATE) AS LD_ATY_REQ_RCV
FROM
	PKUB.AYXX_BR_LON_ATY AYXX
	INNER JOIN PKUB.LNXX_LON_CRB_RPT LNXX
		ON LNXX.BF_SSN = AYXX.BF_SSN
		AND LNXX.LC_RPT_STA_CRB IN (''XX'',''XX'',''XX'',''XX'',''XX'')
		AND LNXX.LD_RPT_CRB > CAST(AYXX.LD_ATY_REQ_RCV AS DATE)
WHERE
	AYXX.PF_REQ_ACT = ''DITLF''
	AND CAST(AYXX.LD_ATY_REQ_RCV AS DATE) BETWEEN ''XXXX-XX-XX'' AND ''XXXX-XX-XX''
')