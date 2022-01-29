USE [NobleCalls]
GO
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SELECT DISTINCT
		ISNULL(PD10.DF_PRS_ID, PD101.DF_PRS_ID) AS SSN,
		NCH.NobleCallHistoryId AS CallIdNumber,
		NCH.ActivityDate AS CallDate,
		NCH.CallLength AS CallLength, -- This will need to be updated once the column is added
		NCH.IsInbound,
		NCH.AgentId AS CSRName_AgentId, 
		NCH.VoxFileId,
		NCH.VoxFileLocation,
		NCH.CallCampaign
	FROM
		NobleCallHistory NCH
		INNER JOIN NobleCalls..DispositionCodeMapping MAP
			ON MAP.DispositionCode = NCH.DispositionCode
		INNER JOIN NobleCalls..Arcs A
			ON A.ArcId = MAP.ArcId
		INNER JOIN NobleCalls..Comments C
			ON C.CommentId = MAP.CommentId
		INNER JOIN NobleCalls..ResponseCodes RC
			ON RC.ResponseCodeId = MAP.ResponseCodeId
		LEFT JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_SPE_ACC_ID = NCH.AccountIdentifier
		LEFT JOIN UDW..PD10_PRS_NME PD101
			ON PD101.DF_PRS_ID = NCH.AccountIdentifier
	WHERE
		
		 --NCH.VoxFileId IS NOT NULL
		 --AND NCH.VoxFileLocation IS NOT NULL
		 RegionId = 2
		AND NCH.DeletedAt IS NULL
		AND CAST(NCH.ActivityDate AS DATE) BETWEEN '12/01/2017' and '12/31/2017' 
		AND NCH.IsInbound = 0
		AND NCH.DispositionCode IN ('PT','P','DF','FB','RC','RP')
		AND NCH.CallCampaign IN ('BDD1', 'UNOW', 'OUT', 'BLST')
		AND NCH.CallLength >= 90
		