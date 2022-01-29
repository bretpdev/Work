USE [NobleCalls]
GO

	SELECT 
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
	WHERE
		
		 NCH.VoxFileId IS NOT NULL
		 AND NCH.VoxFileLocation IS NOT NULL
		AND RegionId = X
		AND NCH.DeletedAt IS NULL
		AND NCH.ActivityDate BETWEEN 'XX/XX/XXXX' and 'XX/XX/XXXX' 
		AND NCH.IsInbound = X
		AND NCH.CallCampaign IN ('OutC', 'XXX', 'DNOW')

