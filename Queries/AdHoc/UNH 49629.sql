-- orig query
SELECT
	LD.SchoolCode,
	PSL.SchoolName,
	SUM(LD.DisbAmount) DisbAmount,
	SI.SlotsAvilblForYear AS SlotsAvailable,
	COUNT(LD.DisbDate) AS UsedSlot,
	SL.*
FROM
	LoanDat LD
	JOIN ParticipatingSchoolsList PSL ON LD.SchoolCode = PSL.SchoolCode
	JOIN SchoolInformation SI ON LD.SchoolCode = SI.SchoolCode AND YearBeginDt >= '07/01/2016'
	INNER JOIN [TLP].[dbo].[SchoolSlot] SL ON SL.SchoolInfoID = SI.ID
WHERE
	PSL.SchoolCode IN ('00367000', '00368000')
	AND 
	CAST(LD.DisbDate AS DATE) BETWEEN '07/01/2016' AND '06/30/2017'
	AND
	SL.SlotStatus = 'A'
GROUP BY
	LD.SchoolCode,
	PSL.SchoolName,
	SI.SlotsAvilblForYear
ORDER BY
	LD.SchoolCode

-- updated summary query
SELECT
	SL.SchoolInfoID,
	PSL.SchoolName,
	SI.SlotsAvilblForYear,
	COUNT(SL.RecNum) [SlotsUsed],
	SUM(LD.DisbAmount) [DusbursementAmount]
FROM
	[TLP].[dbo].[SchoolSlot] SL
	INNER JOIN SchoolInformation SI ON SI.ID = SL.SchoolInfoID
	INNER JOIN ParticipatingSchoolsList PSL ON PSL.SchoolCode = SI.SchoolCode
	LEFT JOIN LoanDat LD ON LD.SchoolCode = SI.SchoolCode AND LD.SSN = SL.SSN AND LD.LoanStatus != 'Rejected' AND CAST(LD.DisbDate AS DATE) BETWEEN '07/01/2016' AND '06/30/2017'
WHERE
	SL.SlotStatus = 'A'
	AND
	CAST(SL.SlotStartDt AS DATE) BETWEEN '07/01/2016' AND '06/30/2017'
	AND
	SI.SchoolCode IN ('00367000', '00368000')
GROUP BY
	SL.SchoolInfoID,
	PSL.SchoolName,
	SI.SlotsAvilblForYear

-- detail query
SELECT
	SL.SchoolInfoID,
	PSL.SchoolName,
	SL.RecNum [SlotNumber],
	SL.SSN,
	LD.DisbAmount [DusbursementAmount],
	SL.SlotStartDt,
	LD.Term	
FROM
	[TLP].[dbo].[SchoolSlot] SL
	INNER JOIN SchoolInformation SI ON SI.ID = SL.SchoolInfoID
	INNER JOIN ParticipatingSchoolsList PSL ON PSL.SchoolCode = SI.SchoolCode
	LEFT JOIN LoanDat LD ON LD.SchoolCode = SI.SchoolCode AND LD.SSN = SL.SSN AND LD.LoanStatus != 'Rejected' AND CAST(LD.DisbDate AS DATE) BETWEEN '07/01/2016' AND '06/30/2017'
WHERE
	SL.SlotStatus = 'A'
	AND
	CAST(SL.SlotStartDt AS DATE) BETWEEN '07/01/2016' AND '06/30/2017'
	AND
	SI.SchoolCode IN ('00367000', '00368000')