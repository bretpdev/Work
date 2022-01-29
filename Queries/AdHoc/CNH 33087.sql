USE [NobleCalls]
GO
/****** Object:  StoredProcedure [dbo].[GetAllCallsForThePreviousMonth]    Script Date: XX/X/XXXX X:XX:XX AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetAllCallsForThePreviousMonth]
	
AS
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
		(NCH.VoxFileId IS NOT NULL OR NCH.VoxFileId <> '')
		AND RegionId = X
		AND NCH.DeletedAt IS NULL
		AND NCH.ActivityDate BETWEEN DATEADD(MONTH, DATEDIFF(MONTH, X, GETDATE())-X, X)  AND DATEADD(MONTH, DATEDIFF(MONTH, -X, GETDATE())-X, -X) --WITHIN THE PREVIOUS MONTH
RETURN X