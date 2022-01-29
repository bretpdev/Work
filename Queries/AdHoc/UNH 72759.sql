

DROP TABLE IF EXISTS #DATA

SELECT 
CAST(M AS VARCHAR(2)) + '-' + CAST(Y AS VARCHAR(4)) AS TF,
m as [month], 
y as [year],
wrapupdata,
COUNT(*) AS WC
INTO #DATA
FROM --INFORMIX Database
 OPENQUERY(uccx,
 ' SELECT  DISTINCT
 cw.SessionID,
 cw.SessionSeqNum,
 cw.ResourceID,
 cw.WrapupData,
 cw.NodeId,
 cw.Qindex,
 cw.WrapupIndex,
 cw.ContactId,
 year(cw.startdatetime) as y,
 month(cw.startdatetime) as m,
 day(cw.startdatetime) as d
 
FROM contactwrapupdata  cw
inner join AgentConnectionDetail acd
	on cw.sessionid = acd.sessionid
inner join  ContactCallDetail CCD
	on CCD.contactid = ACD.contactid
	AND CCD.sessionID = ACD.sessionID
	AND CCD.sessionSeqNum = ACD.sessionSeqNum
	AND CCD.nodeID = ACD.nodeID
	AND CCD.profileID = ACD.profileID
where
	CCD.applicationname like ''LPP%''
	and cw.startdatetime >  DATE(CURRENT - 60 UNITS DAY)
order by sessionid, sessionseqnum, wrapupindex
') 
group by
	M,Y,
wrapupdata
order by [month], [YEAR]

SELECT * FROM #DATA ORDER BY WRAPUPDATA


DECLARE @MON_DATA TABLE (MON VARCHAR(3), YR VARCHAR(4))
INSERT INTO @MON_DATA
VALUES('7','2021'),
('8','2021'),
('9','2021')

DECLARE @cols AS NVARCHAR(MAX),
@colsnonnull AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX),
	@cols1 AS NVARCHAR(MAX)

	
select @cols = STUFF((SELECT ',' + 'isnull(' + QUOTENAME(MON + '-' + YR) + ',0) ' + QUOTENAME(MON + '-' + YR) FROM @MON_DATA ORDER BY CAST(YR AS INT), CAST(MON AS INT) FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')

select @cols1 = STUFF((SELECT ',' + QUOTENAME(MON + '-' + YR)  FROM @MON_DATA ORDER BY CAST(YR AS INT), CAST(MON AS INT) FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'')

		SELECT @QUERY = 
		'
		SELECT
			WRAPUPDATA,
			' + @cols + '
		FROM
		(
			SELECT
				TF,
				WRAPUPDATA,
				WC
			FROM 
				#DATA
		) P
		PIVOT
		(
			SUM(WC)
			FOR TF IN (' + @cols1 + ')
		) p
'
PRINT @QUERY
EXEC (@QUERY)
		
			