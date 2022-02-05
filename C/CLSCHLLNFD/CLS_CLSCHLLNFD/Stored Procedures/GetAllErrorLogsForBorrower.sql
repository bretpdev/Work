CREATE PROCEDURE [clschllnfd].[GetAllErrorLogsForBorrower]
	@BorrowerSsn char(9)
AS
	SELECT 
		BorrowerSsn,
		AccountNumber,
		LoanSeq,
		DisbursementSeq,
		Arc,
		ErrorMessage,
		SessionMessage,
		SchoolClosureDataId,
		ArcAddProcessingId,
		AddedAt
	FROM
		[clschllnfd].ErrorLogs
	WHERE
		BorrowerSsn = @BorrowerSsn
		AND CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE)
RETURN 0
