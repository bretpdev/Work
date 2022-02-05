CREATE PROCEDURE [pridrcrp].[GetDateInDefFor]
(
	@BorrowerInformationId INT,
	@EffectiveDate DATE
)
AS
BEGIN

	SELECT
		CAST(MAX(CASE WHEN Covered IS NULL THEN 0 ELSE Covered END) AS BIT) AS Covered
	FROM
		CLS.pridrcrp.BorrowerInformation BI
		LEFT JOIN
		(
			SELECT
				BI.BorrowerInformationId,
				MAX(CASE WHEN LN50.BF_SSN IS NULL THEN 0 ELSE 1 END) AS Covered
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
				LEFT JOIN CDW..LN50_BR_DFR_APV LN50
					ON FS10.BF_SSN = LN50.BF_SSN
					AND FS10.LN_SEQ = LN50.LN_SEQ	
					AND LN50.LC_STA_LON50 = 'A'
					AND LN50.LC_DFR_RSP != '003'
					AND @EffectiveDate BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END					
			WHERE
				BI.BorrowerInformationId = @BorrowerInformationId
				AND BI.DeletedAt IS NULL
			GROUP BY
				BI.BorrowerInformationId

			UNION
	
			SELECT
				BI.BorrowerInformationId,
				MAX(CASE WHEN LN60.BF_SSN IS NULL THEN 0 ELSE 1 END) AS Covered
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
				LEFT JOIN CDW..LN60_BR_FOR_APV LN60
					ON FS10.BF_SSN = LN60.BF_SSN
					AND FS10.LN_SEQ = LN60.LN_SEQ	
					AND LN60.LC_STA_LON60 = 'A'
					AND LN60.LC_FOR_RSP != '003'	
					AND @EffectiveDate BETWEEN LN60.LD_FOR_BEG AND LN60.LD_FOR_END				
			WHERE
				BI.BorrowerInformationId = @BorrowerInformationId
				AND BI.DeletedAt IS NULL
			GROUP BY
				BI.BorrowerInformationId
		) DefFor
			ON BI.BorrowerInformationId = DefFor.BorrowerInformationId
	WHERE
		BI.DeletedAt IS NULL

END