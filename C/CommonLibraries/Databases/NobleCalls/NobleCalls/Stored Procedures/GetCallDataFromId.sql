CREATE PROCEDURE [dbo].[GetCallDataFromId]
	@CallId int
AS
	SELECT 
		NCH.NobleCallHistoryId AS CallIdNumber,
		NCH.ActivityDate AS CallDate,
		NCH.CallLength AS CallLength, 
		NCH.IsInbound,
		NCH.AgentId AS CSRName_AgentId, 
		NCH.VoxFileId,
		NCH.VoxFileLocation
	FROM	
		NobleCallHistory NCH
	WHERE
		NCH.NobleCallHistoryId = @CallId
