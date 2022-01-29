CREATE PROCEDURE [dbo].[GetAllReconciledCallsForPreviousMonth]
	
AS
	SELECT 
		NCH.NobleCallHistoryId AS CallIdNumber,
		NCH.ActivityDate AS CallDate,
		NCH.CallLength AS CallLength, 
		NCH.IsInbound,
		NCH.AgentId AS CSRName_AgentId, 
		NCH.VoxFileId,
		NCH.VoxFileLocation,
		NCH.CallCampaign
	FROM
		NobleCallHistory NCH
	WHERE		
		ISNULL(NCH.VoxFileId,'') != ''
		AND RegionId = 3
		AND NCH.DeletedAt IS NULL
		AND NCH.VoxFileLocation IS NOT NULL
		AND NCH.ActivityDate BETWEEN DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE())-1, 0)  AND DATEADD(MONTH, DATEDIFF(MONTH, -1, GETDATE())-1, -1) --WITHIN THE PREVIOUS MONTH