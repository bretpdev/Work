CREATE PROCEDURE [peps].[GetClosureData]

AS
	SELECT
		[CLOSURE_ID] AS RecordId,
		[RecordType],
		[OpeId],
		[ChangeIndicator],
		[ClosureDtCurrent],
		[ClosureDtPrevious],
		[HistoryCd],
		[UnauthorizedLocationInd],
		[TuitionRecoveryFund],
		[PerkinsInd],
		[KnownAmount],
		[StateBondInd],
		[SchoolBondAmount],
		[RecordHolderDescription],
		[VerifiedBy],
		[CreatedOnDt],
		[ModifiedDt],
		[Filler]
	FROM 
		[CLS].[peps].[CLOSURE]
	WHERE
		ProcessedAt IS NULL 
		AND
		DeletedAt IS NULL 
RETURN 0
