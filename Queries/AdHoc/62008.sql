use tlp
go



-- updated summary query
SELECT
	SL.SchoolInfoID,
	PSL.SchoolName,
	SI.SlotsAvilblForYear,
	COUNT(SL.RecNum) [SlotsUsed],
	SUM(LD.DisbAmount) [DusbursementAmount]
FROM
	[TLP].[dbo].[SchoolSlot] SL
	INNER JOIN SchoolInformation SI 
		ON SI.ID = SL.SchoolInfoID
	INNER JOIN ParticipatingSchoolsList PSL 
		ON PSL.SchoolCode = SI.SchoolCode
	LEFT JOIN LoanDat LD 
		ON LD.SchoolCode = SI.SchoolCode 
		AND LD.SSN = SL.SSN 
		AND LD.LoanStatus != 'Rejected' 
		AND CAST(LD.DisbDate AS DATE) BETWEEN '07/01/2018' AND '06/30/2019'
WHERE
	SL.SlotStatus = 'A'
	AND	CAST(SL.SlotStartDt AS DATE) BETWEEN '07/01/2018' AND '06/30/2019'
	AND SI.SchoolCode = '00368000'
GROUP BY
	SL.SchoolInfoID,
	PSL.SchoolName,
	SI.SlotsAvilblForYear


-- detail query
SELECT distinct
	SL.SchoolInfoID,
	PSL.SchoolName,
	SL.RecNum [SlotNumber],
	SL.SSN,
	LD.DisbAmount [DusbursementAmount],
	SL.SlotStartDt,
	LD.Term	
FROM
	[TLP].[dbo].[SchoolSlot] SL
	INNER JOIN SchoolInformation SI 
		ON SI.ID = SL.SchoolInfoID
	INNER JOIN ParticipatingSchoolsList PSL 
		ON PSL.SchoolCode = SI.SchoolCode
	LEFT JOIN LoanDat LD 
		ON LD.SchoolCode = SI.SchoolCode 
		AND LD.SSN = SL.SSN 
		AND LD.LoanStatus != 'Rejected' 
		AND CAST(LD.DisbDate AS DATE) BETWEEN '07/01/2018' AND '06/30/2019'
WHERE
	SL.SlotStatus = 'A'
	AND	CAST(SL.SlotStartDt AS DATE) BETWEEN '07/01/2018' AND '06/30/2019'
	AND SI.SchoolCode = '00368000'
