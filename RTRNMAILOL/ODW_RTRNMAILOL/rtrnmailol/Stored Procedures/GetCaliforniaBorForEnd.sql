CREATE PROCEDURE [rtrnmailol].[GetCaliforniaBorForEnd]
	@AccountIdentifier VARCHAR(10)
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	--Get borrower if the account number is the coborrower
	SELECT DISTINCT
		PD01_Bor.DF_SPE_ACC_ID [AccountNumber],
		PD01_Bor.DF_SPE_ACC_ID + ',' + RTRIM(PD01_Bor.DM_PRS_1) + ' ' +  RTRIM(PD01_Bor.DM_PRS_LST) + ',' + PD03.DX_EML_ADR [EmailData],
		1 [Priority],
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
			ON PD03.DF_PRS_ID = PD01_Bor.DF_PRS_ID
			AND PD03.EmailPriority = 1
		LEFT JOIN
		(
			SELECT
				BF_SSN
			FROM
				DC01_LON_CLM_INF
			WHERE
				(
					AF_APL_ID IS NULL
					OR
					LC_AUX_STA IN (01, 04, 05, 07, 08, 09, 10, 11, 12) --Exclude death & disability
				)
		) DC01
			ON DC01.BF_SSN = PD01_Bor.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD01.DF_SPE_ACC_ID, PD01.DF_PRS_ID)
		AND	DC01.BF_SSN IS NOT NULL

END