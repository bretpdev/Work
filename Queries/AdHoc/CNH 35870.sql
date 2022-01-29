USE CDW
GO

SELECT DISTINCT
	PDXX.DF_PRS_ID AS [Social Security Number]
	,LTRIM(RTRIM(PDXX.DM_PRS_LST)) + ', ' + LTRIM(RTRIM(PDXX.DM_PRS_X)) AS [Last Name, First Name]
	,PDXX.DD_DTH_STA AS [Alleged Death Status Applied]
	,PDXX.DC_DTH_STA
FROM
	PDXX_GTR_DTH PDXX
	INNER JOIN PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
WHERE
	PDXX.DC_DTH_STA = 'XX'
;