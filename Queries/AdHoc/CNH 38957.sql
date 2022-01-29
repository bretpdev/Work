/****** Script for SelectTopNRows command from SSMS  ******/
	SELECT 
		'Ecorr' AS [Script],
		EC.Letter,
		EC.SubjectLine
		
  FROM 
		[ECorrFed].[dbo].[Letters] EC
  WHERE 
		EC.Active = X

  UNION ALL

	SELECT 
		'Email' AS [Script],
		EM.LetterId,
		EM.SubjectLine
		
	FROM
		[CLS].[emailbtcf].[EmailCampaigns] EM
	WHERE
		EM.InactivatedAt IS NULL
