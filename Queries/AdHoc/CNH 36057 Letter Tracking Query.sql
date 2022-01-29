
SELECT
	[DocDetailId]
    ,[DocName]
    ,[DocSeqNo]
    ,[DocTyp]
    ,[Status]
    ,[ID]
    ,[Code]
    ,[Letterhead]
    ,[Model]
    ,[ActCd]
    ,[ARC]
    ,[DocID]
    ,[CostCd]
    ,[Addressee]
    ,[Regarding]
    ,[Recip]
    ,[AltRecip]
    ,[Unit]
    ,[ACSParticipant]
    ,[LPD]
    ,[Addresse]
    ,[OtherAddressee]
    ,[Citation]
	,REPLACE(CONVERT(NVARCHAR(MAX), [ReqLang]), char(XX) + char(XX), ' ') AS [ReqLang]
    ,[Path_old]
    ,[BCPCriticality]
    ,[Compliance]
	,REPLACE(CONVERT(NVARCHAR(MAX), [Description]), char(XX) + char(XX), ' ') AS [Description]
FROM 
	[BSYS].[dbo].[LTDB_DAT_DocDetail]
WHERE
	([DocName] LIKE '%FED%' OR DocName LIKE '%Cornerstone%' OR DocID LIKE '%FED%')
	AND [DocTyp] = 'Compass'
    AND Status = 'Active'
ORDER BY
	DocDetailId