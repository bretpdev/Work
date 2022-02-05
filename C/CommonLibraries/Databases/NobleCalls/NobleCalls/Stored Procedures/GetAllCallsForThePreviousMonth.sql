CREATE PROCEDURE [dbo].[GetAllCallsForThePreviousMonth]
	
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
		AND NCH.VoxFileLocation IS NULL
		--/*TESTING ONLY*/ AND CAST(NCH.CreatedAt AS DATE) BETWEEN CAST(DATEADD(DAY,-30,GETDATE()) AS DATE) AND CAST(GETDATE()-1 AS DATE) /*trying to avoid errors with calls that have happened today*/
		AND CAST(NCH.ActivityDate AS DATE) BETWEEN CAST(DATEADD(DAY,-30,GETDATE()) AS DATE) AND CAST(GETDATE()-1 AS DATE) /*trying to avoid errors with calls that have happened today*/
