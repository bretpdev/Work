CREATE PROCEDURE [dbo].[GetAllUnreconciledCalls]

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
	NobleCalls..NobleCallHistory NCH
WHERE 
	ISNULL(NCH.VoxFileId,'') != ''
	AND NCH.RegionId = 3 --cornerstone
	AND NCH.DeletedAt IS NULL
	AND NCH.VoxFileLocation IS NULL