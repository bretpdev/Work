CREATE PROCEDURE [duplrefs].[GetReferences]
	@BorrowerQueueId INT

AS

	--This sproc is for recovery for a specific user so that they can retrieve demo values they inserted into the script
	SELECT 
		RQ.ReferenceQueueId,
		RQ.BorrowerQueueId,
		RQ.RefId,
		RQ.RefName,
		RQ.RefAddress1,
		RQ.RefAddress2,
		RQ.RefCity,
		RQ.RefState,
		RQ.RefZip,
		RQ.RefCountry,
		RQ.RefPhone,
		RQ.RefStatus,
		RQ.ValidAddress,
		RQ.ValidPhone,
		RQ.DemosChanged,
		RQ.ZipChanged,
		RQ.ManuallyWorked,
		RQ.Duplicate,
		RQ.PossibleDuplicate,
		RQ.ProcessedAt,
		RQ.Lp2fProcessedAt,
		RQ.ArcAddProcessingId
	FROM
		duplrefs.ReferenceQueue RQ
		INNER JOIN duplrefs.BorrowerQueue BQ
			ON BQ.BorrowerQueueId = RQ.BorrowerQueueId
			AND BQ.DeletedAt IS NULL
			AND BQ.ProcessedAt IS NULL
	WHERE
		RQ.DeletedAt IS NULL --Processed references filtered out in the C# code after demos compared. Hence, no need for "RQ.ProcessedAt IS NULL" or "DR.Lp2fProcessedAt IS NULL" check.
		AND RQ.BorrowerQueueId = @BorrowerQueueId
		

RETURN 0
