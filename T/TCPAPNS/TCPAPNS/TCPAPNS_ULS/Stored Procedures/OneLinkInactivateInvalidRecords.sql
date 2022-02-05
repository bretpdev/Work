CREATE PROCEDURE [tcpapns].[OneLinkInactivateInvalidRecords]
	
AS
DECLARE 
	@AccountNumberIndex INT = 0,
	@PhoneIndex INT = 1,
	@DateIndex INT = 2,
	@WirIndex INT = 3

BEGIN
	UPDATE 
		FP
	SET 
		DeletedAt = GETDATE(),
		DeletedBy = SUSER_NAME()
	FROM
		(
			SELECT
				FP.AccountNumber,
				FP.AddedAt,
				FP.AddedBy,
				FP.DeletedAt,
				FP.DeletedBy,
				FP.FileProcessingId,
				FP.GroupKey,
				FP.HasConsentArcOneLink,
				FP.LineData,
				FP.MobileIndicator,
				FP.Phone,
				FP.ProcessedOn,
				FP.RecordDate,
				FP.SourceFile
			FROM
			[tcpapns].OneLinkFileProcessing FP
		) FP
		LEFT JOIN
		(
			SELECT
				PD01.DF_PRS_ID,
				PD01.DF_SPE_ACC_ID,
				PD03.DC_CEP,
				PD03.DC_ALT_CEP,
				PD03.DC_OTH_CEP,
				PD03.DN_PHN,
				PD03.DN_ALT_PHN,
				PD03.DN_OTH_PHN,
				PD03.DI_PHN_VLD,
				PD03.DI_ALT_PHN_VLD,
				PD03.DI_OTH_PHN_VLD
			FROM
				ODW..PD03_PRS_ADR_PHN PD03
				INNER JOIN ODW..PD01_PDM_INF PD01
					ON PD03.DF_PRS_ID = PD01.DF_PRS_ID
		) PD03
			ON PD03.DF_SPE_ACC_ID = FP.AccountNumber
			AND 
			(
				(
					PD03.DN_PHN = FP.Phone
					AND PD03.DI_PHN_VLD = 'Y'
					AND
					(
						(
							FP.MobileIndicator = 1 
							AND FP.HasConsentArcOneLink = 1
							AND PD03.DC_CEP != 'P'
						)
						OR
						(
							FP.MobileIndicator = 1 
							AND FP.HasConsentArcOneLink = 0
							AND PD03.DC_CEP != 'N'
						)
						OR
						(
							FP.MobileIndicator = 0
							AND PD03.DC_CEP != 'L'
						)
					)
				)
				OR
				(
					PD03.DN_ALT_PHN = FP.Phone
					AND PD03.DI_ALT_PHN_VLD = 'Y'
					AND
					(
						(
							FP.MobileIndicator = 1 
							AND FP.HasConsentArcOneLink = 1
							AND PD03.DC_ALT_CEP != 'P'
						)
						OR
						(
							FP.MobileIndicator = 1 
							AND FP.HasConsentArcOneLink = 0
							AND PD03.DC_ALT_CEP != 'N'
						)
						OR
						(
							FP.MobileIndicator = 0
							AND PD03.DC_ALT_CEP != 'L'
						)
					)
				)
				OR
				(
					PD03.DN_OTH_PHN = FP.Phone
					AND PD03.DI_OTH_PHN_VLD = 'Y'
					AND
					(
						(
							FP.MobileIndicator = 1 
							AND FP.HasConsentArcOneLink = 1
							AND PD03.DC_OTH_CEP != 'P'
						)
						OR
						(
							FP.MobileIndicator = 1 
							AND FP.HasConsentArcOneLink = 0
							AND PD03.DC_OTH_CEP != 'N'
						)
						OR
						(
							FP.MobileIndicator = 0
							AND PD03.DC_OTH_CEP != 'L'
						)
					)
				)
			)
		LEFT JOIN ODW..BR03_BR_REF BR03
			ON FP.AccountNumber = BR03.DF_PRS_ID_RFR --The account number is an RF@ id in this case which it sometimes is
			AND 
			(
				(
					BR03.BN_RFR_DOM_PHN = FP.Phone
					AND BR03.BI_DOM_PHN_VLD = 'Y'
					AND 
					(
						(
							FP.HasConsentArcOneLink = 1
							AND FP.MobileIndicator = 1
							AND (BR03.BC_PRI_PHN_ALW != 'Y'
							OR BR03.BC_PRI_PHN_TYP NOT IN ('M'))
						)
						OR
						(
							FP.HasConsentArcOneLink = 0
							AND FP.MobileIndicator = 1
							AND (BR03.BC_PRI_PHN_ALW != 'N'
							OR BR03.BC_PRI_PHN_TYP NOT IN ('M'))
						)
						OR
						(
							FP.MobileIndicator = 0
							AND (BR03.BC_PRI_PHN_ALW != 'Y'
							OR BR03.BC_PRI_PHN_TYP NOT IN ('L'))
						)
					)
				)
				OR
				(
					BR03.BN_RFR_ALT_PHN = FP.Phone
					AND BR03.BI_ALT_PHN_VLD = 'Y'
					AND 
					(
						(
							FP.HasConsentArcOneLink = 1
							AND FP.MobileIndicator = 1
							AND (BR03.BC_ALT_PHN_ALW != 'Y'
							OR BR03.BC_ALT_PHN_TYP NOT IN ('M'))
						)
						OR
						(
							FP.HasConsentArcOneLink = 0
							AND FP.MobileIndicator = 1
							AND (BR03.BC_ALT_PHN_ALW != 'N'
							OR BR03.BC_ALT_PHN_TYP NOT IN ('M'))
						)
						OR
						(
							FP.MobileIndicator = 0
							AND (BR03.BC_ALT_PHN_ALW != 'Y'
							OR BR03.BC_ALT_PHN_TYP NOT IN ('L'))
						)
					)
				)
			)
	WHERE
		FP.ProcessedOn IS NULL
		AND FP.DeletedAt IS NULL
		AND FP.DeletedBy IS NULL
		AND UPPER(FP.GroupKey) NOT LIKE('P%')
		AND PD03.DF_PRS_ID IS NULL
		AND BR03.DF_PRS_ID_RFR IS NULL

		
END