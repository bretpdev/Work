/****** Script for SelectTopNRows command from SSMS  ******/
SELECT distinct
	ACCOUNT_NUMBER,
	MAX(SCAN_DATE) AS SCAN_DATE
  FROM [UHEAA].[dbo].[UHEAA_UHEAA_COMMERCIAL_TAB]
  where DOC_ID = 'LSFOR'
  and SCAN_DATE between '03/01/2021' and '05/01/2021'
  and ACCOUNT_NUMBER != ''
  --order by SCAN_DATE desc
GROUP BY
	ACCOUNT_NUMBER