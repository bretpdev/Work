--troubleshoot why I'm not receiving Request Queue Aging report
USE Subscriptions
GO

SELECT
	*
FROM
	Reports R
	INNER JOIN Subscriptions S ON S.ReportId = R.ReportId
	INNER JOIN Subscribers Sb on Sb.SubscriberId = S.SubscriberId
	INNER JOIN ReportFormats RF on RF.ReportFormatId = S.ReportFormatId
WHERE
	R.ReportName = 'Request Queue Aging'
	and FirstName='J.R.'

SELECT *
  FROM [ReportServer].[dbo].[Subscriptions]
  where Description = 'Request Queue Aging'

  SELECT *
  FROM [ReportServer].[dbo].[ExecutionLogStorage]
  where UserName = 'UHEAA\jnolasco'

/* changed email address from rnolasco to jnolasco
also changed full name field from J.R. Nolasco to Refugio Nolasco */


--remove users that are no longer at UHEAA
SELECT *
FROM [Subscriptions].[dbo].[Subscribers]

  --begin tran
  --delete from [Subscriptions].[dbo].[Subscribers]
  --where SubscriberId in (4,9,18)
  --and EmailAddress in ('jdavis@utahsbr.edu','psnyder@utahsbr.edu','mballard@utahsbr.edu')  
