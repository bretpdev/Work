-- =============================================
-- Author:		Bret Pehrson
-- Create date: 10/13/2015
-- Description:	Returns a borrowers demographics from account number
-- =============================================
CREATE PROCEDURE [dbo].[GetBorrowerDemosFromAccountNumber]
	@AccountNumber varchar(10) 
AS
BEGIN
	SELECT
		PD10.DM_PRS_1 AS [FirstName],
		PD10.DM_PRS_MID AS [MiddleInitial],
		PD10.DM_PRS_LST AS [LastName],
		PD10.DD_BRT AS [DateOfBirth],
		PD30.DX_STR_ADR_1 AS [Address1],
		PD30.DX_STR_ADR_2 AS [Address2],
		PD30.DM_CT AS [City],
		PD30.DC_DOM_ST AS [State],
		PD30.DF_ZIP_CDE AS [ZipCode],
		PD30.DM_FGN_ST AS [ForeignState],
		PD30.DM_FGN_CNY AS [ForeignCountry],
		PD32.DX_ADR_EML AS [Email],
		PD42_H.DN_DOM_PHN_ARA + PD42_H.DN_DOM_PHN_XCH + PD42_H.DN_DOM_PHN_LCL AS [HomePhone],
		PD42_A.DN_DOM_PHN_ARA + PD42_A.DN_DOM_PHN_XCH + PD42_A.DN_DOM_PHN_LCL AS [AlternatePhone],
		PD42_W.DN_DOM_PHN_ARA + PD42_W.DN_DOM_PHN_XCH + PD42_W.DN_DOM_PHN_LCL AS [WorkPhone]
	FROM
		PD10_Borrower PD10
		LEFT JOIN PD30_Address PD30 ON PD10.DF_SPE_ACC_ID = PD30.DF_SPE_ACC_ID
		LEFT JOIN PD32_Email PD32 ON PD10.DF_SPE_ACC_ID = PD32.DF_SPE_ACC_ID
		LEFT JOIN PD42_Phone PD42_H ON PD10.DF_SPE_ACC_ID = PD42_H.DF_SPE_ACC_ID AND PD42_H.DC_PHN = 'H'
		LEFT JOIN PD42_Phone PD42_W ON PD10.DF_SPE_ACC_ID = PD42_W.DF_SPE_ACC_ID AND PD42_W.DC_PHN = 'W'
		LEFT JOIN PD42_Phone PD42_A ON PD10.DF_SPE_ACC_ID = PD42_A.DF_SPE_ACC_ID AND PD42_A.DC_PHN = 'A'
	WHERE
		PD10.DF_SPE_ACC_ID = @AccountNumber
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBorrowerDemosFromAccountNumber] TO [db_executor]
    AS [dbo];

