/****** Script for SelectTopNRows command from SSMS  ******/
SELECT DISTINCT C.SchoolName
		,A.SchoolCode
		,A.AwardAmtAvilblForYear
		,SUM(B.DisbAmount) AS DisbAmount
  FROM [TLP].[dbo].[SchoolInformation] A
	INNER JOIN [TLP].[dbo].LoanDat B
		ON A.SchoolCode = B.SchoolCode
	INNER JOIN [TLP].[dbo].ParticipatingSchoolsList C
		ON A.SchoolCode = C.SchoolCode
  WHERE A.YearBeginDt > '06/30/2013'
  AND A.SchoolCode IN ('00367000','00367700','00368000')
  AND B.DisbDate > CONVERT(DATE,'06-30-2013')
  GROUP BY C.SchoolName
			,A.SchoolCode
			,A.AwardAmtAvilblForYear