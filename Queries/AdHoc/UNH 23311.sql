/****** Script for SelectTopNRows command from SSMS  ******/
SELECT [TicketID]
      --,CAST(DATEDIFF(HOUR,StartTime,EndTime) AS VARCHAR) + ':' + CAST(DATEDIFF(MINUTE,StartTime,EndTime) AS VARCHAR) AS ELAPSED--
      ,SUM(DATEDIFF(MINUTE, StartTime, EndTime))/60.0 AS TOTAL_HOURS
  FROM [Reporting].[dbo].[TimeTracking]
  WHERE TicketID IN ('18745',
					'18801',
					'18747',
					'18813',
					'18814',
					'18750',
					'18821',
					'18754',
					'18758',
					'18834',
					'18761',
					'18767',
					'18783',
					'18837')
  GROUP BY TicketID