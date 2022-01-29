

CREATE PROCEDURE [dbo].[InsertCallHistory]
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


	INSERT INTO NobleCallHistory(NobleRowId, AccountIdentifier, CallType, ListId, CallCampaign, ActivityDate, PhoneNumber, AgentId, DispositionCode, AdditionalDispositionCode, RegionId, VoxFileId, IsInbound, CallLength, Deleted, DeletedAt, DeletedBy)
		SELECT
			D.NobleRowId, 
			COALESCE(D.AccountIdentifier, '') AS [AccountIdentifier],
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
			D.CallLength,
			CASE
				WHEN COALESCE(D.AccountIdentifier, '') = '' THEN 1
			ELSE 0 END [Deleted],
			CASE
				WHEN COALESCE(D.AccountIdentifier, '') = '' THEN  GETDATE()
			ELSE NULL END [DeletedAt],
			CASE
				WHEN COALESCE(D.AccountIdentifier, '') = '' THEN  'NobleCallHistory..InsertCallHistory - No Account#'
			ELSE NULL END [DeletedBy]
		FROM
			@Data D
			LEFT JOIN NobleCallHistory NCH
				ON D.NobleRowId = NCH.NobleRowId
				AND	D.CallType = NCH.CallType
				AND	D.CallCampaign = NCH.CallCampaign
				AND	D.ActivityDate = NCH.ActivityDate
				AND D.PhoneNumber = NCH.PhoneNumber
				AND NCH.CreatedAt > CAST(GETDATE() AS DATE)
			LEFT JOIN CallCampaigns CC
				ON CC.CallCampaign = D.CallCampaign
		WHERE
			NCH.NobleRowId IS NULL
END
