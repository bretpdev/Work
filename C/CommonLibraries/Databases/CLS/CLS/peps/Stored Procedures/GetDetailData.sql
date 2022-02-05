CREATE PROCEDURE [peps].[GetDetailData]
	
AS
	SELECT 
		[DETAIL_ID] AS RecordId,
		[RecordType],
		[OpeId],
		[ChangeIndicator],
		[SchoolName],
		[LocName],
		[Line1Adr],
		[Line2Adr],
		[City],
		[State],
		[County],
		[Country],
		[Zip],
		[ForeignProvinceName],
		[EligStatusInd],
		[CertTypeCd],
		[ApprovInd],
		[ActnCd],
		[ActnReasonCd],
		[ActnDt],
		[PpaSentDt],
		[PpaExecutionDt],
		[PpaExpirationDt],
		[PgmLength],
		[SchType],
		[AcadCal],
		[EthnicCd],
		[Surety],
		[RegionCd],
		[CongDist],
		[SicCd],
		[FaadsCd],
		[CloseDt],
		[InitApprDt],
		[DisapprovalDt],
		[LocationReason],
		[SystemFundedOfficeInd],
		[BranchInd],
		[CaseTeamCd],
		[ReinstateDt],
		[WebPage],
		[Filler]
	FROM 
		[CLS].[peps].[DETAIL]
	WHERE
		ProcessedAt IS NULL 
		AND
		DeletedAt IS NULL
RETURN 0
