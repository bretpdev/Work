CREATE PROCEDURE [ndnuminval].[GetCalls]
	@Region VARCHAR(20)
	
AS

	DECLARE @TargetRegionId INT = (SELECT RegionId FROM NobleCalls..Regions WHERE Region = @Region)
	DECLARE @ScriptId VARCHAR(10) = 'NDNUMINVAL'
	DECLARE @Today DATE = CAST(GETDATE() AS DATE)

		--Get non-fax phone records to be invalidated
		SELECT
			CallType, 
			AccountIdentifier, 
			SUBSTRING(PhoneNumber, 1, 3) AS AreaCode, 
			SUBSTRING(PhoneNumber, 4, (LEN(PhoneNumber) - 3)) AS Phone, 
			CallCampaign AS Campaign, 
			DispositionCode AS Disposition, 
			AdditionalDispositionCode AS AdditionDisposition,
			AgentId,
			ActivityDate AS EffectiveDate,
			ListId
		FROM 
			NobleCalls..NobleCallHistory NCH
			INNER JOIN NobleCalls..CampaignPrefixes CP
				ON NCH.CallCampaign LIKE (CP.CampaignPrefix + '%')
				AND CP.RegionId = @TargetRegionId
				AND CP.ScriptId = @ScriptId
				AND CP.Active = 1
		WHERE
			NCH.CallType != 5
			AND NCH.DispositionCode IN ('Not in Service', 'Invalid', 'Wrong Number')
			AND NCH.DeletedAt IS NULL
			AND NCH.Deleted != 1
			AND LTRIM(RTRIM(NCH.PhoneNumber)) != ''
			AND CAST(NCH.ActivityDate AS DATE) = @Today

		UNION ALL

		--Now add fax records if three records on different dates for same phone had fax disposition, with no other dispositions in between, regardless of campaign, call type
		SELECT
			LastFaxRecord.CallType,
			LastFaxRecord.AccountIdentifier,
			LastFaxRecord.AreaCode, 
			LastFaxRecord.Phone, 
			LastFaxRecord.Campaign, 
			LastFaxRecord.Disposition, 
			LastFaxRecord.AdditionDisposition,
			LastFaxRecord.AgentId,
			LastFaxRecord.EffectiveDate,
			LastFaxRecord.ListId
		FROM
			(
				SELECT
					LastCalls.PhoneNumber,
					CASE 
						WHEN COUNT(1) = 3 THEN 1 ELSE 0
						END AS NeedToInvalidate
				FROM
				(
					SELECT
						CallHistory.PhoneNumber,
						CAST(CallHistory.ActivityDate AS DATE) ActivityDate,
						CallHistory.DispositionCode,
						ROW_NUMBER() OVER(PARTITION BY CallHistory.PhoneNumber ORDER BY CAST(CallHistory.ActivityDate AS DATE) DESC) AS RN
					FROM
						(
							SELECT
								LTRIM(RTRIM(NCH.PhoneNumber)) AS PhoneNumber, 
								NCH.ActivityDate,
								NCH.DispositionCode
							FROM
								NobleCalls..NobleCallHistory NCH
								LEFT JOIN NobleCalls..NobleCallHistory NF --NonFax
									ON NF.PhoneNumber = NCH.PhoneNumber
									AND NF.DispositionCode != 'Fax'
									AND NF.ActivityDate > NCH.ActivityDate
							WHERE
								NF.PhoneNumber IS NULL
								AND NCH.DispositionCode = 'Fax'
								AND NCH.Deleted != 1
								AND NCH.DeletedAt IS NULL
								AND NCH.CallType != 5
								AND LTRIM(RTRIM(NCH.PhoneNumber)) != ''
						) CallHistory
					GROUP BY
						CallHistory.PhoneNumber,
						CallHistory.DispositionCode,
						CAST(CallHistory.ActivityDate AS DATE)	
					) LastCalls
				WHERE
					LastCalls.RN <= 3
					AND LastCalls.DispositionCode = 'Fax'
				GROUP BY
					LastCalls.PhoneNumber	
			) FaxRecords
			INNER JOIN --Get last record for phone (which will be a fax)
			(
				SELECT
					PhoneNumber,
					MAX(ActivityDate) AS ActivityDate
				FROM
					NobleCalls..NobleCallHistory
				WHERE
					Deleted != 1
					AND DeletedAt IS NULL
					AND CallType != 5
					AND LTRIM(RTRIM(PhoneNumber)) != ''
				GROUP BY
					PhoneNumber
			) LastActivity 
				ON LastActivity.PhoneNumber = FaxRecords.PhoneNumber
			INNER JOIN --Get record info for last fax
			(
				SELECT 
					CallType,
					AccountIdentifier,
					SUBSTRING(PhoneNumber, 1, 3) AS AreaCode, 
					SUBSTRING(PhoneNumber, 4, (LEN(PhoneNumber) - 3)) AS Phone, 
					LTRIM(RTRIM(PhoneNumber)) AS PhoneNumber,
					CallCampaign AS Campaign, 
					DispositionCode AS Disposition, 
					AdditionalDispositionCode AS AdditionDisposition,
					AgentId,
					ActivityDate AS EffectiveDate,
					ListId,
					RegionId
				FROM
					NobleCalls..NobleCallHistory PhoneInfo
				WHERE
					PhoneInfo.Deleted != 1
					AND PhoneInfo.DeletedAt IS NULL
					AND PhoneInfo.CallType != 5
					AND CAST(PhoneInfo.ActivityDate AS DATE) = @Today --Last fax has to be from the current date
			) LastFaxRecord
				ON LastFaxRecord.PhoneNumber = LastActivity.PhoneNumber
				AND LastFaxRecord.EffectiveDate = LastActivity.ActivityDate
			INNER JOIN NobleCalls..CampaignPrefixes CP
				ON LastFaxRecord.Campaign LIKE (CP.CampaignPrefix + '%')
				AND CP.RegionId = @TargetRegionId --Even though three bad faxes don't need to be in the same region for us to invalidate, only grab if last fax is in region being processed
				AND CP.ScriptId = @ScriptId
				AND CP.Active = 1
		WHERE
			FaxRecords.NeedToInvalidate = 1

RETURN 0
