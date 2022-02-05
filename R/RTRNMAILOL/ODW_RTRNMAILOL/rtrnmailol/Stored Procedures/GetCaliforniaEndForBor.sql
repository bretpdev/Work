CREATE PROCEDURE [rtrnmailol].[GetCaliforniaEndForBor]
	@AccountIdentifier VARCHAR(10)
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	SELECT DISTINCT
		PD01.DF_SPE_ACC_ID [AccountNumber],
		GA01.DF_PRS_ID_BR [BorSsn],
		GA01.DF_PRS_ID_EDS [EndSsn],
		PD01.DF_SPE_ACC_ID + ',' + RTRIM(PD01.DM_PRS_1) + ' ' +  RTRIM(PD01.DM_PRS_LST) + ',' + GA01.DX_EML_ADR [EmailData],
		2 [Priority],
		GA01.DC_DOM_ST [State]
	FROM
		PD01_PDM_INF PD01
		INNER JOIN 
		(
			SELECT
				GA01.DF_PRS_ID_BR,
				GA01.DF_PRS_ID_EDS,
				PD01_CO.DF_SPE_ACC_ID,
				PD03.DX_EML_ADR,
				PD01_CO.DM_PRS_1,
				PD01_CO.DM_PRS_LST,
				PD03.EmailPriority,
				PD03.DC_DOM_ST
			FROM
				GA01_APP GA01
				INNER JOIN PD01_PDM_INF PD01_CO
					ON PD01_CO.DF_PRS_ID = GA01.DF_PRS_ID_EDS
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
								WHEN PD03.DC_ADR = 'T' THEN 2 --Temporary
								WHEN PD03.DC_ADR = 'A' THEN 3 --Alternate Billing
								WHEN PD03.DC_ADR = 'I' THEN 4 --IRS
							END [PriorityNumber]
						FROM	
							PD03_PRS_ADR_PHN PD03
						WHERE
							PD03.DI_EML_ADR_VAL = 'Y'
					)Email
				) PD03
					ON PD03.DF_PRS_ID = PD01_CO.DF_PRS_ID
					AND PD03.EmailPriority = 1
			WHERE
				GA01.AC_EDS_TYP = 'C'
		) GA01
			ON GA01.DF_PRS_ID_BR = PD01.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD01.DF_SPE_ACC_ID, PD01.DF_PRS_ID)

END