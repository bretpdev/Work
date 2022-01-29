/*************** find location of duplicated files **************************/
SELECT DISTINCT
	CONVERT(DATE, CONVERT(VARCHAR(8),C.ActiveDutyStatusDate)) AS DuplicatedFileDate,
	--C.NumberOfFiles,
	H.SourceFile AS LocationOfDuplicatedFile
FROM
	ULS.hsadrptuh.ScraHistoricalFiles H
	INNER JOIN 
	(--find duplicate files based on ActiveDutyStatusDates
		SELECT
			ActiveDutyStatusDate,
			COUNT(ActiveDutyStatusDate) AS NumberOfFiles
		FROM
		(--get distinct filenames and dates
			SELECT DISTINCT
				SourceFile
				,ActiveDutyStatusDate
			FROM
				ULS.hsadrptuh.ScraHistoricalFiles
			--order by ActiveDutyStatusDate
		) filenames
		GROUP BY
			ActiveDutyStatusDate	
	) C
		ON H.ActiveDutyStatusDate = C.ActiveDutyStatusDate
WHERE
	NumberOfFiles > 1
ORDER BY
	DuplicatedFileDate
;

/************** find which historical files are missing *****************************/
SELECT DISTINCT
	CompleteCalendar.CalendarMonth,
	CompleteCalendar.CalendarYear,
	ScraFiles.ScraCalendarMonth,
	ScraFiles.ScraCalendarYear,
	CASE
		WHEN ScraFiles.ScraCalendarMonth IS NULL
		THEN CONCAT('MISSING SCRA FILE: ', DATENAME(MM, DATEADD(MM, CompleteCalendar.CalendarMonth, -1)), ' ', CompleteCalendar.CalendarYear)
		ELSE NULL
	END AS FileStatus
FROM
	(
		SELECT DISTINCT
			CalendarMonth,
			CalendarYear
		FROM
			CentralData..DimDate
		WHERE
			CalendarYear BETWEEN 2015 AND YEAR(GETDATE())
			AND DATEFROMPARTS(CalendarYear, CalendarMonth, 1) <= CONVERT(DATE,GETDATE())
	) CompleteCalendar
	LEFT JOIN
	(
		SELECT DISTINCT 
			MONTH(CONVERT(DATE,CONVERT(VARCHAR(8),SHF.ActiveDutyStatusDate))) AS ScraCalendarMonth, 
			YEAR(CONVERT(DATE,CONVERT(VARCHAR(8),SHF.ActiveDutyStatusDate))) AS ScraCalendarYear 
		FROM 
			ULS.hsadrptuh.ScraHistoricalFiles SHF
	) ScraFiles
		ON CompleteCalendar.CalendarMonth = ScraFiles.ScraCalendarMonth
		AND CompleteCalendar.CalendarYear = ScraFiles.ScraCalendarYear
ORDER BY
	CompleteCalendar.CalendarYear,
	CompleteCalendar.CalendarMonth
;