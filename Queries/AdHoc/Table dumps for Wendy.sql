SELECT * FROM OPENQUERY (DUSTER,'
	SELECT 
		* 
	FROM 
		OLWHRM1.SV10_SER_MST
');

SELECT * FROM OPENQUERY (DUSTER,'
	SELECT 
		* 
	FROM 
		OLWHRM1.SV25_SER_DPT
');

SELECT * FROM OPENQUERY (LEGEND,'
	SELECT 
		* 
	FROM 
		PKUB.SV10_SER_MST
');

SELECT * FROM OPENQUERY (LEGEND,'
	SELECT 
		* 
	FROM 
		PKUB.SV25_SER_DPT
');