CREATE PROCEDURE [pridrcrp].[CheckBorrowerInformationIdHasLoan]
	@BorrowerInformationId INT
AS

--Check that the specific borrower information id maps to a loan with a balance before adding errros
SELECT
	CAST(MAX(CASE WHEN LN10.BF_SSN IS NOT NULL AND FS10.BF_SSN IS NOT NULL THEN 1 ELSE 0 END) AS BIT) AS HasLoan
FROM
	CLS.pridrcrp.BorrowerInformation BI 
	LEFT JOIN CLS.pridrcrp.Disbursements D
		ON D.BorrowerInformationId = BI.BorrowerInformationId
	LEFT JOIN CDW..FS10_DL_LON FS10
		ON FS10.BF_SSN = BI.Ssn
		AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)), 3) = BI.Ssn + 
			CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
					WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
					WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
			END + D.LoanId
	LEFT JOIN CDW..LN10_LON LN10
		ON FS10.BF_SSN = LN10.BF_SSN
		AND FS10.LN_SEQ = LN10.LN_SEQ
		AND LN10.LA_CUR_PRI > 0.00 
		AND LN10.LC_STA_LON10 = 'R'
WHERE
	BI.DeletedAt IS NULL
	AND BI.BorrowerInformationId = @BorrowerInformationId
		
