DECLARE @MON_DATA TABLE (MON VARCHAR(X), YR VARCHAR(X))
INSERT INTO @MON_DATA
VALUES
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('X','XXXX'),
('XX','XXXX'),
('XX','XXXX'),
('XX','XXXX')



DECLARE @cols AS NVARCHAR(MAX),
@colsnonnull AS NVARCHAR(MAX),
    @query  AS NVARCHAR(MAX),
	@colsX AS NVARCHAR(MAX)

	
select @cols = STUFF((SELECT ',' + 'isnull(' + QUOTENAME(MON + '-' + YR) + ',X) ' + QUOTENAME(MON + '-' + YR) FROM @MON_DATA ORDER BY CAST(YR AS INT), CAST(MON AS INT) FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,X,X,'')

select @colsX = STUFF((SELECT ',' + QUOTENAME(MON + '-' + YR)  FROM @MON_DATA ORDER BY CAST(YR AS INT), CAST(MON AS INT) FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,X,X,'')


SELECT @QUERY = 
'SELECT
	LABEL AS LABEL,
	' + @cols + '
FROM
(
	SELECT
		''Compliant Records Account Research FSA:''AS LABEL,
		CAST([MetricMonth] AS VARCHAR(X)) + ''-'' + CAST([MetricYear] AS VARCHAR(X)) AS TF,
		[CompliantRecords] as [COUNT]
	FROM
		[ServicerInventoryMetrics].[dbo].[MetricsSummary]
	where ServicerMetricsId = X
	

) P
PIVOT
(
	SUM([COUNT])
	FOR TF IN (' + @colsX + ')
) p

UNION
SELECT
	LABEL AS LABEL,
	' + @cols + '
FROM
(
	SELECT
		''Total Records Account Research FSA:''AS LABEL,
		CAST([MetricMonth] AS VARCHAR(X)) + ''-'' + CAST([MetricYear] AS VARCHAR(X)) AS TF,
		[TotalRecords] as [COUNT]
	FROM
		[ServicerInventoryMetrics].[dbo].[MetricsSummary]
	where ServicerMetricsId = X
	

) P
PIVOT
(
	SUM([COUNT])
	FOR TF IN (' + @colsX + ')
) p
'

exec (@QUERY)





