
SELECT
	UH.[DocDetailId]
    ,REPLACE(UH.[DocName],',',' ') AS [DocName]
    ,UH.[DocSeqNo]
    ,UH.[DocTyp]
    ,UH.[Status]
    ,UH.[ID]
    ,UH.[Code]
	,UH.[Letterhead]
    ,UH.[Model]
    ,UH.[ActCd]
    ,UH.[ARC]
    ,UH.[DocID]
    ,UH.[CostCd]
    ,UH.[Addressee]
    ,UH.[Regarding]
    ,UH.[Recip]
    ,UH.[AltRecip]
    ,UH.[Unit]
    ,UH.[ACSParticipant]
    ,UH.[LPD]
    ,UH.[Addresse]
    ,UH.[OtherAddressee]
    ,UH.[Citation]
	,REPLACE(REPLACE(CONVERT(NVARCHAR(MAX), UH.[ReqLang]), char(13) + char(10), ' '),',',' ') AS [ReqLang]
    ,UH.[Path_old]
    ,UH.[BCPCriticality]
    ,UH.[Compliance]
	,REPLACE(REPLACE(CONVERT(NVARCHAR(MAX), UH.[Description]), char(13) + char(10), ' '),',',' ') AS [Description]
FROM 
	[BSYS].[dbo].[LTDB_DAT_DocDetail] UH
	LEFT JOIN
		(
			SELECT DISTINCT
			    [DocName]
    
			FROM 
				[BSYS].[dbo].[LTDB_DAT_DocDetail]
			WHERE
				([DocName] LIKE '%FED%' OR DocName LIKE '%Cornerstone%' OR DocID LIKE '%FED%')
				AND [DocTyp] = 'Compass'
				AND Status = 'Active'
		) FED
			ON UH.DocName = FED.DocName
WHERE
	UH.[DocTyp] = 'Compass'
    AND UH.[Status] = 'Active'
	AND FED.DocName IS NULL
ORDER BY
	UH.DocDetailId
