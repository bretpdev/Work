﻿CREATE PROCEDURE [dbo].[LT_Header_Footer]
	@AccountNumber VARCHAR(10)
AS
	
	SELECT
		RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) AS Name,
		RTRIM(PD30.DX_STR_ADR_1) AS Address1,
		RTRIM(PD30.DX_STR_ADR_2) AS Address2,
		RTRIM(PD30.DM_FGN_CNY) as Country,
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		RTRIM(PD30.DM_CT) AS City,
		CASE
			WHEN LEN(RTRIM(PD30.DM_FGN_ST)) > 0 THEN  RTRIM(PD30.DM_FGN_ST)
			ELSE RTRIM(PD30.DC_DOM_ST)
		END AS [State],
		CASE	
			WHEN LEN(RTRIM(PD30.DF_ZIP_CDE)) = 9 THEN LEFT(PD30.DF_ZIP_CDE, 5) + '-' + RIGHT(RTRIM(PD30.DF_ZIP_CDE), 4)
			ELSE RTRIM(PD30.DF_ZIP_CDE)
		END AS Zip,
		CASE PD30.DI_VLD_ADR
			WHEN 'Y' THEN 1
			ELSE 0
		END AS HasValidAddress,
		'Monday - Friday 8 AM - 5 PM MT.' AS Hours1,
		'' AS Hours2
	FROM
		UDW..PD10_PRS_NME PD10
		INNER JOIN UDW..PD30_PRS_ADR PD30 ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
	WHERE
		(PD10.DF_SPE_ACC_ID = @AccountNumber)
		AND PD30.DC_ADR = 'L'

RETURN 0
