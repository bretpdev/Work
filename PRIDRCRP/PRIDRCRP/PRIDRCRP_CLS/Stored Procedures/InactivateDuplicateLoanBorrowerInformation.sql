CREATE PROCEDURE [pridrcrp].[InactivateDuplicateLoanBorrowerInformation]
AS

UPDATE BI
SET 
	DeletedAt = GETDATE(),
	DeletedBy = SUSER_NAME()
FROM
	CLS.pridrcrp.BorrowerInformation BI
	INNER JOIN
	(
		SELECT DISTINCT
			BI_LN_1.BF_SSN,
			BI_LN_1.LN_SEQ,
			CASE 
				WHEN BI_1.[Page] < BI_2.[Page]
					THEN BI_1.BorrowerInformationId
				WHEN BI_1.[Page] > BI_2.[Page]
					THEN BI_2.BorrowerInformationId
			END AS BorrowerInformationId
		FROM
			(
				SELECT DISTINCT
					BI.BorrowerInformationId,
					FS10.BF_SSN,
					FS10.LN_SEQ
				FROM
					CLS.pridrcrp.BorrowerInformation BI
					INNER JOIN CLS.pridrcrp.Disbursements D
						ON D.BorrowerInformationId = BI.BorrowerInformationId
					INNER JOIN CDW..FS10_DL_LON FS10
						ON FS10.BF_SSN = BI.Ssn
						AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)), 3) = BI.Ssn + 
							CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
									WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
									WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
							END + D.LoanId
				WHERE
					BI.DeletedAt IS NULL
					AND BI.DeletedBy IS NULL
			) BI_LN_1
			INNER JOIN
			(
				SELECT DISTINCT
					BI.BorrowerInformationId,
					FS10.BF_SSN,
					FS10.LN_SEQ
				FROM
					CLS.pridrcrp.BorrowerInformation BI
					INNER JOIN CLS.pridrcrp.Disbursements D
						ON D.BorrowerInformationId = BI.BorrowerInformationId
					INNER JOIN CDW..FS10_DL_LON FS10
						ON FS10.BF_SSN = BI.Ssn
						AND FS10.LF_FED_AWD + RIGHT('000' + CAST(FS10.LN_FED_AWD_SEQ AS VARCHAR(3)), 3) = BI.Ssn + 
							CASE WHEN D.LoanType IN('PLUS','GPLUS','CON PLUS') THEN 'P'
									WHEN D.LoanType IN('SUB','CON SUB') THEN 'S'
									WHEN D.LoanType IN('UNSUB','CON USUB') THEN 'U'
							END + D.LoanId
				WHERE
					BI.DeletedAt IS NULL
					AND BI.DeletedBy IS NULL
			)BI_LN_2
				ON BI_LN_1.BF_SSN = BI_LN_2.BF_SSN
				AND BI_LN_1.LN_SEQ = BI_LN_2.LN_SEQ
				AND BI_LN_1.BorrowerInformationId != BI_LN_2.BorrowerInformationId
			INNER JOIN CLS.pridrcrp.BorrowerInformation BI_1
				ON BI_LN_1.BorrowerInformationId = BI_1.BorrowerInformationId 
				--Deleted flags are checked in the above join to BI_LN_1
			INNER JOIN CLS.pridrcrp.BorrowerInformation BI_2
				ON BI_LN_2.BorrowerInformationId = BI_2.BorrowerInformationId 
				--Deleted flags are checked in the above join to BI_LN_2
	) DEL
		ON BI.BorrowerInformationId = DEL.BorrowerInformationId
WHERE
	BI.DeletedAt IS NULL
	AND BI.DeletedBy IS NULL