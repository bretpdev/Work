USE BSYS
GO

SELECT
	ID,
	DS.Job
FROM
	dbo.SCKR_DAT_SASSchedule SS
	INNER JOIN dbo.SCKR_DAT_SAS DS ON DS.Job = SS.Job
WHERE
	SS.EOM = 1
	AND
	DS.[Status] = 'Active'
ORDER BY
	DS.Job