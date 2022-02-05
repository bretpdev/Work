CREATE PROCEDURE [dbo].[InsertInboundCallHistory]
(
	@Data AS CallData READONLY
)
AS
BEGIN

	DECLARE @ScriptId VARCHAR(100) = 'DIACTCMTS'
	DECLARE @OnelinkRegion INT = (SELECT RegionId FROM Regions WHERE UPPER(Region) = 'ONELINK')
	DECLARE @CompassRegion INT = (SELECT RegionId FROM Regions WHERE UPPER(Region) = 'COMPASSUHEAA')

	DELETE FROM _InboundCallData

	INSERT INTO 
		_InboundCallData
	SELECT
		[NobleRowId],            
		[CallType],                  
		[UDW_DF_SPE_ACC_ID],			
		[AccountIdentifier],         
		[AreaCode],                  
		[PhoneNumber],               
		[CallCampaign],              
		[DispositionCode],           
		[AdditionalDispositionCode], 
		[AgentId],                   
		[ActivityDate],              
		[EffectiveTime],             
		[ListId],                    
		[VoxFileId],                
		[IsInbound],                 
		[CallLength],                
		[TimeACW],                   
		[TimeHold],                  
		[AgentHold],                 
		[Filler1],                  
		[Filler3],                   
		[Filler4],                   
		[d_record_id],               
		[DialerField1], 
		[DialerField2], 
		[DialerField3], 
		[DialerField4], 
		[DialerField5], 
		[DialerField6], 
		[DialerField7], 
		[DialerField8], 
		[DialerField9], 
		[DialerField10],
		[DialerField11],
		[DialerField12],
		[DialerField13],
		[DialerField14],
		[DialerField15],
		[DialerField16],
		[DialerField17],
		[DialerField18],
		[DialerField19],
		[DialerField20],
		[DialerField21],
		[DialerField22],
		[DialerField23],
		[DialerField24],
		[DialerField25],
		[UDW_CoborrowerAccountNumber]
	FROM
		@Data

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

	MERGE
		dbo.NobleCallHistory NCH
	USING
	(
		SELECT
			D.NobleRowId, 
			CASE
				WHEN ISNULL(D.AccountIdentifier, '') = '' AND CP.RegionId IN (1,2) THEN UPD42.DF_SPE_ACC_ID
				ELSE D.AccountIdentifier
			END [AccountIdentifier], 
			D.CallType,
			D.ListId,
			D.CallCampaign,
			D.ActivityDate, 
			D.PhoneNumber, 
			D.AgentId,
			D.DispositionCode,
			D.AdditionalDispositionCode,
			CP.RegionId,
			D.VoxFileId,
			D.IsInbound,
			D.CallLength,
			D.TimeACW,
			D.TimeHold,
			D.AgentHold,
			D.Filler1,
			D.Filler3,
			D.Filler4,
			D.d_record_id,
			D.DialerField1, 
			D.DialerField2, 
			D.DialerField3, 
			D.DialerField4, 
			D.DialerField5, 
			D.DialerField6, 
			D.DialerField7, 
			D.DialerField8, 
			D.DialerField9, 
			D.DialerField10,
			D.DialerField11,
			D.DialerField12,
			D.DialerField13,
			D.DialerField14,
			D.DialerField15,
			D.DialerField16,
			D.DialerField17,
			D.DialerField18,
			D.DialerField19,
			D.DialerField20,
			D.DialerField21,
			D.DialerField22,
			D.DialerField23,
			D.DialerField24,
			D.DialerField25
		FROM
			_InboundCallData D
			INNER JOIN CampaignPrefixes CP
				ON D.CallCampaign LIKE (CP.CampaignPrefix + '%')
				AND CP.RegionId IN (@CompassRegion, @OnelinkRegion)
				AND CP.ScriptId = @ScriptId
				AND CP.Active = 1
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
	) PullData
		ON PullData.NobleRowId = NCH.NobleRowId 
		AND	PullData.CallType = NCH.CallType
		AND	PullData.CallCampaign = NCH.CallCampaign
		AND	PullData.ActivityDate = NCH.ActivityDate
		AND	PullData.PhoneNumber = NCH.PhoneNumber
		AND PullData.Filler1 = NCH.Filler1
		AND PullData.Filler3 = NCH.Filler3
		AND PullData.Filler4 = NCH.Filler4
		AND PullData.d_record_id = NCH.d_record_id
		AND PullData.AgentId = NCH.AgentId
	WHEN MATCHED THEN
		UPDATE SET
			NCH.AccountIdentifier = PullData.AccountIdentifier,
			NCH.ListId = PullData.ListId,
			NCH.AgentId = PullData.AgentId,
			NCH.DispositionCode = PullData.DispositionCode,
			NCH.AdditionalDispositionCode = PullData.AdditionalDispositionCode,
			NCH.RegionId = PullData.RegionId,
			NCH.VoxFileId = PullData.VoxFileId,
			NCH.IsInbound = PullData.IsInbound,
			NCH.CallLength = PullData.CallLength,
			NCH.TimeACW = PullData.TimeACW,
			NCH.TimeHold = PullData.TimeHold,
			NCH.AgentHold = PullData.AgentHold,
			NCH.Filler1 = PullData.Filler1,
			NCH.Filler3 = PullData.Filler3,
			NCH.Filler4 = PullData.Filler4,
			NCH.d_record_id = PullData.d_record_id,
			NCH.DialerField1 = PullData.DialerField1,
			NCH.DialerField2 = PullData.DialerField2,
			NCH.DialerField3 = PullData.DialerField3,
			NCH.DialerField4 = PullData.DialerField4,
			NCH.DialerField5 = PullData.DialerField5,
			NCH.DialerField6 = PullData.DialerField6,
			NCH.DialerField7 = PullData.DialerField7,
			NCH.DialerField8 = PullData.DialerField8,
			NCH.DialerField9 = PullData.DialerField9,
			NCH.DialerField10 = PullData.DialerField10,
			NCH.DialerField11 = PullData.DialerField11,
			NCH.DialerField12 = PullData.DialerField12,
			NCH.DialerField13 = PullData.DialerField13,
			NCH.DialerField14 = PullData.DialerField14,
			NCH.DialerField15 = PullData.DialerField15,
			NCH.DialerField16 = PullData.DialerField16,
			NCH.DialerField17 = PullData.DialerField17,
			NCH.DialerField18 = PullData.DialerField18,
			NCH.DialerField19 = PullData.DialerField19,
			NCH.DialerField20 = PullData.DialerField20,
			NCH.DialerField21 = PullData.DialerField21,
			NCH.DialerField22 = PullData.DialerField22,
			NCH.DialerField23 = PullData.DialerField23,
			NCH.DialerField24 = PullData.DialerField24,
			NCH.DialerField25  = PullData.DialerField25
	WHEN NOT MATCHED THEN
		INSERT
		(
			NobleRowId,
			CallType,
			CallCampaign,
			ActivityDate,
			PhoneNumber,
			AccountIdentifier,
			ListId,
			AgentId,
			DispositionCode,
			AdditionalDispositionCode,
			RegionId,
			VoxFileId,
			IsInbound,
			CallLength,
			TimeACW,
			TimeHold,
			AgentHold,
			Filler1,
			Filler3,
			Filler4,
			d_record_id,
			DialerField1, 
			DialerField2, 
			DialerField3, 
			DialerField4, 
			DialerField5, 
			DialerField6, 
			DialerField7, 
			DialerField8, 
			DialerField9, 
			DialerField10,
			DialerField11,
			DialerField12,
			DialerField13,
			DialerField14,
			DialerField15,
			DialerField16,
			DialerField17,
			DialerField18,
			DialerField19,
			DialerField20,
			DialerField21,
			DialerField22,
			DialerField23,
			DialerField24,
			DialerField25
		)
		VALUES
		(
			PullData.NobleRowId,
			PullData.CallType,
			PullData.CallCampaign,
			PullData.ActivityDate,
			PullData.PhoneNumber,
			PullData.AccountIdentifier,
			PullData.ListId,
			PullData.AgentId,
			PullData.DispositionCode,
			PullData.AdditionalDispositionCode,
			PullData.RegionId,
			PullData.VoxFileId,
			PullData.IsInbound,
			PullData.CallLength,
			PullData.TimeACW,
			PullData.TimeHold,
			PullData.AgentHold,
			PullData.Filler1,
			PullData.Filler3,
			PullData.Filler4,
			PullData.d_record_id,
			PullData.DialerField1, 
			PullData.DialerField2, 
			PullData.DialerField3, 
			PullData.DialerField4, 
			PullData.DialerField5, 
			PullData.DialerField6, 
			PullData.DialerField7, 
			PullData.DialerField8, 
			PullData.DialerField9, 
			PullData.DialerField10,
			PullData.DialerField11,
			PullData.DialerField12,
			PullData.DialerField13,
			PullData.DialerField14,
			PullData.DialerField15,
			PullData.DialerField16,
			PullData.DialerField17,
			PullData.DialerField18,
			PullData.DialerField19,
			PullData.DialerField20,
			PullData.DialerField21,
			PullData.DialerField22,
			PullData.DialerField23,
			PullData.DialerField24,
			PullData.DialerField25
		);
		
		DELETE FROM _InboundCallData
END
