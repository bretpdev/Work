CREATE PROCEDURE [achrirdf].[GetPendingQueueWork]
AS
	SELECT
		 pq.ProcessQueueId
		,pq.Report
		,pq.Ssn
		,pq.AccountNumber
		,pq.LoanSequence
		,pq.OwnerCode
		,pq.DefermentOrForbearanceOriginalBeginDate
		,pq.DefermentOrForbearanceOriginalEndDate
		,pq.DefermentOrForbearanceBeginDate
		,pq.DefermentOrForbearanceEndDate
		,pq.UpdatedAt
		,pq.VariableRate
		,pq.HasPartialReducedRate
	FROM
		achrirdf.ProcessQueue pq
	WHERE
		ProcessedAt IS NULL

RETURN 0
;
