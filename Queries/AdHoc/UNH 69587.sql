 SELECT 
	* 
FROM 
	OPENQUERY(DUSTER,
	'
		SELECT 
			* 
		FROM 
			OLWHRM1.LP13_DD_ACT_STA	
		WHERE 
			PC_STA_LPD13 = ''A''
	'
);