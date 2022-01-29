﻿CREATE PROCEDURE [bcsretmail].[GetCaliforniaBorrower]
	@AccountNumber varchar(10)
AS

BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	-- Pulls Borrower data when Borrower account number sent in
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID [AccountNumber],
		PD10.DF_SPE_ACC_ID + ',' + RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) + ',' + ISNULL(PD32.DX_ADR_EML, PD32.DX_CNC_EML_ADR) [EmailData],
		1 [ArcNeeded],
		PD32.EmailPriority [EmailPriority],
		0 [Priority],
		PD30.DC_DOM_ST [State]
	FROM
		PD10_PRS_NME PD10
		INNER JOIN PD30_PRS_ADR PD30
			ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
		INNER JOIN DW01_DW_CLC_CLU DW01
			ON PD10.DF_PRS_ID = DW01.BF_SSN
		INNER JOIN 
			( -- email address
				SELECT
					*,
					ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) [EmailPriority] -- number in order of Email.PriorityNumber
				FROM
				(
					SELECT
						PD10.DF_PRS_ID,
						PD32.DX_ADR_EML,
						PH05.DX_CNC_EML_ADR,
						CASE 
							WHEN PD32.DC_ADR_EML = 'H' THEN 1 -- home
							WHEN PD32.DC_ADR_EML = 'A' THEN 2 -- alternate
							WHEN PD32.DC_ADR_EML = 'W' THEN 3 -- work
						END [PriorityNumber]
					FROM
						PD10_PRS_NME PD10
						LEFT JOIN PD32_PRS_ADR_EML PD32
							ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
							AND PD32.DI_VLD_ADR_EML = 'Y' -- valid email address
							AND PD32.DC_STA_PD32 = 'A' -- active email address record
						LEFT JOIN PH05_CNC_EML PH05
							ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID
							AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
					WHERE
						@AccountNumber IN (PD10.DF_PRS_ID, PD10.DF_SPE_ACC_ID)
				) Email
			) PD32
			ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
			AND PD32.EmailPriority = 1
	WHERE
		@AccountNumber IN (PD10.DF_SPE_ACC_ID, PD10.DF_PRS_ID)
		--We want to filter out the non CA borrowers in the code
		--AND
		--PD30.DC_DOM_ST = 'CA'
		AND
		DW01.WC_DW_LON_STA NOT IN (16, 17, 18, 19, 20, 21)
		AND	(PD32.DX_ADR_EML IS NOT NULL OR PD32.DX_CNC_EML_ADR IS NOT NULL)

END
