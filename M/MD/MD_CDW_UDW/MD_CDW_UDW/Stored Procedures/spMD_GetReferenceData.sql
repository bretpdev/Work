CREATE PROCEDURE [dbo].[spMD_GetReferenceData] 
	@AccountNumber					VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT DF_PRS_ID_RFR as ReferenceID,
			BI_ATH_3_PTY as AuthorizedThirdPartyIndicator,
			RFR_REL_BR as RelationshipToBorrower,
			BM_RFR_1 as FirstName,
			BM_RFR_LST as LastName,
			isnull(BM_RFR_1, '') + ' ' + isnull(BM_RFR_LST, '') as FullName,
			LST_CNC	as LastContact,
			LST_ATT as LastAttempt,
			[BC_STA_BR03] as StatusIndicator
	FROM dbo.BR03_Reference
	WHERE DF_SPE_ACC_ID = @AccountNumber
		
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetReferenceData] TO [Imaging Users]
    AS [dbo];

