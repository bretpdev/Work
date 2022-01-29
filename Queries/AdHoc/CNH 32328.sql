/*			
CNH XXXXX			
Debbie Phillips - XX/XX/XXXX XX:XX AM - In Progress			
Find all loans with an actual add date (LNXX.LD_LON_ACL_ADD) having occurred in August XXXX			
AND .			
Borrower�s loan has a X.XX balance (LNXX.LA_CUR_PRI = X.XX) 			
and 			
	Loan Status Not fully Disbursed(DWXX.WC_DW_LON_STA = �XX�) 		
	and 		
	has no Financial transaction of Disbursement/Automatic(PC_FAT_TYP = �XX� and PC_FAT_SUB_TYP = �XX�)		
	OR		
	Borrower�s loan  has Financial transaction of Disbursemet/Automatic (LNXX.PC_FAT_TYP = �XX� and LNXX.PC_FAT_SUB_TYP = �XX�)		
			
output 			
# Of New Borrower  - Count the distinct number of borrowers where borrower has no other loan sequeces with a LNXX.LD_LON_ACL_ADD before August XXXX			
*/			
USE CDW			
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED			
DECLARE	@startDate DATE = 'XXXX-XX-XX'
DECLARE @endDate DATE = DATEADD(DAY, -X, DATEADD(MONTH, X, @startDate))
	

WHILE @startDate < GETDATE()
BEGIN	
	SELECT
		CAST(YEAR(LNXX.LD_LON_ACL_ADD) AS CHAR(X)) + ' - (' + RIGHT('XX' + CAST(MONTH(LNXX.LD_LON_ACL_ADD) AS VARCHAR(X)), X) + ') ' + DATENAME(MONTH, LNXX.LD_LON_ACL_ADD) [Month],
		COUNT(DISTINCT LNXX.BF_SSN) [BorrowerCount]
	FROM			
		LNXX_LON LNXX -- start with borrowers with a loan add date within the date range		
		LEFT JOIN		
		( -- borrowers with an XX PC_FAT_TYPE on a loan within the date range		
			SELECT DISTINCT	
				LNXX.BF_SSN
			FROM	
				LNXX_LON LNXX
				INNER JOIN DWXX_DW_CLC_CLU DWXX ON DWXX.BF_SSN = LNXX.BF_SSN AND DWXX.LN_SEQ = LNXX.LN_SEQ
				INNER JOIN LNXX_FIN_ATY LNXX ON LNXX.BF_SSN = LNXX.BF_SSN AND LNXX.LN_SEQ = LNXX.LN_SEQ
			WHERE	
				CAST(LNXX.LD_LON_ACL_ADD AS DATE) BETWEEN @startDate AND @endDate
				AND
				LNXX.PC_FAT_TYP = 'XX' -- conversion
		) LNXX_XX ON LNXX_XX.BF_SSN = LNXX.BF_SSN		
		LEFT JOIN LNXX_LON LNXXX ON LNXXX.BF_SSN = LNXX.BF_SSN AND CAST(LNXXX.LD_LON_ACL_ADD AS DATE) < @startDate -- only borrowers with a loan add date before start date		
	WHERE			
		CAST(LNXX.LD_LON_ACL_ADD AS DATE) BETWEEN @startDate AND @endDate -- start with borrowers with a loan add date within the date range		
		AND		
		LNXX_XX.BF_SSN IS NULL -- exclude borrowers with an LNXX 'XX' record occurring within the date range		
		AND		
		LNXXX.BF_SSN IS NULL -- exclude borrowers with a loan add prior to the start date
	GROUP BY
		CAST(YEAR(LNXX.LD_LON_ACL_ADD) AS CHAR(X)) + ' - (' + RIGHT('XX' + CAST(MONTH(LNXX.LD_LON_ACL_ADD) AS VARCHAR(X)), X) + ') ' + DATENAME(MONTH, LNXX.LD_LON_ACL_ADD)

	SET @startDate = DATEADD(month, X, @startDate)
	SET @endDate = DATEADD(DAY, -X, DATEADD(MONTH, X, @startDate))

END