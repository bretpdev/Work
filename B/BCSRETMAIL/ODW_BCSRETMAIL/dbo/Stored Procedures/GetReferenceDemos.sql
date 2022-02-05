CREATE PROCEDURE [dbo].[GetReferenceDemos]
	@AccountIdentifier VARCHAR(10)
AS
	
SELECT
	DF_PRS_ID_RFR [AccountNumber],
	RTRIM(BM_RFR_1) [FirstName],
	RTRIM(BM_RFR_MID) [MiddleInitial],
	RTRIM(BM_RFR_LST) [LastName],
	RTRIM(BX_RFR_STR_ADR_1) [Address1],
	RTRIM(BX_RFR_STR_ADR_2) [Address2],
	RTRIM(BM_RFR_CT) [City],
	RTRIM(BC_RFR_ST) [State],
	RTRIM(BF_RFR_ZIP) [ZipCode],
	BI_VLD_ADR [IsValidAddress]
FROM
	ODW..BR03_BR_REF
WHERE
	DF_PRS_ID_RFR = @AccountIdentifier