CREATE PROCEDURE [pridrcrp].[GetLoanAddDate]
(
	@BorrowerInformationId INT
)
AS 
BEGIN

SELECT
	MAX(LN10.LD_LON_ACL_ADD) AS LoanAddDate
FROM
	[CLS].[pridrcrp].[BorrowerInformation] BI
	INNER JOIN CLS.pridrcrp.Disbursements D
		ON D.BorrowerInformationId = BI.BorrowerInformationId
	INNER JOIN CDW..FS10_DL_LON FS10
		ON FS10.BF_SSN = BI.Ssn
		AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR), 3) = BI.Ssn + 
			CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
					WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
					WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
			END + D.LoanId
	INNER JOIN CDW..LN10_LON LN10
		ON FS10.BF_SSN = LN10.BF_SSN
		AND FS10.LN_SEQ = LN10.LN_SEQ
WHERE
	BI.BorrowerInformationId = @BorrowerInformationId
	--We don't want to use ln10 flags here because they are checked later and checking here adds incomplete data
	--AND LN10.LC_STA_LON10 = 'R'
	--AND LN10.LA_CUR_PRI > 0.00
	AND BI.DeletedAt IS NULL
GROUP BY 
	BI.Ssn
END