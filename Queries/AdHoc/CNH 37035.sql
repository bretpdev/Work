DECLARE @MON_DATA TABLE (MON VARCHAR(X), YR VARCHAR(X))
INSERT INTO @MON_DATA
VALUES('X','XXXX'),
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
('X','XXXX')



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
	RM_DSC_LTR_PRC AS Letter_id,
	' + @cols + '
FROM
(
	SELECT
		LTXX.RM_DSC_LTR_PRC,
		CAST(MONTH(LTXX.CreatedAt) AS VARCHAR(X)) + ''-'' + CAST(YEAR(LTXX.CreatedAt) AS VARCHAR(X)) AS TF,
		COUNT(*) AS E
	FROM
		CDW..LTXX_LTR_REQ_PRC LTXX
	WHERE
		LTXX.CreatedAt BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX''
		AND LTXX.InactivatedAt IS NULL
		AND LTXX.OnEcorr = X
	GROUP BY
		LTXX.RM_DSC_LTR_PRC,
		CAST(MONTH(LTXX.CreatedAt) AS VARCHAR(X)) + ''-'' + CAST(YEAR(LTXX.CreatedAt) AS VARCHAR(X))
	
	UNION ALL

	SELECT
		L.Letter,
		CAST(MONTH(PP.AddedAt) AS VARCHAR(X)) + ''-'' + CAST(YEAR(PP.AddedAt) AS VARCHAR(X)) AS TF,
		COUNT(*) AS E
	FROM
		CLS.[print].PrintProcessing PP
		INNER JOIN CLS.[print].ScriptData SD
			ON SD.ScriptDataId = PP.ScriptDataId
		INNER JOIN CLS.[print].Letters L
			on SD.letterid = L.letterid
	WHERE
		PP.AddedAt BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX''
		AND PP.OnEcorr = X
		AND PP.DeletedAt IS NULL
	GROUP BY
		L.Letter,
		CAST(MONTH(PP.AddedAt) AS VARCHAR(X)) + ''-'' + CAST(YEAR(PP.AddedAt) AS VARCHAR(X))

	UNION ALL

		SELECT
		''EBILLFED'',
		CAST(MONTH(PP.AddedAt) AS VARCHAR(X)) + ''-'' + CAST(YEAR(PP.AddedAt) AS VARCHAR(X)) AS TF,
		COUNT(*) AS E
	FROM
		CLS.[billing].PrintProcessing PP
		
	WHERE
		PP.AddedAt BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX''
		AND PP.OnEcorr = X
		AND PP.DeletedAt IS NULL
	GROUP BY
		CAST(MONTH(PP.AddedAt) AS VARCHAR(X)) + ''-'' + CAST(YEAR(PP.AddedAt) AS VARCHAR(X))


) P
PIVOT
(
	SUM(E)
	FOR TF IN (' + @colsX + ')
) p
'

EXEC (@QUERY)

SELECT @QUERY = 
'SELECT
	RM_DSC_LTR_PRC AS Letter_id,
	' + @cols + '
FROM
(
	SELECT
		LTXX.RM_DSC_LTR_PRC,
		CAST(MONTH(LTXX.CreatedAt) AS VARCHAR(X)) + ''-'' + CAST(YEAR(LTXX.CreatedAt) AS VARCHAR(X)) AS TF,
		COUNT(*) AS E
	FROM
		CDW..LTXX_LTR_REQ_PRC LTXX
	WHERE
		LTXX.CreatedAt BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX''
		AND LTXX.InactivatedAt IS NULL
		AND LTXX.OnEcorr = X
	GROUP BY
		LTXX.RM_DSC_LTR_PRC,
		CAST(MONTH(LTXX.CreatedAt) AS VARCHAR(X)) + ''-'' + CAST(YEAR(LTXX.CreatedAt) AS VARCHAR(X))
	
	UNION ALL

	SELECT
		L.Letter,
		CAST(MONTH(PP.AddedAt) AS VARCHAR(X)) + ''-'' + CAST(YEAR(PP.AddedAt) AS VARCHAR(X)) AS TF,
		COUNT(*) AS E
	FROM
		CLS.[print].PrintProcessing PP
		INNER JOIN CLS.[print].ScriptData SD
			ON SD.ScriptDataId = PP.ScriptDataId
		INNER JOIN CLS.[print].Letters L
			on SD.letterid = L.letterid
	WHERE
		PP.AddedAt BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX''
		AND PP.OnEcorr = X
		AND PP.DeletedAt IS NULL
	GROUP BY
		L.Letter,
		CAST(MONTH(PP.AddedAt) AS VARCHAR(X)) + ''-'' + CAST(YEAR(PP.AddedAt) AS VARCHAR(X))

	UNION ALL

		SELECT
		''BILSTFED'',
		CAST(MONTH(PP.AddedAt) AS VARCHAR(X)) + ''-'' + CAST(YEAR(PP.AddedAt) AS VARCHAR(X)) AS TF,
		COUNT(*) AS E
	FROM
		CLS.[billing].PrintProcessing PP
		
	WHERE
		PP.AddedAt BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX''
		AND PP.OnEcorr = X
		AND PP.DeletedAt IS NULL
	GROUP BY
		CAST(MONTH(PP.AddedAt) AS VARCHAR(X)) + ''-'' + CAST(YEAR(PP.AddedAt) AS VARCHAR(X))


) P
PIVOT
(
	SUM(E)
	FOR TF IN (' + @colsX + ')
) p
'

EXEC (@QUERY)

