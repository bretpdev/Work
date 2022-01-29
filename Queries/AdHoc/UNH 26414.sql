--total time REQUESTED to COMPLETE:
SELECT 
	Ticket
	,[Subject]
	,ROUND(CAST(SUM(CAST(DATEDIFF(SECOND, Requested, LastUpdated) AS FLOAT) / 60 / 60) AS FLOAT),2) AS Total_Hours
	,[Status]	
	,Requested
	,LastUpdated
FROM 
	[NeedHelpUheaa].[dbo].[DAT_Ticket]
WHERE 
	Ticket IN 
	(
	'22008','22049',	'22050','22051',	'22067','22085',	'22086','22087',
	'22088','22089',	'22090','22091',	'22092','22093',	'22094','22095',
	'22096','22101',	'22109','22157',	'22214','22454',	'22458','22609',
	'22610','22638',	'22646','22649',	'22651','22654',	'22666','22732',
	'22778','22796',	'22798','22799',	'22801','22889',	'22890','22919',
	'22924','22933',	'23142','23328',	'23343','23520',	'23559','23567',
	'23582','23860',	'23890','23957',	'24018','24232',	'24259','24261',
	'24477','24592'	
	)
GROUP BY 
	Ticket
	,[Subject]
	,[Status]	
	,Requested
	,LastUpdated



--actual time worked
SELECT
	Ticket
	,[Subject]
	,ROUND(CAST(SUM(CAST(DATEDIFF(second, StartTime, EndTime) AS FLOAT) / 60 ) AS FLOAT),2) AS 'Total Time (minutes)'
FROM
	NeedHelpUheaa.dbo.DAT_Ticket DT
	INNER JOIN Reporting.dbo.TimeTracking TT
		ON DT.Ticket = TT.TicketID
WHERE 
	Ticket IN 
	(
	'22008','22049',	'22050','22051',	'22067','22085',	'22086','22087',
	'22088','22089',	'22090','22091',	'22092','22093',	'22094','22095',
	'22096','22101',	'22109','22157',	'22214','22454',	'22458','22609',
	'22610','22638',	'22646','22649',	'22651','22654',	'22666','22732',
	'22778','22796',	'22798','22799',	'22801','22889',	'22890','22919',
	'22924','22933',	'23142','23328',	'23343','23520',	'23559','23567',
	'23582','23860',	'23890','23957',	'24018','24232',	'24259','24261',
	'24477','24592'	
	)
GROUP BY
	Ticket
	,[Subject]

