


CREATE PROCEDURE [dbo].[GetSoaLoanInfo]
		@BF_SSN char(9)
AS
		SELECT 
		LN10.LN_SEQ AS [Loan Seq],
		LN10.IC_LON_PGM as [Loan Program],
		CONVERT(VARCHAR(10),LN10.LD_LON_1_DSB,101) AS [1st Disb Date],
		'$' + CONVERT(VARCHAR(20),LN10.LA_LON_AMT_GTR, 1) AS [Principal Balance at Transfer],
		'$' + CONVERT(VARCHAR(20),LN10.LA_CUR_PRI, 1) as [Current Principal Balance]
	FROM	
		LN10_LON LN10
	WHERE 
		LN10.BF_SSN = @BF_SSN
	ORDER BY 
		LN10.LN_SEQ
RETURN 0
