USE CDW
GO


SELECT	
	MONTH(P.LD_INT_PD_RPT_BR) AS MON,
	YEAR(P.LD_INT_PD_RPT_BR) AS YR,
	COUNT(*)
FROM OPENQUERY(LEGEND,
					'
					SELECT DISTINCT
						BF_SSN,
						LD_INT_PD_RPT_BR
					FROM
						PKUB.MR64_BR_TAX
					WHERE
						LF_TAX_YR >= 2016
						AND LD_INT_PD_RPT_BR IS NOT NULL --BORROWER HAD THE LETTER SENT (LETTER NOT SENT IF THE BORROWER DOES NOT PAY MORE THAN 600.00 IN INTEREST)
					') P
INNER JOIN CDW..PD10_PRS_NME PD10
	ON PD10.DF_PRS_ID = P.BF_SSN
LEFT JOIN CDW..PH05_CNC_EML PH05 --CHANGE TO INNER AND REMOVE WHERE TO GET E-CORR POP
	ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
	AND PH05.DI_CNC_TAX_OPI = 'Y'
WHERE
	PH05.DF_SPE_ID IS NULL
GROUP BY	
	MONTH(P.LD_INT_PD_RPT_BR),
	YEAR(P.LD_INT_PD_RPT_BR) 	
ORDER BY
	YEAR(P.LD_INT_PD_RPT_BR),
	MONTH(P.LD_INT_PD_RPT_BR) 