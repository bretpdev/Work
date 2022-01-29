
--change date below for month end
select p.[TYPE],count(*) as [January XXXX]    from
(
SELECT DISTINCT
	dfXX.bf_ssn,	
	CASE 
		WHEN DFXX.LC_DFR_TYP IN ('XX', 'XX') THEN 'In School Deferment' 
		WHEN DFXX.LC_DFR_TYP IN ('XX') THEN 'Economic hardship deferment' 
		WHEN DFXX.LC_DFR_TYP IN ('XX') THEN 'Unemployment deferment' 
		WHEN DFXX.LC_DFR_TYP IN ('XX', 'XX') THEN 'Military deferment' 
		WHEN DFXX.LC_DFR_TYP IN ('XX', 'XX') THEN 'Plus deferment' 
		WHEN DFXX.LC_DFR_TYP IN ('XX') THEN 'Plus post enrollment deferment' 
	END AS [TYPE] 
	
FROM 
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..DFXX_BR_DFR_REQ DFXX
		ON DFXX.BF_SSN  = LNXX.BF_SSN
	INNER JOIN CDW..LNXX_BR_DFR_APV LNXX
		ON LNXX.BF_SSN = DFXX.BF_SSN
		AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
WHERE
	DFXX.LC_DFR_TYP IN ('XX','XX','XX','XX','XX','XX','XX','XX','XX')
	AND DFXX.LC_DFR_STA = 'A'
	AND 'XX/XX/XXXX' BETWEEN LNXX.LD_DFR_BEG AND LNXX.LD_DFR_END
) p
group by p.[TYPE]




----change date below for month end
--select p.[TYPE],count(*) as [January XXXX]    from
--(
--SELECT DISTINCT
--	fbXX.bf_ssn,	
--	CASE 
--		WHEN FBXX.LC_FOR_TYP IN ('XX', 'XX', 'XX', 'XX') THEN 'madatory forbearance' 
--		WHEN FBXX.LC_FOR_TYP IN ('XX') THEN 'general forbearance' 
--		WHEN FBXX.LC_FOR_TYP IN ('XX') THEN 'student loand debt burden forb ' 
--		WHEN FBXX.LC_FOR_TYP IN ('XX') THEN 'Teacher loan forgiveness	' 
--		WHEN FBXX.LC_FOR_TYP IN ('XX') THEN 'collection suspension forb' 
--		WHEN FBXX.LC_FOR_TYP IN ('XX') THEN 'Admin forb' 
--	END AS [TYPE] 
	
--FROM 
--	CDW..LNXX_LON LNXX
--	INNER JOIN CDW..FBXX_BR_FOR_REQ FBXX
--		ON LNXX.BF_SSN = FBXX.BF_SSN
--	INNER JOIN CDW..LNXX_BR_FOR_APV LNXX
--		ON LNXX.BF_SSN = FBXX.BF_SSN
--		AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
--WHERE
--	FBXX.LC_FOR_TYP IN ('XX','XX','XX','XX','XX','XX','XX','XX','XX')

--	AND FBXX.LC_FOR_STA = 'A'

--	AND 'XX/XX/XXXX' BETWEEN LNXX.LD_FOR_BEG AND LNXX.LD_FOR_END
--) p
--group by p.[TYPE]

