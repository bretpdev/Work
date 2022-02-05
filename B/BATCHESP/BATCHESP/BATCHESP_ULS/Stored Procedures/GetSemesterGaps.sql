CREATE PROCEDURE [batchesp].[GetSemesterGaps]
	@BorrowerSsn CHAR(9),
	@LoanSequence INT
AS
	SELECT 
		LoanSequence,
		LoanProgramType,	
		EndDate,
		EndType,
		EndSchool,	
		BeginDate,
		BeginType,
		BeginSchool
FROM
	(
		SELECT DISTINCT --Get start of semester gap
			TsayStart.BeginDate,
			TsayStart.LoanSequence,
			TsayStart.LoanProgramType,
			TsayStart.[Type] [BeginType], 
			TsayStart.DeferSchool [BeginSchool],
			ROW_NUMBER() OVER (ORDER BY TsayStart.BeginDate, TsayStart.TsayScrapedLoanInformationId DESC) RN
		FROM 
			batchesp.TsayScrapedLoanInformation TsayStart
			LEFT JOIN batchesp.TsayScrapedLoanInformation OverlapCheck
				ON OverlapCheck.BorrowerSsn = TsayStart.BorrowerSsn
				AND TsayStart.LoanSequence = OverlapCheck.LoanSequence
				AND OverlapCheck.ApprovalStatus = 'APPROVED'
				AND SUBSTRING(OverlapCheck.[Type], 1, 1) = 'D' --Exclude forbs
				AND OverlapCheck.ProcessedAt IS NULL
				AND TsayStart.BeginDate > OverlapCheck.BeginDate 
				AND TsayStart.BeginDate < OverlapCheck.EndDate
		WHERE
			TsayStart.BorrowerSsn = @BorrowerSsn
			AND TsayStart.LoanSequence = @LoanSequence
			AND TsayStart.ApprovalStatus = 'APPROVED'
			AND SUBSTRING(TsayStart.[Type], 1, 1) = 'D' --Only want to include deferments, as forbed dates still constitute semester gaps
			AND TsayStart.ProcessedAt IS NULL
			AND OverlapCheck.BorrowerSsn IS NULL --Filtered out all overlapping dates
	) BeginInterval
INNER JOIN 
	(
		SELECT DISTINCT  --Get end of semester gap
			TsayEnd.EndDate, 
			TsayEnd.[Type] [EndType], 
			TsayEnd.DeferSchool [EndSchool],
			ROW_NUMBER() OVER (ORDER BY TsayEnd.BeginDate, TsayEnd.TsayScrapedLoanInformationId DESC) RN
		FROM 
			batchesp.TsayScrapedLoanInformation TsayEnd
			LEFT JOIN batchesp.TsayScrapedLoanInformation OverlapCheck
				ON OverlapCheck.BorrowerSsn = TsayEnd.BorrowerSsn
				AND TsayEnd.LoanSequence = OverlapCheck.LoanSequence
				AND OverlapCheck.ApprovalStatus = 'APPROVED'
				AND SUBSTRING(OverlapCheck.[Type], 1, 1) = 'D' --Exclude forbs
				AND OverlapCheck.ProcessedAt IS NULL
				AND TsayEnd.EndDate > OverlapCheck.BeginDate 
				AND TsayEnd.EndDate < OverlapCheck.EndDate
		WHERE
			TsayEnd.BorrowerSsn = @BorrowerSsn
			AND TsayEnd.LoanSequence = @LoanSequence
			AND TsayEnd.ApprovalStatus = 'APPROVED'
			AND SUBSTRING(TsayEnd.[Type], 1, 1) = 'D' --Only want to include deferments, as forbed dates still constitute semester gaps
			AND TsayEnd.ProcessedAt IS NULL
			AND OverlapCheck.BorrowerSsn IS NULL --Filtered out all overlapping dates
	) EndInterval
		ON BeginInterval.RN - 1 = EndInterval.RN --Map begginning of specific interval to its matching end by row numbers
WHERE
	EndDate < BeginDate
	AND DATEADD(MONTH, 6, EndDate) >= CAST(GETDATE() AS DATE) --We only want gaps that are within the last six months. Session won't allow older ones to be added.
	AND DATEDIFF(DAY, EndDate, BeginDate) > 1
	AND 
		( --Interval w/o d/f is over summer break
			( 
				MONTH(EndDate) IN (4, 5, 6) 
				AND MONTH(BeginDate) IN (6, 7, 8, 9)
			)
		OR --Or interval w/o d/f is over winter break
			(
				(MONTH(EndDate) = 12) 
				AND (MONTH(BeginDate) = 1)
			)
		)
RETURN 0
