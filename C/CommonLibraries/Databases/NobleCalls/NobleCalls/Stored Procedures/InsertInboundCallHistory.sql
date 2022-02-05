

CREATE PROCEDURE [dbo].[InsertInboundCallHistory]
	@Data AS CallData READONLY
AS
BEGIN
	--UPDATE ANY RECORDS THAT WERE PREVIOUSLY LOADED THAT DO NOT HAVE A REGION
	UPDATE
		NCH
	SET
		RegionId = CC.RegionId
	FROM
		NobleCallHistory NCH
		INNER JOIN CallCampaigns CC
			ON NCH.CallCampaign = CC.CallCampaign
	WHERE
		NCH.RegionId IS NULL


	INSERT INTO NobleCallHistory(NobleRowId, AccountIdentifier, CallType, ListId, CallCampaign, ActivityDate, PhoneNumber, AgentId, DispositionCode, AdditionalDispositionCode, RegionId, VoxFileId, IsInbound, CallLength)
		SELECT
			D.NobleRowId, 
			CASE
				WHEN ISNULL(D.AccountIdentifier, '') = '' AND CC.RegionId IN (1,2) THEN UPD42.DF_SPE_ACC_ID -- Uheaa and OneLink 
				WHEN ISNULL(D.AccountIdentifier, '') = '' AND CC.RegionId = 3 THEN CPD42.DF_SPE_ACC_ID -- CornerStone
			ELSE D.AccountIdentifier END [AccountIdentifier], 
			D.CallType,
			D.ListId,
			D.CallCampaign,
			D.ActivityDate, 
			D.PhoneNumber, 
			D.AgentId,
			D.DispositionCode,
			D.AdditionalDispositionCode,
			CC.RegionId,
			D.VoxFileId,
			D.IsInbound,
			D.CallLength
		FROM
			@Data D
			LEFT JOIN NobleCallHistory NCH
				ON D.NobleRowId = NCH.NobleRowId 
				AND	D.CallType = NCH.CallType
				AND	D.CallCampaign = NCH.CallCampaign
				AND	D.ActivityDate = NCH.ActivityDate
				AND	D.PhoneNumber = NCH.PhoneNumber
				AND NCH.CreatedAt > CAST(GETDATE() AS DATE)
			LEFT JOIN CallCampaigns CC
				ON CC.CallCampaign = D.CallCampaign
			LEFT JOIN 
			(
				SELECT DISTINCT
					PD10.DF_SPE_ACC_ID,
					PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL [PhoneNumber],
					ROW_NUMBER() OVER (PARTITION BY PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL ORDER BY PD42.DD_PHN_VER DESC, PD42.DI_PHN_VLD DESC) [VerifyOrder]
				FROM 
					CDW..PD40_PRS_PHN PD42
					INNER JOIN CDW..PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = PD42.DF_PRS_ID
				WHERE
					PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL != ''
			) CPD42 ON CPD42.PhoneNumber = D.PhoneNumber AND CPD42.VerifyOrder = 1
			LEFT JOIN 
			( 
				SELECT DISTINCT
					PD10.DF_SPE_ACC_ID,
					PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL [PhoneNumber],
					ROW_NUMBER() OVER (PARTITION BY PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL ORDER BY PD42.DD_PHN_VER DESC, PD42.DI_PHN_VLD DESC) [VerifyOrder]
				FROM 
					UDW..PD42_PRS_PHN PD42
					INNER JOIN UDW..PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = PD42.DF_PRS_ID
				WHERE
					PD42.DN_DOM_PHN_ARA + PD42.DN_DOM_PHN_XCH + PD42.DN_DOM_PHN_LCL != ''
			) UPD42 ON UPD42.PhoneNumber = D.PhoneNumber AND UPD42.VerifyOrder = 1
			
		WHERE
			NCH.NobleRowId IS NULL
			
END