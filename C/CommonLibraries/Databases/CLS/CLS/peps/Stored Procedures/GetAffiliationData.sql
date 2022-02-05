CREATE PROCEDURE [peps].[GetAffiliationData]

AS
	SELECT 
		[COA_ID] AS RecordId,
		[RecordType],
		[NewOpeId] AS OpeId,
		[ChangeIndicator],
		[PreviousOpeId],
		[CoaActnCd],
		[CoaEfftDt],
		[DefaultCoaCd],
		[Filler]
	FROM 
	   [CLS].[peps].[COA] 
	WHERE
		ProcessedAt IS NULL 
		AND
		DeletedAt IS NULL 



RETURN 0
