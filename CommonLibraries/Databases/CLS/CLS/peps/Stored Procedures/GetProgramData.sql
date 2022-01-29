CREATE PROCEDURE [peps].[GetProgramData]
	
AS
	SELECT
		[PROGRAM_ID] AS RecordId,
		[RecordType],
		[OpeId],
		[ChangeIndicator],
		[FpellStartDt],
		[FpellEndDt],
		[FpellApprovInd],
		[FfelStafStartDt],
		[FfelStafEndDt],
		[FfelStafApprovInd],
		[FfelStafUnsubStartDt],
		[FfelStafUnsubEndDt],
		[FfelStafUnsubApprovInd],
		[FfelPlusStartDt],
		[FfelPlusEndDt],
		[FfelPlusApprovInd],
		[FfelSlsStartDt],
		[FfelSlsEndDt],
		[FfelSlsApprovInd],
		[FdslpStafStartDt],
		[FdslpStafEndDt],
		[FdslpStafApprovInd],
		[FdslpStafUnsubStartDt],
		[FdslpStafUnsubEndDt],
		[FdslpStafUnsubApprovInd],
		[FdslpPlusStartDt],
		[FdslpPlusEndDt],
		[FdslpPlusApprovInd],
		[FperkinsStartDt],
		[FperkinsEndDt],
		[FperkinsApprovInd],
		[FseogStartDt],
		[FseogEndDt],
		[FseogApprovInd],
		[FwsPrivSecEmplStartDt],
		[FwsPrivSecEmpEndDt],
		[FwsPrivSecEmplApprovInd],
		[FwsJobLocDevStartDt],
		[FwsJobLocDevEndDt],
		[FwsJobLocDevApprovInd],
		[FwsComServStartDt],
		[FwsComServEndDt],
		[FwsComServApprovInd],
		[Filler]
	FROM 
		[CLS].[peps].[PROGRAM]
	WHERE
		ProcessedAt IS NULL 
		AND
		DeletedAt IS NULL
RETURN 0
