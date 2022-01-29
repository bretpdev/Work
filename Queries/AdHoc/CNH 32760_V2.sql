SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

IF OBJECT_ID('tempdb..#POP') IS NOT NULL 
	DROP TABLE #POP

IF OBJECT_ID('tempdb..#POPX') IS NOT NULL 
	DROP TABLE #POPX
IF OBJECT_ID('tempdb..#POPX') IS NOT NULL 
	DROP TABLE #POPX
IF OBJECT_ID('tempdb..#POPX') IS NOT NULL 
	DROP TABLE #POPX

SELECT DISTINCT
	LNXX.BF_SSN,
	LNXX.LC_TYP_SCH_DIS,
	LNXX.LD_CRT_LONXX
INTO #POP
FROM
	CDW..LNXX_LON_RPS LNXX
	INNER JOIN
	(
		SELECT DISTINCT
			BF_SSN
		FROM
			CDW..LNXX_LON_RPS LNXX
		WHERE
			LNXX.LC_TYP_SCH_DIS IN ('CP', 'CQ', 'IL', 'IP')
		AND LNXX.LD_CRT_LONXX BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'

	) SCH
		ON SCH.BF_SSN = LNXX.BF_SSN
	
SELECT 
	*, 
	ROW_NUMBER() OVER (PARTITION BY BF_SSN ORDER BY LD_CRT_LONXX) AS ROW_NUM 
INTO #POPX
FROM 
	#POP

;WITH SUMS
AS
(
	SELECT
		P.BF_SSN,
		P.LC_TYP_SCH_DIS,
		P.LD_CRT_LONXX,
		P.ROW_NUM,	
		CASE WHEN P.LC_TYP_SCH_DIS  IN ('CA', 'CX', 'CX', 'CX', 'IB', 'IS', 'IX', 'IX') THEN X ELSE X END AS PREV,
		X AS CUR
	FROM
		#POPX P
	WHERE
		P.ROW_NUM = X -- start with sequence X
	
	UNION ALL
	
	SELECT
		P.BF_SSN,
		P.LC_TYP_SCH_DIS,
		P.LD_CRT_LONXX,
		P.ROW_NUM,
		CASE WHEN P.LC_TYP_SCH_DIS  IN ('CA', 'CX', 'CX', 'CX', 'IB', 'IS', 'IX', 'IX') THEN X ELSE X END AS PREV,
		CASE WHEN PREV = X AND P.LC_TYP_SCH_DIS IN ('CP', 'CQ', 'IL', 'IP') AND P.LD_CRT_LONXX BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX' THEN X ELSE X END AS CUR
	FROM
		SUMS S
		INNER JOIN #POPX P ON P.ROW_NUM = S.ROW_NUM + X AND P.BF_SSN = S.BF_SSN 
)
SELECT DISTINCT
	S.*
INTO #POPX
FROM 
	SUMS S
WHERE
	CUR = X
ORDER BY
	BF_SSN

SELECT
	COUNT(DISTINCT BF_SSN)
FROM
	#POPX

SELECT DISTINCT	
	BF_SSN
FROM
	#POPX


SELECT
	LNXX.BF_SSN,
	LNXX.LC_TYP_SCH_DIS,
	LNXX.LD_CRT_LONXX,
	DATEDIFF(MONTH, POP.LD_CRT_LONXX, LNXX.LD_CRT_LONXX) AS MON,
	ROW_NUMBER() OVER (PARTITION BY LNXX.BF_SSN, LNXX.LC_TYP_SCH_DIS, LNXX.LD_CRT_LONXX ORDER BY LNXX.LD_CRT_LONXX) AS ROW_NUM
INTO #POPX
FROM
	CDW..LNXX_LON_RPS LNXX
	INNER JOIN
	(
		SELECT DISTINCT
			LNXX.BF_SSN,
			MAX(LNXX.LD_CRT_LONXX) AS LD_CRT_LONXX
		FROM 	
			CDW..LNXX_LON_RPS LNXX
			INNER JOIN #POPX P
				ON P.BF_SSN = LNXX.BF_SSN
		WHERE
			LNXX.LC_TYP_SCH_DIS IN ('CP', 'CQ', 'IL', 'IP')
			AND LNXX.LD_CRT_LONXX BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
			AND LNXX.LD_CRT_LONXX >= P.LD_CRT_LONXX
		GROUP BY
			LNXX.BF_SSN
	) POP
	 ON POP.BF_SSN = LNXX.BF_SSN
	 AND LNXX.LD_CRT_LONXX >= POP.LD_CRT_LONXX




SELECT 
	COUNT(DISTINCT BF_SSN)
FROM 
	#POPX
WHERE 
	ROW_NUM = X
	AND LC_TYP_SCH_DIS IN  ('CA', 'CX', 'CX', 'CX', 'IB', 'IS', 'IX', 'IX')
	AND MON <= X


SELECT DISTINCT 
	BF_SSN
FROM 
	#POPX
WHERE 
	ROW_NUM = X
	AND LC_TYP_SCH_DIS IN  ('CA', 'CX', 'CX', 'CX', 'IB', 'IS', 'IX', 'IX')
	AND MON <= X