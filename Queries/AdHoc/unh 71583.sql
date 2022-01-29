SELECT 
	[CallCampaign] AS [ServiceNumber],
	[ActivityDate] AS [Date],
	FifteenInterval AS [Period],
	SUM(TotalCalls) AS ReceivedCalls,
	SUM(Handled) AS HandledCalls,
	SUM(Abandoned) AS AbandonedCalls,
	CASE 
		WHEN SUM(Handled) > 0 THEN SUM(CallLength)/SUM(Handled)
		ELSE 0 
	END AS AvgTalkTime,
	CASE 
		WHEN SUM(Handled) > 0 THEN SUM(TimeACW)/SUM(Handled)
		ELSE 0
	END AS AvgACWTime,
	'' AS [TransferredCalls],
	SUM([TimeHold]) / SUM(TotalCalls) AS [ASA],
	
	CAST((CAST(SUM(ServiceLevel) AS DECIMAL) / CAST(SUM(TotalCalls) AS DECIMAL))*100 AS INT) AS [ServiceLevel],
	'' QtyOfAgents,
	'' OccupancyRatio,
	'' ContactsInQueue
FROM
	(
		SELECT DISTINCT
			COALESCE(T1.DF_SPE_ACC_ID,T2.DF_SPE_ACC_ID) AS AccountNumber,
			[CallType],
			[ListId],
			CallData.[CallCampaign],
			CAST([ActivityDate] AS DATE) AS [ActivityDate],
			CAST([ActivityDate] AS TIME(0)) AS [ActivityTime],
			LEFT(CONVERT(VARCHAR,CAST(ActivityDate AS TIME(0)),24), 2) + ':' +
			CASE 
				WHEN CAST(RIGHT(LEFT(CONVERT(VARCHAR,CAST(ActivityDate AS TIME(0)),24), 5),2) AS INT) < 15 THEN '00'
				WHEN CAST(RIGHT(LEFT(CONVERT(VARCHAR,CAST(ActivityDate AS TIME(0)),24), 5),2) AS INT) < 30 THEN '15'
				WHEN CAST(RIGHT(LEFT(CONVERT(VARCHAR,CAST(ActivityDate AS TIME(0)),24), 5),2) AS INT) < 45 THEN '30'
				WHEN CAST(RIGHT(LEFT(CONVERT(VARCHAR,CAST(ActivityDate AS TIME(0)),24), 5),2) AS INT) <= 59 THEN '45'
			END AS FifteenInterval,

			ActivityDate AS ActivityDateTime,
			[PhoneNumber],
			[AgentId],
			[DispositionCode],
			[AdditionalDispositionCode],
			CallData.[RegionId],
			[IsInbound],
			isnull([CallLength],0) as [CallLength],
			isnull([TimeACW],0) as [TimeACW],
			[TimeHold] as [TimeHold],
			ISNULL([AgentHold],0) AS [AgentHold],
			CASE
				WHEN IsInbound = 1 THEN 'Inbound'
				ELSE 'Outbound'
			END AS [Type],
			1 AS TotalCalls,
			CASE WHEN NULLIF(AGENTID,'') IS NOT NULL THEN 1 ELSE 0 END AS Handled,
			CASE WHEN NULLIF(AGENTID,'') IS NULL THEN 1 ELSE 0 END AS Abandoned,
			CASE WHEN NULLIF(AGENTID,'') IS NOT NULL AND [TimeHold] <= 20 THEN 1 ELSE 0 END AS ServiceLevel
		FROM 
			[NobleCalls].[dbo].[NobleCallHistory] CallData
		JOIN [NobleCalls].[dbo].[CallCampaigns] CC 
			ON CC.CallCampaign = CallData.CallCampaign
			AND CC.RegionID = 2 -- UHEAA
		LEFT JOIN [UDW].[dbo].[PD10_PRS_NME] t1
			ON t1.DF_PRS_ID = CallData.AccountIdentifier
			AND LEN(CallData.AccountIdentifier) = 9
		LEFT JOIN [UDW].[dbo].[PD10_PRS_NME] t2
			ON t2.DF_SPE_ACC_ID = CallData.AccountIdentifier
			AND LEN(CallData.AccountIdentifier) = 10
		WHERE 
			CallData.RegionId = 2 
			and DeletedAt is null
			and IsInbound = 1
			--and cast(CallData.CreatedAt as date) = '06-02-2021'
		--	CASE 
		--		WHEN IsInbound = 1 AND CallType IN (0) THEN 'Include'
		----WHEN IsInbound = 0 AND [CallType] IN (0,1) THEN 'Include'
		----WHEN IsInbound = 0 AND [CallType] IN (4,5) AND CallData.CallCampaign = 'OUT' THEN 'Include'
		--		ELSE 'Exclude'
		--	END = 'Include'
		--	AND 
		--		CASE --UHEAA HOURS OF OPERATION--
		--			WHEN 
		--				CAST([ActivityDate] AS DATE) >= '2021-1-1' 
		--				AND CallData.RegionId = 2 -- UHEAA 
		--				AND (DATEPART(dw,[ActivityDate]) BETWEEN 2 AND 5) -- Monday - Thursday
		--				AND (CAST([ActivityDate] AS Time(0)) BETWEEN '07:00:00' AND '17:59:59')
		--				AND IsInbound = 1
		--			THEN 'True'
		--			WHEN 
		--				CAST([ActivityDate] AS DATE) >= '2021-1-1' 
		--				AND CallData.RegionId = 2 -- UHEAA 
		--				AND (DATEPART(dw,[ActivityDate]) = 6) --Friday
		--				AND (CAST([ActivityDate] AS Time(0)) BETWEEN '07:00:00' AND '16:59:59')
		--				AND IsInbound = 1
		--			THEN 'True'
		--			WHEN 
		--				CAST([ActivityDate] AS DATE) < '2021-1-1' --Change in Hours of Operation
		--				AND CallData.RegionId = 2 -- UHEAA 
		--				AND (DATEPART(dw,[ActivityDate]) BETWEEN 2 AND 6) -- Monday - Friday
		--				AND (CAST([ActivityDate] AS Time(0)) BETWEEN '08:00:00' AND '16:59:59')
		--				AND IsInbound = 1
		--			THEN 'True'
		--			WHEN 
		--				IsInbound = 1
		--				AND AgentId <> '' 
		--			THEN 'True'
		--			WHEN 
		--				IsInbound = 0 
		--			THEN 'True'
		--			ELSE 'False' 
		--	END = 'True'
)a 
GROUP BY
[CallCampaign] ,
[ActivityDate] ,
FifteenInterval
order by 
	[date] DESC,
	[period] DESC