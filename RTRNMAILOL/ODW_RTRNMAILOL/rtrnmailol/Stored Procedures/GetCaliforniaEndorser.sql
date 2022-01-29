CREATE PROCEDURE [rtrnmailol].[GetCaliforniaEndorser]
	@AccountIdentifier VARCHAR(10)
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	-- Pulls Co-borrower data when using the Co-Borrower account number
	SELECT DISTINCT
		PD01_Bor.DF_SPE_ACC_ID [AccountNumber],
		PD01_Bor.DF_PRS_ID [BorSsn],
		GA01.DF_PRS_ID_EDS [EndSsn],
		PD01_Bor.DF_SPE_ACC_ID + ',' + RTRIM(PD01_Bor.DM_PRS_1) + ' ' +  RTRIM(PD01_Bor.DM_PRS_LST) + ',' + PD03.DX_EML_ADR [EmailData],
		3 [Priority],
		PD03.DC_DOM_ST [State]
	FROM
		PD01_PDM_INF PD01
		INNER JOIN GA01_APP GA01
			ON GA01.DF_PRS_ID_EDS = PD01.DF_PRS_ID
			AND GA01.AC_EDS_TYP = 'C'
		INNER JOIN PD01_PDM_INF PD01_Bor
			ON PD01_Bor.DF_PRS_ID = GA01.DF_PRS_ID_BR
		INNER JOIN 
		(
			SELECT 
				*,
				ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.[PriorityNumber]) [EmailPriority]
			FROM
			(
				SELECT
					PD03.DF_PRS_ID,
					PD03.DX_EML_ADR,
					PD03.DC_DOM_ST,
					CASE
						WHEN PD03.DC_ADR = 'L' THEN 1 --Legal
						WHEN PD03.DC_ADR = 'T' THEN 2 --Tempoorary
						WHEN PD03.DC_ADR = 'A' THEN 3 --Alternate Billing
						WHEN PD03.DC_ADR = 'I' THEN 4 --IRS
					END [PriorityNumber]
				FROM	
					PD03_PRS_ADR_PHN PD03
				WHERE
					PD03.DI_EML_ADR_VAL = 'Y'
			)Email
		) PD03
			ON PD03.DF_PRS_ID = PD01.DF_PRS_ID
			AND PD03.EmailPriority = 1
	WHERE
		@AccountIdentifier IN (PD01.DF_SPE_ACC_ID, PD01.DF_PRS_ID)

END