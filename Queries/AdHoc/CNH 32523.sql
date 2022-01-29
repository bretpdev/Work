/*
AYXX.PF_REQ_ACT = ALTST 
and does not have AYXX.PF_REQ_ACT = ALTPR

***OUTPUT X***
And has PDXX.DI_PHN_VLD = Y

PDXX.DF_SPE_ACC_ID
PDXX.DM_PRS_X
PDXX.DM_PRS_LST

***OUTPUT X***
And does not have PDXX.DI_PHN_VLD = Y

PDXX.DF_SPE_ACC_ID
PDXX.DM_PRS_X
PDXX.DM_PRS_LST

Candice Cole - XX/XX/XXXX XX:XX PM - Review

Issue:
We need a query which will include all borrowers with an outstanding unfulfilled NTIS request for alternate format.  

Identify all borrowers with an ALTST ARC and does not have ALTPR

Output two files:
File X for borrowers with a valid phone
File X for borrowers without a valid phone

Output the following values:
Account Number
Borrower First Name
Borrower Last Name
*/

SELECT
	PDXX.DF_SPE_ACC_ID,
	PDXX.DM_PRS_X,
	PDXX.DM_PRS_LST,
	PDXX.DC_PHN,
	ISNULL(PDXX.DN_DOM_PHN_ARA,'') + ISNULL(PDXX.DN_DOM_PHN_LCL,'') + ISNULL(PDXX.DN_DOM_PHN_XCH,''),
	CASE WHEN PDXX.DF_PRS_ID IS NULL THEN 'N' ELSE 'Y' END AS ValidPhone
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..AYXX_BR_LON_ATY ALTST
		ON ALTST.BF_SSN = PDXX.DF_PRS_ID
		AND ALTST.PF_REQ_ACT = 'ALTST'
		AND ALTST.LC_STA_ACTYXX = 'A'
	LEFT JOIN CDW..AYXX_BR_LON_ATY ALTPR
		ON ALTPR.BF_SSN = PDXX.DF_PRS_ID
		AND ALTPR.PF_REQ_ACT = 'ALTPR'
		AND ALTPR.LC_STA_ACTYXX = 'A'
		AND CAST(ALTPR.LD_ATY_REQ_RCV AS DATE) >= CAST(ALTST.LD_ATY_REQ_RCV AS DATE)
	LEFT JOIN 
	(
		SELECT DISTINCT
			PDXX.DF_PRS_ID,
			PDXX.DC_PHN,
			PDXX.DN_DOM_PHN_ARA,
			PDXX.DN_DOM_PHN_LCL,
			PDXX.DN_DOM_PHN_XCH
		FROM
			CDW..PDXX_PRS_PHN PDXX
		WHERE
			PDXX.DI_PHN_VLD = 'Y'
	) PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
WHERE
	ALTPR.BF_SSN IS NULL --no completed arc after 