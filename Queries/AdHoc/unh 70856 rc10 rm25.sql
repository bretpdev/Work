--select * from openquery(duster,
--'
--SELECT 
--	*
--FROM 
--	OLWHRM1.RC10_DLY_FIN_RCC RC10
--WHERE
--	ID_FIN_RCC_ATY_EFF = ''2021-03-12''
--')

--select * from openquery(duster,
--'
--SELECT 
--	RM25.*
--FROM 
--	OLWHRM1.RM25_RMT_SPS_RFD RM25
--	INNER JOIN OLWHRM1.RC10_DLY_FIN_RCC RC10
--		ON RC10.ID_FIN_RCC_ATY_EFF = RM25.LD_STA_REMT25
--		AND RC10.IA_PST_FIN_RCC = RM25.LA_SPS_RFD
--WHERE
	
--	 RM25.LD_RMT_BCH_INI = ''02/16/2021''
--	and rm25.LC_RMT_BCH_SRC_IPT = ''T''
--	and RM25.LN_RMT_BCH_SEQ = 2
--	AND LN_RMT_SEQ_PST = 101
--')

--select * from openquery(duster,
--'
--SELECT 
--	RC10.*
--FROM 
--	 OLWHRM1.RC10_DLY_FIN_RCC RC10
--	 INNER JOIN OLWHRM1.
--WHERE
	
--	 RC10.ID_FIN_RCC_ATY_EFF = ''03/12/2021''
--	 AND RC10.IA_PST_FIN_RCC = 1800
	

--')

select * from openquery(duster,
'
SELECT DISTINCT
	RC10.* 
FROM 
	OLWHRM1.LN76_LON_RFD LN76
	INNER JOIN OLWHRM1.RC10_DLY_FIN_RCC RC10
		ON RC10.ID_FIN_RCC_ATY_EFF = LN76.LD_STA_LON76
		AND RC10.IA_PST_FIN_RCC = 62.06
WHERE
	BF_SSN IN 
	(
	--''024689435'',
	--''042080496'',
	--''258758099'',
	--''294528407'',
	--''440969091'',
	--''458654160'',
	--''463876605'',
	--''467950330'',
	--''520083744'',
	--''521214052'',
	--''525151265'',
	--''529634563'',
	--''610669547'',
	--''625050829'',
	----''646050753'',
	''646163335''
	)
')

select * from openquery(duster,
'
SELECT 
	ln76.*
FROM 
	OLWHRM1.LN76_LON_RFD LN76
WHERE
	BF_SSN IN 
	(
	--''024689435'',
	--''042080496'',
	--''258758099'',
	--''294528407'',
	--''440969091'',
	--''458654160'',
	--''463876605'',
	--''467950330'',
	--''520083744'',
	--''521214052'',
	--''525151265'',
	--''529634563'',
	--''610669547'',
	--''625050829'',
	----''646050753'',
	''646163335''
	)
')

--SELECT * FROM OPENQUERY(DUSTER,
--'
--SELECT 
--	*
--FROM
--	OLWHRM1.RC10_DLY_FIN_RCC
--WHERE 
--	ia_pst_fin_rcc = ''380.77''
--')


