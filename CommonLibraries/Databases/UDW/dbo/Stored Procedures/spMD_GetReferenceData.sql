﻿CREATE PROCEDURE [dbo].[spMD_GetReferenceData] 
	@AccountNumber					VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SELECT 
	RF10.BF_RFR AS ReferenceID,
	RF10.BI_ATH_3_PTY AS AuthorizedThirdPartyIndicator,
	COALESCE(FMT.label,RF10.BC_RFR_REL_BR) AS RelationshipToBorrower,
	LTRIM(RTRIM(PD10Ref.DM_PRS_1)) AS FirstName,
	LTRIM(RTRIM(PD10Ref.DM_PRS_LST)) AS LastName,
	COALESCE(LTRIM(RTRIM(PD10Ref.DM_PRS_1)),'') + ' ' + COALESCE(LTRIM(RTRIM(PD10Ref.DM_PRS_LST)),'') AS FullName,
	COALESCE(CAST(CASE 
		WHEN AY10.PF_RSP_ACT IN ('CNTCT') THEN AY10.MaxDate
		ELSE NULL
	END AS VARCHAR),'') AS LastContact,
	COALESCE(CAST(AY10.MaxAttempt AS VARCHAR),'') AS LastAttempt,
	RF10.BC_STA_REFR10 AS StatusIndicator
FROM 
	UDW..RF10_RFR RF10
	INNER JOIN UDW..PD10_PRS_NME PD10Borr
		ON RF10.BF_SSN = PD10Borr.DF_PRS_ID
	INNER JOIN UDW..PD10_PRS_NME PD10Ref
		ON RF10.BF_RFR = PD10Ref.DF_PRS_ID
	LEFT OUTER JOIN 
	(
		SELECT DISTINCT
			AY10.PF_RSP_ACT,
			AY10.LF_ATY_RCP,
			MAX(AY10.LD_ATY_RSP) OVER(PARTITION BY AY10.LF_ATY_RCP, AY10.PF_RSP_ACT) AS MaxDate,
			MAX(AY10.LD_ATY_RSP) OVER(PARTITION BY AY10.LF_ATY_RCP) AS MaxAttempt
		FROM
			UDW..PD10_PRS_NME PD10
			INNER JOIN UDW..AY10_BR_LON_ATY AY10
				ON AY10.LF_ATY_RCP = PD10.DF_PRS_ID
		WHERE
			AY10.LC_STA_ACTY10 = 'A'
			AND AY10.PF_RSP_ACT IN ('CNTCT','NOCTC','INVPH')
			AND PD10.DF_SPE_ACC_ID = @AccountNumber
	) AY10
		ON AY10.LF_ATY_RCP = RF10.BF_RFR
	LEFT OUTER JOIN UDW..FormatTranslation FMT
		ON FMT.Start = RF10.BC_RFR_REL_BR
		AND FMT.FmtName = '$RFRTYP'
WHERE
	RF10.BC_STA_REFR10 = 'A'
	AND PD10Borr.DF_SPE_ACC_ID = @AccountNumber

UNION ALL

SELECT
	BR03.DF_PRS_ID_RFR AS ReferenceID,
	BR03.BI_ATH_3_PTY AS AuthorizedThirdPartyIndicator,
	COALESCE(FMT.label,BR03.BC_RFR_REL_BR) AS RelationshipToBorrower,
	LTRIM(RTRIM(BR03.BM_RFR_1)) AS FirstName,
	BR03.BM_RFR_LST AS LastName,
	COALESCE(LTRIM(RTRIM(BR03.BM_RFR_1)), '') + ' ' + COALESCE(LTRIM(RTRIM(BR03.BM_RFR_LST)), '') AS FullName,
	COALESCE(CAST(CASE WHEN COALESCE(BR03.BD_RFR_LST_CNC_HME,'') >= COALESCE(BR03.BD_RFR_LST_CNC_ALT,'') THEN COALESCE(BR03.BD_RFR_LST_CNC_HME, BR03.BD_RFR_LST_CNC_ALT) 
		 WHEN COALESCE(BR03.BD_RFR_LST_CNC_HME,'') < COALESCE(BR03.BD_RFR_LST_CNC_ALT,'') THEN COALESCE(BR03.BD_RFR_LST_CNC_ALT, BR03.BD_RFR_LST_CNC_HME) 
	END AS VARCHAR),'') AS LastContact,
	COALESCE(CAST(CASE WHEN COALESCE(BR03.BD_RFR_LST_ATT_HME,'') >= COALESCE(BR03.BD_RFR_LST_ATT_ALT,'') THEN COALESCE(BR03.BD_RFR_LST_ATT_HME, BR03.BD_RFR_LST_ATT_ALT) 
		 WHEN COALESCE(BR03.BD_RFR_LST_ATT_HME,'') < COALESCE(BR03.BD_RFR_LST_ATT_ALT,'') THEN COALESCE(BR03.BD_RFR_LST_ATT_ALT, BR03.BD_RFR_LST_ATT_HME) 
	END AS VARCHAR),'') AS LastAttempt,
	BR03.BC_STA_BR03 AS StatusIndicator
FROM
	UDW..PD10_PRS_NME PD10Borr
	INNER JOIN ODW..BR03_BR_REF BR03
		ON BR03.DF_PRS_ID_BR = PD10Borr.DF_PRS_ID
	LEFT JOIN UDW..FormatTranslation FMT
		ON FMT.Start = BR03.BC_RFR_REL_BR
		AND FMT.FmtName = '$RFRTYP'
WHERE
	PD10Borr.DF_SPE_ACC_ID = @AccountNumber
	AND BC_STA_BR03 = 'A'

		
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spMD_GetReferenceData] TO [UHEAA\Imaging Users]
    AS [dbo];
