CREATE PROCEDURE [clschllnfd].[GetProcessingData]
AS
	SELECT 
		SCL.SchoolClosureDataId,
		SCL.BorrowerSsn,
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		SCL.StudentSsn,
		SCL.LoanSeq,
		SCL.DisbursementSeq,
		SCL.DischargeAmount,
		SCL.DischargeDate,
		SCL.AddedAt,
		SCL.ProcessedAt
	FROM
		CDW..PD10_PRS_NME PD10
		INNER JOIN clschllnfd.SchoolClosureData SCL
			ON SCL.BorrowerSsn = PD10.DF_PRS_ID
		INNER JOIN
		(
			SELECT
				MIN(SCL.SchoolClosureDataId) AS SchoolClosureDataId
			FROM
				clschllnfd.SchoolClosureData SCL
				LEFT JOIN
				(
					SELECT
						C.BorrowerSsn,
						C.LoanSeq,
						C.SchoolClosureDataId
					FROM
						clschllnfd.SchoolClosureData C
					WHERE
						(C.ProcessedAt IS NOT NULL AND DeletedAt IS NULL AND CAST(C.ProcessedAt AS DATE) = CAST(GETDATE() AS DATE)) --If loan already processed today
					GROUP BY
						C.BorrowerSsn,
						C.LoanSeq,
						C.SchoolClosureDataId
				) LoanLevel
					ON LoanLevel.BorrowerSsn = SCL.BorrowerSsn
					AND LoanLevel.LoanSeq = SCL.LoanSeq
				LEFT JOIN
				(
					SELECT
						E.BorrowerSsn,
						E.LoanSeq
					FROM
						clschllnfd.ErrorLogs E
					WHERE
						CAST(E.AddedAt AS DATE) = CAST(GETDATE() AS DATE)
				) Errors
					ON Errors.BorrowerSsn = SCL.BorrowerSsn
					AND Errors.LoanSeq = SCL.LoanSeq
			WHERE
				SCL.ProcessedAt IS NULL
				AND SCL.DeletedAt IS NULL
				AND LoanLevel.BorrowerSsn IS NULL --Didn't already process today
				AND Errors.BorrowerSsn IS NULL -- Didn't already error today
		) MinRow
			ON MinRow.SchoolClosureDataId = SCL.SchoolClosureDataId
RETURN 0
