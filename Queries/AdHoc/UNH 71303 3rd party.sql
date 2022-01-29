DECLARE @MON_DATA TABLE (MON VARCHAR(3), YR VARCHAR(4))
INSERT INTO @MON_DATA
VALUES
('1','2019'),
('2','2019'),
('3','2019'),
('4','2019'),
('5','2019'),
('6','2019'),
('7','2019'),
('8','2019'),
('9','2019'),
('10','2019'),
('11','2019'),
('12','2019')



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
'SELECT
	LABEL AS LABEL,
	' + @cols + '
FROM
(
	SELECT
		
			AY10.PF_REQ_ACT  AS LABEL,

	
		CAST(MONTH(AY10.LD_ATY_REQ_RCV ) AS VARCHAR(2)) + ''-'' + CAST(YEAR(AY10.LD_ATY_REQ_RCV ) AS VARCHAR(4)) AS TF,
		COUNT(*) AS [COUNT]
	FROM
		CDW..AY10_BR_LON_ATY AY10
	WHERE
		YEAR(AY10.LD_ATY_REQ_RCV) = 2019 
		AND AY10.LC_STA_ACTY10 = ''A''
		AND AY10.PF_REQ_ACT IN 
		(
			''DI3RD''


		)
	GROUP BY
		PF_REQ_ACT,
		CAST(MONTH(AY10.LD_ATY_REQ_RCV ) AS VARCHAR(2)) + ''-'' + CAST(YEAR(AY10.LD_ATY_REQ_RCV ) AS VARCHAR(4))
	
	

) P
PIVOT
(
	SUM([COUNT])
	FOR TF IN (' + @cols1 + ')
) p


'

EXEC (@QUERY)





